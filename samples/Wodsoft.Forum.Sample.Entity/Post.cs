using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.Forum;

namespace Wodsoft.Forum.Sample.Entity
{
    [DisplayColumn("Content", "CreateDate", false)]
    [EntityAuthentication(AllowAnonymous = false)]
    public class Post : EntityBase, IPost
    {
        [Required]
        public virtual string Content { get; set; }

        [Hide]
        public virtual Guid MemberId { get; set; }
        private Member _Member;
        [Required]
        [Hide(IsHiddenOnView = false)]
        public virtual Member Member { get { return _Member; } set { _Member = value; MemberId = value?.Index ?? Guid.Empty; } }

        [Hide]
        public virtual Guid ThreadId { get; set; }
        private Thread _Thread;
        [Required]
        public virtual Thread Thread { get { return _Thread; } set { _Thread = value; ThreadId = value?.Index ?? Guid.Empty; } }
        
        [Hide]
        [Required]
        public virtual bool IsDeleted { get; set; }

        IMember IPost.Member { get { return Member; } set { Member = (Member)value; } }

        IThread IPost.Thread { get { return Thread; } set { Thread = (Thread)value; } }
    }
}
