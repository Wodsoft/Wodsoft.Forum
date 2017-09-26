using System;
using System.Collections.Generic;
using System.Text;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum
{
    [EntityInterface]
    public interface IForum : IEntity
    {
        string Name { get; set; }

        IBoard Board { get; set; }
    }
}
