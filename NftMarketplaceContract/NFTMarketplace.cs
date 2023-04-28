using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace NFTMarketplaceContract
{
    public class NFTMarketplaceConsole
    {
        public static async Task Main()
        {
            /*Configuration config = Configuration.ReadConfig();

            var url = config.Url; //"http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = config.PrivateKey; //"0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";

            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            if(string.IsNullOrEmpty(config.ContractAddress))
            {
                // * Deployment 
                var nFTMarketplaceDeployment = new NFTMarketplaceDeployment();

                var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<NFTMarketplaceDeployment>().SendRequestAndWaitForReceiptAsync(nFTMarketplaceDeployment);
                var contractAddress = transactionReceiptDeployment.ContractAddress;
                //* /
                config.WriteContractAddressToConfig(contractAddress);
            }*/

            /*var contractHandler = web3.Eth.GetContractHandler(config.ContractAddress);*/

            var contractHandler = await new ContractHandlerProvider().GetContractHandlerAsync();

            /** Function: approve**/
            /*
            var approveFunction = new ApproveFunction();
            approveFunction.To = to;
            approveFunction.TokenId = tokenId;
            var approveFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(approveFunction);
            */


            /** Function: balanceOf**/
            /*
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.Owner = owner;
            var balanceOfFunctionReturn = await contractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction);
            */


            /** Function: createMarketSale**/
            /*
            var createMarketSaleFunction = new CreateMarketSaleFunction();
            createMarketSaleFunction.TokenId = tokenId;
            var createMarketSaleFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(createMarketSaleFunction);
            //*/


            /** Function: createToken**/
            /*
            var createTokenFunction = new CreateTokenFunction();
            createTokenFunction.TokenURI = tokenURI;
            createTokenFunction.Price = price;
            var createTokenFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(createTokenFunction);
            */


            /** Function: fetchItemsListed**/
            /*
            var fetchItemsListedOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<FetchItemsListedFunction, FetchItemsListedOutputDTO>();
            */


            /** Function: fetchMarketItems**/
            /*
            var fetchMarketItemsOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<FetchMarketItemsFunction, FetchMarketItemsOutputDTO>();
            */


            /** Function: fetchMyNFTs**/
            /*
            var fetchMyNFTsOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<FetchMyNFTsFunction, FetchMyNFTsOutputDTO>();
            */


            /** Function: getApproved**/
            /*
            var getApprovedFunction = new GetApprovedFunction();
            getApprovedFunction.TokenId = tokenId;
            var getApprovedFunctionReturn = await contractHandler.QueryAsync<GetApprovedFunction, string>(getApprovedFunction);
            */


            /** Function: getListingPrice**/
            /*
            var getListingPriceFunctionReturn = await contractHandler.QueryAsync<GetListingPriceFunction, BigInteger>();
            */


            /** Function: isApprovedForAll**/
            /*
            var isApprovedForAllFunction = new IsApprovedForAllFunction();
            isApprovedForAllFunction.Owner = owner;
            isApprovedForAllFunction.Operator = @operator;
            var isApprovedForAllFunctionReturn = await contractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction);
            */


            /** Function: name**/
            /*
            var nameFunctionReturn = await contractHandler.QueryAsync<NameFunction, string>();
            */


            /** Function: ownerOf**/
            /*
            var ownerOfFunction = new OwnerOfFunction();
            ownerOfFunction.TokenId = tokenId;
            var ownerOfFunctionReturn = await contractHandler.QueryAsync<OwnerOfFunction, string>(ownerOfFunction);
            */


            /** Function: resellToken**/
            /*
            var resellTokenFunction = new ResellTokenFunction();
            resellTokenFunction.TokenId = tokenId;
            resellTokenFunction.Price = price;
            var resellTokenFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(resellTokenFunction);
            */


            /** Function: safeTransferFrom**/
            /*
            var safeTransferFromFunction = new SafeTransferFromFunction();
            safeTransferFromFunction.From = from;
            safeTransferFromFunction.To = to;
            safeTransferFromFunction.TokenId = tokenId;
            var safeTransferFromFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction);
            */


            /** Function: safeTransferFrom**/
            /*
            var safeTransferFrom1Function = new SafeTransferFrom1Function();
            safeTransferFrom1Function.From = from;
            safeTransferFrom1Function.To = to;
            safeTransferFrom1Function.TokenId = tokenId;
            safeTransferFrom1Function.Data = data;
            var safeTransferFrom1FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFrom1Function);
            */


            /** Function: setApprovalForAll**/
            /*
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
            setApprovalForAllFunction.Operator = @operator;
            setApprovalForAllFunction.Approved = approved;
            var setApprovalForAllFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction);
            */


            /** Function: supportsInterface**/
            /*
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
            supportsInterfaceFunction.InterfaceId = interfaceId;
            var supportsInterfaceFunctionReturn = await contractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction);
            */


            /** Function: symbol**/
            /*
            var symbolFunctionReturn = await contractHandler.QueryAsync<SymbolFunction, string>();
            */


            /** Function: tokenURI**/
            /*
            var tokenURIFunction = new TokenURIFunction();
            tokenURIFunction.TokenId = tokenId;
            var tokenURIFunctionReturn = await contractHandler.QueryAsync<TokenURIFunction, string>(tokenURIFunction);
            */


            /** Function: transferFrom**/
            /*
            var transferFromFunction = new TransferFromFunction();
            transferFromFunction.From = from;
            transferFromFunction.To = to;
            transferFromFunction.TokenId = tokenId;
            var transferFromFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction);
            */


            /** Function: updateListingPrice**/
            /*
            var updateListingPriceFunction = new UpdateListingPriceFunction();
            updateListingPriceFunction.ListingPrice = listingPrice;
            var updateListingPriceFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(updateListingPriceFunction);
            */
        }

    }
        
}
