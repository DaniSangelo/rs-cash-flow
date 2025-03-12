﻿using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infra.Security;
public class BCrypt : IPasswordEncrypter
{
    public string Encrypt(string password)
    {
        return BC.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}
