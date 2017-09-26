using System;
using System.Collections.Generic;
using System.Text;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum
{
    [EntityInterface]
    public interface IBoard : IEntity
    {
        string Name { get; set; }

        ICollection<IForum> Forums { get; }
    }
}
