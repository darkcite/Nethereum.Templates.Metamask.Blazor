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

namespace Marketplace.Wasm.Services
{
    public class NFTService
    {
        private EthereumClientService _ethereumClientService;
        private readonly IConfiguration _configuration;

        private string _contractAddress;
        private MyERC1155Service _erc1155Service;

        public NFTService(EthereumClientService ethereumClientService, IConfiguration configuration)
        {
            _ethereumClientService = ethereumClientService;
            _configuration = configuration;

            var web3 = _ethereumClientService.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            _contractAddress = _configuration.GetValue<string>("Ethereum:ContractAddress");
            _erc1155Service = new MyERC1155Service(web3, _contractAddress);
        }

        private async Task<List<NFT>> LoadNFTs(Func<TokenDataOutputDTO, bool> filter, string account = null)
        {
            List<NFT> NFTs = new List<NFT>();

            var web3 = _ethereumClientService.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var filterInput = _erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(new HexBigInteger(0)), // TODO: replace with new BlockParameter(deploymentReceipt.BlockNumber),
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

                    if (filter(tokenData))
                    {
                        // If account is specified, check the balance
                        if (!string.IsNullOrEmpty(account))
                        {
                            var balance = await _erc1155Service.BalanceOfQueryAsync(account, log.Event.TokenId);
                            if (balance.IsZero)
                            {
                                // Skip this NFT if the balance is zero for the specified account
                                continue;
                            }
                        }

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

        public Task<List<NFT>> LoadAllNFTs(string account = null) => LoadNFTs(_ => true, account);

        public Task<List<NFT>> LoadNFTsForSale(string account = null) => LoadNFTs(tokenData => tokenData.ForSale, account);

        public Task<List<NFT>> LoadNFTsOwnedByAccount(string account) => LoadNFTs(_ => true, account);

        public async Task UpdateNFTDetailsAsync(string accountId, BigInteger tokenId, BigInteger newPrice, bool newStatus, string newContactInfo)
        {
            var receipt = await _erc1155Service.SetTokenForSaleStatusAsync(tokenId, newPrice, newStatus, newContactInfo);
        }
    }
}
