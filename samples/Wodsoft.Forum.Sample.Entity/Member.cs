using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.ComBoost;
using Wodsoft.ComBoost.Data.Entity;
using Wodsoft.Forum;

namespace Wodsoft.Forum.Sample.Entity
{
    [DisplayColumn("Username", "CreateDate", true)]
    [DisplayName("用户")]
    [EntityAuthentication(AllowAnonymous = false,
        AddRolesRequired = new object[] { AdminType.Admin },
        EditRolesRequired = new object[] { AdminType.Admin },
        RemoveRolesRequired = new object[] { AdminType.Admin })]
    public class Member : UserBase, IMember, IPermission
    {
        [Searchable]
        [Display(Name = "用户名", Order = 0)]
        [Required]
        public string Username { get; set; }

        [Hide(IsHiddenOnView = false)]
        [Display(Name = "创建时间", Order = 20)]
        [Searchable]
        public override DateTime CreateDate { get { return base.CreateDate; } set { base.CreateDate = value; } }

        [Display(Name = "密码", Order = 10)]
        [Hide(IsHiddenOnEdit = false, IsHiddenOnCreate = false, IsHiddenOnDetail = true, IsHiddenOnView = true)]
        [CustomDataType(CustomDataType.Password)]
        [Required]
        public override byte[] Password { get { return base.Password; } set { base.Password = value; } }

        [Required]
        [Display(Name = "用户组", Order = 40)]
        public virtual MemberGroup Group { get; set; }

        [Hide]
        public ICollection<Thread> Threads { get; set; }

        string IPermission.Identity { get { return Index.ToString(); } }

        string IPermission.Name { get { return Username; } }

        object[] IPermission.GetStaticRoles()
        {
            return new object[0];
        }

        private static readonly Guid _AdminGroupId = new Guid("80000000-1000-0000-0003-000000000000");
        private static readonly Guid _SuperModeratorGroupId = new Guid("80000000-1000-0000-0003-000000000001");
        private static readonly Guid _ModeratorGroupId = new Guid("80000000-1000-0000-0003-000000000002");
        bool IPermission.IsInRole(object role)
        {
            if (role is AdminType type)
            {
                var currentType = AdminType.None;
                if (Group.Index == _AdminGroupId)
                    currentType = AdminType.Admin;
                else if (Group.Index == _SuperModeratorGroupId)
                    currentType = AdminType.SuperModerator;
                else if (Group.Index == _ModeratorGroupId)
                    currentType = AdminType.Moderator;
                return (int)currentType >= (int)type;
            }
            return false;
        }

        ICollection<IThread> IMember.Threads { get { throw new NotSupportedException(); } }
    }
}
