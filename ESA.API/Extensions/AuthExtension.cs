using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace ESA.API.Extensions
{
    public static class AuthExtension
    {
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtSetting:Issuer"],
                        ValidAudience = configuration["JwtSetting:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSetting:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/json";

                                if (string.IsNullOrEmpty(context.Error))
                                    context.Error = "Token invalido";
                                if (string.IsNullOrEmpty(context.ErrorDescription))
                                    context.ErrorDescription = "Esta solicitud requiere que se proporcione un token de acceso válido";

                                if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                                {
                                    var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                                    context.Response.Headers.Add("x-token-expired", authenticationException?.Expires.ToString("o"));
                                    context.ErrorDescription = $"Token caducó el {authenticationException?.Expires}";
                                }

                                context.Response.ContentType = "application/json";
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                return context.Response.WriteAsJsonAsync(new ValidationError
                                {
                                    Identifier = context.Error,
                                    ErrorMessage = context.ErrorDescription,
                                    Severity = ValidationSeverity.Error
                                });
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("x-token-expired", "true");

                            var ls = new List<string>();
                            foreach (var item in context.Exception.Data.Values)
                            {
                                ls.Add((string)item);
                            }
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return context.Response.WriteAsJsonAsync(new ValidationError
                            {
                                Identifier = "AuthenticationFailed",
                                ErrorMessage = string.Join(", ", ls.ToArray()),
                                Severity = ValidationSeverity.Error
                            });
                        }
                    };
                });
        }
    }
}
