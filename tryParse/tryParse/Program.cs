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

        public static string GetHtmlString(string url)                                  //получаем всю стараницу с кодировкой
        {
            request = WebRequest.Create(url);
            request.Proxy = null;
            response = request.GetResponse();
            using (var sReader = new StreamReader(response.GetResponseStream(), encode))
            {
                Console.WriteLine("Downloaded full HTML");
                return sReader.ReadToEnd();
            }
        }

        public static void MovingOnPages(int numberOfPages, string url)
        {
            for (int i = 1; i <= numberOfPages; i++)
            {
                var url2 =
                    $"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0&page={i}";

                GetHtmlString(url2);

                var w1Client = new WebClient();
                w1Client.Proxy = null;
                w1Client.Encoding = encode;
                var html1 = new HtmlDocument();

                html1.LoadHtml(w1Client.DownloadString(url2));

                GetId(html1);
            }
        }

        static int GetPagesCount(HtmlDocument html)
        {
            var divs = html.DocumentNode.SelectSingleNode("//span[@class='col-md-6']"); 

            string str = divs.FirstChild.InnerText;
            var numberOfPosition = Convert.ToInt32(str.Remove(6));

            return numberOfPosition;
        }

        public static void GetFullInformation(HtmlDocument html)
        { 
            var divs = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5']");
            var divs1 = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5 boldInfo']");


            for (int i = 0; i < divs.Count; i++)
            {
                string str = divs[i].FirstChild.InnerText;
                str = str.Remove(0, 7);
                string str1 = divs1[i].FirstChild.InnerText;

                Console.WriteLine("{0} {1}", str, str1);
            }

        }

        public static void GetId(HtmlDocument html)
        {

            var spans = html.DocumentNode.SelectNodes("//span[@class='area']"); 

            int i = 1;

            foreach (var span in spans)
            {
                string str1 = span.LastChild.InnerText;
                str1 = str1.Remove(0, 1);

                var url3 = $"http://www.grekodom.ru/realtyobject/{str1}";

                Console.WriteLine("  " + i + ")" + " " + str1);
                Console.WriteLine("   " + url3);

                //________________________Вынести в отдельную функцию______________________________________________________________
                var w2Client = new WebClient
                {
                    Proxy = null,
                    Encoding = encode
                };
                var html2 = new HtmlDocument();

                html2.LoadHtml(w2Client.DownloadString(url3));
                Console.WriteLine();
                i++;

                GetFullInformation(html2);
                //________________________________________________________________________________________________________________
            }
        }

        //static void Get


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1251);                                        //Кодировка для русского вывода в консоль

            //var url = (
            //    @"http://www.grekodom.ru/RealtyObjects?multiType=null&multiReg                        //длинная версия ссылки
            //ion=null&type=undefined&subregion=undefined&span=undefined&distance=
            //0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT
            //=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0");

            var url = (@"http://www.grekodom.ru/realtyobjects");                                        //короткая версия ссылки (работает как и длинная)

            GetHtmlString(url);
            var wClient = new WebClient
            {
                Proxy = null,
                Encoding = encode
            }; 
     
            var html = new HtmlDocument();                      
            html.LoadHtml(wClient.DownloadString(url));         

            
            GetPagesCount(html);

            var numberOfPages = (GetPagesCount(html) / 15) + 1;             //считаем количество страниц, ибо нефиг парсить лишнего

            Console.WriteLine("Number Of Positions = " + GetPagesCount(html));
            Console.WriteLine("Number Of sites = " + numberOfPages);

            MovingOnPages(numberOfPages, url); //обход страниц сайта

        }
    }
}
