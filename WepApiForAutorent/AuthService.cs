using AutoRent;
using System;

public class AuthService
{
    private readonly AutoRentDbContext _dbContext;

    public AuthService(AutoRentDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public bool Authenticate(string username, string password)
    {
        // Felhasználók kikeresése az adatbázisból
        var user = _dbContext.Users.ToList();

        foreach (var item in user)
        {
            if (item.Username == username && item.Password == password)
            {
                //Ha a felhasználó és a jelszó eggyezik akkor visszatérés true
                return true;
            }
        }

        // Ha a felhasználó nem létezik, vagy a jelszava nem egyezik, akkor sikertelen az autentikáció
        return false;
    }


    public bool IsAdmin_(string username)
    {
        // Felhasználó kikeresése az adatbázisból
        var user = _dbContext.Users.ToList();
        foreach (var item in user)
        {
            if (user != null && item.IsAdmin == 1)
            {
                // A felhasználó adminisztrátor
                return true;
            }
        }
        // A felhasználó nem adminisztrátor vagy nem létezik
        return false;
    }



}
