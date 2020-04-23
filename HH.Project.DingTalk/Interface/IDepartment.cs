using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Interface
{
    public interface IDepartment
    {
        /// <summary>
        /// 递归部门
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="conn"></param>
        /// <param name="Id"></param>
        void DingTalkDepartmentList(in List<OapiDepartmentListResponse.DepartmentDomain> departments, IDbConnection conn, long? Id = 0);
        /// <summary>
        /// 移除无用部门
        /// </summary>
        /// <param name="DepartmentList"></param>
        /// <param name="conn"></param>
        void RemoveNotInSystemDepartment(List<long> DepartmentList,IDbConnection conn);
    }
}
