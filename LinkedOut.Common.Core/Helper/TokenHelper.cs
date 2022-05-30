using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LinkedOut.Common.Api;
using LinkedOut.Common.Domain;
using LinkedOut.Common.Exception;
using Microsoft.IdentityModel.Tokens;

namespace LinkedOut.Common.Helper;

public static class TokenHelper
{
    public static string GenerateToken(int unifiedId,string userName,string userType)
    {
        var claim = new Claim[]
        {
            new("unifiedId",unifiedId.ToString()),
            new("userName", userName),
            new("userType", userType),
            new("datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yanglingcong@qq.com")); //密钥大小要超过128bt，最少要16位

        var token = new JwtSecurityToken(
            issuer: "NZH", //发起人：当前项目
            audience: "kevin project", //订阅：我们需要谁去使用这个Token
            claims: claim, //声明的数组
            expires: DateTime.Now.AddSeconds(60),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256) //数字签名 第一部分是密钥，第二部分是加密方式
        );
        
        //生成token
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }


    public static void VerifyToken(string token)
    {
        
        Console.WriteLine(token);
        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadJwtToken(token);

            var parameters = new TokenValidationParameters()
            {
              
            };
            jwtHandler.ValidateToken(token, parameters, out var securityToken);
            var enumerable = jwtSecurityToken.Claims;
        }
        catch (SecurityTokenExpiredException e)
        {
            throw new ApiException(ResultCode.Unauthorized());
        }
        catch (SecurityTokenException e)
        {
            throw new ApiException(ResultCode.Unauthorized());
        }

    }
}