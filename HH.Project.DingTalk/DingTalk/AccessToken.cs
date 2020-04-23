using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace HH.Project.DingTalk
{

   public class AccessToken
    {
        private static readonly string appKey = ConfigurationManager.AppSettings["AppKey"];
        private static readonly string appSecret = ConfigurationManager.AppSettings["AppSecret"];
        #region 获取AccessToken
        public static string GetAccessToken()
        {
            // 将AccessToken存储至缓存
            SetAccessToken(new CacheItemRemovedReason());
            // 输出缓存中存储的AccessToken
            return HttpRuntime.Cache.Get("Dingtalk_AccessToken").ToString();
        }
        #endregion

        #region 设置AccessToken(企业内部开发)
        public static void SetAccessToken(CacheItemRemovedReason reason)
        {
            //企业内部开发
            var accessToken = HttpRuntime.Cache.Get("Dingtalk_AccessToken");
            if (accessToken == null)
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
                OapiGettokenRequest request = new OapiGettokenRequest();
                request.Appkey = appKey;
                request.Appsecret = appSecret;
                request.SetHttpMethod("GET");
                OapiGettokenResponse response = client.Execute(request);
                HttpRuntime.Cache.Insert("Dingtalk_AccessToken", response.AccessToken, null, DateTime.Now.AddSeconds(3600), Cache.NoSlidingExpiration);
            }
        }
        #endregion
    }
}
