using Dapper;
using DingTalk.Api.Response;
using HH.Project.DingTalk.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HH.Project.DingTalk
{
    internal class Program
    {
        private static readonly string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static readonly string _appKey = ConfigurationManager.AppSettings["AppKey"];
        private static readonly string _appSecret = ConfigurationManager.AppSettings["AppSecret"];
        private static void Main(string[] args)
        {
            //****使用外观设计模式完成本次钉钉组织同步****//
            Facade facade = new Facade();
            facade.Operator(connection);
            Console.WriteLine("同步完成");
        }

    }
}
