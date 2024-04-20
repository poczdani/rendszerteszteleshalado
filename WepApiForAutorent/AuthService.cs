using System;

public class AuthService
{
    private const string TestUsername = "testName";
    private const string TestPassword = "1234";

    public bool Authenticate(string username, string password)
    {
        return username == TestUsername && password == TestPassword;
    }
}
