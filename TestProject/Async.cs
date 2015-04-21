using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject
{
    class AsyncTest
    {
        //.Net 2.0 Thread
        //.Net 2.5 Thread Pool
        //.Net 4.0 Task
        //.Net 4.5 Async,Await修飾詞
        static void Main(string[] args)
        {
            //若方法指定async "提醒"這是一個可以非同步的方法
            //若加上await 則會等候await執行後才執行await後的方法內容(Run2)
            //但遇上await的方法，會先返回方法上一層繼續執行，
            //等到await完成才會繼續執行剛剛的方法還沒完成的程式碼(Run1)

            Console.WriteLine("Start...");

            var a = Run();
            var b = Run2();
            
            Console.WriteLine("s2");
            Console.WriteLine("ALL Done");
            Console.Read();
        }

        /// <summary>
        /// 非同步執行，且不等候
        /// </summary>
        /// <returns></returns>
        static async Task Run()
        {
            await GetName();
            Console.WriteLine("s1");
            //Console.WriteLine(name);
        }

        static async Task GetName()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("s3");
                //return "Damin.";
            });

        }

        /// <summary>
        /// 非同步執行，但等候結果
        /// </summary>
        /// <returns></returns>
        static async Task Run2()
        {
            string name = await GetName2();
            Console.WriteLine("s4");
            Console.WriteLine(name);
        }

        static async Task<string> GetName2()
        {
            return await Task.Run<string>(() =>
            {
                Thread.Sleep(2000);
                //Console.WriteLine("s3");
                return "s5";
            });
            
        }
    }
}
