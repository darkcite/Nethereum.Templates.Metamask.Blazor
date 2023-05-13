using Nethereum.Contracts.Standards.ERC1155;

namespace Marketplace.Wasm.Models
{
	public class NFTMetadata : NFT1155Metadata
	{
        public int ProductId { get; set; }
    }
}

