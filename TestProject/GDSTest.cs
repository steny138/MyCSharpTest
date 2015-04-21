using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestProject
{
    class GDSTest
    {
        static void Main(string[] args)
        {
            string path = "E:\\Steny\\TestProject\\TestProject\\TextFile1.txt";
            string jsonstring = string.Empty;

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8)) 
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    jsonstring += line;
                }
            }

            List<AirSegment> segments = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<AirSegment>>(jsonstring);

            getGDSSegmentListPara para = new getGDSSegmentListPara()
            {
                crs = "AB",
                pcc = "",
                listAirSegment = segments,
                hasProdno="1"
            };

            Console.WriteLine(getGdsAPI(para));
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
        public class getGDSSegmentListPara : BasicModel
        {
            public List<AirSegment> listAirSegment { get; set; }
            public string hasProdno { get; set; }
        }
        public class AirSegment
        {
            /// <summary>
            /// 去回程
            /// </summary>
            public string Comeback;
            /// <summary>
            /// 出發城市或機場
            /// </summary>
            public string DeptCity;
            /// <summary>
            /// 抵達城市或機場
            /// </summary>
            public string ArrCity;
            /// <summary>
            /// 出發年
            /// </summary>
            public string Year;
            /// <summary>
            /// 出發月
            /// </summary>
            public string Month;
            /// <summary>
            /// 出發日
            /// </summary>
            public string DayOfMonth;
            /// <summary>
            /// Time(HH:mm)
            /// </summary>
            public string Time;
            /// <summary>
            /// 航空公司
            /// </summary>
            public string AddAirLine;
            /// <summary>
            /// 艙等
            /// </summary>
            public string ClassCode;
            /// <summary>
            /// 轉機點
            /// </summary>
            public string AddConnCity;
            /// <summary>
            /// 聯營
            /// </summary>
            public string CodeShare;
            /// <summary>
            /// 聯營限制 可搭的航空公司
            /// </summary>
            public string Codedhare2;
            /// <summary>
            /// 限禁搭時間1(起)
            /// </summary>
            public string Pd10Ftime1;
            /// <summary>
            /// 限禁搭時間1(迄)
            /// </summary>
            public string Pd10Ttime1;
            /// <summary>
            /// 限禁搭時間2(起)
            /// </summary>
            public string Pd10Ftime2;
            /// <summary>
            /// 限禁搭時間2(迄)
            /// </summary>
            public string Pd10Ttime2;
            /// <summary>
            /// 限禁搭航班種類 空白-無.0-限搭,1-禁搭
            /// </summary>
            public string Pd10Kid2;
            /// <summary>
            /// 限禁搭航班
            /// </summary>
            public string Pd10FlightNo;
            /// <summary>
            /// 航段出發城市
            /// </summary>
            public string SingleSegDeptCity;
            /// <summary>
            /// 航段抵達城市
            /// </summary>
            public string SingleSegArrcity;
            /// <summary>
            /// 航段數
            /// </summary>
            public string SegLength;
            /// <summary>
            /// 出發地以x為主
            /// </summary>
            public string Pd00Fcity;
            /// <summary>
            /// 最早出發日
            /// </summary>
            public string Pr00Tdate;
            /// <summary>
            /// BackEndDate
            /// </summary>
            public string BackEndDate;
            /// <summary>
            /// 最晚出發日
            /// </summary>
            /// <returns></returns>
            public string Pr00Fdate;
            /// <summary>
            /// 出發起始日
            /// </summary>
            /// <returns></returns>
            public string Fdate;
            /// <summary>
            /// 票源航空
            /// </summary>
            /// <returns></returns>
            public string Pd00Carr;
            /// <summary>
            /// 是否要切換到airline
            /// </summary>
            /// <returns></returns>
            public bool changeToAirlineHost;
            /// <summary>是否直飛</summary>
            public bool directFlight;
            /// <summary>
            /// 來自Abacus or Amadeus的航段列表
            /// </summary>
            /// <returns></returns>
            public System.Collections.ArrayList GDSSegments;

        }

        private static string getGdsAPI(getGDSSegmentListPara para)
        {
            string result = string.Empty;
            try
            {
                string url = "http://localhost:4228/api/getGDSSegmentList";
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
