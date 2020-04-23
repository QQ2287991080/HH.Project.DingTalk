using Dapper;
using DingTalk.Api.Response;
using HH.Project.DingTalk.Concrete;
using HH.Project.DingTalk.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk
{
    /// <summary>
    /// 外观类
    /// </summary>
   public class Facade
    {
        private IDepartment _department;
        private IPerson _person;
        private ISynchroFactory _factory;
        public Facade()
        {
            _factory = new SynchroFactory();
            _person = new ConcretePerson(_factory);
            _department = new ConcreteDepartment();
        }
        public void Operator(string connection)
        {
            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                SynchroFactory factory = new SynchroFactory();
                //同步部门信息
                string token = AccessToken.GetAccessToken();
                //移除部门-人员信息
                conn.Execute("Delete REF_Person_Department");
                //获取部门信息
                List<OapiDepartmentListResponse.DepartmentDomain> departments = factory.GetDepartmentList(token);
                //如果没有部门那么程序中断
                if (departments == null)
                {
                    return;
                }
                //递归部门
                _department.DingTalkDepartmentList(in departments, conn);
                //新增人员和用户并且返回需要删除的部门code和离职的用户钉钉id
                var DepartmentList = _person.AddPersonWithSysUser(departments, conn, token, out List<string> PersonList);
                //删除离职人员
                _person.RemovePersonWithSysUser(PersonList, conn);
                //删除需要删除的部门
                _department.RemoveNotInSystemDepartment(DepartmentList, conn);
            }
        }
            
    }
}
