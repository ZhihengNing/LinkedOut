using System.ComponentModel.DataAnnotations;

namespace LinkedOut.Common.Domain;

public class UserLoginVo
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }

    [Required] public string UserType { get; set; }
}