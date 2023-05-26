// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

contract MinterProfile {

    struct UserMetadata {
        string logoIpfsHash;
        string bannerIpfsHash;
        string collectionDefinitionIpfsHash;
    }

    mapping(address => UserMetadata) public userMetadata;

    function setUserMetadata(string memory _logoIpfsHash, string memory _bannerIpfsHash, string memory _collectionDefinitionIpfsHash) public {
        UserMetadata storage metadata = userMetadata[msg.sender];
        metadata.logoIpfsHash = _logoIpfsHash;
        metadata.bannerIpfsHash = _bannerIpfsHash;
        metadata.collectionDefinitionIpfsHash = _collectionDefinitionIpfsHash;
    }

    function getUserMetadata(address _userAddress) public view returns (string memory, string memory, string memory) {
        UserMetadata memory metadata = userMetadata[_userAddress];
        return (metadata.logoIpfsHash, metadata.bannerIpfsHash, metadata.collectionDefinitionIpfsHash);
    }
}
