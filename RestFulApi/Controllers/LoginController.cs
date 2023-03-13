using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestFulApi.Data;
using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestFulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly AppSettings _appSettings;

        public LoginController(MyDBContext context, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        public async Task<IActionResult>  Validate(MLogin model)
        {
            var user = _context.Users.SingleOrDefault(User => User.UserName == model.UserName && User.PassWord == model.PassWord);
            if(user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }

            //Cấp token

            var token = await GenerateToken(user);

            return Ok(new ApiResponse { 
                Success = true,
                Message ="Authenticate success",
                Data = token
            });        
        }

        private async Task<MToken>  GenerateToken(Users users)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes =  Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDecription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] {
                    new Claim(ClaimTypes.Name, users.FullName),
                    new Claim(JwtRegisteredClaimNames.Email, users.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, users.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", users.UserName),
                    new Claim("Id", users.Id.ToString()),


                    //roles

                }),

                Expires = DateTime.UtcNow.AddSeconds(15),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
               
            };
            var token = JwtTokenHandler.CreateToken(tokenDecription);
            var accessToken = JwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //save database

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = users.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(5)

            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new MToken
            {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }


        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RenewToken(MToken model)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters()
            {
                //Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //Ký vào token

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // Không kiểm tra token hết hạn
                

            };
            try
            {
                //check 1: Access Token valid format 
                var tokenInverification = JwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: check alg kiểm tra xem thuật toán đang được dùng

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }

        
                }
               

                //check 3: check accessToken expire ? 

                var utcExpireDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if(expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse { 
                        
                        Success = false,
                        Message = " Access Token has not yet expired"
                    
                    });
                }

                //check 4: check refreshtoken exist in db
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if(storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exists"
                    });
                
                }

                //check 5: checkrefreshToken is used/Revoked

                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }
                //check 6: AccessToken id = JwtId in RefreshToken

                var jti  = tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if(storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesnt match"
                    });
                }
            

                //Update token is used

                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;

                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new Token
                var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == storedToken.UserId);
                var token = await GenerateToken(user);
               

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Refresh Token success",
                    Data = token
                });


            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Somthing went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
