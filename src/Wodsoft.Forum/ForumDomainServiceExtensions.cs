using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.ComBoost.Security;

namespace Wodsoft.Forum
{
    public static class ForumDomainServiceExtensions
    {
        public static Task<IForum[]> ExecuteListForums(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, IForum[]>(context, domain.ListForums);
        }

        public static Task<IForum> ExecuteGetForum(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IForum>(context, domain.GetForum);
        }

        public static Task<IViewModel<IThread>> ExecuteListThreads(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IViewModel<IThread>>(context, domain.ListThreads);
        }

        public static Task<IThread> ExecuteGetThread(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IThread>(context, domain.GetThread);
        }

        public static Task<IThread> ExecuteCreateThread(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IThread>(context, domain.CreateThread);
        }

        public static Task<IPost> ExecuteEditThread(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IPost>(context, domain.EditThread);
        }

        public static Task<IThread> ExecuteUpdateThread(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, IAuthenticationProvider, string, string, IThread>(context, domain.UpdateThread);
        }

        public static Task ExecuteDeleteThread(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, IAuthenticationProvider, string>(context, domain.DeleteThread);
        }

        public static Task<IViewModel<IPost>> ExecuteListPosts(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IViewModel<IPost>>(context, domain.ListPosts);
        }

        public static Task<IPost> ExecuteCreatePost(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, IAuthenticationProvider, string, IPost>(context, domain.CreatePost);
        }

        public static Task<IPost> ExecuteEditPost(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, string, IPost>(context, domain.EditPost);
        }

        public static Task<IPost> ExecuteUpdatePost(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, IAuthenticationProvider, string, string, IPost>(context, domain.UpdatePost);
        }

        public static Task ExecuteDeletePost(this ForumDomainService domain, IDomainContext context)
        {
            return domain.ExecuteAsync<IDatabaseContext, IAuthenticationProvider, string>(context, domain.DeletePost);
        }
    }
}
