using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.ComBoost;

namespace Wodsoft.Forum.Sample
{
    public class AliasFilter : DomainServiceFilterAttribute
    {
        public override Task OnExecutingAsync(IDomainExecutionContext context)
        {
            var valueProvider = context.DomainContext.GetRequiredService<IValueProvider>() as IConfigurableValueProvider;
            if (valueProvider != null)
                valueProvider.SetAlias("id", "Index");
            return Task.CompletedTask;
        }
    }
}
