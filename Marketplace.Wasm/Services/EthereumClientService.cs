using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Threading.Tasks;

namespace Marketplace.Wasm.Services
{
    public class EthereumClientService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly Web3 _web3;
        private readonly IConfiguration _configuration;

        public EthereumClientService(IConfiguration configuration, IJSRuntime jsRuntime)
        {
            _configuration = configuration;
            _jsRuntime = jsRuntime;
            _web3 = BuildWeb3();
        }

        private Web3 BuildWeb3()
        {
            var url = GetUrl();

            if (url != null)
            {
                return new Web3(url);
            }
            else
            {
                throw new Exception("Ethereum client URL not configured");
            }
        }

        private string GetUrl()
        {
            var url = _configuration["Ethereum:ClientUrl"];
            return url;
        }

        public Web3 GetWeb3()
        {
            return _web3;
        }

        // Method to sign a message:
        public async Task<string> SignMessage(string message)
        {
            return await _jsRuntime.InvokeAsync<string>("ethereum.request", new
            {
                method = "eth_sign",
                @params = new object[]
                {
            await _jsRuntime.InvokeAsync<string>("ethereum.selectedAddress"),
            message
                }
            });
        }

        // Method to send a transaction:
        public async Task<string> SendTransaction(TransactionInput transaction)
        {
            return await _jsRuntime.InvokeAsync<string>("ethereum.request", new
            {
                method = "eth_sendTransaction",
                @params = new object[] { transaction }
            });
        }

        // Method to connect MetaMask:
        public async Task<string> ConnectMetamask()
        {
            return await _jsRuntime.InvokeAsync<string>("ethereum.request", new { method = "eth_requestAccounts" });
        }

    }
}
