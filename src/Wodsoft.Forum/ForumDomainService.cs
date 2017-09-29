using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.ComBoost.Data.Entity.Metadata;
using Wodsoft.ComBoost.Security;

namespace Wodsoft.Forum
{
    public class ForumDomainService : DomainService
    {
        private MethodInfo _ForumCastMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(EntityDescriptor.GetMetadata<IForum>().Type);
        private MethodInfo _ForumToArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(EntityDescriptor.GetMetadata<IForum>().Type);
        public static readonly DomainServiceEventRoute ForumQueryEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityQueryEventArgs<IForum>>("ForumQuery", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityQueryEventArgs<IForum>> ForumQuery { add { AddAsyncEventHandler(ForumQueryEvent, value); } remove { RemoveAsyncEventHandler(ForumQueryEvent, value); } }
        public async Task<IForum[]> ListForums([FromService]IDatabaseContext databaseContext)
        {
            var context = databaseContext.GetWrappedContext<IForum>();
            var queryable = context.Query().Include(t => t.Board);
            IValueProvider valueProvider = Context.DomainContext.GetRequiredService<IValueProvider>();
            string id = valueProvider.GetValue<string>("id");
            if (id != null)
            {
                var converter = TypeDescriptor.GetConverter(EntityDescriptor.GetMetadata<IBoard>().KeyType);
                var idObj = converter.ConvertFrom(id);
                queryable = queryable.Where(t => t.Board.Index == idObj);
            }
            var e = new EntityQueryEventArgs<IForum>(queryable);
            await RaiseAsyncEvent(ForumQueryEvent, e);
            queryable = e.Queryable;
            var items = await queryable.ToArrayAsync();
            var obj = _ForumCastMethod.Invoke(null, new object[] { items });
            obj = _ForumToArrayMethod.Invoke(null, new object[] { obj });
            return (IForum[])obj;
        }

        public static readonly DomainServiceEventRoute ForumReadEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IForum>>("ForumRead", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IForum>> ForumRead { add { AddAsyncEventHandler(ForumReadEvent, value); } remove { RemoveAsyncEventHandler(ForumReadEvent, value); } }
        public async Task<IForum> GetForum([FromService]IDatabaseContext databaseContext, [FromValue]string id)
        {
            var context = databaseContext.GetWrappedContext<IForum>();
            var item = await context.GetAsync(id);
            if (item == null)
                throw new DomainServiceException(new KeyNotFoundException(id));
            await item.LoadAsync(t => t.Board);
            var e = new EntityFilterEventArgs<IForum>(item);
            await RaiseAsyncEvent(ForumReadEvent, e);
            return item;
        }

        private Type _ThreadViewModelType = typeof(ViewModel<>).MakeGenericType(EntityDescriptor.GetMetadata<IThread>().Type);
        private MethodInfo _ThreadUnwrapMethod = typeof(QueryableExtensions).GetMethod("Unwrap").MakeGenericMethod(typeof(IThread), EntityDescriptor.GetMetadata<IThread>().Type);
        public static readonly DomainServiceEventRoute ThreadQueryEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityQueryEventArgs<IThread>>("ThreadQuery", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityQueryEventArgs<IThread>> ThreadQuery { add { AddAsyncEventHandler(ThreadQueryEvent, value); } remove { RemoveAsyncEventHandler(ThreadQueryEvent, value); } }
        public async Task<IViewModel<IThread>> ListThreads([FromService]IDatabaseContext databaseContext, [FromValue]string id)
        {
            var converter = TypeDescriptor.GetConverter(EntityDescriptor.GetMetadata<IThread>().KeyType);
            var idObj = converter.ConvertFrom(id);
            var context = databaseContext.GetWrappedContext<IThread>();
            var queryable = context.Query().Include(t => t.Member).Where(t => t.Forum.Index == idObj);
            var e = new EntityQueryEventArgs<IThread>(queryable);
            await RaiseAsyncEvent(ThreadQueryEvent, e);
            queryable = e.Queryable;
            if (e.IsOrdered)
                queryable = ((IOrderedQueryable<IThread>)queryable).ThenByDescending(t => t.CreateDate);
            else
                queryable = queryable.OrderByDescending(t => t.CreateDate);
            IViewModel model = (IViewModel)Activator.CreateInstance(_ThreadViewModelType, _ThreadUnwrapMethod.Invoke(null, new object[] { queryable }));
            EntityPagerOption pagerOption = Context.DomainContext.Options.GetOption<EntityPagerOption>();
            if (pagerOption != null)
                model.SetSize(pagerOption.CurrentSize);
            await model.UpdateTotalPageAsync();
            if (pagerOption != null)
                model.SetPage(pagerOption.CurrentPage);
            await model.UpdateItemsAsync();
            return (IViewModel<IThread>)model;
        }

        public static readonly DomainServiceEventRoute ThreadReadEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IThread>>("ThreadRead", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IThread>> ThreadRead { add { AddAsyncEventHandler(ThreadReadEvent, value); } remove { RemoveAsyncEventHandler(ThreadReadEvent, value); } }
        public async Task<IThread> GetThread([FromService]IDatabaseContext databaseContext, [FromValue]string id)
        {
            var context = databaseContext.GetWrappedContext<IThread>();
            var item = await context.GetAsync(id);
            if (item == null)
                throw new DomainServiceException(new KeyNotFoundException(id));
            var forum = await item.LoadAsync(t => t.Forum);
            await forum.LoadAsync(t => t.Board);
            var e = new EntityFilterEventArgs<IThread>(item);
            await RaiseAsyncEvent(ThreadReadEvent, e);
            return item;
        }

        public static readonly DomainServiceEventRoute ForumWriteEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IForum>>("ForumWrite", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IForum>> ForumWrite { add { AddAsyncEventHandler(ForumWriteEvent, value); } remove { RemoveAsyncEventHandler(ForumWriteEvent, value); } }
        public static readonly DomainServiceEventRoute ThreadCreateEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IThread>>("ThreadCreate", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IThread>> ThreadCreate { add { AddAsyncEventHandler(ThreadCreateEvent, value); } remove { RemoveAsyncEventHandler(ThreadCreateEvent, value); } }
        public static readonly DomainServiceEventRoute PostUpdateEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IPost>>("PostUpdate", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IPost>> PostUpdate { add { AddAsyncEventHandler(PostUpdateEvent, value); } remove { RemoveAsyncEventHandler(PostUpdateEvent, value); } }
        public async Task<IThread> CreateThread([FromService]IDatabaseContext databaseContext, [FromService]IAuthenticationProvider authenticationProvider, [FromValue]string forumId)
        {
            IValueProvider valueProvider = Context.DomainContext.GetRequiredService<IValueProvider>();
            IForum forum;
            IThread thread;
            {
                var context = databaseContext.GetWrappedContext<IForum>();
                forum = await context.GetAsync(forumId);
                if (forum == null)
                    throw new DomainServiceException(new KeyNotFoundException(forumId));
                var e = new EntityFilterEventArgs<IForum>(forum);
                await RaiseAsyncEvent(ForumWriteEvent, e);
            }
            {
                var context = databaseContext.GetWrappedContext<IThread>();
                thread = context.Create();
                thread.Forum = forum;
                thread.Member = await authenticationProvider.GetAuthentication().GetUserAsync<IMember>();
                thread.Title = valueProvider.GetValue<string>("title");
                var e = new EntityFilterEventArgs<IThread>(thread);
                await RaiseAsyncEvent(ThreadCreateEvent, e);
                context.Add(thread);
            }
            {
                var context = databaseContext.GetWrappedContext<IPost>();
                var post = context.Create();
                post.Thread = thread;
                post.Member = thread.Member;
                post.Content = valueProvider.GetValue<string>("content");
                var e = new EntityFilterEventArgs<IPost>(post);
                await RaiseAsyncEvent(PostUpdateEvent, e);
                context.Add(post);
            }
            await databaseContext.SaveAsync();
            return thread;
        }

        public static readonly DomainServiceEventRoute ThreadEditEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IThread>>("ThreadEdit", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IThread>> ThreadEdit { add { AddAsyncEventHandler(ThreadEditEvent, value); } remove { RemoveAsyncEventHandler(ThreadEditEvent, value); } }
        public async Task<IThread> EditThread([FromService]IDatabaseContext databaseContext, [FromValue]string id)
        {
            IValueProvider valueProvider = Context.DomainContext.GetRequiredService<IValueProvider>();
            IThread thread;
            {
                var context = databaseContext.GetWrappedContext<IThread>();
                thread = await context.GetAsync(id);
                thread.Title = valueProvider.GetValue<string>("title");
                var e = new EntityFilterEventArgs<IThread>(thread);
                await RaiseAsyncEvent(ThreadEditEvent, e);
                context.Update(thread);
            }
            {
                var context = databaseContext.GetWrappedContext<IPost>();
                var posts = await thread.LoadAsync(t => t.Replies);
                var post = await posts.OrderBy(t => t.CreateDate).FirstOrDefaultAsync();
                if (post == null)
                    throw new InvalidOperationException("回贴不存在。");
                post.Content = valueProvider.GetValue<string>("content");
                var e = new EntityFilterEventArgs<IPost>(post);
                await RaiseAsyncEvent(PostUpdateEvent, e);
                context.Update(post);
            }
            await databaseContext.SaveAsync();
            return thread;
        }

        private Type _PostViewModelType = typeof(ViewModel<>).MakeGenericType(EntityDescriptor.GetMetadata<IPost>().Type);
        private MethodInfo _PostUnwrapMethod = typeof(QueryableExtensions).GetMethod("Unwrap").MakeGenericMethod(typeof(IPost), EntityDescriptor.GetMetadata<IPost>().Type);
        public static readonly DomainServiceEventRoute PostQueryEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityQueryEventArgs<IPost>>("PostQuery", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityQueryEventArgs<IPost>> PostQuery { add { AddAsyncEventHandler(PostQueryEvent, value); } remove { RemoveAsyncEventHandler(PostQueryEvent, value); } }
        public async Task<IViewModel<IPost>> ListPosts([FromService]IDatabaseContext databaseContext, [FromValue]string id)
        {
            var converter = TypeDescriptor.GetConverter(EntityDescriptor.GetMetadata<IThread>().KeyType);
            var idObj = converter.ConvertFrom(id);
            var context = databaseContext.GetWrappedContext<IPost>();
            var queryable = context.Query().Include(t => t.Member).Where(t => t.Thread.Index == idObj);
            var e = new EntityQueryEventArgs<IPost>(queryable);
            await RaiseAsyncEvent(PostQueryEvent, e);
            queryable = e.Queryable;
            if (e.IsOrdered)
                queryable = ((IOrderedQueryable<IPost>)queryable).ThenBy(t => t.CreateDate);
            else
                queryable = queryable.OrderBy(t => t.CreateDate);
            IViewModel model = (IViewModel)Activator.CreateInstance(_PostViewModelType, _PostUnwrapMethod.Invoke(null, new object[] { queryable }));
            EntityPagerOption pagerOption = Context.DomainContext.Options.GetOption<EntityPagerOption>();
            if (pagerOption != null)
                model.SetSize(pagerOption.CurrentSize);
            await model.UpdateTotalPageAsync();
            if (pagerOption != null)
                model.SetPage(pagerOption.CurrentPage);
            await model.UpdateItemsAsync();
            return (IViewModel<IPost>)model;
        }

        public static readonly DomainServiceEventRoute ThreadWriteEvent = DomainServiceEventRoute.RegisterAsyncEvent<EntityFilterEventArgs<IThread>>("ThreadWrite", typeof(ForumDomainService));
        public event DomainServiceAsyncEventHandler<EntityFilterEventArgs<IThread>> ThreadWrite { add { AddAsyncEventHandler(ThreadWriteEvent, value); } remove { RemoveAsyncEventHandler(ThreadWriteEvent, value); } }
        public async Task<IPost> CreatePost([FromService]IDatabaseContext databaseContext, [FromService]IAuthenticationProvider authenticationProvider, [FromValue]string threadId)
        {
            IValueProvider valueProvider = Context.DomainContext.GetRequiredService<IValueProvider>();
            IThread thread;
            IPost post;
            {
                var context = databaseContext.GetWrappedContext<IThread>();
                thread = await context.GetAsync(threadId);
                if (thread == null)
                    throw new DomainServiceException(new KeyNotFoundException(threadId));
                var e = new EntityFilterEventArgs<IThread>(thread);
                await RaiseAsyncEvent(ThreadWriteEvent, e);
            }
            {
                var context = databaseContext.GetWrappedContext<IPost>();
                post = context.Create();
                post.Thread = thread;
                post.Member = await authenticationProvider.GetAuthentication().GetUserAsync<IMember>();
                post.Content = valueProvider.GetValue<string>("content");
                var e = new EntityFilterEventArgs<IPost>(post);
                await RaiseAsyncEvent(PostUpdateEvent, e);
                context.Add(post);
            }
            await databaseContext.SaveAsync();
            return post;
        }

        public async Task<IPost> EditPost([FromService]IDatabaseContext databaseContext, [FromValue]string id)
        {
            IValueProvider valueProvider = Context.DomainContext.GetRequiredService<IValueProvider>();
            IPost post;
            {
                var context = databaseContext.GetWrappedContext<IPost>();
                post = await context.GetAsync(id);
                post.Content = valueProvider.GetValue<string>("content");
                var e = new EntityFilterEventArgs<IPost>(post);
                await RaiseAsyncEvent(PostUpdateEvent, e);
                context.Update(post);
            }
            await databaseContext.SaveAsync();
            return post;
        }


    }
}
