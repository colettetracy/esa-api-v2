using Ardalis.Specification;
using ESA.Core.Entities;
using ESA.Core.Models.Auth;

namespace ESA.Core.Specs
{
    public class UserSpec : Specification<Account>, ISingleResultSpecification
    {
        public UserSpec(UserCredential credentials)
        {
            Query.Include(r => r.AccountProfile)
                .Include(r => r.Role)
                .Where(x => x.Email == credentials.Email);
        }
    }
}
