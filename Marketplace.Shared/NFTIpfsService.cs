using Nethereum.Web3;
using Microsoft.AspNetCore.Components.Forms;


namespace Marketplace.Shared
{
    public class NFTIpfsService
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly string _ipfsUrl;

       

        public NFTIpfsService(string ipfsUrl)
        {
            _ipfsUrl = ipfsUrl;
            _userName = "2OyEUF7r4netkvcqOQNPkAkyVHQ";
            _password = "d98294208b5d4302c6fd510a4d4df4c1";
        }

        public NFTIpfsService(string ipfsUrl, string userName, string password) :this(ipfsUrl)
        {
            _userName = userName;
            _password = password;
        }
        public Task<IPFSFileInfo> AddNftsMetadataToIpfsAsync<T>(T metadata, string fileName) where T : TokenMetadata
        {
            var ipfsClient = GetSimpleHttpIpfs();
            return ipfsClient.AddObjectAsJson(metadata, fileName);
        }

        public async Task<IPFSFileInfo> AddFileToIpfsAsync(string path)
        {
            var ipfsClient = GetSimpleHttpIpfs();
            var node = await ipfsClient.AddFileAsync(path);
            return node;

        }

        public async Task<IPFSFileInfo> AddFileToIpfsAsync(IBrowserFile file)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);

            var ipfsClient = GetSimpleHttpIpfs();

            return await ipfsClient.AddAsync(buffer, file.Name);
        }



        private IpfsHttpService GetSimpleHttpIpfs()
        {
            if (_userName == null) return new IpfsHttpService(_ipfsUrl);
            return new IpfsHttpService(_ipfsUrl, _userName, _password);
        }

    }
}
