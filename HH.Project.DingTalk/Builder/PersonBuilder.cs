using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Builder
{
   public abstract class PersonBuilder
    {
        protected Person _person = new Person();
        public PersonBuilder()
        {
        }
        protected bool IsPersonFlag { get; set; }
        protected object Partmeters { get; set; }

        /// <summary>
        /// 查询是否存在该人员
        /// </summary>
        /// <param name="user"></param>
        /// <param name="conn"></param>
        public abstract void SelectPerson(OapiUserListResponse.UserlistDomain user,IDbConnection conn, string code);
        /// <summary>
        /// 新增人员
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract Domain.OR_Person InsertPerson(IDbConnection conn);
        /// <summary>
        /// 修改人员
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract Domain.OR_Person UpdatePerson(IDbConnection conn);
        /// <summary>
        /// 在中间表插入数据
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        public abstract void SetRefDepartment(IDbConnection conn, string code, decimal id);

        public virtual Person GetPerson()
        {
            return _person;
        }
        public virtual bool IsPerson()
        {
            return IsPersonFlag;
        }
    }
}
