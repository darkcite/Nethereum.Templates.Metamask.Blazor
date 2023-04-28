using System;
namespace NFTMarketplaceContract
{
    public partial class NFTMarketplaceDeployment : NFTMarketplaceDeploymentBase
    {
        public NFTMarketplaceDeployment() : base(BYTECODE) { }
        public NFTMarketplaceDeployment(string byteCode) : base(byteCode) { }
    }
}

