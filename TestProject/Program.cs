using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.googleapis.com/urlshortener/v1/url?key=AIzaSyDYJkWGkFQEPmcUqJgCtKvtdaUAA3qNrcc";
            string POST_PATTERN = @"{{""longUrl"": ""{0}""}}";
            string param = string.Format(POST_PATTERN, "https://docs.google.com/presentation/d/1YrI7wnDtGrWMBfylwJoLdPaxt5bAgrlGEGGYbvJKsSA/edit#slide=id.p");
            byte[] bs = Encoding.ASCII.GetBytes(param);
            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Timeout = 10000;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = param.Length;

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            using (WebResponse wr = request.GetResponse())
            {
                string result = string.Empty;
                //在這裡對接收到的頁面內容進行處理
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
                Console.WriteLine(result);
            } 


            //callWeb();
            double? x = null;
            bool isOk = false;
            double? y = 4;
            double z = 3;
            double test = 2.1351;
            Console.WriteLine("Math.Round(3.44, 1) = {0}", Math.Round(3.44, 1)); //Returns 3.4.
            Console.WriteLine("Math.Round(3.45, 1) = {0}", Math.Round(3.45, 1)); //Returns 3.4.
            Console.WriteLine("Math.Round(3.46, 1) = {0}", Math.Round(3.46, 1)); //Returns 3.5.
            Console.WriteLine("Math.Round(3.445, 1) = {0}", Math.Round(3.445, 1)); //Returns 3.4.
            Console.WriteLine("Math.Round(3.455, 1) = {0}", Math.Round(3.455, 1)); //Returns 3.5.
            Console.WriteLine("Math.Round(3.465, 1) = {0}", Math.Round(3.465, 1)); //Returns 3.5.
            Console.WriteLine("Math.Round(3.450, 1) = {0}", Math.Round(test, 2, MidpointRounding.AwayFromZero)); //Returns 3.4.(补0是无效的)
            Console.WriteLine("Math.Round(3.350, 1) = {0}", Math.Round(3.350, 1)); //Returns 3.4.(补0是无效的)
            Console.WriteLine("Math.Round(3.4452, 2) = {0}", Math.Round(3.4452, 2)); //Returns 3.45.
            Console.WriteLine("Math.Round(3.4552, 2) = {0}", Math.Round(3.4552, 2)); //Returns 3.46.
            Console.WriteLine("Math.Round(3.4652, 2) = {0}", Math.Round(3.4652, 2)); //Returns 3.47.
            //Console.WriteLine("輸入結果 : {0} , 而且值更改為 {1}", isOk, x??y??z);
            Console.Read();
        }
        private static void callWeb()
        {
            string k = "Hello world";
            Console.WriteLine(k);
            bool x = true;
            k = x.ToString();
            Console.WriteLine(k);
            string url = "http://udn.com/udnrss/BREAKINGNEWS1.xml";
            WebClient wc = new WebClient();

            Stream st = wc.OpenRead(url);
            string rss;
            using (StreamReader sr = new StreamReader(st))
            {
                rss = sr.ReadToEnd();
            }

            XElement xmlTree1 = new XElement("rss",
                new XElement("channel",
                    new XElement("item",
                        new XElement("title", "綠委絕食70小時 抗議藍暴力處理服貿"))),
                new XElement("channel",
                    new XElement("item",
                        new XElement("title", "綠委絕食110小時 抗議藍暴力處理服貿")))
            );


            Console.WriteLine(rss);
            XDocument doc = XDocument.Parse(rss);
            XElement rssDoc = doc.Element("rss");
            /*使用xmlTree1 此處驗證 IEnumerble.element 在lineq內 等同跑巢狀迴圈  */
            //var channel = rssDoc.Elements("channel");
            var channel = xmlTree1.Elements("channel");
            var items = from _items in channel.Elements("item")
                        let itemName = _items.Element("title")
                        where itemName.Value.Contains("抗議藍暴力處理服貿")
                        select _items;

            foreach (var item in items)
                Console.WriteLine(item.Element("title").Value);


            List<ModalData> listData = new List<ModalData>(){
                new ModalData("Steny","Steny138","1234","steny"),
                new ModalData("Allen","Allen","1234","allen"),
                new ModalData("Daniel","Daniel","1234","daniel"),
                new ModalData("Harris","Harris","1234","harris"),
                new ModalData("Hao","Hao","1234","hao")
            };
            string[] list = { "A", "B", "C", "D", "E" };
            var b = from a in list
                    where a.Equals("B")
                    select a;
            Console.WriteLine(b.ElementAt(0));

            /* Linq for modal class */
            var datas = from data in listData
                        where data.name == "Steny"
                        select data.pwd;
            foreach (string pwd in datas)
                Console.WriteLine(pwd);
        }
    }
    public class ModalData
    {
        public ModalData()
        {
            name = string.Empty;
            id = string.Empty;
            pwd = string.Empty;
            account = string.Empty;
        }
        public ModalData(string _name, string _id, string _pwd, string _account)
        {
            name = _name;
            id = _id;
            pwd = _pwd;
            account = _account;
        }
        public string name { get; set; }
        public string id { get; set; }
        public string pwd { get; set; }
        public string account { get; set; }

    }
}
