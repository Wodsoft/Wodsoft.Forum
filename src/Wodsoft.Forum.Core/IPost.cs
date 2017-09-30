using System;
using System.Collections.Generic;
using System.Text;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum
{
    [EntityInterface]
    public interface IPost : IEntity
    {
        IMember Member { get; set; }

        string Content { get; set; }

        IThread Thread { get; set; }

        bool IsDeleted { get; set; }
    }
}
