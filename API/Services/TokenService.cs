﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AsparagusN.Data.Entities.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AsparagusN.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration con)
    {
        _configuration = con;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));
    }

    public string CreateToken(AppUser user)
    {
        var claims = new  List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            
        };
        var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha384Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

public interface ITokenService
{
    string CreateToken(AppUser user);
}