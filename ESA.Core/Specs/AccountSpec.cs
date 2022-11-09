using Ardalis.Specification;

namespace ESA.Core.Specs
{
    public class AccountSpec : Specification<Account>
    {
        public AccountSpec(string email, bool includeRelations = true)
        {
            if (includeRelations)
            {
                Query.Include(x => x.AccountProfile)
                    .Include(x => x.Role)
                    .Where(x => x.Email == email);
            }
            else
            {
                Query.Where(x => x.Email == email);
            }
        }

        public AccountSpec(short roleId, bool includeRelations = true)
        {
            if (includeRelations)
            {
                Query.Include(x => x.AccountProfile)
                    .Include(x => x.Role)
                    .Where(x => x.RoleId == roleId);
            }
            else
            {
                Query.Where(x => x.RoleId == roleId);
            }
        }

        public AccountSpec(int accountId, bool includeRelations = true)
        {
            if (includeRelations)
            {
                Query.Include(x => x.AccountProfile)
                    .Include(x => x.Role)
                    .Where(x => x.Id == accountId);
            }
            else
            {
                Query.Where(x => x.Id == accountId);
            }
        }
    }

    public class AccountProfileSpec : Specification<AccountProfile>
    {
        public AccountProfileSpec(int accountId, bool includeRelations = false)
        {
            if (includeRelations)
            {
                Query.Include(x => x.Account).Where(x => x.Id == accountId);
            }
            else
            {
                Query.Where(x => x.Id == accountId);
            }
        }
    }

    public class AccountSurveySpec : Specification<AccountSurvey>
    {
        public AccountSurveySpec(int accountId)
        {
            Query.Where(x => x.AccountId == accountId);
        }

        public AccountSurveySpec()
        {
            Query.Include(x=>x.Account);
        }
    }
}
