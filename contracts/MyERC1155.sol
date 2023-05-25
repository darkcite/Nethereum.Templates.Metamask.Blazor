// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/AccessControlEnumerable.sol";
import "@openzeppelin/contracts/security/Pausable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Burnable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Supply.sol";
import "@openzeppelin/contracts/security/ReentrancyGuard.sol";
import "@openzeppelin/contracts/interfaces/IERC2981.sol";

contract MyERC1155 is
    ERC1155,
    AccessControlEnumerable,
    Pausable,
    ERC1155Burnable,
    ERC1155Supply,
    ReentrancyGuard
{
    bytes32 public constant MINTER_ROLE = keccak256("MINTER_ROLE");
    uint256 private _currentTokenId = 0;

    mapping(uint256 => string) private _tokenURIs;
    mapping(uint256 => uint8) public royalties;
    mapping(uint256 => bool) private _tokenExists;

    struct TokenData {
        uint256 price;
        uint256 quantityForSale;
        bool forSale;
    }

    mapping(uint256 => TokenData) public tokenData;
    mapping(uint256 => address) public originalMinters;

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

    constructor() ERC1155("") {
        _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
        _setupRole(MINTER_ROLE, msg.sender);
    }

    function withdraw() public onlyRole(DEFAULT_ADMIN_ROLE) {
        payable(_msgSender()).transfer(address(this).balance);
    }

    function setURI(string memory newuri) public onlyRole(DEFAULT_ADMIN_ROLE) {
        _setURI(newuri);
    }

    function uri(uint256 tokenId) public view override returns (string memory) {
        return (_tokenURIs[tokenId]);
    }

    function setTokenUri(uint256 tokenId, string memory tokenURI)
        public
        onlyRole(DEFAULT_ADMIN_ROLE)
    {
        _tokenURIs[tokenId] = tokenURI;
    }

    function pause() public onlyRole(DEFAULT_ADMIN_ROLE) {
        _pause();
    }

    function unpause() public onlyRole(DEFAULT_ADMIN_ROLE) {
        _unpause();
    }

    function mint(
        address account,
        uint256 amount,
        uint8 royalty,
        bytes memory data
    ) public onlyRole(MINTER_ROLE) returns (uint256) {
        require(account != address(0), "ERC1155: mint to the zero address");
        require(royalty <= 100, "Royalty cannot be more than 100%");
        _mint(account, _currentTokenId, amount, data);
        tokenData[_currentTokenId] = TokenData(0, 0, false);
        originalMinters[_currentTokenId] = account;
        royalties[_currentTokenId] = royalty;
        emit TokenMinted(account, _currentTokenId, amount);
        _currentTokenId++;
        return _currentTokenId - 1;
    }

    function mintBatch(
        address to,
        uint256[] memory amounts,
        uint8[] memory royaltiesArray,
        bytes memory data
    ) public onlyRole(MINTER_ROLE) returns (uint256[] memory) {
        require(to != address(0), "ERC1155: mint to the zero address");
        require(
            amounts.length == royaltiesArray.length,
            "ERC1155: amounts and royalties length mismatch"
        );
        uint256[] memory ids = new uint256[](amounts.length);
        for (uint256 i = 0; i < amounts.length; i++) {
            require(
                royaltiesArray[i] <= 100,
                "Royalty cannot be more than 100%"
            );
            ids[i] = _currentTokenId;
            tokenData[_currentTokenId] = TokenData(0, 0, false);
            originalMinters[_currentTokenId] = to;
            royalties[_currentTokenId] = royaltiesArray[i];
            _currentTokenId++;
        }
        _mintBatch(to, ids, amounts, data);
        return ids;
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
            msg.sender == getRoleMember(DEFAULT_ADMIN_ROLE, 0) ||
                balanceOf(msg.sender, id) > 0,
            "Caller is not owner nor the token owner"
        );

        tokenData[id].price = newPrice;
        tokenData[id].forSale = newStatus;
        tokenData[id].quantityForSale = quantityForSale;
        emit TokenSaleStatusUpdated(id, newStatus, quantityForSale);
    }

    function buyToken(uint256 tokenId, uint256 quantity)
        external
        payable
        nonReentrant
    {
        require(tokenData[tokenId].forSale, "Token is not for sale");
        require(
            quantity <= tokenData[tokenId].quantityForSale,
            "Requested quantity exceeds the quantity for sale"
        );
        require(
            msg.value >= tokenData[tokenId].price * quantity,
            "Insufficient Ether value sent"
        );

        address seller = originalMinters[tokenId];
        uint256 royaltyAmount = (msg.value * royalties[tokenId]) / 100;
        uint256 sellerAmount = msg.value - royaltyAmount;

        (bool sellerTransferSucceeded, ) = payable(seller).call{
            value: sellerAmount
        }("");
        require(sellerTransferSucceeded, "Transfer to seller failed");

        (bool royaltyTransferSucceeded, ) = payable(originalMinters[tokenId])
            .call{value: royaltyAmount}("");
        require(royaltyTransferSucceeded, "Transfer of royalty failed");

        // Transfer the token from the seller to the buyer
        _safeTransferFrom(seller, msg.sender, tokenId, quantity, "");
        tokenData[tokenId].quantityForSale -= quantity;
        if (tokenData[tokenId].quantityForSale == 0) {
            tokenData[tokenId].forSale = false;
        }

        emit TokenSold(tokenId, seller, msg.sender, msg.value, quantity);
    }

    function royaltyInfo(uint256 _tokenId, uint256 _salePrice)
        external
        view
        returns (address receiver, uint256 royaltyAmount)
    {
        receiver = originalMinters[_tokenId];
        royaltyAmount = (_salePrice * royalties[_tokenId]) / 100;
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

    function supportsInterface(bytes4 interfaceId)
        public
        view
        override(AccessControlEnumerable, ERC1155)
        returns (bool)
    {
        return
            super.supportsInterface(interfaceId) ||
            interfaceId == type(IERC2981).interfaceId;
    }
}
