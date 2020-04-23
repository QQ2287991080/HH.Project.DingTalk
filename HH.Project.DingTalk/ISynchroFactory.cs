using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DingTalk.Api.Response.OapiDepartmentListResponse;

namespace HH.Project.DingTalk
{
    public abstract class ISynchroFactory
    {
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual List<DepartmentDomain> GetDepartmentList(string token)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            request.SetHttpMethod("GET");
            OapiDepartmentListResponse response = client.Execute(request, token);
            if (response.Errcode != 0)
            {
                Console.WriteLine("错误原因：【" + response.Errmsg + "】");
            }
            return response.Department;
        }

        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string DepartmentGetResponse(string token, string id)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/get");
            OapiDepartmentGetRequest request = new OapiDepartmentGetRequest();
            request.Id = id;
            request.SetHttpMethod("GET");
            OapiDepartmentGetResponse response = client.Execute(request, token);
            return response.DeptManagerUseridList.Replace("|", ",");
        }
        /// <summary>
        /// 获取钉钉部门下的用户列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public virtual List<OapiUserListResponse.UserlistDomain> UserSimplelistResponse(string token, long departmentId)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/list");
            OapiUserListRequest request = new OapiUserListRequest();
            request.DepartmentId = departmentId;
            request.SetHttpMethod("GET");
            OapiUserListResponse response = client.Execute(request, token);
            if (response.Errcode != 0)
            {
                Console.WriteLine("错误原因：【" + response.Errmsg + "】");
            }
            return response.Userlist;
        }
       
    }
}
