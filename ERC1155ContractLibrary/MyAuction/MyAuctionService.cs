using System;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;
using System.Numerics;
using ERC1155ContractLibrary.Contracts.MyAuction.ContractDefinition;
using System.Threading;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace ERC1155ContractLibrary.Contracts.MyAuction
{
	public partial class MyAuctionService
	{
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Web3 web3, MyAuctionDeployment myAuctionDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MyAuctionDeployment>().SendRequestAndWaitForReceiptAsync(myAuctionDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Web3 web3, MyAuctionDeployment myAuctionDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MyAuctionDeployment>().SendRequestAsync(myAuctionDeployment);
        }

        public static async Task<MyAuctionService> DeployContractAndGetServiceAsync(Web3 web3, MyAuctionDeployment myAuctionDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, myAuctionDeployment, cancellationTokenSource);
            return new MyAuctionService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public MyAuctionService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        //==================

        public async Task<AuctionsOutputDTO> GetAuctionsAsync(BigInteger returnValue1)
        {
            var auctionsFunctionMessage = new AuctionsFunction()
            {
                ReturnValue1 = returnValue1,
            };

            return await ContractHandler.QueryDeserializingToObjectAsync<AuctionsFunction, AuctionsOutputDTO>(auctionsFunctionMessage);
        }

        //==================

        public Task<string> BidRequestAsync(BidFunction bidFunction)
        {
            return ContractHandler.SendRequestAsync(bidFunction);
        }

        public Task<TransactionReceipt> BidRequestAndWaitForReceiptAsync(BidFunction bidFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(bidFunction, cancellationToken);
        }

        public Task<string> BidRequestAsync(BigInteger tokenId)
        {
            var bidFunction = new BidFunction();
            bidFunction.TokenId = tokenId;

            return ContractHandler.SendRequestAsync(bidFunction);
        }

        public Task<TransactionReceipt> BidRequestAndWaitForReceiptAsync(BigInteger tokenId, CancellationTokenSource cancellationToken = null)
        {
            var bidFunction = new BidFunction();
            bidFunction.TokenId = tokenId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(bidFunction, cancellationToken);
        }

        //==================

        public Task<string> CancelAuctionRequestAsync(CancelAuctionFunction cancelAuctionFunction)
        {
            return ContractHandler.SendRequestAsync(cancelAuctionFunction);
        }

        public Task<TransactionReceipt> CancelAuctionRequestAndWaitForReceiptAsync(CancelAuctionFunction cancelAuctionFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelAuctionFunction, cancellationToken);
        }

        public Task<string> CancelAuctionRequestAsync(BigInteger tokenId)
        {
            var cancelAuctionFunction = new CancelAuctionFunction();
            cancelAuctionFunction.TokenId = tokenId;

            return ContractHandler.SendRequestAsync(cancelAuctionFunction);
        }

        public Task<TransactionReceipt> CancelAuctionRequestAndWaitForReceiptAsync(BigInteger tokenId, CancellationTokenSource cancellationToken = null)
        {
            var cancelAuctionFunction = new CancelAuctionFunction();
            cancelAuctionFunction.TokenId = tokenId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelAuctionFunction, cancellationToken);
        }

        //==================

        public Task<string> EndAuctionRequestAsync(EndAuctionFunction endAuctionFunction)
        {
            return ContractHandler.SendRequestAsync(endAuctionFunction);
        }

        public Task<TransactionReceipt> EndAuctionRequestAndWaitForReceiptAsync(EndAuctionFunction endAuctionFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(endAuctionFunction, cancellationToken);
        }

        public Task<string> EndAuctionRequestAsync(BigInteger tokenId)
        {
            var endAuctionFunction = new EndAuctionFunction();
            endAuctionFunction.TokenId = tokenId;

            return ContractHandler.SendRequestAsync(endAuctionFunction);
        }

        public Task<TransactionReceipt> EndAuctionRequestAndWaitForReceiptAsync(BigInteger tokenId, CancellationTokenSource cancellationToken = null)
        {
            var endAuctionFunction = new EndAuctionFunction();
            endAuctionFunction.TokenId = tokenId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(endAuctionFunction, cancellationToken);
        }

        //==================

        public Task<string> GetAuctionStatusQueryAsync(GetAuctionStatusFunction getAuctionStatusFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetAuctionStatusFunction, string>(getAuctionStatusFunction, blockParameter);
        }

        public Task<string> GetAuctionStatusQueryAsync(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var getAuctionStatusFunction = new GetAuctionStatusFunction();
            getAuctionStatusFunction.TokenId = tokenId;

            return ContractHandler.QueryAsync<GetAuctionStatusFunction, string>(getAuctionStatusFunction, blockParameter);
        }

        //==================

        public Task<string> UnpauseRequestAsync(UnpauseFunction unpauseFunction)
        {
            return ContractHandler.SendRequestAsync(unpauseFunction);
        }

        public Task<string> UnpauseRequestAsync()
        {
            return ContractHandler.SendRequestAsync<UnpauseFunction>();
        }

        public Task<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(UnpauseFunction unpauseFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(unpauseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<UnpauseFunction>(null, cancellationToken);
        }

        //==================

        public Task<string> TokenQueryAsync(TokenFunction tokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFunction, string>(tokenFunction, blockParameter);
        }

        public Task<string> TokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFunction, string>(null, blockParameter);
        }

        //==================

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
            return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;

            return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        //==================

        public Task<bool> WithdrawQueryAsync(WithdrawFunction withdrawFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WithdrawFunction, bool>(withdrawFunction, blockParameter);
        }

        public Task<bool> WithdrawQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WithdrawFunction, bool>(null, blockParameter);
        }

        //==================

        public Event<AuctionCanceledEventDTO> GetAuctionCanceledEvent()
        {
            return ContractHandler.GetEvent<AuctionCanceledEventDTO>();
        }

        //==================

        public Event<AuctionEndedEventDTO> GetAuctionEndedEvent()
        {
            return ContractHandler.GetEvent<AuctionEndedEventDTO>();
        }

        //==================

        public Event<AuctionStartedEventDTO> GetAuctionStartedEvent()
        {
            return ContractHandler.GetEvent<AuctionStartedEventDTO>();
        }

        //==================

        public Event<NewHighestBidEventDTO> GetNewHighestBidEvent()
        {
            return ContractHandler.GetEvent<NewHighestBidEventDTO>();
        }

        //==================

        public Event<OwnershipTransferredEventDTO> GetOwnershipTransferredEvent()
        {
            return ContractHandler.GetEvent<OwnershipTransferredEventDTO>();
        }


        //==================

        public Event<PausedEventDTO> GetPausedEvent()
        {
            return ContractHandler.GetEvent<PausedEventDTO>();
        }

        //==================

        public Event<ReservePriceSetEventDTO> GetReservePriceSetEvent()
        {
            return ContractHandler.GetEvent<ReservePriceSetEventDTO>();
        }

        //==================

        public Event<UnpausedEventDTO> GetUnpausedEvent()
        {
            return ContractHandler.GetEvent<UnpausedEventDTO>();
        }
    }
}

