using ESA.Core.Models.Session;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ESA.Core.Middleware
{
    public class Jwt : IJwtMiddleware<JwtSetting, JwtInfo, JwtResponse, JwtStatus>
    {
        private readonly IAppLogger<Jwt> _logger;

        public Jwt(IAppLogger<Jwt> logger)
        {
            _logger = logger;
        }

        public Result<JwtResponse> GenerateToken(JwtSetting jwtSetting, JwtInfo tokenInfo)
        {
            var result = new Result<JwtResponse>();
            try
            {
                if (jwtSetting == null || tokenInfo == null)
                {
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = typeof(JwtResponse).Name,
                            ErrorMessage = "Información de token inválida",
                            Severity = ValidationSeverity.Warning
                        }
                    });
                }

                var validator = new JwtValidator();
                var validation = validator.Validate(tokenInfo);
                if (!validation.IsValid)
                    return result.Invalid(validation.AsErrors());

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sid, tokenInfo.Identifier.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, tokenInfo.ServiceName),
                    new Claim(JwtRegisteredClaimNames.Email, tokenInfo.Username),
                    new Claim(JwtRegisteredClaimNames.Sub, "api.secura.slv"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToLongDateString())
                };
                var token = new JwtSecurityToken(
                    issuer: jwtSetting.Issuer,
                    audience: jwtSetting.Audience,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(jwtSetting.DurationMinutes),
                    signingCredentials: credentials
                );

                return result.Success(new JwtResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expires = token.ValidTo,
                    TokenType = "jwt"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando el Jwt");
                return result.Error(nameof(GenerateToken), new[] { ex.Message });
            }
        }

        public Result<JwtStatus> ValidateToken(JwtSetting jwtSetting, string token)
        {
            var result = new Result<JwtStatus>();
            try
            {
                if (jwtSetting == null || string.IsNullOrWhiteSpace(token))
                {
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = typeof(JwtSetting).Name,
                            ErrorMessage = "Información de token inválida",
                            Severity = ValidationSeverity.Warning
                        }
                    });
                }

                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtSecurityToken = (JwtSecurityToken)validatedToken;
                var sid = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "sid")?.Value;
                if (int.TryParse(sid, out int identifier))
                {
                    return result.Success(new JwtStatus
                    {
                        Identifier = identifier,
                        IsActive = true
                    });
                }
                else return result.NotFound("Invalid identifier");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando el Jwt");
                return result.Error("Validating token", new[] { ex.Message });
            }
        }
    }
}
