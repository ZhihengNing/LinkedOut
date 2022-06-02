using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Manager;

public class UserManager
{
    private readonly LinkedOutContext _context;


    public UserManager(LinkedOutContext context)
    {
        _context = context;
    }

    public bool CheckEmailExist(string email)
    {
        var needUser = _context.Users
            .Select(o => o)
            .SingleOrDefault(o => o.Email == email);
        return needUser != null;
    }

    public bool CheckUserSame(string userName)
    {
        var users = _context.Users.Select(o => o)
            .SingleOrDefault(o => o.UserName == userName);
        return users != null;
    }

    public DB.Entity.User? GetUserById(int unifiedId)
    {
        return _context.Users.Select(o => o)
            .SingleOrDefault(o => o.UnifiedId == unifiedId);
    }


}