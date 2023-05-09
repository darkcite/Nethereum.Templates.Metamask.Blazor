using System;
using Marketplace.Wasm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERC721ContractLibrary.Contracts.MyERC1155;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System.Net.Http;

namespace Marketplace.Wasm.Services
{
    public class NFTService
    {
        private EthereumClientService _ethereumClientService;

        public NFTService(EthereumClientService ethereumClientService)
        {
            _ethereumClientService = ethereumClientService;
        }

        public async Task<List<NFT>> LoadNFTs()
        {
            List<NFT> NFTs = new List<NFT>();

            // Add your logic to load NFTs
            var web3 = new Nethereum.Web3.Web3("http://localhost:8545");
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //creating a new service with the new contract address
            var erc1155Service = new MyERC1155Service(web3, "0x9ad9d20142E49925160E806D6e2611A872124cbb");
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
                string httpGatewayUrl = tokenUri.Replace("ipfs://", "https://we3ge.infura-ipfs.io/ipfs/");

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

