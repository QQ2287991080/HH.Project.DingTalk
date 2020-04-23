using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Domain
{
  public  class AC_SysUsers
    {
        public decimal Id { get; set; }
        public Nullable<decimal> IdSysSkin { get; set; }
        public string UserName { get; set; }
        public string SecurityStamp { get; set; }
        public Nullable<byte> PasswordType { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> PhoneNumberConfirmed { get; set; }
        public short AccessFailedCount { get; set; }
        public bool IsMutiLogin { get; set; }
    }
}
