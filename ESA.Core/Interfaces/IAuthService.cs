using ESA.Core.Models.Auth;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserAccountInfo>> UserAuthenticationAsync(UserCredential userCredentials);
    }
}
