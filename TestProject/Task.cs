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
    class TaskTest
    {
        
        //.Net 2.0 Thread
        //.Net 2.5 Thread Pool
        //.Net 4.0 Task
        //.Net 4.5 Async,Await修飾詞
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            var result = Task.Factory.StartNew<string>(() =>
            {
                Thread.Sleep(20000);

                return "Done!";
            }).ContinueWith(task =>
            {

                Console.WriteLine("2000 {0}", task.Result);
            }); ;

            var result2 = Task.Factory.StartNew<string>(() =>
            {
                Thread.Sleep(3000);
                return "Done1!";
            }).ContinueWith(task =>
            {
                Console.WriteLine("3000 {0}", task.Result);
            });


            CancellationTokenSource cts = new CancellationTokenSource(5000);
            CancellationToken ct = cts.Token;

            List<Task> taskList = new List<Task>();
            taskList.Add(result);
            taskList.Add(result2);
            try
            {
                Task.WaitAll(taskList.ToArray(), 19500, ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Cancel");
            }
            Console.WriteLine("ALL Done");
            Console.Read();
        }
    }
}
