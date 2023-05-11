using System;
using System.Numerics;
namespace Marketplace.Wasm.Models
{
    public class NFT
    {
        public BigInteger Price { get; set; }
        public int TokenId { get; set; }
        public string Seller { get; set; }
        public string Owner { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TokenURI { get; set; }
        public bool ForSale { get; set; }
        public string ContactInfo { get; set; }
    }

}

