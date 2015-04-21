using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class TestParams
    {
        public const int EXECUTE_COUNT = 1000000;
        static void Main(string[] args)
        {
            Console.WriteLine(TestTime.ExecuteTime(() =>
            {
                for (int i = 0; i < EXECUTE_COUNT; i++)
                {
                    var t = Activator.CreateInstance(typeof(ModalData));
                }
            })+"毫秒\n\n");


            Console.WriteLine(TestTime.ExecuteTime(() =>
            {
                for (int i = 0; i < EXECUTE_COUNT; i++)
                {
                    var t = Activator.CreateInstance(typeof(ModalData), "Steny", "steny138", "1234", "steny138");
                }
            }) + "毫秒\n\n");


            try
            {
                showParams();
                showParams("1", "A", "2", "B", "3", "C");
                
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message.ToString());
            }
            finally
            {
                Console.WriteLine("Over~");
            }
            Console.Read();
        }

        static void showParams(params string[] contents)
        {
            if (contents == null || contents.Length == 0)
            {
                throw new ArgumentException("Params: No Arguments");
            }

            foreach(string content in contents)
                Console.WriteLine(content);
        }

    }


    public class TestTime
    {
        public static string ExecuteTime(Action works)
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            works();
            watch.Stop();
            return watch.Elapsed.TotalMilliseconds.ToString();
        }
    }

}
