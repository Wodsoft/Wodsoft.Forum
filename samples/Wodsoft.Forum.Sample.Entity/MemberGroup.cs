using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wodsoft.ComBoost.Data.Entity;

namespace Wodsoft.Forum.Sample.Entity
{
    [DisplayColumn("Username", "Point", false)]
    [DisplayName("用户组")]
    [EntityAuthentication(AllowAnonymous = false,
        AddRolesRequired = new object[] { AdminType.Admin },
        EditRolesRequired = new object[] { AdminType.Admin },
        RemoveRolesRequired = new object[] { AdminType.Admin })]
    public class MemberGroup : EntityBase
    {
        [Required]
        [Display(Name = "用户名", Order = 0)]
        public virtual string Name { get; set; }

        [Required]
        [Display(Name = "系统组", Order = 20)]
        public virtual bool IsSystem { get; set; }

        [Required]
        [Display(Name = "所需分数", Order = 10)]
        public virtual int Point { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public override bool IsRemoveAllowed => !IsSystem;
    }
}
