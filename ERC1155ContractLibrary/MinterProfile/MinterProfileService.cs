using System;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using System.Threading;
using System.Threading.Tasks;
using ERC1155ContractLibrary.Contracts.MinterProfile.ContractDefinition;

namespace ERC1155ContractLibrary.Contracts.MinterProfile
{
	public partial class MinterProfileService
	{
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MinterProfileDeployment minterProfileDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MinterProfileDeployment>().SendRequestAndWaitForReceiptAsync(minterProfileDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MinterProfileDeployment minterProfileDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MinterProfileDeployment>().SendRequestAsync(minterProfileDeployment);
        }

        public static async Task<MinterProfileService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MinterProfileDeployment minterProfileDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, minterProfileDeployment, cancellationTokenSource);
            return new MinterProfileService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public MinterProfileService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}

