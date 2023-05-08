using Microsoft.Extensions.Configuration;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;

namespace Marketplace.Wasm.Services
{
    public class EthereumClientService
    {
        private readonly Web3 _web3;
        private readonly IConfiguration _configuration;

        public EthereumClientService(IConfiguration configuration)
        {
            _configuration = configuration;
            _web3 = BuildWeb3();
        }

        private Web3 BuildWeb3()
        {
            var url = GetUrl();
            var account = GetAccount();

            if (account != null)
            {
                return new Web3(account, url);
            }
            else
            {
                return new Web3(url);
            }
        }

        private string GetUrl()
        {
            var url = _configuration["Ethereum:ClientUrl"];
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("Ethereum client URL not configured");
            }
            return url;
        }

        private Account GetAccount()
        {
            var privateKey = _configuration["Ethereum:PrivateKey"];
            if (string.IsNullOrEmpty(privateKey))
            {
                return null;
            }
            return new Account(privateKey);
        }

        public Web3 GetWeb3()
        {
            return _web3;
        }
    }
}
