using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Numerics;

namespace Marketplace.Wasm.Services
{
    public class MetaMaskService
    {
        private readonly IJSRuntime _jsRuntime;

        public MetaMaskService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetUserAccountAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("ethereum.request", "eth_requestAccounts");
        }

        public ValueTask<string> SendTransaction(string to, string data)
        {
            // Call JavaScript function to interact with Metamask
            return _jsRuntime.InvokeAsync<string>("sendTransaction", to, data);
        }

        public ValueTask<string> SendTransaction(string to, string data, BigInteger value)
        {
            // Convert the value to a hexadecimal string
            var hexValue = "0x" + value.ToString("X");

            // Call JavaScript function to interact with Metamask
            return _jsRuntime.InvokeAsync<string>("sendTransactionWithValue", to, data, hexValue);
        }
    }
}
