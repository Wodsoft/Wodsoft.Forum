using System;
using System.Collections.Generic;
using System.Text;
using Wodsoft.ComBoost.Data;
using Wodsoft.Forum;

namespace Wodsoft.ComBoost
{
    public static class ForumExtensions
    {
        public static void AddForumExtensions(this IDomainServiceProvider provider)
        {
            provider.AddExtension<ForumDomainService, PagerExtension>();
        }
    }
}
