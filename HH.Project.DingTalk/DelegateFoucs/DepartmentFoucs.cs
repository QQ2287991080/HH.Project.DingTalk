using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.DelegateFoucs
{
    public class DepartmentFoucs
    {
        public bool Valid(IDbConnection conn,string item)
        {
            //判断是否用子部门被使用
            var list = conn.Query<string>("select Name from OR_Department where CodeDepartment=@Code", new { Code = item });
            if (list.Count()>0)
            {
                return false;
            }
            var isRef = conn.QueryFirstOrDefault<int>(" select count(*) from REF_Person_Department where CodeDepartment=@Code", new { Code = item });
            if (isRef>0)
            {
                return false;
            }
            var isRef2 = conn.QueryFirstOrDefault<int>(" SELECT COUNT(*) FROM  dbo.REF_Department_TakeFoodTime WHERE CodeDepartment=@Code", new { Code = item });
            if (isRef2>0)
            {
                return false;
            }
            return true;
        }
    }
}
