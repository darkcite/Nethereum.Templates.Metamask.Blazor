// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/security/Pausable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Burnable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Supply.sol";
import "@openzeppelin/contracts/security/ReentrancyGuard.sol";
import "@openzeppelin/contracts/interfaces/IERC2981.sol";

contract MyERC1155 is
    ERC1155,
    Ownable,
    Pausable,
    ERC1155Burnable,
    ERC1155Supply,
    ReentrancyGuard,
    IERC2981
{
    mapping(uint256 => string) private _tokenURIs;
    mapping(uint256 => bool) private _tokenExists;
    mapping(uint256 => address) public owners;
    mapping(uint256 => uint8) public royalties;
    mapping(address => uint256) nonces; // Add a nonce for each address

    struct TokenData {
        uint256 price;
        uint256 quantityForSale;
        bool forSale;
    }

    mapping(uint256 => TokenData) public tokenData;

    event TokenMinted(
        address indexed account,
        uint256 indexed tokenId,
        uint256 amount
    );
    event TokenSaleStatusUpdated(
        uint256 indexed tokenId,
        bool forSale,
        uint256 quantityForSale
    );
    event TokenSold(
        uint256 indexed tokenId,
        address indexed seller,
        address indexed buyer,
        uint256 price,
        uint256 quantity
    );

    constructor() ERC1155("") {}

    function setURI(string memory newuri) public onlyOwner {
        _setURI(newuri);
    }

    function uri(uint256 tokenId) public view override returns (string memory) {
        return (_tokenURIs[tokenId]);
    }

    function setTokenUri(uint256 tokenId, string memory tokenURI)
        public
        onlyOwner
    {
        _tokenURIs[tokenId] = tokenURI;
    }

    function pause() public onlyOwner {
        _pause();
    }

    function unpause() public onlyOwner {
        _unpause();
    }

    function mint(
        address account,
        uint256 id,
        uint256 amount,
        uint8 royalty, // New royalty parameter
        bytes memory data
    ) public onlyOwner {
        require(account != address(0), "ERC1155: mint to the zero address");
        require(
            !_tokenExists[id],
            "ERC1155: minting a token which already exists"
        );
        require(royalty <= 100, "Royalty cannot be more than 100%");
        _mint(account, id, amount, data);
        tokenData[id] = TokenData(0, 0, false);
        owners[id] = account;
        _tokenExists[id] = true;
        royalties[id] = royalty; // Set the royalty
        emit TokenMinted(account, id, amount);
    }

    function mintBatch(
        address to,
        uint256[] memory ids,
        uint256[] memory amounts,
        bytes memory data
    ) public onlyOwner {
        require(to != address(0), "ERC1155: mint to the zero address");
        _mintBatch(to, ids, amounts, data);
        for (uint256 i = 0; i < ids.length; i++) {
            owners[ids[i]] = to;
        }
    }

    function updateTokenForSale(
        uint256 id,
        uint256 newPrice,
        uint256 quantityForSale,
        bool newStatus
    ) public {
        require(
            balanceOf(msg.sender, id) >= quantityForSale,
            "ERC1155: not enough tokens owned for the sale"
        );
        require(
            msg.sender == owner() || balanceOf(msg.sender, id) > 0,
            "Caller is not owner nor the token owner"
        );

        tokenData[id].price = newPrice;
        tokenData[id].forSale = newStatus;
        tokenData[id].quantityForSale = quantityForSale;
        emit TokenSaleStatusUpdated(id, newStatus, quantityForSale);
    }

    function buyToken(
        uint256 tokenId,
        uint256 quantity,
        uint256 userNonce
    ) external payable nonReentrant {
        require(userNonce == nonces[msg.sender]++, "Invalid nonce"); // Check the nonce

        require(tokenData[tokenId].forSale, "Token is not for sale");
        require(
            quantity <= tokenData[tokenId].quantityForSale,
            "Requested quantity exceeds the quantity for sale"
        );
        require(
            msg.value >= tokenData[tokenId].price * quantity,
            "Insufficient Ether value sent"
        );

        address seller = owners[tokenId];
        uint256 royaltyAmount = (msg.value * royalties[tokenId]) / 100;
        uint256 sellerAmount = msg.value - royaltyAmount;

        (bool sellerTransferSucceeded, ) = payable(seller).call{
            value: sellerAmount
        }("");
        require(sellerTransferSucceeded, "Transfer to seller failed");

        (bool royaltyTransferSucceeded, ) = payable(owners[tokenId]).call{
            value: royaltyAmount
        }("");
        require(royaltyTransferSucceeded, "Transfer of royalty failed"); // Check if the transfer was successful

        _safeTransferFrom(seller, msg.sender, tokenId, quantity, "");
        tokenData[tokenId].quantityForSale -= quantity;
        if (tokenData[tokenId].quantityForSale == 0) {
            tokenData[tokenId].forSale = false;
        }
        owners[tokenId] = msg.sender;

        emit TokenSold(tokenId, seller, msg.sender, msg.value, quantity);
    }

    function getOwner(uint256 tokenId) public view returns (address) {
        return owners[tokenId];
    }

    function royaltyInfo(uint256 _tokenId, uint256 _salePrice)
        external
        view
        override
        returns (address receiver, uint256 royaltyAmount)
    {
        receiver = owners[_tokenId];
        royaltyAmount = (_salePrice * royalties[_tokenId]) / 100;
    }

    function _mint(
        address account,
        uint256 id,
        uint256 amount,
        bytes memory data
    ) internal override(ERC1155) {
        super._mint(account, id, amount, data);
        owners[id] = account;
    }

    function _mintBatch(
        address to,
        uint256[] memory ids,
        uint256[] memory amounts,
        bytes memory data
    ) internal override(ERC1155) {
        super._mintBatch(to, ids, amounts, data);
        for (uint256 i = 0; i < ids.length; i++) {
            owners[ids[i]] = to;
        }
    }

    function _burn(
        address account,
        uint256 id,
        uint256 amount
    ) internal override(ERC1155) {
        super._burn(account, id, amount);
        owners[id] = address(0);
    }

    function _burnBatch(
        address account,
        uint256[] memory ids,
        uint256[] memory amounts
    ) internal override(ERC1155) {
        super._burnBatch(account, ids, amounts);
        for (uint256 i = 0; i < ids.length; i++) {
            owners[ids[i]] = address(0);
        }
    }

    function _beforeTokenTransfer(
        address operator,
        address from,
        address to,
        uint256[] memory ids,
        uint256[] memory amounts,
        bytes memory data
    ) internal override(ERC1155, ERC1155Supply) whenNotPaused {
        super._beforeTokenTransfer(operator, from, to, ids, amounts, data);
    }
}
