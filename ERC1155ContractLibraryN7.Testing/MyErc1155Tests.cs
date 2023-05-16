using System.Numerics;
using ERC1155ContractLibrary.Contracts.MyERC1155;
using ERC1155ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using ERC1155ContractLibrary.Contracts.MyAuction;
using ERC1155ContractLibrary.Contracts.MyAuction.ContractDefinition;
using Nethereum.Contracts.Standards.ERC1155;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.XUnitEthereumClients;
using Newtonsoft.Json.Linq;
using Nethereum.Model;
using Nethereum.RPC.NonceServices;
using Marketplace.Shared;

namespace ERC1155ContractLibraryN7.Testing;

[Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
public class MyErc1155Test
{
    private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

    private readonly string _contractId;
    private readonly string _auctionContractId;
    private readonly HexBigInteger _deploymentBlockNumber;
    private readonly string infU = "";
    private readonly string infP = "";

    /// <summary>
    /// Test properties
    /// </summary>
    private readonly byte royalty = 10;
    private readonly BigInteger howManyTokensOfThisTypeToMint = 1;
    private readonly string addressToRegisterOwnership = "0x6F637f5f49a5A10684a9eeC1Ea5ffef025413509";
    //private readonly BigInteger userDefinedTokenId = 666;

    public MyErc1155Test(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
    {
        _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;

        string appsettingsTestJsonPath = "appsettings.test.json";

        var appsettingstest = JObject.Parse(File.ReadAllText(appsettingsTestJsonPath));

        _contractId = appsettingstest["ContractAddress"].Value<string>();
        _auctionContractId = appsettingstest["AuctionContractAddress"].Value<string>();
        _deploymentBlockNumber = new HexBigInteger(BigInteger.Parse(appsettingstest["DeploymentBlockNumber"].Value<string>()));
    }

    [Fact]
    public async void ShouldDeployAndMintoken()
    {
        var web3 = _ethereumClientIntegrationFixture.GetWeb3();
        web3.Eth.TransactionManager.UseLegacyAsDefault = true;

        var ercERC1155Deployment = new MyERC1155Deployment();
        var deploymentReceipt = await MyERC1155Service.DeployContractAndWaitForReceiptAsync(web3, ercERC1155Deployment);

        var myAuctionDeployment = new MyAuctionDeployment { TokenAddress = deploymentReceipt.ContractAddress };
        var auctionDeploymentReceipt = await MyAuctionService.DeployContractAndWaitForReceiptAsync(web3, myAuctionDeployment);


        string appsettingsTestJsonPath = "../../../appsettings.test.json";
        var appsettingstest = JObject.Parse(File.ReadAllText(appsettingsTestJsonPath));
        appsettingstest["ContractAddress"] = deploymentReceipt.ContractAddress;
        appsettingstest["AuctionContractAddress"] = auctionDeploymentReceipt.ContractAddress;
        appsettingstest["DeploymentBlockNumber"] = deploymentReceipt.BlockNumber.Value.ToString();
        File.WriteAllText(appsettingsTestJsonPath, appsettingstest.ToString());

        string appsettingsJsonPath = "../../../../Marketplace.Wasm/wwwroot/appsettings.json";
        var appsettings = JObject.Parse(File.ReadAllText(appsettingsJsonPath));
        ((JObject)appsettings["Ethereum"])["ContractAddress"] = deploymentReceipt.ContractAddress;
        ((JObject)appsettings["Ethereum"])["AuctionContractAddress"] = auctionDeploymentReceipt.ContractAddress;
        ((JObject)appsettings["Ethereum"])["DeploymentBlockNumber"] = deploymentReceipt.BlockNumber.Value.ToString();
        File.WriteAllText(appsettingsJsonPath, appsettings.ToString());

        var erc1155Service = new MyERC1155Service(web3, deploymentReceipt.ContractAddress);
        var auctionService = new MyAuctionService(web3, auctionDeploymentReceipt.ContractAddress);
        var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: infU, password: infP);

        string[] files = Directory.GetFiles("ShopImages/");

        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var imageIpfs = await nftIpfsService.AddFileToIpfsAsync(file);

            //var //userDefinedTokenIid = i+1;

            var tokenMetadata = new TokenMetadata()
            {
                Name = Path.GetFileNameWithoutExtension(file),
                Image = "ipfs://" + imageIpfs.Hash,
                Description = $"Gem {i} - {Path.GetFileNameWithoutExtension(file)}", // Using index and file name as Description
                ExternalUrl = "",
                BackgroundColor = "#FFFFFF",
                Traits = new Trait[] { }
            };
                      
            var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, howManyTokensOfThisTypeToMint, royalty, new byte[] { });
            var justMintedTokenId =  erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(mintReceipt.Logs).FirstOrDefault().Event.TokenId;

            var metadataIpfs = await nftIpfsService.AddNftsMetadataToIpfsAsync(tokenMetadata, justMintedTokenId + ".json");
            var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(justMintedTokenId, "ipfs://" + metadataIpfs.Hash);

            var tokenData = await erc1155Service.GetTokenDataAsync(justMintedTokenId);

            var balance = await erc1155Service.BalanceOfQueryAsync(addressToRegisterOwnership, justMintedTokenId);
            Assert.Equal(howManyTokensOfThisTypeToMint, balance);
            var addressOfToken = await erc1155Service.UriQueryAsync(justMintedTokenId);
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

        //var nonce = await web3.TransactionManager.Account.NonceService.GetNextNonceAsync();
        //var contractNonce = await erc1155Service.GetUserNonceAsync(web3.TransactionManager.Account.Address);

        var tokenData = await erc1155Service.GetTokenDataAsync(2);
        //var oldOwner = erc1155Service.GetOwnerOfTokenAsync(2);
        //await Task.Delay(TimeSpan.FromSeconds(3));
        var result = await erc1155Service.BuyTokenAsync(2, 1, 3000000000000000000); //9000000000000000000
        var updtE = erc1155Service.GetTokenSaleStatusUpdatedEvent();

        await Task.Delay(TimeSpan.FromSeconds(3));
        tokenData = await erc1155Service.GetTokenDataAsync(2);
        //var newOwner = erc1155Service.GetOwnerOfTokenAsync(2);
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

        var setForSaleResult = erc1155Service.SetTokenForSaleStatusAsync(111, 3000000000000000000, 1, true);

        var approveCOntractAsOperator = await erc1155Service.SetApprovalForAllRequestAndWaitForReceiptAsync(_contractId, true);
    }

    [Fact]
    public async void CheckOwner()
    {
        var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
        web3.Eth.TransactionManager.UseLegacyAsDefault = true;
        var erc1155Service = new MyERC1155Service(web3, _contractId);

        //var oldOwner = erc1155Service.GetOwnerOfTokenAsync(333);

        var tokenData = await erc1155Service.GetTokenDataAsync(333);
        
        //var newOwner = erc1155Service.GetOwnerOfTokenAsync(333);
    }

    [Fact]
    public async void CreateNft()
    {
        var addressToRegisterOwnership = "0x3C3D1822Fff0DdcB26A8C7FdD17472834bd5855E";

        var web3 = _ethereumClientIntegrationFixture.GetWeb3(); //if you want to use your local node (ie geth, uncomment this, see appsettings.test.json for further info)
                                                                //example of configuration as legacy (not eip1559) to work on L2s
        web3.Eth.TransactionManager.UseLegacyAsDefault = true;
        var erc1155Service = new MyERC1155Service(web3, _contractId);

        //uploading to ipfs our documents
        var nftIpfsService = new NFTIpfsService("https://ipfs.infura.io:5001", userName: infU, password: infP);
        var imageIpfs = await nftIpfsService.AddFileToIpfsAsync("ShopImages/14 Ct.gif");

        //adding all our document ipfs links to the metadata and the description

        var tokenMetadata = new TokenMetadata()
        {
            Name = "",
            Image = "ipfs://" + imageIpfs.Hash,
            Description = "sraka",
            ExternalUrl = "",
            BackgroundColor = "#FFFFFF",
            Traits = new Trait[] { }
        };

        var mintReceipt = await erc1155Service.MintRequestAndWaitForReceiptAsync(addressToRegisterOwnership, howManyTokensOfThisTypeToMint, royalty, new byte[] { });
        var justMintedTokenId = erc1155Service.GetTokenMintedEvent().DecodeAllEventsForEvent(mintReceipt.Logs).FirstOrDefault().Event.TokenId;
        //Adding the metadata to ipfs
        var metadataIpfs =
            await nftIpfsService.AddNftsMetadataToIpfsAsync(tokenMetadata, justMintedTokenId + ".json");

        

        

        var tokenUriReceipt = await erc1155Service.SetTokenUriRequestAndWaitForReceiptAsync(justMintedTokenId,
             "ipfs://" + metadataIpfs.Hash);

        
        await Task.Delay(TimeSpan.FromSeconds(30));
        var tokenData = await erc1155Service.GetTokenDataAsync(justMintedTokenId);

        var balance = await erc1155Service.BalanceOfQueryAsync(addressToRegisterOwnership, (BigInteger)justMintedTokenId);
        Assert.Equal(howManyTokensOfThisTypeToMint, balance);
        var addressOfToken = await erc1155Service.UriQueryAsync(justMintedTokenId);
        Assert.Equal("ipfs://" + metadataIpfs.Hash, addressOfToken);
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


    //public class ProductNFTMetadata : NFT1155Metadata
    //{
    //    public int ProductId { get; set; }
    //}

}
