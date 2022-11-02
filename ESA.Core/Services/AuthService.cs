
using ESA.Core.Interfaces;
using ESA.Core.Middleware;
using ESA.Core.Models.Auth;
using ESA.Core.Models.Session;
using ESA.Core.Specs;
using ESA.Core.Utils;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.Extensions.Options;

namespace ESA.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly Jwt jwtMiddleware;
        private readonly JwtSetting JwtSetting;
        private readonly IAppLogger<AuthService> logger;
        private readonly IReadRepository<Account> userReadRepository;

        public AuthService(
            Jwt jwtMiddleware,
            IOptions<JwtSetting> JwtSetting,
            IAppLogger<AuthService> logger,
            IReadRepository<Account> userReadRepository)
        {
            this.JwtSetting = JwtSetting.Value;
            this.jwtMiddleware = jwtMiddleware;
            this.logger = logger;
            this.userReadRepository = userReadRepository;
        }

        public async Task<Result<UserAccountInfo>> UserAuthenticationAsync(UserCredential userCredentials)
        {
            var result = new Result<UserAccountInfo>();
            try
            {
                var validator = new UserCredentialValidator();
                var validation = validator.Validate(userCredentials);
                if (!validation.IsValid)
                    return result.Invalid(validation.AsErrors());

                var ct = new CancellationTokenSource();
                ct.CancelAfter(TimeSpan.FromSeconds(30));
                var user = await userReadRepository.FirstOrDefaultAsync(new UserSpec(userCredentials), ct.Token);
                if (user == null)
                    return result.NotFound("Cuenta de usuario no existe");

                if (!user.IsActive)
                    return result.Unauthorized();

                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    var hash = Encrypt.GetHash(user.Password);
                    if (user.Password != hash)
                        return result.Unauthorized();
                }

                var token = jwtMiddleware.GenerateToken(JwtSetting, new JwtInfo
                {
                    Identifier = user.Id,
                    ServiceName = Guid.NewGuid().ToString(),
                    Username = string.IsNullOrWhiteSpace(user.Email) ? Guid.NewGuid().ToString() : user.Email
                });
                if (!token.IsSuccess)
                    return result.Conflict("Token de autenticación no generado");

                return result.Success(new UserAccountInfo
                {
                    Id = user.Id,
                    RoleId = user.RoleId,
                    Username = user.Email,
                    Authorization = token.Value
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, userCredentials);
                return result.Error("Ha ocurrido un error", ex.Message);
            }
        }
    }
}
