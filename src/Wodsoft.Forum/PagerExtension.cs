using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data;

namespace Wodsoft.Forum
{
    public class PagerExtension : DomainExtension
    {
        public override void OnInitialize(IServiceProvider serviceProvider, IDomainService domainService)
        {
            ForumDomainService domain = (ForumDomainService)domainService;
            domain.ThreadQuery += Domain_ThreadQuery;
            domain.PostQuery += Domain_PostQuery;
        }

        private Task Domain_PostQuery(IDomainExecutionContext context, EntityQueryEventArgs<IPost> e)
        {
            var valueProvider = context.DomainContext.GetRequiredService<IValueProvider>();
            var page = valueProvider.GetValue<int?>("page");
            EntityPagerOption option = context.DomainContext.Options.GetOption<EntityPagerOption>();
            if (option == null)
            {
                option = new EntityPagerOption();
                context.DomainContext.Options.SetOption(option);
                option.CurrentSize = 10;
            }
            option.CurrentPage = page ?? 1;
            return Task.CompletedTask;
        }

        private Task Domain_ThreadQuery(IDomainExecutionContext context, ComBoost.Data.EntityQueryEventArgs<IThread> e)
        {
            var valueProvider = context.DomainContext.GetRequiredService<IValueProvider>();
            var page = valueProvider.GetValue<int?>("page");
            EntityPagerOption option = context.DomainContext.Options.GetOption<EntityPagerOption>();
            if (option == null) {
                option = new EntityPagerOption();
                context.DomainContext.Options.SetOption(option);
                option.CurrentSize = 20;
            }
            option.CurrentPage = page ?? 1;
            return Task.CompletedTask;
        }
    }
}
