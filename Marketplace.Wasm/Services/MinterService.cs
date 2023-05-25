using System;
using ERC1155ContractLibrary.Contracts.MyERC1155;
using Microsoft.Extensions.Configuration;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using System.Numerics;
using ERC1155ContractLibrary.Contracts.MyERC1155.ContractDefinition;
using System.Threading.Tasks;
using System.Data;
using Nethereum.Util;
using System.Text;
using Org.BouncyCastle.Asn1.Cms;
using Nethereum.Contracts.ContractHandlers;

namespace Marketplace.Wasm.Services
{
	public class MinterService
	{
        private readonly EthereumClientService _ethereumClientService;
        private readonly MyERC1155Service _erc1155Service;
        private readonly MetaMaskService _metaMaskService;

        private readonly IConfiguration _configuration;
        
        private readonly string _contractAddress;
        private readonly HexBigInteger _deploymentBlockNumber;
        private readonly Web3 _web3;
        

        public MinterService(EthereumClientService ethereumClientService, IConfiguration configuration, MetaMaskService metaMaskService)
		{
            _ethereumClientService = ethereumClientService;
            _configuration = configuration;
            _metaMaskService = metaMaskService;

            _web3 = _ethereumClientService.GetWeb3();
            _web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            _contractAddress = _configuration.GetValue<string>("Ethereum:ContractAddress");
            _deploymentBlockNumber = new HexBigInteger(BigInteger.Parse(_configuration.GetValue<string>("Ethereum:DeploymentBlockNumber")));

            _erc1155Service = new MyERC1155Service(_web3, _contractAddress);
        }

        public async Task<bool> HasMinterRoleAsync(string account)
        {
            var hasRoleFunction = new HasRoleFunction();
            hasRoleFunction.Role = GetRoleBytes("MINTER_ROLE");
            hasRoleFunction.Account = account;
            var hasRoleFunctionReturn = await _erc1155Service.ContractHandler.QueryAsync<HasRoleFunction, bool>(hasRoleFunction);

            return hasRoleFunctionReturn;
        }

        public byte[] GetRoleBytes(string role)
        {
            var sha3Keccack = new Sha3Keccack();
            return sha3Keccack.CalculateHash(Encoding.UTF8.GetBytes(role));
        }
        //public async Task BuyMinterRoleAsync()
        //{
        //    var BuyMinterRoleFunction = _erc1155Service.ContractHandler.GetFunction<BuyMinterRoleFunction>();

        //    var BuyMinterRoleFunctionMessage = new BuyMinterRoleFunction
        //    {
        //        AmountToSend = BigInteger.Parse("20000000000000000000")
        //    };

        //    var BuyMinterRoleData = BuyMinterRoleFunction.GetData(BuyMinterRoleFunctionMessage);

        //    // Send the transaction through Metamask
        //    var BuyMinterRoleTxHash = await _metaMaskService.SendTransaction(_contractAddress, BuyMinterRoleData);

        //    // Wait for the transaction to be mined and get the receipt
        //    var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(BuyMinterRoleTxHash);

        //    if (receipt.Status.Value == 0)
        //    {
        //        throw new Exception("Failed to buy minter role");
        //    }
        //}
    }
}

