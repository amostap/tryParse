using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace tryParse
{
    class Program
    {
        public static WebClient wClient;
        public static WebRequest request;                                               //запрос на получение
        public static WebResponse response;                                             //ответ

        public static Encoding encode = Encoding.GetEncoding("utf-8");      //задаем кодировку

        static int GetPagesCount(HtmlDocument html)
        {

            var divs = html.DocumentNode.SelectSingleNode("//span[@class='col-md-6']"); //SelectNodes("//span[@class='col-md-6']");

            int a = 0;
            int numberOfPosition = 0;

            string str = divs.FirstChild.InnerText;
            numberOfPosition = Convert.ToInt32(str.Remove(6));

            return numberOfPosition;
        }



        public static string GetHtmlString(string url)                                  //получаем всю стараницу с кодировкой
        {
            request = WebRequest.Create(url);
            request.Proxy = null;
            response = request.GetResponse();
            using (var sReader = new StreamReader(response.GetResponseStream(), encode))
            {

                return sReader.ReadToEnd();

            }
        }

        public static void GetId(HtmlDocument html)
        {

            var spans = html.DocumentNode.SelectNodes("//span[@class='area']"); //SelectNodes("//span[@class='col-md-6']");

            int i = 1;

            foreach (var span in spans)
            {
                string str1 = span.LastChild.InnerText;
                str1 = str1.Remove(0, 1);

                var url3 = $"http://www.grekodom.ru/realtyobject/{str1}";

                Console.WriteLine("  " + i + ")" + " " + str1);
                Console.WriteLine("   " + url3);

                //_____________________________________________________________________________________________________________
                var w2Client = new WebClient();
                w2Client.Proxy = null;
                w2Client.Encoding = encode;
                var html2 = new HtmlDocument();

                html2.LoadHtml(w2Client.DownloadString(url3));
                Console.WriteLine();
                i++;



                //byte[] bytes = Encoding.Default.GetBytes(str1);
                //var myString = Encoding.UTF32.GetString(bytes);
                //________________________________________________________________________________________________________________



            }



        }

        public static void GetPages(int a, string url)
        {
            var b = 1.0d;

            for (int i = 1; i < a; i++)
            {
                var url2 =
                    $"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0&page={i}";


                GetHtmlString(url2);

                b = ((i / a) * 100d);

                Console.WriteLine(i + "/" + a + "                  " + b + " %");

                var w1Client = new WebClient();
                w1Client.Proxy = null;
                w1Client.Encoding = encode;
                var html1 = new HtmlDocument();

                html1.LoadHtml(w1Client.DownloadString(url2));

                GetId(html1);
            }

        }


        static void Main(string[] args)
        {
            //var url = (
            //    @"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0");

            var url = (@"http://www.grekodom.ru/realtyobjects");

            GetHtmlString(url);
            Console.WriteLine("Downloaded full HTML");

            var wClient = new WebClient();
            wClient.Proxy = null;
            wClient.Encoding = encode;

            var html = new HtmlDocument();

            html.LoadHtml(wClient.DownloadString(url));

            //var a = GetPagesCount(html); //количество страниц

            int a = (GetPagesCount(html) / 15) + 1;
            //Console.WriteLine("DONE");
            Console.WriteLine("Number Of Positions = " + GetPagesCount(html));
            Console.WriteLine("Number Of sites = " + a);

            GetPages(a, url); //обход страниц сайта




            //var b = 1.0d;

            //for (double i = 1.0; i < a; i++)
            //{
            //    var url2 =
            //        $"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0&page={i}";
            //    GetHtmlString(url2);

            //    b = ((i / a) * 100d);

            //    Console.WriteLine(i + "/" + a + "                  " + b + " %");

            //}

            //var html = new HtmlDocument();
            //html.LoadHtml(new WebClient().DownloadString("http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0"));

            //var root = html.DocumentNode;
            //var p = root.Descendants()
            //    .Where(n => n.GetAttributeValue("class", "").Equals("module-profile-recognition"))
            //    .Single()
            //    .Descendants("p")
            //    .Single();
            //var content = p.InnerText;

            //Console.WriteLine("dONE");
        }
    }
}
