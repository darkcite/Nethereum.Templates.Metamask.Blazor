using System;
using System.Numerics;

namespace Marketplace.Wasm.Models
{
    public class UpdateNftModel
    {
        public BigInteger Price { get; set; }
        public bool ForSale { get; set; }
        public string ContactInfo { get; set; }
    }
}

