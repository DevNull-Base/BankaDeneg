using WalletAPI.Models;

namespace WalletAPI.Services;

public interface IUserAccountService
{
    void AddUser(UserAccountCredentials user);
    UserAccountCredentials GetUserById(string id);
    IEnumerable<UserAccountCredentials> GetAllUsers();
}

public class UserAccountService : IUserAccountService
{
    private readonly List<UserAccountCredentials> _users = new List<UserAccountCredentials>();

    public void AddUser(UserAccountCredentials user)
    {
        _users.Add(user);
    }

    public UserAccountCredentials GetUserById(string id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<UserAccountCredentials> GetAllUsers()
    {
        return _users;
    } 
}