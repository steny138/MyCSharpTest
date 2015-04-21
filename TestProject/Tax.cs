using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
namespace TestProject
{
    class TaxTest
    {
        static  int MAX_COUNT = 1000;
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            List<Task> taskList = new List<Task>();
            for (int i = 1; i <= MAX_COUNT; i++)
            {
                taskList.Add(Task.Factory.StartNew<string>(()=>
                    {
                        getSegTaxPara para = null;
                        try
                        {
                            using (FileStream fs = new FileStream(string.Format("Json/Tax/{0}.json", (Task.CurrentId.Value % 5) + 1), FileMode.Open, FileAccess.Read))
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    para = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<getSegTaxPara>(sr.ReadToEnd());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        return getSegTax(para);
                    }).ContinueWith(task =>
                    {
                        logger.Info(string.Format("{0} : {1}", task.Id, task.Result));
                        logger.Warn(string.Format("{0}: success ", task.Id));
                        //Console.WriteLine("{0}: success", task.Id);
                        //Console.WriteLine();
                    }));
            }

            try
            {
                System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource(40000);
                System.Threading.CancellationToken ct = cts.Token;
                if (Task.WaitAll(taskList.ToArray(),-1, ct))
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Not Get Tax");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            Console.Read();
        }

        public class BasicModel
        {
            public string pnr { get; set; }
            public string pcc { get; set; }
            public string crs { get; set; }
            #region constructor
            public BasicModel()
            {
                this.pnr = string.Empty;
                this.pcc = string.Empty;
                this.crs = string.Empty;
            }
            public BasicModel(string _pnr, string _pcc, string _crs)
            {
                this.pnr = _pnr;
                this.pcc = _pcc;
                this.crs = _crs;
            }
            #endregion
        }
        public class getSegTaxPara : BasicModel
        {
            public List<segTaxRoute> _segTaxRoute { get; set; }

            public getSegTaxPara()
                : base()
            {
                _segTaxRoute = new List<segTaxRoute>();
            }
        }

        public class segTaxRoute
        {
            public string tCity { get; set; }                           //最後一段抵達城市
            public string tCountry { get; set; }                        //最後一段抵達國家
            public string carr { get; set; }                            //票源航空
            public string fareBasis { get; set; }                       //fareBasis
            public List<segTaxRouteInfo> segTaxRouteInfos { get; set; }     //行程稅金資訊
            public Tax tax { get; set; }                               //要回傳的稅金資訊
            public bool no_Q { get; set; }                             //是否不要Q稅
            public double keepQTax { get; set; }                       //計算Q稅使用
            public segTaxRoute()
            {
                tCity = "";
                tCountry = "";
                carr = "";
                fareBasis = "";
                keepQTax = 0;
                segTaxRouteInfos = new List<segTaxRouteInfo>();
                tax = new Tax();
            }
        }

        public class Tax
        {
            /// <summary>大人稅金</summary>
            public double Adt_Tax { get; set; }
            /// <summary>小孩稅金</summary>
            public double Chd_Tax { get; set; }
            /// <summary>嬰兒稅金</summary>
            public double Inf_Tax { get; set; }
            ///<summary>大人稅金明細 對應bg03_amt_desc</summary>
            public string Adt_Tax_desc { get; set; }
            ///<summary>小孩稅金明細 對應bg03_amt_desc</summary>
            public string Chd_Tax_desc { get; set; }
            ///<summary>嬰兒稅金明細 對應bg03_amt_desc</summary>
            public string Inf_Tax_desc { get; set; }
            ///<summary>大人稅金明細 對應bg03_desc</summary>
            public string Adt_desc { get; set; }
            ///<summary>小孩稅金明細 對應bg03_desc</summary>
            public string Chd_desc { get; set; }
            ///<summary>嬰兒稅金明細 對應bg03_desc</summary>
            public string Inf_desc { get; set; }

        }

        public class segTaxRouteInfo
        {
            public string fCity { get; set; }       //出發城市
            public string tCity { get; set; }       //目的城市
            public string fAirport { get; set; }    //出發機場
            public string tAirport { get; set; }    //目的機場
            public string rtow { get; set; }        //去回程 0-ow 1-rt , 2-不同點進出
            public string carr { get; set; }        //航空公司
            public string cls { get; set; }         //艙等
            public string fDate { get; set; }       //出發日期
        }

        private static string getSegTax(getSegTaxPara para)
        {
            string result = string.Empty;
            try
            {
                string url = "http://localhost:4228/api/getSegTax";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                string jsonstring = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(para);
                /* POST*/
                byte[] bs = Encoding.ASCII.GetBytes(jsonstring);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bs.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length); //向伺服器 post 資訊。
                }
                

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader tReader = new StreamReader(response.GetResponseStream());
                result = tReader.ReadToEnd();
                tReader.Close();
                tReader.Dispose();
                //result = Regex.Replace(result, "\\",  "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}
