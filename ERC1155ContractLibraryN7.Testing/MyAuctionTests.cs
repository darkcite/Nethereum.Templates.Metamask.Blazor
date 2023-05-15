using System;
using Nethereum.Hex.HexTypes;
using Nethereum.XUnitEthereumClients;
using Newtonsoft.Json.Linq;
using System.Numerics;
using ERC1155ContractLibrary.Contracts.MyAuction;
using Nethereum.Util;
using Nethereum.Web3;
using System.Security.Cryptography;
using ERC1155ContractLibrary.Contracts.MyERC1155;

namespace ERC1155ContractLibraryN7.Testing
{

    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class MyAuctionTest
    {
        private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

        private readonly string _contractId;
        private readonly string _auctionContractId;
        private readonly HexBigInteger _deploymentBlockNumber;



        private readonly string addressToRegisterOwnership = "0x6F637f5f49a5A10684a9eeC1Ea5ffef025413509";

        public MyAuctionTest(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
        {
            _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;

            string appsettingsTestJsonPath = "appsettings.test.json";

            var appsettingstest = JObject.Parse(File.ReadAllText(appsettingsTestJsonPath));

            _contractId = appsettingstest["ContractAddress"].Value<string>();
            _auctionContractId = appsettingstest["AuctionContractAddress"].Value<string>();
            _deploymentBlockNumber = new HexBigInteger(BigInteger.Parse(appsettingstest["DeploymentBlockNumber"].Value<string>()));
        }

        [Fact]
        public async Task StartAuctionAndBidAndEndTest()
        {
            var tokenId = 1;
            var duration = new BigInteger(3600);
            var reservePrice = new BigInteger(1000000000000000000);
            var bid = new BigInteger(2000000000000000000);
            var senderAddress = addressToRegisterOwnership;  // The address of the sender

            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;



            var auctionService = new MyAuctionService(web3, _auctionContractId);
            var erc1155Service = new MyERC1155Service(web3, _contractId);

            var tokenData = await erc1155Service.GetTokenDataAsync(tokenId);
            var oldOwner = erc1155Service.GetOwnerOfTokenAsync(tokenId);
            var balance = await erc1155Service.BalanceOfQueryAsync(senderAddress, (BigInteger)tokenId);

            // Start the auction
            var startResult = await auctionService.StartAuction(tokenId, duration, reservePrice);
            //Assert.IsTrue(startResult.);

            // Place a bid
            var bidResult = await auctionService.BidRequestAndWaitForReceiptAsync(tokenId, bid);

            //Assert.IsTrue(bidResult.Success);

            var endResult = await auctionService.EndAuctionRequestAndWaitForReceiptAsync(tokenId);
        }

        [Fact]
        public async Task StartAuctionAndCancelTest()
        {
            var tokenId = 1;
            var duration = new BigInteger(3600);
            var reservePrice = new BigInteger(1000000000000000000);
            var bid = new BigInteger(2000000000000000000);
            var senderAddress = addressToRegisterOwnership;  // The address of the sender

            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var auctionService = new MyAuctionService(web3, _auctionContractId);
            // Start the auction
            var startResult = await auctionService.StartAuction(tokenId, duration, reservePrice);
            //Assert.IsTrue(startResult.);

            //Assert.IsTrue(bidResult.Success);

            var endResult = await auctionService.EndAuctionRequestAndWaitForReceiptAsync(tokenId);
        }

        [Fact]
        public async Task ShouldPauseAndUnpauseContractSuccessfully()
        {
            // Arrange
            // Replace these with actual values
            BigInteger tokenId = new BigInteger(1);
            BigInteger duration = new BigInteger(604800); // 1 week in seconds
            BigInteger reservePrice = Web3.Convert.ToWei(1, UnitConversion.EthUnit.Ether); // 1 Ether

            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var auctionService = new MyAuctionService(web3, _auctionContractId);

            var startResult = await auctionService.StartAuction(tokenId, duration, reservePrice);

            //WTF not implemented?
            //var pauseReceipt = await auctionService..PauseRequestAndWaitForReceiptAsync();

        }

        [Fact]
        public async Task ShouldAllowWithdrawalOfPendingReturnsSuccessfully()
        {
            BigInteger tokenId = new BigInteger(1);
            BigInteger duration = new BigInteger(604800); // 1 week in seconds
            BigInteger reservePrice = Web3.Convert.ToWei(1, UnitConversion.EthUnit.Ether); // 1 Ether

            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            var auctionService = new MyAuctionService(web3, _auctionContractId);

            // Bid on the auction
            var bidAmountInEther = new BigInteger(2000000000000000000); // 2 Ether
            var bidResult = await auctionService.BidRequestAndWaitForReceiptAsync(tokenId, bidAmountInEther);

            // Bid on the auction again with a higher amount
            var higherBidAmountInEther = new BigInteger(3000000000000000000); // 3 Ether
            await auctionService.BidRequestAndWaitForReceiptAsync(tokenId, higherBidAmountInEther);

            // At this point, the first bid should be added to the pending returns of the bidder

            // Check the balance of the bidder before withdrawal
            var balanceBeforeWithdrawal = await web3.Eth.GetBalance.SendRequestAsync(addressToRegisterOwnership);

            // Act - Withdraw the pending returns
            var withdrawReceipt = await auctionService.WithdrawQueryAsync();

            // Assert - Check that the withdraw transaction was successful
            //Assert.NotNull(withdrawReceipt);
            Assert.True(withdrawReceipt);

            // Check the balance of the bidder after withdrawal
            var balanceAfterWithdrawal = await web3.Eth.GetBalance.SendRequestAsync(addressToRegisterOwnership);

            // Assert - Check that the balance increased by the correct amount
            Assert.True(balanceAfterWithdrawal == balanceBeforeWithdrawal + Web3.Convert.ToWei(bidAmountInEther, UnitConversion.EthUnit.Ether));
        }
    }
}
