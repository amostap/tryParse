using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using System.Threading;


namespace tryParse
{
    class Program
    {
        public static WebClient wClient;
        public static WebRequest request;                                               //запрос на получение
        public static WebResponse response;                                             //ответ
        public static Encoding encode = Encoding.GetEncoding("utf-8");                  //задаем кодировку

        //получаем всю стараницу с кодировкой
        public static string GetFullHtmlAsString(string url)
        {

            request = WebRequest.Create(url);
            request.Proxy = null;
            response = request.GetResponse();
            using (var sReader = new StreamReader(stream: response.GetResponseStream(), encoding: encode))
            {
                Console.WriteLine("Downloaded full HTML");
                return sReader.ReadToEnd();
            }
        }

        //Проходим по страницам
        public static void MovingOnPages(int numberOfPages, string url)
        {
            //for (var i = 1; i <= numberOfPages; i++)
            for (var i = 1; i <= 1; i++)
            {
                var url2 =
                    $"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0&page={i}";

                GetFullHtmlAsString(url2);

                var w1Client = new WebClient
                {
                    Proxy = null,
                    Encoding = encode
                };

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


        public static void GetMainInformation(HtmlDocument html)
        {

            //var imgs = html.DocumentNode.Descendants("//div[@class='rsSlide ']//img/@src");

            var imgs = html.DocumentNode.SelectNodes("//img");
            var divs = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5'] | //div[@class='col-md-5 col-sm-5 col-xs-5 ']");
            var divs1 = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5 boldInfo'] | //div[@class='col-md-10 col-sm-10 col-xs-10 boldInfo']");
            var divs2 = html.DocumentNode.SelectNodes("//div[@class='additional-amenities']//p");
            //var divs = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5']");
            //var divs1 = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5']");
            //var divs2 = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5 boldInfo']");
            //var divs3 = html.DocumentNode.SelectNodes("//div[@class='additional-amenities']");

            //Console.WriteLine("{0} {1}", divs.Count, divs1.Count);

            //foreach (var i in divs3)
            //{
            //    Console.WriteLine(i.SelectNodes("//*[@class='col-md-5 col-sm-5 col-xs-5 ']").Count);
            //    Console.WriteLine(i.SelectNodes("//*[@class='col-md-5 col-sm-5 col-xs-5']").Count);
            //}


            try
            {
                for (var i = 0; i < divs.Count; i++)
                {

                    if (i == 0 && imgs != null)
                    {
                        foreach (var img in imgs)
                        {
                            Console.WriteLine(
                            img.GetAttributeValue("src", null));
                        }
                    }


                    string str = divs[i].FirstChild.InnerHtml;
                    string str1 = divs1[i].FirstChild.InnerText;
                    Console.WriteLine((i + 1) + "{0} {1}", str, str1);
                    if (divs.Count - 1 == i)
                    {
                        foreach (var p in divs2)
                        {
                            if (p.InnerText == "")
                                Console.WriteLine("------=======EMPTY======-------");
                            else
                            {
                                Console.WriteLine(p.InnerHtml);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("FUCK THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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

                GetMainInformation(html2);
                //________________________________________________________________________________________________________________
            }

        }

        //static void Get


        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.GetEncoding(1251);                //Кодировка для русского вывода в консоль

            const string url = (@"http://www.grekodom.ru/realtyobjects");       //короткая версия ссылки (работает как и длинная)
            GetFullHtmlAsString(url);                                               //get HTML as String from url


            var wClient = new WebClient
            {
                Proxy = null,
                Encoding = encode
            };

            var html = new HtmlDocument();
            html.LoadHtml(wClient.DownloadString(url));

            GetPagesCount(html);

            var numberOfPages = (GetPagesCount(html) / 15) + 1;                 //считаем количество страниц, ибо нефиг парсить лишнего

            Console.WriteLine("Number Of Positions = " + GetPagesCount(html));
            Console.WriteLine("Number Of sites = " + numberOfPages);

            MovingOnPages(numberOfPages, url); //обход страниц сайта

        }

    }

}
