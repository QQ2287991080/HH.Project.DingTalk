using Dapper;
using DingTalk.Api.Response;
using HH.Project.DingTalk.DelegateFoucs;
using HH.Project.DingTalk.Domain;
using HH.Project.DingTalk.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Concrete
{
  public class ConcreteDepartment : IDepartment
    {
        public void DingTalkDepartmentList(in List<OapiDepartmentListResponse.DepartmentDomain> departments, IDbConnection conn, long? Id = 0)
        {
            foreach (var item in departments.Where(k => k.Parentid == Id).ToList())
            {
                //读取部门详情信息
                var info = conn.QueryFirstOrDefault<OR_Department>("Select * from Or_Department where Code=@Code", new { Code = item.Id.ToString() });
                if (info == null)
                {
                    conn.Execute("insert into Or_Department (Code,Name,CodeDepartment,FullName) values(@Code,@Name,@CodeDepartment,@Name)  ",
                        new
                        {
                            Code = item.Id.ToString(),
                            Name = item.Name,
                            CodeDepartment = item.Parentid.ToString() == "0" ? null : item.Parentid.ToString()
                        });
                }
                else
                {
                    string updateSql = "Update OR_Department Set Name=@Name,CodeDepartment=@Code2,FullName=@FullName where Code=@Code";
                    conn.Execute(updateSql, new
                    {
                        Name = item.Name,
                        Code2 = item.Parentid.ToString() == "0" ? null : item.Parentid.ToString(),
                        FullName = item.Name,
                        Code = item.Id
                    });
                }
                Console.WriteLine("【" + item.Name + "】部门-同步成功");
                DingTalkDepartmentList(departments, conn, item.Id);
            }
        }

        public void RemoveNotInSystemDepartment(List<long> DepartmentList, IDbConnection conn)
        {
            //删除部门-(我们系统存在钉钉不存在)
            var Deletedepartment = conn.Query<string>("select * from OR_Department ").Select(p => Convert.ToInt64(p));
            var delete = Deletedepartment.Except(DepartmentList).OrderByDescending(p => p);
            foreach (var item in delete)
            {
                string name = conn.QueryFirstOrDefault<string>("select Name from OR_Department where Code=@Code", new { Code = item.ToString() });
                if (name == null)
                {
                    continue;
                }
                DepartmentFoucs facus = new DepartmentFoucs();
                if (!facus.Valid(conn,item.ToString()).Invoke())
                {
                    continue;
                }
                //递归删除
                string deleteSql = "delete from  OR_Department  where Code=@Code";
                conn.Execute(deleteSql, new { Code = item.ToString() });
                Console.WriteLine("【" + name + "】部门-删除成功");
            }
        }
    }
}
