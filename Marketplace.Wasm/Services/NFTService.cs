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

namespace Marketplace.Wasm.Services
{
    public class NFTService
    {
        private EthereumClientService _ethereumClientService;
        private readonly IConfiguration _configuration;

        public NFTService(EthereumClientService ethereumClientService, IConfiguration configuration)
        {
            _ethereumClientService = ethereumClientService;
            _configuration = configuration;
        }

        private async Task<List<NFT>> LoadNFTs(Func<TokenDataOutputDTO, bool> filter)
        {
            List<NFT> NFTs = new List<NFT>();

            var web3 = _ethereumClientService.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var contractAddress = _configuration.GetValue<string>("Ethereum:ContractAddress");
            var erc1155Service = new MyERC1155Service(web3, contractAddress);

            var filterInput = erc1155Service.GetTokenMintedEvent().CreateFilterInput(new BlockParameter(), BlockParameter.CreateLatest());
            var eventLogs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput);

            var decodedLog = erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);
            foreach (var log in decodedLog)
            {
                string tokenUri = await erc1155Service.UriQueryAsync(log.Event.TokenId);
                var ipfsGatewayUrl = _configuration.GetValue<string>("IPFS:GatewayUrl");
                string httpGatewayUrl = tokenUri.Replace("ipfs://", ipfsGatewayUrl);

                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(httpGatewayUrl);

                if (response.IsSuccessStatusCode)
                {
                    string metadataJson = await response.Content.ReadAsStringAsync();
                    NFTMetadata metadata = JsonConvert.DeserializeObject<NFTMetadata>(metadataJson);

                    var tokenData = await erc1155Service.GetTokenDataAsync(log.Event.TokenId);

                    if (filter(tokenData))
                    {
                        NFTs.Add(new NFT
                        {
                            Image = metadata.Image,
                            Description = metadata.Description,
                            Name = metadata.Name,
                            TokenId = metadata.ProductId,
                            Price = tokenData.Price.ToString(),
                            ForSale = tokenData.ForSale
                        });
                    }
                }
            }

            return NFTs;
        }

        public Task<List<NFT>> LoadAllNFTs() => LoadNFTs(_ => true);

        public Task<List<NFT>> LoadNFTsForSale() => LoadNFTs(tokenData => tokenData.ForSale);

        public async Task<List<NFT>> LoadNFTsOld()
        {
            List<NFT> NFTs = new List<NFT>();

            // Add your logic to load NFTs
            var web3 = _ethereumClientService.GetWeb3();
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //creating a new service with the new contract address
            var contractAddress = _configuration.GetValue<string>("Ethereum:ContractAddress");
            var erc1155Service = new MyERC1155Service(web3, contractAddress);
            // Retrieve logs for the "TokenCreated" event
            var filterInput = erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(), // block number  from deployment reciept???
                BlockParameter.CreateLatest()
            );

            var eventLogs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput);

            // Parse logs and extract the created token IDs
            var decodedLog = erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);
            foreach (var log in decodedLog)
            {
                Console.WriteLine($"Token ID: {log.Event.TokenId}");

                /////////////
                ///Get Image by tokenId
                ///
                string tokenUri = await erc1155Service.UriQueryAsync(log.Event.TokenId);

                // Convert the IPFS URL to an HTTP gateway URL
                var ipfsGatewayUrl = _configuration.GetValue<string>("IPFS:GatewayUrl");
                string httpGatewayUrl = tokenUri.Replace("ipfs://", ipfsGatewayUrl);

                // Fetch the metadata JSON from the token URI
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(httpGatewayUrl);

                // If the request is successful, extract metadata
                if (response.IsSuccessStatusCode)
                {
                    string metadataJson = await response.Content.ReadAsStringAsync();
                    NFTMetadata metadata = JsonConvert.DeserializeObject<NFTMetadata>(metadataJson);
                    NFTs.Add(new NFT
                    {
                        Image = metadata.Image,
                        Description = metadata.Description,
                        Name = metadata.Name,
                        TokenId = metadata.ProductId
                    });
                }

            }


            return NFTs;
        }
    }



}

