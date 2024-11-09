﻿using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace WalletApp.Services;

public interface IBiometricService
{
    Task<bool> AuthenticateAsync();
    Task<bool> IsFingerprintAvailableAsync();
}

public class BiometricService: IBiometricService
{

    public async Task<bool> AuthenticateAsync()
    {
        var result = await CrossFingerprint.Current.AuthenticateAsync(
            new AuthenticationRequestConfiguration("Аутентификация", "Подтвердите вход с помощью отпечатка пальца"));
    
        if (result.Authenticated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> IsFingerprintAvailableAsync()
    {
        return await CrossFingerprint.Current.IsAvailableAsync();
    }
}