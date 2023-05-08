using System;
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

namespace ERC721ContractLibrary.Contracts.MyERC1155.ContractDefinition
{


    public partial class MyERC1155Deployment : MyERC1155DeploymentBase
    {
        public MyERC1155Deployment() : base(BYTECODE) { }
        public MyERC1155Deployment(string byteCode) : base(byteCode) { }
    }

    public class MyERC1155DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040523480156200001157600080fd5b506040805160208101909152600081526200002c816200004b565b50620000383362000064565b6003805460ff60a01b1916905562000198565b805162000060906002906020840190620000b6565b5050565b600380546001600160a01b038381166001600160a01b0319831681179093556040519116919082907f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e090600090a35050565b828054620000c4906200015c565b90600052602060002090601f016020900481019282620000e8576000855562000133565b82601f106200010357805160ff191683800117855562000133565b8280016001018555821562000133579182015b828111156200013357825182559160200191906001019062000116565b506200014192915062000145565b5090565b5b8082111562000141576000815560010162000146565b600181811c908216806200017157607f821691505b6020821081036200019257634e487b7160e01b600052602260045260246000fd5b50919050565b6123c780620001a86000396000f3fe608060405234801561001057600080fd5b50600436106101415760003560e01c80636b20c454116100b8578063a22cb4651161007c578063a22cb465146102aa578063bd85b039146102bd578063e985e9c5146102dd578063f242432a14610319578063f2fde38b1461032c578063f5298aca1461033f57600080fd5b80636b20c45414610259578063715018a61461026c578063731133e9146102745780638456cb59146102875780638da5cb5b1461028f57600080fd5b80632eb2c2d61161010a5780632eb2c2d6146101d75780633f4ba83a146101ea5780634e1273f4146101f25780634f558e791461021257806357f7789e146102345780635c975abb1461024757600080fd5b8062fdd58e1461014657806301ffc9a71461016c57806302fe53051461018f5780630e89341c146101a45780631f7fdffa146101c4575b600080fd5b61015961015436600461180c565b610352565b6040519081526020015b60405180910390f35b61017f61017a36600461184c565b6103e9565b6040519015158152602001610163565b6101a261019d366004611925565b61043b565b005b6101b76101b2366004611961565b610471565b60405161016391906119c7565b6101a26101d2366004611a6e565b610513565b6101a26101e5366004611b06565b61054f565b6101a26105e6565b610205610200366004611baf565b61061a565b6040516101639190611cb4565b61017f610220366004611961565b600090815260046020526040902054151590565b6101a2610242366004611cc7565b610743565b600354600160a01b900460ff1661017f565b6101a2610267366004611d03565b610791565b6101a26107d4565b6101a2610282366004611d76565b610808565b6101a261083e565b6003546040516001600160a01b039091168152602001610163565b6101a26102b8366004611dca565b610870565b6101596102cb366004611961565b60009081526004602052604090205490565b61017f6102eb366004611e06565b6001600160a01b03918216600090815260016020908152604080832093909416825291909152205460ff1690565b6101a2610327366004611e39565b61087f565b6101a261033a366004611e9d565b6108c4565b6101a261034d366004611eb8565b61095c565b60006001600160a01b0383166103c35760405162461bcd60e51b815260206004820152602b60248201527f455243313135353a2062616c616e636520717565727920666f7220746865207a60448201526a65726f206164647265737360a81b60648201526084015b60405180910390fd5b506000908152602081815260408083206001600160a01b03949094168352929052205490565b60006001600160e01b03198216636cdb3d1360e11b148061041a57506001600160e01b031982166303a24d0760e21b145b8061043557506301ffc9a760e01b6001600160e01b03198316145b92915050565b6003546001600160a01b031633146104655760405162461bcd60e51b81526004016103ba90611eeb565b61046e8161099f565b50565b600081815260056020526040902080546060919061048e90611f20565b80601f01602080910402602001604051908101604052809291908181526020018280546104ba90611f20565b80156105075780601f106104dc57610100808354040283529160200191610507565b820191906000526020600020905b8154815290600101906020018083116104ea57829003601f168201915b50505050509050919050565b6003546001600160a01b0316331461053d5760405162461bcd60e51b81526004016103ba90611eeb565b610549848484846109b2565b50505050565b6001600160a01b03851633148061056b575061056b85336102eb565b6105d25760405162461bcd60e51b815260206004820152603260248201527f455243313135353a207472616e736665722063616c6c6572206973206e6f74206044820152711bdddb995c881b9bdc88185c1c1c9bdd995960721b60648201526084016103ba565b6105df8585858585610b0c565b5050505050565b6003546001600160a01b031633146106105760405162461bcd60e51b81526004016103ba90611eeb565b610618610cb6565b565b6060815183511461067f5760405162461bcd60e51b815260206004820152602960248201527f455243313135353a206163636f756e747320616e6420696473206c656e677468604482015268040dad2e6dac2e8c6d60bb1b60648201526084016103ba565b600083516001600160401b0381111561069a5761069a611870565b6040519080825280602002602001820160405280156106c3578160200160208202803683370190505b50905060005b845181101561073b5761070e8582815181106106e7576106e7611f5a565b602002602001015185838151811061070157610701611f5a565b6020026020010151610352565b82828151811061072057610720611f5a565b602090810291909101015261073481611f86565b90506106c9565b509392505050565b6003546001600160a01b0316331461076d5760405162461bcd60e51b81526004016103ba90611eeb565b6000828152600560209081526040909120825161078c92840190611757565b505050565b6001600160a01b0383163314806107ad57506107ad83336102eb565b6107c95760405162461bcd60e51b81526004016103ba90611f9f565b61078c838383610d53565b6003546001600160a01b031633146107fe5760405162461bcd60e51b81526004016103ba90611eeb565b6106186000610ee1565b6003546001600160a01b031633146108325760405162461bcd60e51b81526004016103ba90611eeb565b61054984848484610f33565b6003546001600160a01b031633146108685760405162461bcd60e51b81526004016103ba90611eeb565b610618611009565b61087b338383611091565b5050565b6001600160a01b03851633148061089b575061089b85336102eb565b6108b75760405162461bcd60e51b81526004016103ba90611f9f565b6105df8585858585611171565b6003546001600160a01b031633146108ee5760405162461bcd60e51b81526004016103ba90611eeb565b6001600160a01b0381166109535760405162461bcd60e51b815260206004820152602660248201527f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206160448201526564647265737360d01b60648201526084016103ba565b61046e81610ee1565b6001600160a01b038316331480610978575061097883336102eb565b6109945760405162461bcd60e51b81526004016103ba90611f9f565b61078c83838361128e565b805161087b906002906020840190611757565b6001600160a01b0384166109d85760405162461bcd60e51b81526004016103ba90611fe8565b81518351146109f95760405162461bcd60e51b81526004016103ba90612029565b33610a098160008787878761138f565b60005b8451811015610aa457838181518110610a2757610a27611f5a565b6020026020010151600080878481518110610a4457610a44611f5a565b602002602001015181526020019081526020016000206000886001600160a01b03166001600160a01b031681526020019081526020016000206000828254610a8c9190612071565b90915550819050610a9c81611f86565b915050610a0c565b50846001600160a01b031660006001600160a01b0316826001600160a01b03167f4a39dc06d4c0dbc64b70af90fd698a233a518aa5d07e595d983b8c0526c8f7fb8787604051610af5929190612089565b60405180910390a46105df816000878787876113ea565b8151835114610b2d5760405162461bcd60e51b81526004016103ba90612029565b6001600160a01b038416610b535760405162461bcd60e51b81526004016103ba906120b7565b33610b6281878787878761138f565b60005b8451811015610c48576000858281518110610b8257610b82611f5a565b602002602001015190506000858381518110610ba057610ba0611f5a565b602090810291909101810151600084815280835260408082206001600160a01b038e168352909352919091205490915081811015610bf05760405162461bcd60e51b81526004016103ba906120fc565b6000838152602081815260408083206001600160a01b038e8116855292528083208585039055908b16825281208054849290610c2d908490612071565b9250508190555050505080610c4190611f86565b9050610b65565b50846001600160a01b0316866001600160a01b0316826001600160a01b03167f4a39dc06d4c0dbc64b70af90fd698a233a518aa5d07e595d983b8c0526c8f7fb8787604051610c98929190612089565b60405180910390a4610cae8187878787876113ea565b505050505050565b600354600160a01b900460ff16610d065760405162461bcd60e51b815260206004820152601460248201527314185d5cd8589b194e881b9bdd081c185d5cd95960621b60448201526064016103ba565b6003805460ff60a01b191690557f5db9ee0a495bf2e6ff9c91a7834c1ba4fdd244a5e8aa4e537bd38aeae4b073aa335b6040516001600160a01b03909116815260200160405180910390a1565b6001600160a01b038316610d795760405162461bcd60e51b81526004016103ba90612146565b8051825114610d9a5760405162461bcd60e51b81526004016103ba90612029565b6000339050610dbd8185600086866040518060200160405280600081525061138f565b60005b8351811015610e82576000848281518110610ddd57610ddd611f5a565b602002602001015190506000848381518110610dfb57610dfb611f5a565b602090810291909101810151600084815280835260408082206001600160a01b038c168352909352919091205490915081811015610e4b5760405162461bcd60e51b81526004016103ba90612189565b6000928352602083815260408085206001600160a01b038b1686529091529092209103905580610e7a81611f86565b915050610dc0565b5060006001600160a01b0316846001600160a01b0316826001600160a01b03167f4a39dc06d4c0dbc64b70af90fd698a233a518aa5d07e595d983b8c0526c8f7fb8686604051610ed3929190612089565b60405180910390a450505050565b600380546001600160a01b038381166001600160a01b0319831681179093556040519116919082907f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e090600090a35050565b6001600160a01b038416610f595760405162461bcd60e51b81526004016103ba90611fe8565b33610f7981600087610f6a88611545565b610f7388611545565b8761138f565b6000848152602081815260408083206001600160a01b038916845290915281208054859290610fa9908490612071565b909155505060408051858152602081018590526001600160a01b0380881692600092918516917fc3d58168c5ae7397731d063d5bbf3d657854427343f4c083240f7aacaa2d0f62910160405180910390a46105df81600087878787611590565b600354600160a01b900460ff16156110565760405162461bcd60e51b815260206004820152601060248201526f14185d5cd8589b194e881c185d5cd95960821b60448201526064016103ba565b6003805460ff60a01b1916600160a01b1790557f62e78cea01bee320cd4e420270b5ea74000d11b0c9f74754ebdbfc544b05a258610d363390565b816001600160a01b0316836001600160a01b0316036111045760405162461bcd60e51b815260206004820152602960248201527f455243313135353a2073657474696e6720617070726f76616c20737461747573604482015268103337b91039b2b63360b91b60648201526084016103ba565b6001600160a01b03838116600081815260016020908152604080832094871680845294825291829020805460ff191686151590811790915591519182527f17307eab39ab6107e8899845ad3d59bd9653f200f220920489ca2b5937696c31910160405180910390a3505050565b6001600160a01b0384166111975760405162461bcd60e51b81526004016103ba906120b7565b336111a7818787610f6a88611545565b6000848152602081815260408083206001600160a01b038a168452909152902054838110156111e85760405162461bcd60e51b81526004016103ba906120fc565b6000858152602081815260408083206001600160a01b038b8116855292528083208785039055908816825281208054869290611225908490612071565b909155505060408051868152602081018690526001600160a01b03808916928a821692918616917fc3d58168c5ae7397731d063d5bbf3d657854427343f4c083240f7aacaa2d0f62910160405180910390a4611285828888888888611590565b50505050505050565b6001600160a01b0383166112b45760405162461bcd60e51b81526004016103ba90612146565b336112e3818560006112c587611545565b6112ce87611545565b6040518060200160405280600081525061138f565b6000838152602081815260408083206001600160a01b0388168452909152902054828110156113245760405162461bcd60e51b81526004016103ba90612189565b6000848152602081815260408083206001600160a01b03898116808652918452828520888703905582518981529384018890529092908616917fc3d58168c5ae7397731d063d5bbf3d657854427343f4c083240f7aacaa2d0f62910160405180910390a45050505050565b600354600160a01b900460ff16156113dc5760405162461bcd60e51b815260206004820152601060248201526f14185d5cd8589b194e881c185d5cd95960821b60448201526064016103ba565b610cae86868686868661164b565b6001600160a01b0384163b15610cae5760405163bc197c8160e01b81526001600160a01b0385169063bc197c819061142e90899089908890889088906004016121cd565b6020604051808303816000875af1925050508015611469575060408051601f3d908101601f191682019092526114669181019061222b565b60015b61151557611475612248565b806308c379a0036114ae5750611489612264565b8061149457506114b0565b8060405162461bcd60e51b81526004016103ba91906119c7565b505b60405162461bcd60e51b815260206004820152603460248201527f455243313135353a207472616e7366657220746f206e6f6e20455243313135356044820152732932b1b2b4bb32b91034b6b83632b6b2b73a32b960611b60648201526084016103ba565b6001600160e01b0319811663bc197c8160e01b146112855760405162461bcd60e51b81526004016103ba906122ed565b6040805160018082528183019092526060916000919060208083019080368337019050509050828160008151811061157f5761157f611f5a565b602090810291909101015292915050565b6001600160a01b0384163b15610cae5760405163f23a6e6160e01b81526001600160a01b0385169063f23a6e61906115d49089908990889088908890600401612335565b6020604051808303816000875af192505050801561160f575060408051601f3d908101601f1916820190925261160c9181019061222b565b60015b61161b57611475612248565b6001600160e01b0319811663f23a6e6160e01b146112855760405162461bcd60e51b81526004016103ba906122ed565b6001600160a01b0385166116d25760005b83518110156116d05782818151811061167757611677611f5a565b60200260200101516004600086848151811061169557611695611f5a565b6020026020010151815260200190815260200160002060008282546116ba9190612071565b909155506116c9905081611f86565b905061165c565b505b6001600160a01b038416610cae5760005b8351811015611285578281815181106116fe576116fe611f5a565b60200260200101516004600086848151811061171c5761171c611f5a565b602002602001015181526020019081526020016000206000828254611741919061237a565b90915550611750905081611f86565b90506116e3565b82805461176390611f20565b90600052602060002090601f01602090048101928261178557600085556117cb565b82601f1061179e57805160ff19168380011785556117cb565b828001600101855582156117cb579182015b828111156117cb5782518255916020019190600101906117b0565b506117d79291506117db565b5090565b5b808211156117d757600081556001016117dc565b80356001600160a01b038116811461180757600080fd5b919050565b6000806040838503121561181f57600080fd5b611828836117f0565b946020939093013593505050565b6001600160e01b03198116811461046e57600080fd5b60006020828403121561185e57600080fd5b813561186981611836565b9392505050565b634e487b7160e01b600052604160045260246000fd5b601f8201601f191681016001600160401b03811182821017156118ab576118ab611870565b6040525050565b600082601f8301126118c357600080fd5b81356001600160401b038111156118dc576118dc611870565b6040516118f3601f8301601f191660200182611886565b81815284602083860101111561190857600080fd5b816020850160208301376000918101602001919091529392505050565b60006020828403121561193757600080fd5b81356001600160401b0381111561194d57600080fd5b611959848285016118b2565b949350505050565b60006020828403121561197357600080fd5b5035919050565b6000815180845260005b818110156119a057602081850181015186830182015201611984565b818111156119b2576000602083870101525b50601f01601f19169290920160200192915050565b602081526000611869602083018461197a565b60006001600160401b038211156119f3576119f3611870565b5060051b60200190565b600082601f830112611a0e57600080fd5b81356020611a1b826119da565b604051611a288282611886565b83815260059390931b8501820192828101915086841115611a4857600080fd5b8286015b84811015611a635780358352918301918301611a4c565b509695505050505050565b60008060008060808587031215611a8457600080fd5b611a8d856117f0565b935060208501356001600160401b0380821115611aa957600080fd5b611ab5888389016119fd565b94506040870135915080821115611acb57600080fd5b611ad7888389016119fd565b93506060870135915080821115611aed57600080fd5b50611afa878288016118b2565b91505092959194509250565b600080600080600060a08688031215611b1e57600080fd5b611b27866117f0565b9450611b35602087016117f0565b935060408601356001600160401b0380821115611b5157600080fd5b611b5d89838a016119fd565b94506060880135915080821115611b7357600080fd5b611b7f89838a016119fd565b93506080880135915080821115611b9557600080fd5b50611ba2888289016118b2565b9150509295509295909350565b60008060408385031215611bc257600080fd5b82356001600160401b0380821115611bd957600080fd5b818501915085601f830112611bed57600080fd5b81356020611bfa826119da565b604051611c078282611886565b83815260059390931b8501820192828101915089841115611c2757600080fd5b948201945b83861015611c4c57611c3d866117f0565b82529482019490820190611c2c565b96505086013592505080821115611c6257600080fd5b50611c6f858286016119fd565b9150509250929050565b600081518084526020808501945080840160005b83811015611ca957815187529582019590820190600101611c8d565b509495945050505050565b6020815260006118696020830184611c79565b60008060408385031215611cda57600080fd5b8235915060208301356001600160401b03811115611cf757600080fd5b611c6f858286016118b2565b600080600060608486031215611d1857600080fd5b611d21846117f0565b925060208401356001600160401b0380821115611d3d57600080fd5b611d49878388016119fd565b93506040860135915080821115611d5f57600080fd5b50611d6c868287016119fd565b9150509250925092565b60008060008060808587031215611d8c57600080fd5b611d95856117f0565b9350602085013592506040850135915060608501356001600160401b03811115611dbe57600080fd5b611afa878288016118b2565b60008060408385031215611ddd57600080fd5b611de6836117f0565b915060208301358015158114611dfb57600080fd5b809150509250929050565b60008060408385031215611e1957600080fd5b611e22836117f0565b9150611e30602084016117f0565b90509250929050565b600080600080600060a08688031215611e5157600080fd5b611e5a866117f0565b9450611e68602087016117f0565b9350604086013592506060860135915060808601356001600160401b03811115611e9157600080fd5b611ba2888289016118b2565b600060208284031215611eaf57600080fd5b611869826117f0565b600080600060608486031215611ecd57600080fd5b611ed6846117f0565b95602085013595506040909401359392505050565b6020808252818101527f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e6572604082015260600190565b600181811c90821680611f3457607f821691505b602082108103611f5457634e487b7160e01b600052602260045260246000fd5b50919050565b634e487b7160e01b600052603260045260246000fd5b634e487b7160e01b600052601160045260246000fd5b600060018201611f9857611f98611f70565b5060010190565b60208082526029908201527f455243313135353a2063616c6c6572206973206e6f74206f776e6572206e6f7260408201526808185c1c1c9bdd995960ba1b606082015260800190565b60208082526021908201527f455243313135353a206d696e7420746f20746865207a65726f206164647265736040820152607360f81b606082015260800190565b60208082526028908201527f455243313135353a2069647320616e6420616d6f756e7473206c656e677468206040820152670dad2e6dac2e8c6d60c31b606082015260800190565b6000821982111561208457612084611f70565b500190565b60408152600061209c6040830185611c79565b82810360208401526120ae8185611c79565b95945050505050565b60208082526025908201527f455243313135353a207472616e7366657220746f20746865207a65726f206164604082015264647265737360d81b606082015260800190565b6020808252602a908201527f455243313135353a20696e73756666696369656e742062616c616e636520666f60408201526939103a3930b739b332b960b11b606082015260800190565b60208082526023908201527f455243313135353a206275726e2066726f6d20746865207a65726f206164647260408201526265737360e81b606082015260800190565b60208082526024908201527f455243313135353a206275726e20616d6f756e7420657863656564732062616c604082015263616e636560e01b606082015260800190565b6001600160a01b0386811682528516602082015260a0604082018190526000906121f990830186611c79565b828103606084015261220b8186611c79565b9050828103608084015261221f818561197a565b98975050505050505050565b60006020828403121561223d57600080fd5b815161186981611836565b600060033d11156122615760046000803e5060005160e01c5b90565b600060443d10156122725790565b6040516003193d81016004833e81513d6001600160401b0381602484011181841117156122a157505050505090565b82850191508151818111156122b95750505050505090565b843d87010160208285010111156122d35750505050505090565b6122e260208286010187611886565b509095945050505050565b60208082526028908201527f455243313135353a204552433131353552656365697665722072656a656374656040820152676420746f6b656e7360c01b606082015260800190565b6001600160a01b03868116825285166020820152604081018490526060810183905260a06080820181905260009061236f9083018461197a565b979650505050505050565b60008282101561238c5761238c611f70565b50039056fea2646970667358221220e6345901894672cd148af97d11a0da586cfd7d5796c0959cc84b4af8eee993d164736f6c634300080d0033";
        public MyERC1155DeploymentBase() : base(BYTECODE) { }
        public MyERC1155DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("uint256", "id", 2)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class BalanceOfBatchFunction : BalanceOfBatchFunctionBase { }

