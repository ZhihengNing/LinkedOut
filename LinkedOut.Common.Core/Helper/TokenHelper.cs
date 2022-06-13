using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LinkedOut.Common.Domain;
using Microsoft.IdentityModel.Tokens;

namespace LinkedOut.Common.Helper;

public class TokenHelper
{
    public static readonly TokenProperties Token = FileHelper
        .ReadJsonFile("../LinkedOut.Common.Core/config.json")
        .ToObj<TokenProperties>("token");

    public static readonly SymmetricSecurityKey Key = new(Encoding.UTF8.GetBytes(Token.SecretKey));
    
    
    public static string GenerateToken(int unifiedId, string userName, string userType)
    {
        var claim = new Claim[]
        {
            new("unifiedId", unifiedId.ToString()),
            new("userName", userName),
            new("userType", userType),
            new("datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        };
        var token = new JwtSecurityToken(
            issuer: Token.Issuer,
            audience: Token.Audience, //订阅：我们需要谁去使用这个Token
            claims: claim, //声明的数组
            expires: DateTime.Now.AddSeconds(Token.ExpiresTime),
            signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256) //数字签名 第一部分是密钥，第二部分是加密方式
        );

        //生成token
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }
}