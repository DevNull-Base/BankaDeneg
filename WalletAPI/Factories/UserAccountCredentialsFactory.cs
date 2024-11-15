﻿using WalletAPI.Models;
using WalletAPI.Services;

namespace WalletAPI.Factories;

public class UserAccountCredentialsFactory
{
    private readonly ITokenService _tokenService;

    public UserAccountCredentialsFactory(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public UserCredentials Create(string id, string login,string password)
    {
        return new UserCredentials(_tokenService)
        {
            Id = id,
            Login = login,
            Password = password
        };
    }
}