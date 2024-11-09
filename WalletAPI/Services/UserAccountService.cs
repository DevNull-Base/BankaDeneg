using WalletAPI.Models;

namespace WalletAPI.Services;

public interface IUserAccountService
{
    void AddUser(UserCredentials user);
    UserCredentials GetUserById(string id);
    IEnumerable<UserCredentials> GetAllUsers();
}

public class UserAccountService : IUserAccountService
{
    private readonly List<UserCredentials> _users = new List<UserCredentials>();

    public void AddUser(UserCredentials user)
    {
        _users.Add(user);
    }

    public UserCredentials GetUserById(string id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<UserCredentials> GetAllUsers()
    {
        return _users;
    } 
}