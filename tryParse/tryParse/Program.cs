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
            //var liNodes = html.GetElementbyId("nav-pages").ChildNodes.Where(x => x.Name == "li");
            //HtmlAttribute href = liNodes.Last().FirstChild.Attributes["href"];
            //int pagesCount = (int)Char.GetNumericValue(href.Value[href.Value.Length - 2]);

            var divs = html.DocumentNode.SelectSingleNode("//span[@class='col-md-6']"); //SelectNodes("//span[@class='col-md-6']");

            int a = 0;
            int numberOfPosition = 0;
            //foreach (var span in divs)
            //{
            //    //a++;
            //    //Console.WriteLine(span.FirstChild.InnerText);
            //    string str = span.FirstChild.InnerText;
            //    numberOfPosition = Convert.ToInt32(str.Remove(6));
            //}

            string str = divs.FirstChild.InnerText;
            numberOfPosition = Convert.ToInt32(str.Remove(6));

            //    numberOfPosition = Convert.ToInt32(str.Remove(6));

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

            var b = 1.0d;

            for (double i = 1.0; i < a; i++)
            {
                var url2 =
                    $"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0&page={i}";
                GetHtmlString(url2);

                b = ((i / a) * 100d);

                Console.WriteLine(i + "/" + a + "                  " + b + " %");

            }

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
