using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ERC721ContractLibrary.Contracts.MyERC1155;
using ERC721ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using ERC721ContractLibrary.Contracts.MyERC721;
using ERC721ContractLibrary.Contracts.MyERC721.ContractDefinition;
using Nethereum.Contracts.Standards.ERC1155;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.XUnitEthereumClients;
using Newtonsoft.Json;
using Xunit;

namespace ERC721ContractLibrary.Testing
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class MyErc1155Test
    {
        private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

        public MyErc1155Test(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
        {
            _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;
        }


        [Fact]
        public async void ShouldDeployAndMintoken()
        {
            //Using rinkeby to demo opensea, if we dont want to use the configured client
            //please input your infura id in appsettings.test.json
            //var web3 = _ethereumClientIntegrationFixture.GetInfuraWeb3(InfuraNetwork.Rinkeby);
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

 
            var ercERC1155Deployment = new MyERC1155Deployment(); 
            //Deploy the 1155 contract (shop)
            var deploymentReceipt = await MyERC1155Service.DeployContractAndWaitForReceiptAsync(web3, ercERC1155Deployment);

            //creating a new service with the new contract address
            var erc1155Service = new MyERC1155Service(web3, deploymentReceipt.ContractAddress);

            //uploading to ipfs our documents
            var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: "2OyEUF7r4netkvcqOQNPkAkyVHQ", password: "d98294208b5d4302c6fd510a4d4df4c1");
            var imageIpfs = await nftIpfsService.AddFileToIpfsAsync("ShopImages/hard-drive-by-vincent-botta-from-unsplash.jpg");


            //adding all our document ipfs links to the metadata and the description
            var metadataNFT = new ProductNFTMetadata()
            {
                ProductId = 12131,
                Name = "4TB Black Performance Desktop Hard Drive",
                Image = "ipfs://" + imageIpfs.Hash, //The image is what is displayed in market places like opean sea
                Description = @"Capacity: 4TB
                                Interface: SATA 6Gb / s
                                Form Factor: 3.5 Inch
                                Easy Backup and Upgrade
                                5 Year Warranty (Photo by: Vincent Botta https://unsplash.com/@0asa)",
                ExternalUrl = "https://github.com/Nethereum/ERC721ContractLibrary.Template",
                Decimals = 0
            };
            var stockHardDrive = 100;
            //Adding the metadata to ipfs
            var metadataIpfs =
                await nftIpfsService.AddNftsMetadataToIpfsAsync(metadataNFT, metadataNFT.ProductId + ".json");

            var addressToRegisterOwnership = "0x465685122734334d2486D48bc340898BC314f71a";

            //Adding the product information
            var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(metadataNFT.ProductId,
                 "ipfs://" + metadataIpfs.Hash);

            var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, metadataNFT.ProductId, stockHardDrive, new byte[]{});


            // the balance should be 
            var balance = await erc1155Service.BalanceOfQueryAsync(addressToRegisterOwnership, (BigInteger)metadataNFT.ProductId);

            Assert.Equal(stockHardDrive, balance);

            var addressOfToken = await erc1155Service.UriQueryAsync(metadataNFT.ProductId);

            Assert.Equal("ipfs://" + metadataIpfs.Hash, addressOfToken);

            //Url format  https://testnets.opensea.io/assets/[nftAddress]/[id]
            //opening opensea testnet to visualise the nft
            var ps = new ProcessStartInfo("https://testnets.opensea.io/assets/" + deploymentReceipt.ContractAddress + "/" + metadataNFT.ProductId)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);

            //lets sell 2 hard drives 
            var transfer = await erc1155Service.SafeTransferFromRequestAndWaitForReceiptAsync(addressToRegisterOwnership, addressToRegisterOwnership, (BigInteger)metadataNFT.ProductId, 2, new byte[]{});
            Assert.False(transfer.HasErrors());

            // Retrieve logs for the "TokenCreated" event
            var filterInput = erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(deploymentReceipt.BlockNumber),
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

                // If the request is successful, extract the image URL from the metadata
                string image = null;
                if (response.IsSuccessStatusCode)
                {
                    string metadataJson = await response.Content.ReadAsStringAsync();
                    ProductNFTMetadata metadata = JsonConvert.DeserializeObject<ProductNFTMetadata>(metadataJson);
                    image = metadata.Image;
                }

                var tokenData = await erc1155Service.GetTokenDataAsync(log.Event.TokenId);
                var setForSaleResult = erc1155Service.SetTokenForSaleStatusAsync(log.Event.TokenId, 10, true, "sraka");
                tokenData = await erc1155Service.GetTokenDataAsync(log.Event.TokenId);
            }




        }

        [Fact]
        public async void CreateNft()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //creating a new service with the new contract address
            var erc1155Service = new MyERC1155Service(web3, "0x92fd8Fe190a149685164Cf60C6188Aff90993277");

            //uploading to ipfs our documents
            var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: "2OyEUF7r4netkvcqOQNPkAkyVHQ", password: "d98294208b5d4302c6fd510a4d4df4c1");
            var imageIpfs = await nftIpfsService.AddFileToIpfsAsync("ShopImages/hard-drive-by-vincent-botta-from-unsplash.jpg");


            //adding all our document ipfs links to the metadata and the description
            var metadataNFT = new ProductNFTMetadata()
            {
                ProductId = 12131,
                Name = "4TB Black Performance Desktop Hard Drive",
                Image = "ipfs://" + imageIpfs.Hash, //The image is what is displayed in market places like opean sea
                Description = @"Capacity: 4TB
                                Interface: SATA 6Gb / s
                                Form Factor: 3.5 Inch
                                Easy Backup and Upgrade
                                5 Year Warranty (Photo by: Vincent Botta https://unsplash.com/@0asa)",
                ExternalUrl = "https://github.com/Nethereum/ERC721ContractLibrary.Template",
                Decimals = 0
            };
            var stockHardDrive = 100;
            //Adding the metadata to ipfs
            var metadataIpfs =
                await nftIpfsService.AddNftsMetadataToIpfsAsync(metadataNFT, metadataNFT.ProductId + ".json");

            var addressToRegisterOwnership = "0x465685122734334d2486D48bc340898BC314f71a";

            //Adding the product information
            var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(metadataNFT.ProductId,
                 "ipfs://" + metadataIpfs.Hash);

            var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, metadataNFT.ProductId, stockHardDrive, new byte[] { });
        }

        [Fact]
        public async void CreateNftAndSetForSale()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //creating a new service with the new contract address
            var erc1155Service = new MyERC1155Service(web3, "0x92fd8Fe190a149685164Cf60C6188Aff90993277");

            //uploading to ipfs our documents
            var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: "2OyEUF7r4netkvcqOQNPkAkyVHQ", password: "");
            var imageIpfs = await nftIpfsService.AddFileToIpfsAsync("ShopImages/hard-drive-by-vincent-botta-from-unsplash.jpg");


            //adding all our document ipfs links to the metadata and the description
            var metadataNFT = new ProductNFTMetadata()
            {
                ProductId = 666,
                Name = "wakawaka",
                Image = "ipfs://" + imageIpfs.Hash, //The image is what is displayed in market places like opean sea
                Description = @"sraka",
                ExternalUrl = "https://github.com/Nethereum/ERC721ContractLibrary.Template",
                Decimals = 0
            };
            var stockHardDrive = 30;
            //Adding the metadata to ipfs
            var metadataIpfs =
                await nftIpfsService.AddNftsMetadataToIpfsAsync(metadataNFT, metadataNFT.ProductId + ".json");

            var addressToRegisterOwnership = "0x465685122734334d2486D48bc340898BC314f71a";

            //Adding the product information
            var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(metadataNFT.ProductId,
                 "ipfs://" + metadataIpfs.Hash);

            var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, metadataNFT.ProductId, stockHardDrive, new byte[] { });

            var setForSaleResult = erc1155Service.SetTokenForSaleStatusAsync(metadataNFT.ProductId, 10000000, true, "sraka");
        }

        [Fact]
        public async void GetAllTokens()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //creating a new service with the new contract address
            var erc1155Service = new MyERC1155Service(web3, "0x92fd8Fe190a149685164Cf60C6188Aff90993277");
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
                }
                // listing for sale
                var tokenData = await erc1155Service.GetTokenDataAsync(log.Event.TokenId);
                var setForSaleResult = erc1155Service.SetTokenForSaleStatusAsync(log.Event.TokenId, 10, true, "sraka");
                tokenData = await erc1155Service.GetTokenDataAsync(log.Event.TokenId);
                // TODO: test buy 
            }

        }


        public class ProductNFTMetadata : NFT1155Metadata
        {
           public int ProductId { get; set; }
        }

    }
}
