// SPDX-License-Identifier: MIT
// Check https://wizard.openzeppelin.com/#erc1155 o generate / customise your own ERC1155 contract
pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/security/Pausable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Burnable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Supply.sol";

contract MyERC1155 is ERC1155, Ownable, Pausable, ERC1155Burnable, ERC1155Supply {
    
    mapping (uint256 => string) private _tokenURIs;

    struct TokenData {
        uint256 price;
        bool forSale;
        string contactInfo;
    }

    mapping(uint256 => TokenData) public tokenData;

    event TokenMinted(address indexed account, uint256 indexed tokenId, uint256 amount);

    constructor() ERC1155("") {}

    function setURI(string memory newuri) public onlyOwner {
        _setURI(newuri);
    }

    function uri(uint256 tokenId) override public view   returns (string memory) { 
        return(_tokenURIs[tokenId]); 
    }

    function setTokenUri(uint256 tokenId, string memory tokenURI)  public
        onlyOwner {
         _tokenURIs[tokenId] = tokenURI; 
    }  

    function pause() public onlyOwner {
        _pause();
    }

    function unpause() public onlyOwner {
        _unpause();
    }

    function mint(address account, uint256 id, uint256 amount, bytes memory data)
        public
        onlyOwner
    {
        _mint(account, id, amount, data);
        tokenData[id] = TokenData(0, false, "");
        emit TokenMinted(account, id, amount);
    }

    function mintBatch(address to, uint256[] memory ids, uint256[] memory amounts, bytes memory data)
        public
        onlyOwner
    {
        _mintBatch(to, ids, amounts, data);
    }

    function updateTokenForSale(uint256 id, uint256 newPrice, bool newStatus, string memory newContactInfo) public {
        require(balanceOf(msg.sender, id) > 0, "ERC1155: operator query for nonexistent token");
        require(msg.sender == owner() || balanceOf(msg.sender, id) > 0, "Caller is not owner nor the token owner");

        tokenData[id].price = newPrice;
        tokenData[id].forSale = newStatus;
        tokenData[id].contactInfo = newContactInfo;
    }


    function _beforeTokenTransfer(address operator, address from, address to, uint256[] memory ids, uint256[] memory amounts, bytes memory data)
        internal
        whenNotPaused
        override(ERC1155, ERC1155Supply)
    {
        super._beforeTokenTransfer(operator, from, to, ids, amounts, data);
    }
}