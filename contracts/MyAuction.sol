// SPDX-License-Identifier: MIT

pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC1155/IERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/security/Pausable.sol";

contract MyAuction is Ownable, Pausable {
    struct Auction {
        bool isRunning;
        uint256 startTime;
        uint256 endTime;
        uint256 highestBid;
        uint256 reservePrice;
        address highestBidder;
        uint256 tokenId;
        address tokenOwner;
    }

    mapping(uint256 => Auction) public auctions;
    mapping(address => uint256) public pendingReturns;
    IERC1155 public token;

    constructor(address tokenAddress) {
        token = IERC1155(tokenAddress);
    }

    event AuctionStarted(uint256 tokenId, uint256 endTime);
    event NewHighestBid(uint256 tokenId, address bidder, uint256 amount);
    event AuctionEnded(uint256 tokenId, address winner, uint256 amount);
    event AuctionCanceled(uint256 tokenId);
    event ReservePriceSet(uint256 tokenId, uint256 reservePrice);

    function startAuction(uint256 tokenId, uint256 duration, uint256 reservePrice) public whenNotPaused {
        require(token.balanceOf(msg.sender, tokenId) > 0, "No token balance");
        require(duration > 0, "Duration should be greater than 0");

        auctions[tokenId] = Auction({
            isRunning: true,
            startTime: block.timestamp,
            endTime: block.timestamp + duration,
            highestBid: 0,
            reservePrice: reservePrice,
            highestBidder: address(0),
            tokenId: tokenId,
            tokenOwner: msg.sender
        });

        emit ReservePriceSet(tokenId, reservePrice);
        emit AuctionStarted(tokenId, block.timestamp + duration);
    }

    function bid(uint256 tokenId) public payable whenNotPaused {
        Auction storage auction = auctions[tokenId];
        require(auction.isRunning, "Auction is not running");
        require(block.timestamp <= auction.endTime, "Auction ended");
        require(msg.value > auction.highestBid, "Bid must be higher than current highest bid");

        // Refund the previous highest bidder
        if (auction.highestBidder != address(0)) {
            pendingReturns[auction.highestBidder] += auction.highestBid;
        }

        auction.highestBid = msg.value;
        auction.highestBidder = msg.sender;

        emit NewHighestBid(tokenId, msg.sender, msg.value);
    }

    function cancelAuction(uint256 tokenId) public whenNotPaused {
        Auction storage auction = auctions[tokenId];
        require(auction.isRunning, "Auction is not running");
        require(msg.sender == auction.tokenOwner || msg.sender == owner(), "Only the token owner or the contract owner can cancel the auction");

        // Add highest bid to the pending returns
        if (auction.highestBidder != address(0)) {
            pendingReturns[auction.highestBidder] += auction.highestBid;
        }

        auction.isRunning = false;
        emit AuctionCanceled(tokenId);
    }

    function endAuction(uint256 tokenId) public whenNotPaused {
        Auction storage auction = auctions[tokenId];
        require(auction.isRunning, "Auction is not running");
        require(block.timestamp > auction.endTime, "Auction not yet ended");
        require(msg.sender == auction.tokenOwner || msg.sender == owner(), "Only the token owner or the contract owner can end the auction");
                require(auction.highestBid >= auction.reservePrice, "The highest bid is lower than the reserve price");

        // Transfer the token to the highest bidder
        token.safeTransferFrom(auction.tokenOwner, auction.highestBidder, tokenId, 1, "");

        // Transfer the highest bid to the token owner
        (bool success, ) = payable(auction.tokenOwner).call{value: auction.highestBid}("");
        require(success, "Transfer failed");

        auction.isRunning = false;

        emit AuctionEnded(tokenId, auction.highestBidder, auction.highestBid);
    }

    function withdraw() public returns (bool) {
        uint amount = pendingReturns[msg.sender];
        if (amount > 0) {
            // Using the 'check-effects-interactions' pattern
            pendingReturns[msg.sender] = 0;
            
            (bool success, ) = payable(msg.sender).call{value: amount}("");
            if (!success) {
                // If the send fails (due to out of gas or a revert), store the funds back into pendingReturns
                pendingReturns[msg.sender] = amount;
                return false;
            }
        }
        return true;
    }

    function pause() public onlyOwner {
        _pause();
    }

    function unpause() public onlyOwner {
        _unpause();
    }

    function getAuctionStatus(uint256 tokenId) public view returns (string memory) {
        Auction storage auction = auctions[tokenId];
        if (!auction.isRunning) return "Not Started";
        if (auction.isRunning && block.timestamp <= auction.endTime) return "Running";
        return "Ended";
    }

    // Fallback function
    receive() external payable {
        revert("Direct payment not allowed");
    }
}

