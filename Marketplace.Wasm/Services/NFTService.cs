using System;
using Marketplace.Wasm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERC721ContractLibrary.Contracts.MyERC1155;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using ERC721ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using Nethereum.Hex.HexTypes;
using System.Numerics;
using Nethereum.Contracts.Standards.ERC1155;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.Web3;

namespace Marketplace.Wasm.Services
{
    public class NFTService
    {
        private EthereumClientService _ethereumClientService;
        private readonly IConfiguration _configuration;
        private MetaMaskService _metaMaskService;

        private readonly string _contractAddress;
        private readonly HexBigInteger _deploymentBlockNumber;

        private MyERC1155Service _erc1155Service;

        public NFTService(EthereumClientService ethereumClientService, IConfiguration configuration, MetaMaskService metaMaskService)
        {
            _ethereumClientService = ethereumClientService;
            _configuration = configuration;
            _metaMaskService = metaMaskService;

            var web3 = _ethereumClientService.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            _contractAddress = _configuration.GetValue<string>("Ethereum:ContractAddress");
            _deploymentBlockNumber = new HexBigInteger(BigInteger.Parse(_configuration.GetValue<string>("Ethereum:DeploymentBlockNumber")));

            _erc1155Service = new MyERC1155Service(web3, _contractAddress);
        }

        private async Task<List<NFT>> LoadNFTs(Func<TokenDataOutputDTO, bool> filter, string account = null, bool checkOwnership = false)
        {
            List<NFT> NFTs = new List<NFT>();

            var web3 = _ethereumClientService.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var filterInput = _erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(_deploymentBlockNumber),
                BlockParameter.CreateLatest());
            var eventLogs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput);

            var decodedLog = _erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);
            foreach (var log in decodedLog)
            {
                string tokenUri = await _erc1155Service.UriQueryAsync(log.Event.TokenId);
                var ipfsGatewayUrl = _configuration.GetValue<string>("IPFS:GatewayUrl");
                string httpGatewayUrl = tokenUri.Replace("ipfs://", ipfsGatewayUrl);

                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(httpGatewayUrl);

                if (response.IsSuccessStatusCode)
                {
                    string metadataJson = await response.Content.ReadAsStringAsync();
                    NFTMetadata metadata = JsonConvert.DeserializeObject<NFTMetadata>(metadataJson);

                    var tokenData = await _erc1155Service.GetTokenDataAsync(log.Event.TokenId);

                    // Check if the current owner of the token is the provided account
                    if (checkOwnership)
                    {
                        string currentOwner = await _erc1155Service.GetOwnerOfTokenAsync(metadata.ProductId);
                        if (account != null && currentOwner != account)
                        {
                            // Skip this token if the current owner is not the provided account
                            continue;
                        }
                    }

                    if (filter(tokenData))
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


        public Task<List<NFT>> LoadAllNFTs(string account = null) => LoadNFTs(_ => true, account, false);

        public Task<List<NFT>> LoadNFTsForSale(string account = null) => LoadNFTs(tokenData => tokenData.ForSale, account, false);

        public Task<List<NFT>> LoadNFTsOwnedByAccount(string account) => LoadNFTs(_ => true, account, true);


        public async Task UpdateNFTDetailsAsync(BigInteger tokenId, BigInteger newPrice, bool newStatus, string newContactInfo)
        {
            // Get the function for SetApprovalForAll
            var setApprovalFunction = _erc1155Service.ContractHandler.GetFunction<SetApprovalForAllFunction>();

            // Prepare the function call
            var approvalFunctionMessage = new SetApprovalForAllFunction
            {
                Operator = _contractAddress,
                Approved = true
            };

            // Encode the function call
            var approvalData = setApprovalFunction.GetData(approvalFunctionMessage);

            // Send the transaction through Metamask
            var approvalTxHash = await _metaMaskService.SendTransaction(_contractAddress, approvalData);

            // TODO: Wait for the transaction to be mined and get the receipt, and check the receipt status
            // if the transaction was not successful, throw an exception
            // throw new Exception("Failed to approve contract as operator");

            // Prepare function call for updating token
            var updateFunctionMessage = new UpdateTokenForSaleFunction
            {
                Id = tokenId,
                NewPrice = newPrice,
                NewStatus = newStatus,
                NewContactInfo = newContactInfo
            };

            // Get function from service for updating token
            var updateFunction = _erc1155Service.ContractHandler.GetFunction<UpdateTokenForSaleFunction>();

            // Encode the function call for updating token
            var updateData = updateFunction.GetData(updateFunctionMessage);

            // Send the transaction through Metamask
            var updateTxHash = await _metaMaskService.SendTransaction(_contractAddress, updateData);

            // TODO: Wait for the transaction to be mined and get the receipt, and check the receipt status
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

            // TODO: Wait for the transaction to be mined and get the receipt, and check the receipt status
            // if the transaction was not successful, throw an exception
            // throw new Exception("Failed to buy token");
        }

        public async Task<List<NFT>> GetAllTokensOwnedByAccountAsync(string account)
        {

            List<NFT> NFTs = new List<NFT>();

            var web3 = _ethereumClientService.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var filterInput = _erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(_deploymentBlockNumber),
                BlockParameter.CreateLatest());
            var eventLogs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput);

            var decodedLog = _erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);

            foreach (var log in decodedLog)
            {
                string tokenUri = await _erc1155Service.UriQueryAsync(log.Event.TokenId);
                var ipfsGatewayUrl = _configuration.GetValue<string>("IPFS:GatewayUrl");
                string httpGatewayUrl = tokenUri.Replace("ipfs://", ipfsGatewayUrl);

                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(httpGatewayUrl);

                if (response.IsSuccessStatusCode)
                {
                    string metadataJson = await response.Content.ReadAsStringAsync();
                    NFTMetadata metadata = JsonConvert.DeserializeObject<NFTMetadata>(metadataJson);

                    var tokenData = await _erc1155Service.GetTokenDataAsync(log.Event.TokenId);

                    var balance = await _erc1155Service.BalanceOfQueryAsync(account, log.Event.TokenId);
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
