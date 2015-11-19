using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace rrrrrrrest
{
    class Program
    {
        static void Main(string[] args)
        {
            // 当检测到刷卡事件时，调用checkAccess函数
            string IP = "192.168.0.10";
            string CardId = "00746081";
            checkAcess(CardId, IP);
            Console.WriteLine("已检测到并转发一次刷卡事件");
            // 模拟第二次刷卡事件
            IP = "192.168.0.11";
            CardId = "22222222222222";
            checkAcess(CardId, IP);
            Console.WriteLine("已检测到并转发第二次刷卡事件");

            while (true)
            {

            }
        }

        private static void checkAcess(string CardID, string IP)
        {
            string Key = "2kJ0Ry19WcyZ2NGDXPXYoQfNG5WRzgjWbAdq6m1E"; //指定的API Key，从配置文件中读取
            //string site = "http://tagger.chinacloudsites.cn"; //可替换，从配置文件中读取
            string site = "http://localhost:59329";
        
            var client = new RestClient();
            client.BaseUrl = new Uri(site);
            var request = new RestRequest();
            request.Method = Method.POST;
            request.Resource = "SwipeCard/Check";
            request.AddParameter("Key", Key);
            request.AddParameter("CardId", CardID);
            request.AddParameter("IP", IP);
            var invokeTime = DateTime.Now;//刷卡的时间
            // 异步查询预约管理系统的API，根据返回值在回调函数中执行相关操作
            client.ExecuteAsync(request, response => {
                //在这里写是否执行发送指令打开门禁的命令的逻辑
                //response.Content为0或1
                if(String.IsNullOrEmpty(response.Content) || (response.Content != "1" && response.Content != "0"))
                {
                    Console.WriteLine("发送请求失败！");
                }
                else
                {
                    Console.WriteLine(response.Content == "1" ? "开门" : "关门");
                    Console.WriteLine(String.Format("发送指令至门禁：{0}", IP));
                    var currentTime = DateTime.Now;//HTTP请求返回的时间
                    Console.WriteLine(String.Format("发送请求时间：{0}，收到回复时间：{1}", invokeTime.ToString("HH:mm:ss"), currentTime.ToString("HH:mm:ss")));                    
                }
                //存储本次刷卡事件至数据文件
            });
        }
    }
}

