using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Domain
{
   public class OR_Person
    {
        public decimal Id { get; set; }
        public Nullable<decimal> IdDirectSuperior { get; set; }
        public string PsnNum { get; set; }
        public string Name { get; set; }
        public string NamePinYin { get; set; }
        public string NameCharFirst { get; set; }
        public string CodeDepartment { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public string LeaveReson { get; set; }
        public Nullable<decimal> EnJob { get; set; }
        public Nullable<decimal> EnPersonType { get; set; }
        public short LearnTimes { get; set; }
        public Nullable<decimal> IdOriginArea { get; set; }
        public string JobTitle { get; set; }
        public string JobNumber { get; set; }
        public byte Sex { get; set; }
        public string Skills { get; set; }
        public string IdentityDesc { get; set; }
        public string PsnMobilePhone { get; set; }
        public string PsnEmail { get; set; }
        public string PsnQQCode { get; set; }
        public string HeadUrl { get; set; }
        public Nullable<bool> IsHaveAccount { get; set; }
        public string LoginName { get; set; }
        public string OrgEmail { get; set; }
        public Nullable<decimal> IdSysArea { get; set; }
        public string WorkPlace { get; set; }
        public string Seat { get; set; }
        public string Ext { get; set; }
        public string Memo { get; set; }
    }
}