    [Function("balanceOfBatch", "uint256[]")]
    public class BalanceOfBatchFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "accounts", 1)]
        public virtual List<string> Accounts { get; set; }
        [Parameter("uint256[]", "ids", 2)]
        public virtual List<BigInteger> Ids { get; set; }
    }

    public partial class BurnFunction : BurnFunctionBase { }

    [Function("burn")]
    public class BurnFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("uint256", "id", 2)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "value", 3)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class BurnBatchFunction : BurnBatchFunctionBase { }

    [Function("burnBatch")]
    public class BurnBatchFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("uint256[]", "ids", 2)]
        public virtual List<BigInteger> Ids { get; set; }
        [Parameter("uint256[]", "values", 3)]
        public virtual List<BigInteger> Values { get; set; }
    }

    public partial class ExistsFunction : ExistsFunctionBase { }

    [Function("exists", "bool")]
    public class ExistsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

    [Function("isApprovedForAll", "bool")]
    public class IsApprovedForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("address", "operator", 2)]
        public virtual string Operator { get; set; }
    }

    public partial class MintFunction : MintFunctionBase { }

    [Function("mint")]
    public class MintFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("uint256", "id", 2)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
    }

    public partial class MintBatchFunction : MintBatchFunctionBase { }

    [Function("mintBatch")]
    public class MintBatchFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256[]", "ids", 2)]
        public virtual List<BigInteger> Ids { get; set; }
        [Parameter("uint256[]", "amounts", 3)]
        public virtual List<BigInteger> Amounts { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
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

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class SafeBatchTransferFromFunction : SafeBatchTransferFromFunctionBase { }

    [Function("safeBatchTransferFrom")]
    public class SafeBatchTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256[]", "ids", 3)]
        public virtual List<BigInteger> Ids { get; set; }
        [Parameter("uint256[]", "amounts", 4)]
        public virtual List<BigInteger> Amounts { get; set; }
        [Parameter("bytes", "data", 5)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "id", 3)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "amount", 4)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes", "data", 5)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SetApprovalForAllFunction : SetApprovalForAllFunctionBase { }

    [Function("setApprovalForAll")]
    public class SetApprovalForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "operator", 1)]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 2)]
        public virtual bool Approved { get; set; }
    }

    public partial class SetTokenUriFunction : SetTokenUriFunctionBase { }

    [Function("setTokenUri")]
    public class SetTokenUriFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("string", "tokenURI", 2)]
        public virtual string TokenURI { get; set; }
    }

    public partial class SetURIFunction : SetURIFunctionBase { }

    [Function("setURI")]
    public class SetURIFunctionBase : FunctionMessage
    {
        [Parameter("string", "newuri", 1)]
        public virtual string Newuri { get; set; }
    }

    public partial class SupportsInterfaceFunction : SupportsInterfaceFunctionBase { }

    [Function("supportsInterface", "bool")]
    public class SupportsInterfaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes4", "interfaceId", 1)]
        public virtual byte[] InterfaceId { get; set; }
    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
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

    public partial class UriFunction : UriFunctionBase { }

    [Function("uri", "string")]
    public class UriFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }

    [Event("ApprovalForAll")]
    public class ApprovalForAllEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
        [Parameter("address", "operator", 2, true )]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 3, false )]
        public virtual bool Approved { get; set; }
    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true )]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true )]
        public virtual string NewOwner { get; set; }
    }

    public partial class PausedEventDTO : PausedEventDTOBase { }

    [Event("Paused")]
    public class PausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false )]
        public virtual string Account { get; set; }
    }

    public partial class TransferBatchEventDTO : TransferBatchEventDTOBase { }

    [Event("TransferBatch")]
    public class TransferBatchEventDTOBase : IEventDTO
    {
        [Parameter("address", "operator", 1, true )]
        public virtual string Operator { get; set; }
        [Parameter("address", "from", 2, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 3, true )]
        public virtual string To { get; set; }
        [Parameter("uint256[]", "ids", 4, false )]
        public virtual List<BigInteger> Ids { get; set; }
        [Parameter("uint256[]", "values", 5, false )]
        public virtual List<BigInteger> Values { get; set; }
    }

    public partial class TransferSingleEventDTO : TransferSingleEventDTOBase { }

    [Event("TransferSingle")]
    public class TransferSingleEventDTOBase : IEventDTO
    {
        [Parameter("address", "operator", 1, true )]
        public virtual string Operator { get; set; }
        [Parameter("address", "from", 2, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 3, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "id", 4, false )]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "value", 5, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class URIEventDTO : URIEventDTOBase { }

    [Event("URI")]
    public class URIEventDTOBase : IEventDTO
    {
        [Parameter("string", "value", 1, false )]
        public virtual string Value { get; set; }
        [Parameter("uint256", "id", 2, true )]
        public virtual BigInteger Id { get; set; }
    }

    public partial class UnpausedEventDTO : UnpausedEventDTOBase { }

    [Event("Unpaused")]
    public class UnpausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false )]
        public virtual string Account { get; set; }
    }

    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class BalanceOfBatchOutputDTO : BalanceOfBatchOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfBatchOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }





    public partial class ExistsOutputDTO : ExistsOutputDTOBase { }

    [FunctionOutput]
    public class ExistsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsApprovedForAllOutputDTO : IsApprovedForAllOutputDTOBase { }

    [FunctionOutput]
    public class IsApprovedForAllOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
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













    public partial class SupportsInterfaceOutputDTO : SupportsInterfaceOutputDTOBase { }

    [FunctionOutput]
    public class SupportsInterfaceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class UriOutputDTO : UriOutputDTOBase { }

    [FunctionOutput]
    public class UriOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
}
