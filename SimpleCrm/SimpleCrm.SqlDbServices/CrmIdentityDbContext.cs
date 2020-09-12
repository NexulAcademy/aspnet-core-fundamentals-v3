using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SimpleCrm.SqlDbServices
{
    public class CrmIdentityDbContext : IdentityDbContext<CrmUser>
    {
        public CrmIdentityDbContext(DbContextOptions<CrmIdentityDbContext> options)
            :base(options)
        {
        }
    }
}
