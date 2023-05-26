﻿using System;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ERC1155ContractLibrary.Contracts.MinterProfile.ContractDefinition
{

    public partial class MinterProfileDeployment : MinterProfileDeploymentBase
    {
        public MinterProfileDeployment() : base(BYTECODE) { }
        public MinterProfileDeployment(string byteCode) : base(byteCode) { }
    }

    public class MinterProfileDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b506107fd806100206000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c806384f96e2b14610046578063d4618aab1461005b578063ef2a876414610086575b600080fd5b61005961005436600461053d565b610099565b005b61006e6100693660046105c5565b6100d8565b60405161007d9392919061063b565b60405180910390f35b61006e6100943660046105c5565b610292565b336000908152602081905260409020806100b38582610707565b50600181016100c28482610707565b50600281016100d18382610707565b5050505050565b6000602081905290815260409020805481906100f39061067e565b80601f016020809104026020016040519081016040528092919081815260200182805461011f9061067e565b801561016c5780601f106101415761010080835404028352916020019161016c565b820191906000526020600020905b81548152906001019060200180831161014f57829003601f168201915b5050505050908060010180546101819061067e565b80601f01602080910402602001604051908101604052809291908181526020018280546101ad9061067e565b80156101fa5780601f106101cf576101008083540402835291602001916101fa565b820191906000526020600020905b8154815290600101906020018083116101dd57829003601f168201915b50505050509080600201805461020f9061067e565b80601f016020809104026020016040519081016040528092919081815260200182805461023b9061067e565b80156102885780601f1061025d57610100808354040283529160200191610288565b820191906000526020600020905b81548152906001019060200180831161026b57829003601f168201915b5050505050905083565b60608060606000806000866001600160a01b03166001600160a01b031681526020019081526020016000206040518060600160405290816000820180546102d89061067e565b80601f01602080910402602001604051908101604052809291908181526020018280546103049061067e565b80156103515780601f1061032657610100808354040283529160200191610351565b820191906000526020600020905b81548152906001019060200180831161033457829003601f168201915b5050505050815260200160018201805461036a9061067e565b80601f01602080910402602001604051908101604052809291908181526020018280546103969061067e565b80156103e35780601f106103b8576101008083540402835291602001916103e3565b820191906000526020600020905b8154815290600101906020018083116103c657829003601f168201915b505050505081526020016002820180546103fc9061067e565b80601f01602080910402602001604051908101604052809291908181526020018280546104289061067e565b80156104755780601f1061044a57610100808354040283529160200191610475565b820191906000526020600020905b81548152906001019060200180831161045857829003601f168201915b5050509190925250508151602083015160409093015190989297509550909350505050565b634e487b7160e01b600052604160045260246000fd5b600082601f8301126104c157600080fd5b813567ffffffffffffffff808211156104dc576104dc61049a565b604051601f8301601f19908116603f011681019082821181831017156105045761050461049a565b8160405283815286602085880101111561051d57600080fd5b836020870160208301376000602085830101528094505050505092915050565b60008060006060848603121561055257600080fd5b833567ffffffffffffffff8082111561056a57600080fd5b610576878388016104b0565b9450602086013591508082111561058c57600080fd5b610598878388016104b0565b935060408601359150808211156105ae57600080fd5b506105bb868287016104b0565b9150509250925092565b6000602082840312156105d757600080fd5b81356001600160a01b03811681146105ee57600080fd5b9392505050565b6000815180845260005b8181101561061b576020818501810151868301820152016105ff565b506000602082860101526020601f19601f83011685010191505092915050565b60608152600061064e60608301866105f5565b828103602084015261066081866105f5565b9050828103604084015261067481856105f5565b9695505050505050565b600181811c9082168061069257607f821691505b6020821081036106b257634e487b7160e01b600052602260045260246000fd5b50919050565b601f82111561070257600081815260208120601f850160051c810160208610156106df5750805b601f850160051c820191505b818110156106fe578281556001016106eb565b5050505b505050565b815167ffffffffffffffff8111156107215761072161049a565b6107358161072f845461067e565b846106b8565b602080601f83116001811461076a57600084156107525750858301515b600019600386901b1c1916600185901b1785556106fe565b600085815260208120601f198616915b828110156107995788860151825594840194600190910190840161077a565b50858210156107b75787850151600019600388901b60f8161c191681555b5050505050600190811b0190555056fea264697066735822122058369db180f186860b0e0a5a86736c4474315257f460f7ab8437d1043afb71d564736f6c63430008120033";
        public MinterProfileDeploymentBase() : base(BYTECODE) { }
        public MinterProfileDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetUserMetadataFunction : GetUserMetadataFunctionBase { }

    [Function("getUserMetadata", typeof(GetUserMetadataOutputDTO))]
    public class GetUserMetadataFunctionBase : FunctionMessage
    {
        [Parameter("address", "_userAddress", 1)]
        public virtual string UserAddress { get; set; }
    }

    public partial class SetUserMetadataFunction : SetUserMetadataFunctionBase { }

    [Function("setUserMetadata")]
    public class SetUserMetadataFunctionBase : FunctionMessage
    {
        [Parameter("string", "_logoIpfsHash", 1)]
        public virtual string LogoIpfsHash { get; set; }
        [Parameter("string", "_bannerIpfsHash", 2)]
        public virtual string BannerIpfsHash { get; set; }
        [Parameter("string", "_collectionDefinitionIpfsHash", 3)]
        public virtual string CollectionDefinitionIpfsHash { get; set; }
    }

    public partial class UserMetadataFunction : UserMetadataFunctionBase { }

    [Function("userMetadata", typeof(UserMetadataOutputDTO))]
    public class UserMetadataFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetUserMetadataOutputDTO : GetUserMetadataOutputDTOBase { }

    [FunctionOutput]
    public class GetUserMetadataOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("string", "", 2)]
        public virtual string ReturnValue2 { get; set; }
        [Parameter("string", "", 3)]
        public virtual string ReturnValue3 { get; set; }
    }



    public partial class UserMetadataOutputDTO : UserMetadataOutputDTOBase { }

    [FunctionOutput]
    public class UserMetadataOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "logoIpfsHash", 1)]
        public virtual string LogoIpfsHash { get; set; }
        [Parameter("string", "bannerIpfsHash", 2)]
        public virtual string BannerIpfsHash { get; set; }
        [Parameter("string", "collectionDefinitionIpfsHash", 3)]
        public virtual string CollectionDefinitionIpfsHash { get; set; }
    }

}
