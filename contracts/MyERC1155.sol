// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/security/Pausable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Burnable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Supply.sol";
import "@openzeppelin/contracts/security/ReentrancyGuard.sol";

contract MyERC1155 is ERC1155, Ownable, Pausable, ERC1155Burnable, ERC1155Supply, ReentrancyGuard {
    
    mapping (uint256 => string) private _tokenURIs;
    mapping (uint256 => address) public owners;

    struct TokenData {
        uint256 price;
        bool forSale;
        string contactInfo;
    }

    mapping(uint256 => TokenData) public tokenData;

    event TokenMinted(address indexed account, uint256 indexed tokenId, uint256 amount);
    event TokenSold(uint256 indexed tokenId, address indexed seller, address indexed buyer, uint256 price);

    constructor() ERC1155("") {}

    function setURI(string memory newuri)
        public
        onlyOwner
    {
        _setURI(newuri);
    }

    function uri(uint256 tokenId) override public view   returns (string memory) { 
        return(_tokenURIs[tokenId]); 
    }

    function setTokenUri(uint256 tokenId, string memory tokenURI)
        public
        onlyOwner
    {
         _tokenURIs[tokenId] = tokenURI; 
    }  

    function pause()
        public
        onlyOwner
    {
        _pause();
    }

    function unpause()
        public
        onlyOwner
    {
        _unpause();
    }

    function mint(address account, uint256 id, uint256 amount, bytes memory data)
        public
        onlyOwner
    {
        _mint(account, id, amount, data);
        tokenData[id] = TokenData(0, false, "");
        owners[id] = account;
        emit TokenMinted(account, id, amount);
    }

    function mintBatch(address to, uint256[] memory ids, uint256[] memory amounts, bytes memory data)
        public
        onlyOwner
    {
        _mintBatch(to, ids, amounts, data);
        for (uint i = 0; i < ids.length; i++) {
            owners[ids[i]] = to;
        }
    }

    function updateTokenForSale(uint256 id, uint256 newPrice, bool newStatus, string memory newContactInfo)
        public
    {
        require(balanceOf(msg.sender, id) > 0, "ERC1155: operator query for nonexistent token");
        require(msg.sender == owner() || balanceOf(msg.sender, id) > 0, "Caller is not owner nor the token owner");

        tokenData[id].price = newPrice;
        tokenData[id].forSale = newStatus;
        tokenData[id].contactInfo = newContactInfo;
    }

    function buyToken(uint256 tokenId) external payable nonReentrant {
        require(tokenData[tokenId].forSale, "Token is not for sale");
        require(msg.value >= tokenData[tokenId].price, "Insufficient Ether value sent");

        address seller = owners[tokenId];
        payable(seller).transfer(msg.value);

        // The _safeTransferFrom function will perform the same operation as safeTransferFrom, 
        // but without the check for approval, since we've confirmed that the contract is the operator.
        _safeTransferFrom(seller, msg.sender, tokenId, 1, "");

        tokenData[tokenId].forSale = false;
        owners[tokenId] = msg.sender;

        emit TokenSold(tokenId, seller, msg.sender, msg.value);
    }

    function getOwner(uint256 tokenId) public view returns (address) {
        return owners[tokenId];
    }


    function _beforeTokenTransfer(address operator, address from, address to, uint256[] memory ids, uint256[] memory amounts, bytes memory data)
        internal
        whenNotPaused
        override(ERC1155, ERC1155Supply)
    {
        super._beforeTokenTransfer(operator, from, to, ids, amounts, data);

        if (from == address(0)) {
            // Minting new tokens
            for (uint i = 0; i < ids.length; i++) {
                owners[ids[i]] = to;
            }
        } else if (to == address(0)) {
            // Burning tokens
            for (uint i = 0; i < ids.length; i++) {
                owners[ids[i]] = address(0);
            }
        } else {
            // Transferring tokens
            for (uint i = 0; i < ids.length; i++) {
                require(owners[ids[i]] == from, "Transfer: 'from' address does not own the token");
                owners[ids[i]] = to;
            }
        }
    }
}
