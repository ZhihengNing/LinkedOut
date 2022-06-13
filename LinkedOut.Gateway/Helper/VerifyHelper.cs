using System.IdentityModel.Tokens.Jwt;
using LinkedOut.Common.Api;
using LinkedOut.Common.Domain;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using Microsoft.IdentityModel.Tokens;

namespace LinkedOut.Gateway.Helper;

public static class VerifyHelper
{
    private static readonly TokenProperties Token = TokenHelper.Token;
    
    private static readonly SymmetricSecurityKey Key = TokenHelper.Key;
    public static void VerifyToken(string? token)
    {
        if (Token.Enable is null or false)
        {
            return;
        }
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!jwtHandler.CanReadToken(token))
        {
            throw new ApiException(ResultCode.Unauthorized());
        }

        Console.WriteLine(Token.Issuer);
        Console.WriteLine(Token.Audience);
        var parameters = new TokenValidationParameters
        {
            ValidateAudience = true,
              
            ValidateIssuer = true,
            // 验证过期时间
            ValidateLifetime = false,
            // 验证秘钥
            ValidateIssuerSigningKey = true,
            // 
            ValidIssuer = Token.Issuer,
            // 读配置Audience
            ValidAudience = Token.Audience,
            // 设置生成token的秘钥
            IssuerSigningKey = Key
        };
        
        try
        {
            jwtHandler.ValidateToken(token, parameters, out var securityToken);

            var jwtToken = securityToken as JwtSecurityToken;

            var unifiedId = jwtToken?.Claims.Single(o=>o.Type.Equals("unifiedId")).Value;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new ApiException(ResultCode.Unauthorized());
        }
        catch (SecurityTokenException)
        {
            throw new ApiException(ResultCode.Unauthorized());
        }

    }
}