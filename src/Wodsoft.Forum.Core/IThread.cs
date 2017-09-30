using System;
using System.Collections.Generic;
using System.Text;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum
{
    [EntityInterface]
    public interface IThread : IEntity
    {
        IMember Member { get; set; }

        IForum Forum { get; set; }

        string Title { get; set; }

        bool IsDeleted { get; set; }

        ICollection<IPost> Replies { get; }
    }
}
