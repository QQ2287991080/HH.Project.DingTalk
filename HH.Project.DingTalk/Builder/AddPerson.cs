using Dapper;
using DingTalk.Api.Response;
using HH.Project.DingTalk.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Builder
{
    public class AddPerson : PersonBuilder
    {
       
        public override void SelectPerson(OapiUserListResponse.UserlistDomain user, IDbConnection conn,string code)
        {
            var person = conn.QueryFirstOrDefault<OR_Person>("select * from OR_Person where PsnNum='" + user.Userid + "'");
            IsPersonFlag = person == null ? true : false;
            _person.OR_Person = person;
            Partmeters= new
            {
                PsnNum = user.Userid,
                Name = user.Name,
                CodeDepartment =code,
                Phone = user.Mobile,
                HeadUrl = user.Avatar,
                LoginName = user.Userid
            };
        }
        public override OR_Person InsertPerson(IDbConnection conn)
        {
            string insertSql = "insert into OR_Person (PsnNum,Name,CodeDepartment,PsnMobilePhone,HeadUrl,LoginName,CreateTime) values (@PsnNum,@Name,@CodeDepartment,@Phone,@HeadUrl,@LoginName,'" + DateTime.Now + "') SELECT CAST(SCOPE_IDENTITY() as numeric(18, 0))";
            var id= conn.Query<decimal>(insertSql, Partmeters).FirstOrDefault();
            _person.OR_Person = conn.QueryFirstOrDefault<OR_Person>("select * from OR_Person where Id=@Id", new { Id = id });
            return _person.OR_Person;
        }

        public override OR_Person UpdatePerson(IDbConnection conn)
        {
            string updateSql = "update OR_Person set PsnNum=@PsnNum,Name=@Name,CodeDepartment=@CodeDepartment,PsnMobilePhone=@Phone,HeadUrl=@HeadUrl,LoginName=@LoginName where PsnNum=@PsnNum";
            conn.Execute(updateSql, Partmeters);
            return _person.OR_Person ;
        }

        public override void SetRefDepartment(IDbConnection conn, string code, decimal id)
        {
            string REF_Person_DepartmentSql = "Insert into REF_Person_Department(CodeDepartment,IdPerson) values (@Code,@Id)";
            conn.Execute(REF_Person_DepartmentSql, new { Code = code, Id = id });
        }
    }
}
