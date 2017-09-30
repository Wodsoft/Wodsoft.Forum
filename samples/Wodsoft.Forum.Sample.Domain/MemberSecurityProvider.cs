using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.ComBoost.Security;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample.Domain
{
    public class ForumSecurityProvider : GeneralSecurityProvider
    {
        public ForumSecurityProvider(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }

        private IServiceProvider _ServiceProvider;

        private IDatabaseContext _DatabaseContext;
        public IDatabaseContext DatabaseContext
        {
            get
            {
                if (_DatabaseContext == null)
                    _DatabaseContext = _ServiceProvider.GetRequiredService<IDatabaseContext>();
                return _DatabaseContext;
            }
        }

        protected override async Task<IPermission> GetPermissionByIdentity(string identity)
        {
            var memberContext = DatabaseContext.GetContext<Member>();
            Guid id = Guid.Parse(identity);
            var item = await memberContext.Query().Include(t => t.Group).Where(t => t.Index == id).SingleOrDefaultAsync();
            return item;
        }

        protected override async Task<IPermission> GetPermissionByUsername(string username)
        {
            username = username.ToLower();
            var memberContext = DatabaseContext.GetContext<Member>();
            var item = await memberContext.Query().Include(t => t.Group).Where(t => t.Username.ToLower() == username).SingleOrDefaultAsync();
            return item;
        }

        public override async Task<IPermission> GetPermissionAsync(IDictionary<string, string> properties)
        {
            var user = (Member)await base.GetPermissionAsync(properties);
            if (user != null && properties.ContainsKey("password") && !user.VerifyPassword(properties["password"]))
                return null;
            return user;
        }
    }
}
