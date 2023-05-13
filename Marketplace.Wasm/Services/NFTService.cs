using System;
using Marketplace.Wasm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERC1155ContractLibrary.Contracts.MyERC1155;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using ERC1155ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using Nethereum.Hex.HexTypes;
using System.Numerics;
using Nethereum.Web3;

namespace Marketplace.Wasm.Services
{
    public class NFTService
    {
        private readonly EthereumClientService _ethereumClientService;
        private readonly IConfiguration _configuration;
        private readonly MetaMaskService _metaMaskService;
        private readonly string _contractAddress;
        private readonly HexBigInteger _deploymentBlockNumber;
        private readonly Web3 _web3;
        private readonly MyERC1155Service _erc1155Service;

        public NFTService(EthereumClientService ethereumClientService, IConfiguration configuration, MetaMaskService metaMaskService)
        {
            _ethereumClientService = ethereumClientService;
            _configuration = configuration;
            _metaMaskService = metaMaskService;

            _web3 = _ethereumClientService.GetWeb3();
            _web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            _contractAddress = _configuration.GetValue<string>("Ethereum:ContractAddress");
            _deploymentBlockNumber = new HexBigInteger(BigInteger.Parse(_configuration.GetValue<string>("Ethereum:DeploymentBlockNumber")));

            _erc1155Service = new MyERC1155Service(_web3, _contractAddress);
        }

        private async Task<List<NFT>> LoadNFTs(Func<TokenDataOutputDTO, bool> filter, string account = null, bool checkOwnership = false)
        {
            List<NFT> NFTs = new List<NFT>();
            var filterInput = _erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(_deploymentBlockNumber),
                BlockParameter.CreateLatest());
            var eventLogs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput).ConfigureAwait(false);

            var decodedLog = _erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);
            foreach (var log in decodedLog)
            {
                var (tokenData, metadata) = await GetTokenData(log.Event.TokenId).ConfigureAwait(false);
                if (tokenData != null && metadata != null && filter(tokenData) && (!checkOwnership || await IsTokenOwnedByAccount(account, log.Event.TokenId)))
                {
                    NFTs.Add(new NFT
                    {
                        Image = metadata.Image,
                        Description = metadata.Description,
                        Name = metadata.Name,
                        TokenId = metadata.ProductId,
                        Price = tokenData.Price,
                        ForSale = tokenData.ForSale
                    });
                }
            }

            return NFTs;
        }

        private async Task<(TokenDataOutputDTO, NFTMetadata)> GetTokenData(BigInteger tokenId)
        {
            string tokenUri = await _erc1155Service.UriQueryAsync(tokenId).ConfigureAwait(false);
            var metadata = await GetMetadataFromUri(tokenUri).ConfigureAwait(false);
            var tokenData = await _erc1155Service.GetTokenDataAsync(tokenId).ConfigureAwait(false);
            return (tokenData, metadata);
        }

        private async Task<NFTMetadata> GetMetadataFromUri(string tokenUri)
        {
            var ipfsGatewayUrl = _configuration.GetValue<string>("IPFS:GatewayUrl");
            string httpGatewayUrl = tokenUri.Replace("ipfs://", ipfsGatewayUrl);

            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(httpGatewayUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string metadataJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<NFTMetadata>(metadataJson);
        }

        // Сheck if the current owner of the token is the provided account
        private async Task<bool> IsTokenOwnedByAccount(string account, BigInteger tokenId)
        {
            string currentOwner = await _erc1155Service.GetOwnerOfTokenAsync(tokenId).ConfigureAwait(false);
            return account != null && currentOwner == account;
        }

        public Task<List<NFT>> LoadAllNFTs(string account = null) => LoadNFTs(_ => true, account, false);

        public Task<List<NFT>> LoadNFTsForSale(string account = null) => LoadNFTs(tokenData => tokenData.ForSale, account, false);

        public Task<List<NFT>> LoadNFTsOwnedByAccount(string account) => LoadNFTs(_ => true, account, true);

        public async Task UpdateNFTDetailsAsync(BigInteger tokenId, BigInteger newPrice, bool newStatus)
        {
            var setApprovalFunction = _erc1155Service.ContractHandler.GetFunction<SetApprovalForAllFunction>();

            var approvalFunctionMessage = new SetApprovalForAllFunction
            {
                Operator = _contractAddress,
                Approved = true
            };

            var approvalData = setApprovalFunction.GetData(approvalFunctionMessage);

            var approvalTxHash = await _metaMaskService.SendTransaction(_contractAddress, approvalData);

            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(approvalTxHash);

            if (receipt.Status.Value == 0)
            {
                throw new Exception("Failed to approve transaction");
            }

            var updateFunctionMessage = new UpdateTokenForSaleFunction
            {
                Id = tokenId,
                NewPrice = newPrice,
                NewStatus = newStatus,
                NewContactInfo = ""
            };
            var updateFunction = _erc1155Service.ContractHandler.GetFunction<UpdateTokenForSaleFunction>();

            var updateData = updateFunction.GetData(updateFunctionMessage);

            var updateTxHash = await _metaMaskService.SendTransaction(_contractAddress, updateData);

            var receipt2 = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(updateTxHash);

            if (receipt2.Status.Value == 0)
            {
                throw new Exception("Failed to update token");
            }
        }

        public async Task BuyTokenAsync(BigInteger tokenId, BigInteger etherAmount)
        {
            // Get the function for BuyToken
            var buyTokenFunction = _erc1155Service.ContractHandler.GetFunction<BuyTokenFunction>();

            // Prepare the function call
            var buyTokenFunctionMessage = new BuyTokenFunction
            {
                TokenId = tokenId,
                AmountToSend = etherAmount
            };

            // Encode the function call
            var buyTokenData = buyTokenFunction.GetData(buyTokenFunctionMessage);

            // Send the transaction through Metamask
            var buyTokenTxHash = await _metaMaskService.SendTransaction(_contractAddress, buyTokenData, etherAmount);

            // Wait for the transaction to be mined and get the receipt
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(buyTokenTxHash);

            if (receipt.Status.Value == 0)
            {
                throw new Exception("Failed to buy token");
            }
        }

        public async Task<List<NFT>> GetAllTokensOwnedByAccountAsync(string account)
        {
            var NFTs = new List<NFT>();
            var filterInput = _erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(_deploymentBlockNumber),
                BlockParameter.CreateLatest());
            var eventLogs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput).ConfigureAwait(false);

            var decodedLog = _erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);

            foreach (var log in decodedLog)
            {
                var (tokenData, metadata) = await GetTokenData(log.Event.TokenId).ConfigureAwait(false);
                if (tokenData != null && metadata != null)
                {
                    var balance = await _erc1155Service.BalanceOfQueryAsync(account, log.Event.TokenId).ConfigureAwait(false);
                    if (balance > 0)
                    {
                        NFTs.Add(new NFT
                        {
                            Image = metadata.Image,
                            Description = metadata.Description,
                            Name = metadata.Name,
                            TokenId = metadata.ProductId,
                            Price = tokenData.Price,
                            ForSale = tokenData.ForSale
                        });
                    }
                }
            }
            return NFTs;
        }
    }
}

