using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum.Sample.Domain
{
    public class ForumDomainExtension : DomainExtension
    {
        public override void OnInitialize(IServiceProvider serviceProvider, IDomainService domainService)
        {
            ForumDomainService domain = (ForumDomainService)domainService;
            domain.ForumQuery += Domain_ForumQuery;
        }

        private Task Domain_ForumQuery(IDomainExecutionContext context, ComBoost.Data.EntityQueryEventArgs<IForum> e)
        {
            e.Queryable = e.Queryable.Unwrap<IForum, Entity.Forum>().Where(t => t.IsDisplay).OrderBy(t => t.Order).Wrap<IForum, Entity.Forum>();
            return Task.CompletedTask;
        }
    }
}
