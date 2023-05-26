using System.Threading.Tasks;
using Nethereum.Web3;
using ERC1155ContractLibrary.Contracts;
using ERC1155ContractLibrary.Contracts.MinterProfile.ContractDefinition;
using System;
using Microsoft.Extensions.Configuration;
using Nethereum.Contracts.ContractHandlers;

namespace Marketplace.Wasm.Services
{
    public class MinterProfileService
    {
        private readonly Web3 _web3;
        private readonly MetaMaskService _metaMaskService;
        private readonly EthereumClientService _ethereumClientService;
        private readonly string _minterProfileContractAddress;
        private readonly IConfiguration _configuration;
        private readonly ContractHandler _contractHandler;

        public MinterProfileService(EthereumClientService ethereumClientService, IConfiguration configuration, MetaMaskService metaMaskService)
        {
            _ethereumClientService = ethereumClientService;
            _configuration = configuration;
            _metaMaskService = metaMaskService;

            _web3 = _ethereumClientService.GetWeb3();
            _web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            _minterProfileContractAddress = _configuration.GetValue<string>("Ethereum:MinterProfileContractAddress");

            _contractHandler = _web3.Eth.GetContractHandler(_minterProfileContractAddress);
        }

        public async Task<GetUserMetadataOutputDTO> GetMinterProfileAsync(string userAddress)
        {
            var getUserMetadataFunction = new GetUserMetadataFunction();
            getUserMetadataFunction.UserAddress = userAddress;
            var getUserMetadataOutputDTO = await _contractHandler.QueryDeserializingToObjectAsync<GetUserMetadataFunction, GetUserMetadataOutputDTO>(getUserMetadataFunction);
            return getUserMetadataOutputDTO;
        }

        public async Task SetMinterProfileAsync(string logoIpfsHash, string bannerIpfsHash, string collectionDefinitionIpfsHash)
        {
            var setUserMetadataFunction = _contractHandler.GetFunction<SetUserMetadataFunction>(); //   GetContractFunction<SetUserMetadataFunction>(_minterProfileContractAddress);

            var setUserMetadataFunctionMessage = new SetUserMetadataFunction
            {
                LogoIpfsHash = logoIpfsHash,
                BannerIpfsHash = bannerIpfsHash,
                CollectionDefinitionIpfsHash = collectionDefinitionIpfsHash
            };

            // Encode the function call
            var setData = setUserMetadataFunction.GetData(setUserMetadataFunctionMessage);

            // Send the transaction through Metamask
            var txHash = await _metaMaskService.SendTransaction(_minterProfileContractAddress, setData);

            // Wait for the transaction to be mined and get the receipt
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);

            if (receipt.Status.Value == 0)
            {
                throw new Exception("Failed to update minter profile");
            }
        }
    }
}
