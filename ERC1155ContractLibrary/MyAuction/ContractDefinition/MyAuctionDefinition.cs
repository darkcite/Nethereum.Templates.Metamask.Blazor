﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace ERC1155ContractLibrary.Contracts.MyAuction.ContractDefinition
{
    public partial class MyAuctionDeployment : MyAuctionDeploymentBase
    {
        public MyAuctionDeployment() : base(BYTECODE) { }
        public MyAuctionDeployment(string byteCode) : base(byteCode) { }
    }

    public class MyAuctionDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040523480156200001157600080fd5b50604051620026a1380380620026a1833981810160405281019062000037919062000252565b620000576200004b6200011c60201b60201c565b6200012460201b60201c565b60008060146101000a81548160ff02191690831515021790555080600360006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16600460006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055505062000284565b600033905090565b60008060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050816000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055508173ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a35050565b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b60006200021a82620001ed565b9050919050565b6200022c816200020d565b81146200023857600080fd5b50565b6000815190506200024c8162000221565b92915050565b6000602082840312156200026b576200026a620001e8565b5b60006200027b848285016200023b565b91505092915050565b61240d80620002946000396000f3fe6080604052600436106100f75760003560e01c80638456cb591161008a5780639d76ea58116100595780639d76ea581461032e578063b9a2de3a14610359578063f2fde38b14610382578063fc0c546a146103ab57610137565b80638456cb591461029a5780638da5cb5b146102b157806391e078bb146102dc57806396b5a7551461030557610137565b8063571a26a0116100c6578063571a26a0146101d75780635c975abb1461021b578063715018a6146102465780637765c52c1461025d57610137565b806326b387bb1461013c5780633ccfd60b146101795780633f4ba83a146101a4578063454a2ab3146101bb57610137565b36610137576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161012e90611721565b60405180910390fd5b600080fd5b34801561014857600080fd5b50610163600480360381019061015e91906117a4565b6103d6565b60405161017091906117ea565b60405180910390f35b34801561018557600080fd5b5061018e6103ee565b60405161019b9190611820565b60405180910390f35b3480156101b057600080fd5b506101b961054c565b005b6101d560048036038101906101d09190611867565b61055e565b005b3480156101e357600080fd5b506101fe60048036038101906101f99190611867565b6107bd565b6040516102129897969594939291906118a3565b60405180910390f35b34801561022757600080fd5b50610230610852565b60405161023d9190611820565b60405180910390f35b34801561025257600080fd5b5061025b610868565b005b34801561026957600080fd5b50610284600480360381019061027f9190611867565b61087c565b60405161029191906119a0565b60405180910390f35b3480156102a657600080fd5b506102af61098d565b005b3480156102bd57600080fd5b506102c661099f565b6040516102d391906119c2565b60405180910390f35b3480156102e857600080fd5b5061030360048036038101906102fe91906119dd565b6109c8565b005b34801561031157600080fd5b5061032c60048036038101906103279190611867565b610ce0565b005b34801561033a57600080fd5b50610343610f4e565b60405161035091906119c2565b60405180910390f35b34801561036557600080fd5b50610380600480360381019061037b9190611867565b610f74565b005b34801561038e57600080fd5b506103a960048036038101906103a491906117a4565b611379565b005b3480156103b757600080fd5b506103c06113fc565b6040516103cd9190611a8f565b60405180910390f35b60026020528060005260406000206000915090505481565b600080600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205490506000811115610543576000600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555060003373ffffffffffffffffffffffffffffffffffffffff16826040516104a790611adb565b60006040518083038185875af1925050503d80600081146104e4576040519150601f19603f3d011682016040523d82523d6000602084013e6104e9565b606091505b50509050806105415781600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002081905550600092505050610549565b505b60019150505b90565b610554611422565b61055c6114a0565b565b610566611502565b60006001600083815260200190815260200160002090508060000160009054906101000a900460ff166105ce576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016105c590611b3c565b60405180910390fd5b8060020154421115610615576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161060c90611ba8565b60405180910390fd5b8060030154341161065b576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161065290611c3a565b60405180910390fd5b600073ffffffffffffffffffffffffffffffffffffffff168160050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614610732578060030154600260008360050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600082825461072a9190611c89565b925050819055505b348160030181905550338160050160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055507ff13cb67d4c8841d2c893b242b7d61382d8e3bb53c5c58a8584106e74e8c4721b8233346040516107b193929190611cbd565b60405180910390a15050565b60016020528060005260406000206000915090508060000160009054906101000a900460ff16908060010154908060020154908060030154908060040154908060050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16908060060154908060070160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905088565b60008060149054906101000a900460ff16905090565b610870611422565b61087a600061154c565b565b606060006001600084815260200190815260200160002090508060000160009054906101000a900460ff166108e9576040518060400160405280600b81526020017f4e6f742053746172746564000000000000000000000000000000000000000000815250915050610988565b8060000160009054906101000a900460ff16801561090b575080600201544211155b1561094e576040518060400160405280600781526020017f52756e6e696e6700000000000000000000000000000000000000000000000000815250915050610988565b6040518060400160405280600581526020017f456e6465640000000000000000000000000000000000000000000000000000008152509150505b919050565b610995611422565b61099d611610565b565b60008060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905090565b6109d0611502565b6000600460009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1662fdd58e33866040518363ffffffff1660e01b8152600401610a2e929190611cf4565b602060405180830381865afa158015610a4b573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610a6f9190611d32565b11610aaf576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610aa690611dab565b60405180910390fd5b60008211610af2576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610ae990611e3d565b60405180910390fd5b6040518061010001604052806001151581526020014281526020018342610b199190611c89565b815260200160008152602001828152602001600073ffffffffffffffffffffffffffffffffffffffff1681526020018481526020013373ffffffffffffffffffffffffffffffffffffffff168152506001600085815260200190815260200160002060008201518160000160006101000a81548160ff0219169083151502179055506020820151816001015560408201518160020155606082015181600301556080820151816004015560a08201518160050160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555060c0820151816006015560e08201518160070160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055509050507fd337215810e180ab422a21e08c59d2a33e1012b704ba6c5322f64c26120e474a8382604051610c8f929190611e5d565b60405180910390a17ff8910119ddbef5440c54532457dfe8250a10ed39e583292818f44724b9e1344c838342610cc59190611c89565b604051610cd3929190611e5d565b60405180910390a1505050565b610ce8611502565b60006001600083815260200190815260200160002090508060000160009054906101000a900460ff16610d50576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610d4790611b3c565b60405180910390fd5b8060070160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff161480610de05750610db161099f565b73ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16145b610e1f576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610e1690611f1e565b60405180910390fd5b600073ffffffffffffffffffffffffffffffffffffffff168160050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614610ef6578060030154600260008360050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828254610eee9190611c89565b925050819055505b60008160000160006101000a81548160ff0219169083151502179055507f28601d865dccc9f113e15a7185c1b38c085d598c71250d3337916a428536d77182604051610f4291906117ea565b60405180910390a15050565b600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b610f7c611502565b60006001600083815260200190815260200160002090508060000160009054906101000a900460ff16610fe4576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610fdb90611b3c565b60405180910390fd5b8060020154421161102a576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161102190611f8a565b60405180910390fd5b8060070160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614806110ba575061108b61099f565b73ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16145b6110f9576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016110f09061201c565b60405180910390fd5b806004015481600301541015611144576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161113b906120ae565b60405180910390fd5b600460009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1663f242432a8260070160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff168360050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff168560016040518563ffffffff1660e01b81526004016111ee949392919061213d565b600060405180830381600087803b15801561120857600080fd5b505af115801561121c573d6000803e3d6000fd5b5050505060008160070160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16826003015460405161126e90611adb565b60006040518083038185875af1925050503d80600081146112ab576040519150601f19603f3d011682016040523d82523d6000602084013e6112b0565b606091505b50509050806112f4576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016112eb906121e1565b60405180910390fd5b60008260000160006101000a81548160ff0219169083151502179055507fd2aa34a4fdbbc6dff6a3e56f46e0f3ae2a31d7785ff3487aa5c95c642acea501838360050160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16846003015460405161136c93929190611cbd565b60405180910390a1505050565b611381611422565b600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff16036113f0576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016113e790612273565b60405180910390fd5b6113f98161154c565b50565b600460009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b61142a611673565b73ffffffffffffffffffffffffffffffffffffffff1661144861099f565b73ffffffffffffffffffffffffffffffffffffffff161461149e576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611495906122df565b60405180910390fd5b565b6114a861167b565b60008060146101000a81548160ff0219169083151502179055507f5db9ee0a495bf2e6ff9c91a7834c1ba4fdd244a5e8aa4e537bd38aeae4b073aa6114eb611673565b6040516114f891906119c2565b60405180910390a1565b61150a610852565b1561154a576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016115419061234b565b60405180910390fd5b565b60008060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050816000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055508173ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a35050565b611618611502565b6001600060146101000a81548160ff0219169083151502179055507f62e78cea01bee320cd4e420270b5ea74000d11b0c9f74754ebdbfc544b05a25861165c611673565b60405161166991906119c2565b60405180910390a1565b600033905090565b611683610852565b6116c2576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016116b9906123b7565b60405180910390fd5b565b600082825260208201905092915050565b7f446972656374207061796d656e74206e6f7420616c6c6f776564000000000000600082015250565b600061170b601a836116c4565b9150611716826116d5565b602082019050919050565b6000602082019050818103600083015261173a816116fe565b9050919050565b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b600061177182611746565b9050919050565b61178181611766565b811461178c57600080fd5b50565b60008135905061179e81611778565b92915050565b6000602082840312156117ba576117b9611741565b5b60006117c88482850161178f565b91505092915050565b6000819050919050565b6117e4816117d1565b82525050565b60006020820190506117ff60008301846117db565b92915050565b60008115159050919050565b61181a81611805565b82525050565b60006020820190506118356000830184611811565b92915050565b611844816117d1565b811461184f57600080fd5b50565b6000813590506118618161183b565b92915050565b60006020828403121561187d5761187c611741565b5b600061188b84828501611852565b91505092915050565b61189d81611766565b82525050565b6000610100820190506118b9600083018b611811565b6118c6602083018a6117db565b6118d360408301896117db565b6118e060608301886117db565b6118ed60808301876117db565b6118fa60a0830186611894565b61190760c08301856117db565b61191460e0830184611894565b9998505050505050505050565b600081519050919050565b60005b8381101561194a57808201518184015260208101905061192f565b60008484015250505050565b6000601f19601f8301169050919050565b600061197282611921565b61197c81856116c4565b935061198c81856020860161192c565b61199581611956565b840191505092915050565b600060208201905081810360008301526119ba8184611967565b905092915050565b60006020820190506119d76000830184611894565b92915050565b6000806000606084860312156119f6576119f5611741565b5b6000611a0486828701611852565b9350506020611a1586828701611852565b9250506040611a2686828701611852565b9150509250925092565b6000819050919050565b6000611a55611a50611a4b84611746565b611a30565b611746565b9050919050565b6000611a6782611a3a565b9050919050565b6000611a7982611a5c565b9050919050565b611a8981611a6e565b82525050565b6000602082019050611aa46000830184611a80565b92915050565b600081905092915050565b50565b6000611ac5600083611aaa565b9150611ad082611ab5565b600082019050919050565b6000611ae682611ab8565b9150819050919050565b7f41756374696f6e206973206e6f742072756e6e696e6700000000000000000000600082015250565b6000611b266016836116c4565b9150611b3182611af0565b602082019050919050565b60006020820190508181036000830152611b5581611b19565b9050919050565b7f41756374696f6e20656e64656400000000000000000000000000000000000000600082015250565b6000611b92600d836116c4565b9150611b9d82611b5c565b602082019050919050565b60006020820190508181036000830152611bc181611b85565b9050919050565b7f426964206d75737420626520686967686572207468616e2063757272656e742060008201527f6869676865737420626964000000000000000000000000000000000000000000602082015250565b6000611c24602b836116c4565b9150611c2f82611bc8565b604082019050919050565b60006020820190508181036000830152611c5381611c17565b9050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b6000611c94826117d1565b9150611c9f836117d1565b9250828201905080821115611cb757611cb6611c5a565b5b92915050565b6000606082019050611cd260008301866117db565b611cdf6020830185611894565b611cec60408301846117db565b949350505050565b6000604082019050611d096000830185611894565b611d1660208301846117db565b9392505050565b600081519050611d2c8161183b565b92915050565b600060208284031215611d4857611d47611741565b5b6000611d5684828501611d1d565b91505092915050565b7f4e6f20746f6b656e2062616c616e636500000000000000000000000000000000600082015250565b6000611d956010836116c4565b9150611da082611d5f565b602082019050919050565b60006020820190508181036000830152611dc481611d88565b9050919050565b7f4475726174696f6e2073686f756c642062652067726561746572207468616e2060008201527f3000000000000000000000000000000000000000000000000000000000000000602082015250565b6000611e276021836116c4565b9150611e3282611dcb565b604082019050919050565b60006020820190508181036000830152611e5681611e1a565b9050919050565b6000604082019050611e7260008301856117db565b611e7f60208301846117db565b9392505050565b7f4f6e6c792074686520746f6b656e206f776e6572206f722074686520636f6e7460008201527f72616374206f776e65722063616e2063616e63656c207468652061756374696f60208201527f6e00000000000000000000000000000000000000000000000000000000000000604082015250565b6000611f086041836116c4565b9150611f1382611e86565b606082019050919050565b60006020820190508181036000830152611f3781611efb565b9050919050565b7f41756374696f6e206e6f742079657420656e6465640000000000000000000000600082015250565b6000611f746015836116c4565b9150611f7f82611f3e565b602082019050919050565b60006020820190508181036000830152611fa381611f67565b9050919050565b7f4f6e6c792074686520746f6b656e206f776e6572206f722074686520636f6e7460008201527f72616374206f776e65722063616e20656e64207468652061756374696f6e0000602082015250565b6000612006603e836116c4565b915061201182611faa565b604082019050919050565b6000602082019050818103600083015261203581611ff9565b9050919050565b7f546865206869676865737420626964206973206c6f776572207468616e20746860008201527f6520726573657276652070726963650000000000000000000000000000000000602082015250565b6000612098602f836116c4565b91506120a38261203c565b604082019050919050565b600060208201905081810360008301526120c78161208b565b9050919050565b6000819050919050565b60006120f36120ee6120e9846120ce565b611a30565b6117d1565b9050919050565b612103816120d8565b82525050565b600082825260208201905092915050565b6000612127600083612109565b915061213282611ab5565b600082019050919050565b600060a0820190506121526000830187611894565b61215f6020830186611894565b61216c60408301856117db565b61217960608301846120fa565b818103608083015261218a8161211a565b905095945050505050565b7f5472616e73666572206661696c65640000000000000000000000000000000000600082015250565b60006121cb600f836116c4565b91506121d682612195565b602082019050919050565b600060208201905081810360008301526121fa816121be565b9050919050565b7f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206160008201527f6464726573730000000000000000000000000000000000000000000000000000602082015250565b600061225d6026836116c4565b915061226882612201565b604082019050919050565b6000602082019050818103600083015261228c81612250565b9050919050565b7f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e6572600082015250565b60006122c96020836116c4565b91506122d482612293565b602082019050919050565b600060208201905081810360008301526122f8816122bc565b9050919050565b7f5061757361626c653a2070617573656400000000000000000000000000000000600082015250565b60006123356010836116c4565b9150612340826122ff565b602082019050919050565b6000602082019050818103600083015261236481612328565b9050919050565b7f5061757361626c653a206e6f7420706175736564000000000000000000000000600082015250565b60006123a16014836116c4565b91506123ac8261236b565b602082019050919050565b600060208201905081810360008301526123d081612394565b905091905056fea2646970667358221220d67170ba502f666b9753f19513d87c461627b823d7e95cad23d10c80a573902f64736f6c63430008120033";
        public MyAuctionDeploymentBase() : base(BYTECODE) { }
        public MyAuctionDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "_tokenAddress", 1)]
        public virtual string TokenAddress { get; set; }
    }

    public partial class AuctionsFunction : AuctionsFunctionBase { }

    [Function("auctions", typeof(AuctionsOutputDTO))]
    public class AuctionsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class BidFunction : BidFunctionBase { }

    [Function("bid")]
    public class BidFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class CancelAuctionFunction : CancelAuctionFunctionBase { }

    [Function("cancelAuction")]
    public class CancelAuctionFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class EndAuctionFunction : EndAuctionFunctionBase { }

    [Function("endAuction")]
    public class EndAuctionFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class GetAuctionStatusFunction : GetAuctionStatusFunctionBase { }

    [Function("getAuctionStatus", "string")]
    public class GetAuctionStatusFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class PauseFunction : PauseFunctionBase { }

    [Function("pause")]
    public class PauseFunctionBase : FunctionMessage
    {

    }

    public partial class PausedFunction : PausedFunctionBase { }

    [Function("paused", "bool")]
    public class PausedFunctionBase : FunctionMessage
    {

    }

    public partial class PendingReturnsFunction : PendingReturnsFunctionBase { }

    [Function("pendingReturns", "uint256")]
    public class PendingReturnsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class StartAuctionFunction : StartAuctionFunctionBase { }

    [Function("startAuction")]
    public class StartAuctionFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "duration", 2)]
        public virtual BigInteger Duration { get; set; }
        [Parameter("uint256", "reservePrice", 3)]
        public virtual BigInteger ReservePrice { get; set; }
    }

    public partial class TokenFunction : TokenFunctionBase { }

    [Function("token", "address")]
    public class TokenFunctionBase : FunctionMessage
    {

    }

    public partial class TokenAddressFunction : TokenAddressFunctionBase { }

    [Function("tokenAddress", "address")]
    public class TokenAddressFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class UnpauseFunction : UnpauseFunctionBase { }

    [Function("unpause")]
    public class UnpauseFunctionBase : FunctionMessage
    {

    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw", "bool")]
    public class WithdrawFunctionBase : FunctionMessage
    {

    }

    public partial class AuctionCanceledEventDTO : AuctionCanceledEventDTOBase { }

    [Event("AuctionCanceled")]
    public class AuctionCanceledEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class AuctionEndedEventDTO : AuctionEndedEventDTOBase { }

    [Event("AuctionEnded")]
    public class AuctionEndedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "winner", 2, false)]
        public virtual string Winner { get; set; }
        [Parameter("uint256", "amount", 3, false)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class AuctionStartedEventDTO : AuctionStartedEventDTOBase { }

    [Event("AuctionStarted")]
    public class AuctionStartedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "endTime", 2, false)]
        public virtual BigInteger EndTime { get; set; }
    }

    public partial class NewHighestBidEventDTO : NewHighestBidEventDTOBase { }

    [Event("NewHighestBid")]
    public class NewHighestBidEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "bidder", 2, false)]
        public virtual string Bidder { get; set; }
        [Parameter("uint256", "amount", 3, false)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true)]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true)]
        public virtual string NewOwner { get; set; }
    }

    public partial class PausedEventDTO : PausedEventDTOBase { }

    [Event("Paused")]
    public class PausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false)]
        public virtual string Account { get; set; }
    }

    public partial class ReservePriceSetEventDTO : ReservePriceSetEventDTOBase { }

    [Event("ReservePriceSet")]
    public class ReservePriceSetEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "reservePrice", 2, false)]
        public virtual BigInteger ReservePrice { get; set; }
    }

    public partial class UnpausedEventDTO : UnpausedEventDTOBase { }

    [Event("Unpaused")]
    public class UnpausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false)]
        public virtual string Account { get; set; }
    }

    public partial class AuctionsOutputDTO : AuctionsOutputDTOBase { }

    [FunctionOutput]
    public class AuctionsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bool", "isRunning", 1)]
        public virtual bool IsRunning { get; set; }
        [Parameter("uint256", "startTime", 2)]
        public virtual BigInteger StartTime { get; set; }
        [Parameter("uint256", "endTime", 3)]
        public virtual BigInteger EndTime { get; set; }
        [Parameter("uint256", "highestBid", 4)]
        public virtual BigInteger HighestBid { get; set; }
        [Parameter("uint256", "reservePrice", 5)]
        public virtual BigInteger ReservePrice { get; set; }
        [Parameter("address", "highestBidder", 6)]
        public virtual string HighestBidder { get; set; }
        [Parameter("uint256", "tokenId", 7)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "tokenOwner", 8)]
        public virtual string TokenOwner { get; set; }
    }







    public partial class GetAuctionStatusOutputDTO : GetAuctionStatusOutputDTOBase { }

    [FunctionOutput]
    public class GetAuctionStatusOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }



    public partial class PausedOutputDTO : PausedOutputDTOBase { }

    [FunctionOutput]
    public class PausedOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class PendingReturnsOutputDTO : PendingReturnsOutputDTOBase { }

    [FunctionOutput]
    public class PendingReturnsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class TokenOutputDTO : TokenOutputDTOBase { }

    [FunctionOutput]
    public class TokenOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TokenAddressOutputDTO : TokenAddressOutputDTOBase { }

    [FunctionOutput]
    public class TokenAddressOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
}

