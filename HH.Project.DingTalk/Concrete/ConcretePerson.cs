using Dapper;
using DingTalk.Api.Response;
using HH.Project.DingTalk.Builder;
using HH.Project.DingTalk.Domain;
using HH.Project.DingTalk.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Concrete
{
    public class ConcretePerson : IPerson
    {
        private string DingTalk = ConfigurationManager.AppSettings["DingTalk"];
        private string Office = ConfigurationManager.AppSettings["Office"];
        private ISynchroFactory _SynchroFactory;
        public ConcretePerson(ISynchroFactory synchroFactory)
        {
            _SynchroFactory = synchroFactory;
        }
        public List<long> AddPersonWithSysUser(List<OapiDepartmentListResponse.DepartmentDomain> departments, IDbConnection conn, string token, out List<string> PersonList)
        {
            //获取钉钉部门和人员集合
            List<long> DepartmentList = new List<long>();
            PersonList = new List<string>();
            //获取人员信息
            List<OR_Person> personList = conn.Query<OR_Person>("select * from OR_Person").ToList();
            //获取用户信息
            List<AC_SysUsers> userList = conn.Query<AC_SysUsers>("select * from  AC_SysUsers ").ToList();
            foreach (var item in departments.OrderBy(k => k.Id))
            {
                //部门人员
                //添加读取该部门所有人员信息
                var personlist = _SynchroFactory.UserSimplelistResponse(token, item.Id);
                if (personlist == null)
                {
                    continue;
                }
                bool flag = false;
                foreach (var user in personlist ?? new List<OapiUserListResponse.UserlistDomain>())
                {
                    if (user.Name.Contains("007"))
                    {
                        Console.WriteLine();
                    }
                    //人员建造者
                    PersonController controller = new PersonController(user,conn,item.Id.ToString());
                    PersonBuilder add = new AddPerson();
                    var person= controller.Construct(add);
                    Console.WriteLine("【" + user.Name + "】人员-同步成功");
                    //同步信息到账户表
                    var userDetails = conn.QueryFirstOrDefault<OR_Person>("select * from AC_SysUsers where UserName='" + user.Userid + "'");
                    if (userDetails == null)
                    {
                        string insertSql = "INSERT  INTO  AC_SysUsers  (UserName,PasswordType,PasswordHash,PhoneNumber,Email) VALUES(@UserName,1,@PasswordHash,@PhoneNumber,@Email) SELECT CAST(SCOPE_IDENTITY() as numeric(18, 0))";
                        var acId = conn.Query<decimal>(insertSql, new { UserName = user.Userid, PasswordHash = user.Userid, PhoneNumber = user.Mobile, Email = user.Email ?? "" });
                        //同步到AC_AccountBinding表
                        string insertBingdingSql = "INSERT  INTO  AC_AccountBinding(GidThirdSystem,IdSysUsers,UnifiedID) VALUES(@Sys,@AC,@UnionId)";
                        conn.Execute(insertBingdingSql, new { Sys = DingTalk, AC = acId, UnionId = user.Unionid });
                        //同步到第三方中间表
                        string thirdSystemSql = "INSERT AUTH_User_ThirdSystem (IdSysUsers,GidThirdSystem,AUTH)VALUES (@AC,@Sys,1)";
                        conn.Execute(thirdSystemSql, new[] { new { AC = acId, Sys = DingTalk }, new { AC = acId, Sys = Office } });
                        //一卡通表新增数据
                        string cardSql = "INSERT dbo.DB_MetroCard(Creater,IdCreater,CardNumber)VALUES( @Creater,@Id,@Card)";
                        conn.Execute(cardSql, new { Id = person.OR_Person.Id, Card = user.Userid, Creater = user.Name });
                    }
                  
                    //部门及人员 赋值
                    PersonList.Add(user.Userid);
                    flag = true;
                }
                if (flag)
                {
                    DepartmentList.Add(item.Id);
                }
            }
            return DepartmentList;
        }

        public void RemovePersonWithSysUser(List<string> PersonList, IDbConnection conn)
        {
            //离职人员 -(我们系统存在钉钉不存在)
            var employee = conn.Query<string>("select PsnNum from OR_Person ").ToList();
            //获取人员信息
            List<OR_Person> nowPersonList = conn.Query<OR_Person>("select * from OR_Person").ToList();
            foreach (var item in employee.Except(PersonList))
            {
                var yee = nowPersonList.FirstOrDefault(k => k.PsnNum == item);
                //yee.CodeDepartment = "1";
                //yee.LeaveDate = DateTime.Now;
                string updateSql = "Update OR_Person set CodeDepartment=@Code,LeaveDate=@Date where PsnNum=@item";
                string removeSql = "Delete  From REF_Person_Department where IdPerson=@Id";
                conn.Execute(updateSql, new { Code = "1", Date = DateTime.Now,item });
                conn.Execute(removeSql, new { Id = yee.Id });
                Console.WriteLine("【" + yee.Name + "】人员-离职成功");
            }
        }
    }
}
