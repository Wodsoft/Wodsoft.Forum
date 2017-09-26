using System;
using System.Collections.Generic;
using System.Text;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum
{
    [EntityInterface]
    public interface IMember : IEntity
    {
        string Username { get; set; }

        ICollection<IThread> Threads { get; }
    }
}
