using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Interface
{
  public  interface IPerson
    {
        /// <summary>
        /// 新增钉钉用户到Person表
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="conn"></param>
        /// <param name="token"></param>
        /// <param name="PersonList"></param>
        /// <returns></returns>
        List<long> AddPersonWithSysUser(List<OapiDepartmentListResponse.DepartmentDomain> departments,IDbConnection conn,string token,out List<string> PersonList);
        /// <summary>
        /// 移除离职人员
        /// </summary>
        /// <param name="PersonList"></param>
        /// <param name="conn"></param>
        void RemovePersonWithSysUser(List<string> PersonList, IDbConnection conn);
    }
}
