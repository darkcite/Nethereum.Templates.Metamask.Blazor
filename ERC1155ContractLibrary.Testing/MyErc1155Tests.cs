using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ERC1155ContractLibrary.Contracts.MyERC1155;
using ERC1155ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using Nethereum.Contracts.Standards.ERC1155;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.Hex.HexTypes;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.XUnitEthereumClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using System.IO;

namespace ERC1155ContractLibrary.Testing
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class MyErc1155Test
    {
        private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

        private readonly string _contractId;
        private readonly HexBigInteger _deploymentBlockNumber;
        private readonly string infU = "";
        private readonly string infP = "";

        public MyErc1155Test(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
        {
            _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;

            string appsettingsTestJsonPath = "appsettings.test.json";

            var appsettingstest = JObject.Parse(File.ReadAllText(appsettingsTestJsonPath));

            //try
            //{
            _contractId = appsettingstest["ContractAddress"].Value<string>();
            _deploymentBlockNumber = new HexBigInteger(BigInteger.Parse(appsettingstest["DeploymentBlockNumber"].Value<string>()));
            //}
            //catch
            //{ }
        }
    
        [Fact]
        public async void ShouldDeployAndMintoken()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var ercERC1155Deployment = new MyERC1155Deployment();
            var deploymentReceipt = await MyERC1155Service.DeployContractAndWaitForReceiptAsync(web3, ercERC1155Deployment);

            string appsettingsTestJsonPath = "appsettings.test.json";
            var appsettingstest = JObject.Parse(File.ReadAllText(appsettingsTestJsonPath));
            appsettingstest["ContractAddress"] = deploymentReceipt.ContractAddress;
            appsettingstest["DeploymentBlockNumber"] = deploymentReceipt.BlockNumber.Value.ToString();

            string appsettingsJsonPath = "../../../../Marketplace.Wasm/wwwroot/appsettings.json";
            var appsettings = JObject.Parse(File.ReadAllText(appsettingsJsonPath));
            ((JObject)appsettings["Ethereum"])["ContractAddress"] = deploymentReceipt.ContractAddress;
            ((JObject)appsettings["Ethereum"])["DeploymentBlockNumber"] = deploymentReceipt.BlockNumber.Value.ToString();
            File.WriteAllText(appsettingsJsonPath, appsettings.ToString());

            var erc1155Service = new MyERC1155Service(web3, deploymentReceipt.ContractAddress);
            var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: infU, password: infP);

            var addressToRegisterOwnership = "0x3C3D1822Fff0DdcB26A8C7FdD17472834bd5855E";

            string[] files = Directory.GetFiles("ShopImages/");

            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var imageIpfs = await nftIpfsService.AddFileToIpfsAsync(file);

                var metadataNFT = new ProductNFTMetadata()
                {
                    ProductId = i, // Using index as ProductId
                    Name = Path.GetFileNameWithoutExtension(file),
                    Image = "ipfs://" + imageIpfs.Hash,
                    Description = $"Gem {i} - {Path.GetFileNameWithoutExtension(file)}", // Using index and file name as Description
                    ExternalUrl = "",
                    Decimals = 0
                };

                var howManyTokensOfThisTypeToMint = 1;
                var metadataIpfs = await nftIpfsService.AddNftsMetadataToIpfsAsync(metadataNFT, metadataNFT.ProductId + ".json");

                var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(metadataNFT.ProductId, "ipfs://" + metadataIpfs.Hash);
                var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, metadataNFT.ProductId, howManyTokensOfThisTypeToMint, new byte[] { });

                var balance = await erc1155Service.BalanceOfQueryAsync(addressToRegisterOwnership, (BigInteger)metadataNFT.ProductId);
                Assert.Equal(howManyTokensOfThisTypeToMint, balance);
                var addressOfToken = await erc1155Service.UriQueryAsync(metadataNFT.ProductId);
                Assert.Equal("ipfs://" + metadataIpfs.Hash, addressOfToken);
            }
        }

        [Fact]
        public async void ApproveContractAsOperator()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            var erc1155Service = new MyERC1155Service(web3, _contractId);
            var tokenData = await erc1155Service.GetTokenDataAsync(333);

            var approveCOntractAsOperator = await erc1155Service.SetApprovalForAllRequestAndWaitForReceiptAsync(_contractId, true);

            tokenData = await erc1155Service.GetTokenDataAsync(333);
        }

        [Fact]
        public async void BuyNft()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            var erc1155Service = new MyERC1155Service(web3, _contractId);

            var tokenData = await erc1155Service.GetTokenDataAsync(333);
            var oldOwner = erc1155Service.GetOwnerOfTokenAsync(333);
            var result = await erc1155Service.BuyTokenAsync(333, 9000000000000000000);
            tokenData = await erc1155Service.GetTokenDataAsync(333);
            var newOwner = erc1155Service.GetOwnerOfTokenAsync(333);
        }

        [Fact]
        public async void UpdateNft()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //creating a new service with the new contract address
            var erc1155Service = new MyERC1155Service(web3, _contractId);
            // Retrieve logs for the "TokenCreated" event
            var tokenData = await erc1155Service.GetTokenDataAsync(111);

            var setForSaleResult = erc1155Service.SetTokenForSaleStatusAsync(111, 3000000000000000000, true, "srk");

            var approveCOntractAsOperator = await erc1155Service.SetApprovalForAllRequestAndWaitForReceiptAsync(_contractId, true);

            tokenData = await erc1155Service.GetTokenDataAsync(111);
        }

        [Fact]
        public async void CheckOwner()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            var erc1155Service = new MyERC1155Service(web3, _contractId);

            var oldOwner = erc1155Service.GetOwnerOfTokenAsync(333);

            var tokenData = await erc1155Service.GetTokenDataAsync(333);

            var newOwner = erc1155Service.GetOwnerOfTokenAsync(333);
        }

        [Fact]
        public async void CreateNft()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            var erc1155Service = new MyERC1155Service(web3, _contractId);

            //uploading to ipfs our documents
            var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: infU, password: infP);
            var imageIpfs = await nftIpfsService.AddFileToIpfsAsync("ShopImages/1.gif");

            //adding all our document ipfs links to the metadata and the description
            var metadataNFT = new ProductNFTMetadata()
            {
                ProductId = 222,
                Name = "Gem 2",
                Image = "ipfs://" + imageIpfs.Hash, //The image is what is displayed in market places like opean sea
                Description = @"7 CT",
                ExternalUrl = "",
                Decimals = 0
            };
            var howManyTokensOfThisTypeToMint = 1;
            //Adding the metadata to ipfs
            var metadataIpfs =
                await nftIpfsService.AddNftsMetadataToIpfsAsync(metadataNFT, metadataNFT.ProductId + ".json");

            var addressToRegisterOwnership = "0x3C3D1822Fff0DdcB26A8C7FdD17472834bd5855E";
            //Adding the product information
            var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(metadataNFT.ProductId,
                 "ipfs://" + metadataIpfs.Hash);

            var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, metadataNFT.ProductId, howManyTokensOfThisTypeToMint, new byte[] { });
        }

        [Fact]
        public async void GetAllTokens()
        {
            var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
            //example of configuration as legacy (not eip1559) to work on L2s
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var erc1155Service = new MyERC1155Service(web3, _contractId);
            // Retrieve logs for the "TokenCreated" event
            var filterInput = erc1155Service.GetTokenMintedEvent().CreateFilterInput(
                new BlockParameter(_deploymentBlockNumber),
                BlockParameter.CreateLatest()
            );

            var eventLogs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput);

            // Parse logs and extract the created token IDs
            var decodedLog = erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(eventLogs);
            foreach (var log in decodedLog)
            {
                ///Get Image by tokenId
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

                var tokenData = await erc1155Service.GetTokenDataAsync(log.Event.TokenId);
            }

        }


        public class ProductNFTMetadata : NFT1155Metadata
        {
           public int ProductId { get; set; }
        }

    }
}
