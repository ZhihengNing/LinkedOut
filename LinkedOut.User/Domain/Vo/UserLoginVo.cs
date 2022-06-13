using System.ComponentModel.DataAnnotations;

namespace LinkedOut.User.Domain.Vo;

public class UserLoginVo
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserType { get; set; } = null!;
}