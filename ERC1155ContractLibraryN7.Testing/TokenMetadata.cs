using System;
namespace ERC1155ContractLibraryN7.Testing
{
    public class TokenMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ExternalUrl { get; set; }
        public string BackgroundColor { get; set; }
        public Trait[] Traits { get; set; }
    }

    public class Trait
    {
        public string TraitType { get; set; }
        public string Value { get; set; }
    }
}

