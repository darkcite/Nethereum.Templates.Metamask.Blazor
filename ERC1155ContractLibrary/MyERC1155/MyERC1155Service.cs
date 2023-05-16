using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using ERC1155ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using Nethereum.ABI.Model;
using System.Linq;
using Nethereum.Util;

namespace ERC1155ContractLibrary.Contracts.MyERC1155
{
    public partial class MyERC1155Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MyERC1155Deployment myERC1155Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MyERC1155Deployment>().SendRequestAndWaitForReceiptAsync(myERC1155Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MyERC1155Deployment myERC1155Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MyERC1155Deployment>().SendRequestAsync(myERC1155Deployment);
        }

        public static async Task<MyERC1155Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MyERC1155Deployment myERC1155Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, myERC1155Deployment, cancellationTokenSource);
            return new MyERC1155Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public MyERC1155Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BigInteger id, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
                balanceOfFunction.Id = id;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(BalanceOfBatchFunction balanceOfBatchFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(List<string> accounts, List<BigInteger> ids, BlockParameter blockParameter = null)
        {
            var balanceOfBatchFunction = new BalanceOfBatchFunction();
                balanceOfBatchFunction.Accounts = accounts;
                balanceOfBatchFunction.Ids = ids;
            
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        public Task<string> BurnRequestAsync(BurnFunction burnFunction)
        {
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public Task<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BurnFunction burnFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public Task<string> BurnRequestAsync(string account, BigInteger id, BigInteger value)
        {
            var burnFunction = new BurnFunction();
                burnFunction.Account = account;
                burnFunction.Id = id;
                burnFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public Task<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(string account, BigInteger id, BigInteger value, CancellationTokenSource cancellationToken = null)
        {
            var burnFunction = new BurnFunction();
                burnFunction.Account = account;
                burnFunction.Id = id;
                burnFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public Task<string> BurnBatchRequestAsync(BurnBatchFunction burnBatchFunction)
        {
             return ContractHandler.SendRequestAsync(burnBatchFunction);
        }

        public Task<TransactionReceipt> BurnBatchRequestAndWaitForReceiptAsync(BurnBatchFunction burnBatchFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnBatchFunction, cancellationToken);
        }

        public Task<string> BurnBatchRequestAsync(string account, List<BigInteger> ids, List<BigInteger> values)
        {
            var burnBatchFunction = new BurnBatchFunction();
                burnBatchFunction.Account = account;
                burnBatchFunction.Ids = ids;
                burnBatchFunction.Values = values;
            
             return ContractHandler.SendRequestAsync(burnBatchFunction);
        }

        public Task<TransactionReceipt> BurnBatchRequestAndWaitForReceiptAsync(string account, List<BigInteger> ids, List<BigInteger> values, CancellationTokenSource cancellationToken = null)
        {
            var burnBatchFunction = new BurnBatchFunction();
                burnBatchFunction.Account = account;
                burnBatchFunction.Ids = ids;
                burnBatchFunction.Values = values;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnBatchFunction, cancellationToken);
        }

        public Task<bool> ExistsQueryAsync(ExistsFunction existsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ExistsFunction, bool>(existsFunction, blockParameter);
        }

        
        public Task<bool> ExistsQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var existsFunction = new ExistsFunction();
                existsFunction.Id = id;
            
            return ContractHandler.QueryAsync<ExistsFunction, bool>(existsFunction, blockParameter);
        }

        public Task<bool> IsApprovedForAllQueryAsync(IsApprovedForAllFunction isApprovedForAllFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        
        public Task<bool> IsApprovedForAllQueryAsync(string account, string @operator, BlockParameter blockParameter = null)
        {
            var isApprovedForAllFunction = new IsApprovedForAllFunction();
                isApprovedForAllFunction.Account = account;
                isApprovedForAllFunction.Operator = @operator;
            
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        public Task<string> MintRequestAsync(MintFunction mintFunction)
        {
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> MintRequestAsync(string account, BigInteger amount, byte royalty, byte[] data)
        {
            var mintFunction = new MintFunction();
                mintFunction.Account = account;
                mintFunction.Amount = amount;
                mintFunction.Royalty = royalty;
                mintFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(string account, BigInteger amount, byte royalty, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var mintFunction = new MintFunction();
                mintFunction.Account = account;
                mintFunction.Amount = amount;
                mintFunction.Royalty = royalty;
                mintFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> MintBatchRequestAsync(MintBatchFunction mintBatchFunction)
        {
             return ContractHandler.SendRequestAsync(mintBatchFunction);
        }

        public Task<TransactionReceipt> MintBatchRequestAndWaitForReceiptAsync(MintBatchFunction mintBatchFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintBatchFunction, cancellationToken);
        }

        public Task<string> MintBatchRequestAsync(string to, List<BigInteger> amounts, byte[] data)
        {
            var mintBatchFunction = new MintBatchFunction();
                mintBatchFunction.To = to;
                mintBatchFunction.Amounts = amounts;
                mintBatchFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(mintBatchFunction);
        }

        public Task<TransactionReceipt> MintBatchRequestAndWaitForReceiptAsync(string to, List<BigInteger> amounts, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var mintBatchFunction = new MintBatchFunction();
                mintBatchFunction.To = to;
                mintBatchFunction.Amounts = amounts;
                mintBatchFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintBatchFunction, cancellationToken);
        }
        
        public Task<string> PauseRequestAsync(PauseFunction pauseFunction)
        {
             return ContractHandler.SendRequestAsync(pauseFunction);
        }

        public Task<string> PauseRequestAsync()
        {
             return ContractHandler.SendRequestAsync<PauseFunction>();
        }

        public Task<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(PauseFunction pauseFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<PauseFunction>(null, cancellationToken);
        }

        public Task<bool> PausedQueryAsync(PausedFunction pausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(pausedFunction, blockParameter);
        }

        
        public Task<bool> PausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(null, blockParameter);
        }

        public Task<string> SafeBatchTransferFromRequestAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction)
        {
             return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeBatchTransferFromRequestAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
                safeBatchTransferFromFunction.From = from;
                safeBatchTransferFromFunction.To = to;
                safeBatchTransferFromFunction.Ids = ids;
                safeBatchTransferFromFunction.Amounts = amounts;
                safeBatchTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
                safeBatchTransferFromFunction.From = from;
                safeBatchTransferFromFunction.To = to;
                safeBatchTransferFromFunction.Ids = ids;
                safeBatchTransferFromFunction.Amounts = amounts;
                safeBatchTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeTransferFromRequestAsync(SafeTransferFromFunction safeTransferFromFunction)
        {
             return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(SafeTransferFromFunction safeTransferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeTransferFromRequestAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.Id = id;
                safeTransferFromFunction.Amount = amount;
                safeTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.Id = id;
                safeTransferFromFunction.Amount = amount;
                safeTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public Task<string> SetApprovalForAllRequestAsync(SetApprovalForAllFunction setApprovalForAllFunction)
        {
             return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public Task<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(SetApprovalForAllFunction setApprovalForAllFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public Task<string> SetApprovalForAllRequestAsync(string @operator, bool approved)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            
             return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public Task<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(string @operator, bool approved, CancellationTokenSource cancellationToken = null)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public Task<string> SetTokenUriRequestAsync(SetTokenUriFunction setTokenUriFunction)
        {
             return ContractHandler.SendRequestAsync(setTokenUriFunction);
        }

        public Task<TransactionReceipt> SetTokenUriRequestAndWaitForReceiptAsync(SetTokenUriFunction setTokenUriFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTokenUriFunction, cancellationToken);
        }

        public Task<string> SetTokenUriRequestAsync(BigInteger tokenId, string tokenURI)
        {
            var setTokenUriFunction = new SetTokenUriFunction();
                setTokenUriFunction.TokenId = tokenId;
                setTokenUriFunction.TokenURI = tokenURI;
            
             return ContractHandler.SendRequestAsync(setTokenUriFunction);
        }

        public Task<TransactionReceipt> SetTokenUriRequestAndWaitForReceiptAsync(BigInteger tokenId, string tokenURI, CancellationTokenSource cancellationToken = null)
        {
            var setTokenUriFunction = new SetTokenUriFunction();
                setTokenUriFunction.TokenId = tokenId;
                setTokenUriFunction.TokenURI = tokenURI;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTokenUriFunction, cancellationToken);
        }

        public Task<string> SetURIRequestAsync(SetURIFunction setURIFunction)
        {
             return ContractHandler.SendRequestAsync(setURIFunction);
        }

        public Task<TransactionReceipt> SetURIRequestAndWaitForReceiptAsync(SetURIFunction setURIFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setURIFunction, cancellationToken);
        }

        public Task<string> SetURIRequestAsync(string newuri)
        {
            var setURIFunction = new SetURIFunction();
                setURIFunction.Newuri = newuri;
            
             return ContractHandler.SendRequestAsync(setURIFunction);
        }

        public Task<TransactionReceipt> SetURIRequestAndWaitForReceiptAsync(string newuri, CancellationTokenSource cancellationToken = null)
        {
            var setURIFunction = new SetURIFunction();
                setURIFunction.Newuri = newuri;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setURIFunction, cancellationToken);
        }

        public Task<bool> SupportsInterfaceQueryAsync(SupportsInterfaceFunction supportsInterfaceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        
        public Task<bool> SupportsInterfaceQueryAsync(byte[] interfaceId, BlockParameter blockParameter = null)
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
                supportsInterfaceFunction.InterfaceId = interfaceId;
            
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var totalSupplyFunction = new TotalSupplyFunction();
                totalSupplyFunction.Id = id;
            
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

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

        public Task<string> UriQueryAsync(UriFunction uriFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }

        
        public Task<string> UriQueryAsync(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var uriFunction = new UriFunction();
                uriFunction.TokenId = tokenId;
            
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }

        public Event<TokenSoldEventDTO> GetTokenSoldEvent()
        {
            return ContractHandler.GetEvent<TokenSoldEventDTO>();
        }

        public Event<TokenMintedEventDTO> GetTokenMintedEvent()
        {
            return ContractHandler.GetEvent<TokenMintedEventDTO>();
        }

        public async Task<TokenDataOutputDTO> GetTokenDataAsync(BigInteger tokenId)
        {
            var tokenDataFunctionMessage = new TokenDataFunction()
            {
                ReturnValue1 = tokenId,
            };

            return await ContractHandler.QueryDeserializingToObjectAsync<TokenDataFunction, TokenDataOutputDTO>(tokenDataFunctionMessage);
        }

        public async Task<TransactionReceipt> SetTokenForSaleStatusAsync(BigInteger tokenId, BigInteger newPrice, BigInteger quantityForSale, bool newStatus, CancellationTokenSource cancellationToken = null)
        {
            var updateTokenForSaleFunction = new UpdateTokenForSaleFunction
            {
                Id = tokenId,
                NewPrice = newPrice,
                QuantityForSale = quantityForSale,
                NewStatus = newStatus
            };

            return await ContractHandler.SendRequestAndWaitForReceiptAsync(updateTokenForSaleFunction, cancellationToken);
        }


        public async Task<string> BuyTokenAsync(BigInteger tokenId, BigInteger quantity, BigInteger etherAmount)
        {
            var buyTokenFunction = new BuyTokenFunction
            {
                TokenId = tokenId,
                Quantity = quantity,
                AmountToSend = etherAmount
                //GasPrice = Web3.Convert.ToWei(80, UnitConversion.EthUnit.Gwei)
            };

            return await ContractHandler.SendRequestAsync(buyTokenFunction);
        }

        public async Task<byte> QueryRoyaltiesAsync(BigInteger tokenId)
        {
            var royaltiesFunction = new RoyaltiesFunction() { ReturnValue1 = tokenId };
            var royaltiesOutputDTO = await ContractHandler.QueryDeserializingToObjectAsync<RoyaltiesFunction, RoyaltiesOutputDTO>(royaltiesFunction);
            return royaltiesOutputDTO.ReturnValue1;
        }

        public async Task<RoyaltyInfoOutputDTO> QueryRoyaltyInfoAsync(BigInteger tokenId, BigInteger salePrice)
        {
            var royaltyInfoFunction = new RoyaltyInfoFunction() { TokenId = tokenId, SalePrice = salePrice };
            var royaltyInfoOutputDTO = await ContractHandler.QueryDeserializingToObjectAsync<RoyaltyInfoFunction, RoyaltyInfoOutputDTO>(royaltyInfoFunction);
            return royaltyInfoOutputDTO;
        }

        public Event<TokenSaleStatusUpdatedEventDTO> GetTokenSaleStatusUpdatedEvent()
        {
            return ContractHandler.GetEvent<TokenSaleStatusUpdatedEventDTO>();
        }
    }
}
