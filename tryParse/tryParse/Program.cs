using System;
using System.Collections.Generic;

using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Xml.Serialization;


namespace tryParse
{
    public class Post
    {
        public Post()
        {
            Id = null;
            Region = null;
            Type = null;
            Category = null;
            Square = null;
            YearOfBuild = null;
            YearOfRebuild = null;
            DistanceToSea = null;
            NumberOfRooms = null;
            Map = null;
            Country = null;
            DistanceToAeroport = null;
            Parking = null;
            Pool = null;
            PriseValue = null;
            Video = null;
            Description = null;
        }

        public string Id { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Map { get; set; }
        public string Square { get; set; }
        public string YearOfBuild { get; set; }
        public string YearOfRebuild { get; set; }
        public string DistanceToSea { get; set; }
        public string DistanceToAeroport { get; set; }
        public string NumberOfRooms { get; set; }
        public string Other { get; set; }
        public string Parking { get; set; }
        public string Pool { get; set; }
        public string PriseValue { get; set; }
        public string Video { get; set; }
        public string Description { get; set; }

        public static explicit operator Post(string v)
        {
            throw new NotImplementedException();
        }
    }

    class WpPosts
    {
        #region asd
        public string ID;
        public string PostAuthor;
        public string PostDate;
        public string PostDateGmt;
        public string PostContent;
        public string PostTitle;
        public string PostExcerpt;
        public string PostStatus;
        public string CommentStatus;
        public string PingStatus;
        public string PostPassword;
        public string PostName;
        public string ToPing;
        public string Pinged;
        public string PostModified;
        public string PostModifiedGmt;
        public string PostContentFiltered;
        public string PostParent;
        public string Guid;
        public string MenuOrder;
        public string PostType;
        public string PostMimeType;
        public string CommentCount;
        #endregion
    }

    class Program
    {
        public static WebClient WClient;
        public static WebRequest Request;
        public static WebResponse Response;
        public static Encoding Encode = Encoding.GetEncoding("utf-8");

        public static List<WpPosts> ListOPosts;

        public static string PathForPostMeta = @"D:/ms/PostMeta.txt";
        public static StreamWriter SwForPostMeta;
        public static string PathForPosts = @"D:/ms/Posts.txt";
        public static StreamWriter SwForPosts;
        public static string PathForPhotos = @"D:/ms/Photos.txt";
        public static StreamWriter SwForPhotos;


        //public static void SerializeToXml(List<Post> listOfPosts)
        //{
        //    using (TextWriter output = new StreamWriter("report.xml"))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(List<Post>));
        //        serializer.Serialize(output, listOPosts);
        //    }
        //}

        public static void GenerateSqlToPostMeta(string postId, string metaKey, string metaValue)
        {
            using (SwForPostMeta = File.AppendText(PathForPostMeta))
            {
                SwForPostMeta.WriteLine($"({postId}, '{metaKey}', '{metaValue}'),");
            }

            Console.WriteLine($"({postId}, '{metaKey}', '{metaValue}'),");
        }

        //insert into `wp_posts`(`ID`,`post_author`,`post_date`,`post_date_gmt`,
        //`post_content`,`post_title`,`post_excerpt`,`post_status`,
        //`comment_status`,	`ping_status`,`post_password`,`post_name`,
        //`to_ping`,`pinged`,`post_modified`,`post_modified_gmt`,
        //`post_content_filtered`,`post_parent`,`guid`,`menu_order`,
        //`post_type`,`post_mime_type`,`comment_count`) values

        public static void GenerateSqlToPosts(string Id, string postContent, string pictures, string title)
        {
            using (SwForPosts = File.AppendText(PathForPosts))
            {
                //SwForPosts.WriteLine("insert into `wp_posts`(`ID`,`post_author`,`post_date`,`post_date_gmt`,"+
                //                     "`post_content`,`post_title`,`post_excerpt`,`post_status`," +
                //                     "`comment_status`,`ping_status`,`post_password`,`post_name`," +
                //                     "`to_ping`,`pinged`,`post_modified`,`post_modified_gmt`," +
                //                     "`post_content_filtered`,`post_parent`,`guid`,`menu_order`," +
                //                     "`post_type`,`post_mime_type`,`comment_count`) values");

                SwForPosts.WriteLine($"({Id}," +                                                   //ID
                                     $"1," +                                                       //post_author
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +         //post_date
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +         //post_date_gmt  
                                     $"'[:ru][gallery ids=\"{pictures}\"]{postContent}[:]'," +     //post_content
                                     $"'{title}'," +                                            //post_title
                                     $"''," +                                                      //post_excerpt
                                     $"'publish'," +                                               //post_status
                                     $"'open'," +                                                  //comment_status
                                     $"'closed'," +                                                //ping_status
                                     $"''," +                                                      //post_password
                                     $"'{Id}'," +                                             //post_name
                                     $"''," +                                                      //to_ping
                                     $"''," +                                                      //pinged
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +         //post_modified
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +         //post_modified_gmt
                                     $"''," +                                                      //post_content_filtered
                                     $"0," +                                                       //post_parent
                                     $"'http://www.markstanley.estate/?post_type=realty&#038;p={Id}'," +  //guid
                                     $"0," +                                                       //menu_order
                                     $"'realty'," +                                                //post_type
                                     $"''," +                                                      //post_mime_type
                                     $"0),");                                                      //comment_count

               //SwForPosts.WriteLine(",");
            }
        }

        public static void GenerateSqlToPhotos(string id, string title, string postName, string postParent)
        {
            using (SwForPhotos = File.AppendText(PathForPhotos))
            {
                //SwForPhotos.WriteLine("insert into `wp_posts`(`ID`,`post_author`,`post_date`,`post_date_gmt`," +
                //                     "`post_content`,`post_title`,`post_excerpt`,`post_status`," +
                //                     "`comment_status`,`ping_status`,`post_password`,`post_name`," +
                //                     "`to_ping`,`pinged`,`post_modified`,`post_modified_gmt`," +
                //                     "`post_content_filtered`,`post_parent`,`guid`,`menu_order`," +
                //                     "`post_type`,`post_mime_type`,`comment_count`) values");

                SwForPhotos.WriteLine($"({id}," +                                                        //ID
                                     $"1," +                                                             //post_author
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +               //post_date
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +               //post_date_gmt  
                                     $"''," +                                                            //post_content
                                     $"'{title}'," +                                                     //post_title
                                     $"''," +                                                //post_excerpt                                                                 //$"`post_excerpt`," +                                                //post_excerpt
                                     $"'inherit'," +                                                     //post_status
                                     $"'open'," +                                                        //comment_status
                                     $"'closed'," +                                                      //ping_status
                                     $"''," +                                                            //post_password
                                     $"'{postName}'," +                                                  //post_name
                                     $"''," +                                                            //to_ping
                                     $"''," +                                                            //pinged
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +               //post_modified
                                     $"'{DateTime.Now.ToString("u").Replace("Z", "")}'," +               //post_modified_gmt
                                     $"''," +                                                            //post_content_filtered
                                     $"{postParent}," +                                                  //post_parent
                                     $"'http://www.markstanley.estate/wp-content/uploads/2016/02/{postName}'" + //guid
                                     $",0," +                                                            //menu_order
                                     $"'attachment'," +                                                  //post_type
                                     $"'image/jpeg'," +                                                  //post_mime_type
                                     $"0),");                                                             //comment_count

                //SwForPosts.WriteLine(",");
            }
        }

        public static void DownloadImage(string url, string name)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(url, name);
            }
        }

        public static void GetMainInformation(HtmlDocument html, string id)
        {
            var prise = html.DocumentNode.SelectSingleNode("//div[@class='block-heading']//span[3]");
            var firstColumn = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5'] | //div[@class='col-md-5 col-sm-5 col-xs-5 ']");
            var secondColumn = html.DocumentNode.SelectNodes("//div[@class='col-md-5 col-sm-5 col-xs-5 boldInfo'] | //div[@class='col-md-10 col-sm-10 col-xs-10 boldInfo']");
            var description = html.DocumentNode.SelectNodes("//div[@class='additional-amenities']//p");
            var images = html.DocumentNode.SelectNodes("//div[@id='gallery-1']/a");
            var title = html.DocumentNode.SelectSingleNode("//h2").InnerText;


            Console.WriteLine("insert into `wp_postmeta`(`post_id`,`meta_key`,`meta_value`) values");

            var s = prise.InnerText;
            s = s.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("от", "от ");

            GenerateSqlToPostMeta(id, "price_value", s);
            GenerateSqlToPostMeta(id, "_price_value", "field_55f7df4f76119");
            GenerateSqlToPostMeta(id, "price_sign", "EUR");
            GenerateSqlToPostMeta(id, "_price_sign", "field_55f7dfa47611a");
            GenerateSqlToPostMeta(id, "price_previous_value", s != "Позапросу" ? $"{(s.Replace("от ", ""))}" : "NULL");
            GenerateSqlToPostMeta(id, "_price_previous_value", "field_55ffbbbb9516d");
            GenerateSqlToPostMeta(id, "country", "Greece");
            GenerateSqlToPostMeta(id, "_country", "field_55f6ed6825131");
            

            string descr = "";

            try
            {
                for (var i = 0; i < firstColumn.Count; i++)
                {
                    string firstCol = (firstColumn[i].FirstChild.InnerText).Trim();
                    string secondCol = (secondColumn[i].FirstChild.InnerText).Trim();
                    
                    switch (firstCol)
                    {
                       
                        case "Регион:":
                            GenerateSqlToPostMeta(id, "region", secondCol);
                            GenerateSqlToPostMeta(id, "_region", "field_55f7c06f7d5fa");
                            break;
                        case "Тип:":
                            GenerateSqlToPostMeta(id, "_type", "field_55ffba349c9cd");
                            if (secondCol.ToLower() == "квартира")
                            {
                                GenerateSqlToPostMeta(id, "type", "Apartment");
                            }
                            else
                            {
                                GenerateSqlToPostMeta(id, "type", "House");
                            }
                            break;

                        case "Категория:":
                            GenerateSqlToPostMeta(id, "_category", "field_55f7d9d34c744");
                            if (secondCol.ToLower() == "аренда")
                            {
                                GenerateSqlToPostMeta(id, "category", "Rent");
                            }
                            else
                            {
                                GenerateSqlToPostMeta(id, "category", "Sale");
                            }
                            
                            break;
                        case "Площадь:":
                            GenerateSqlToPostMeta(id, "square", secondCol);
                            GenerateSqlToPostMeta(id, "_square", "field_55f7da704c745");
                            break;

                        case "Расст. от моря:":
                            GenerateSqlToPostMeta(id, "distance_to_sea", secondCol);
                            GenerateSqlToPostMeta(id, "_distance_to_sea", "field_55f7dc384c746");
                            break;
                        case "Расст. до аэроп.:":
                            GenerateSqlToPostMeta(id, "distance_to_airport", secondCol);
                            GenerateSqlToPostMeta(id, "_distance_to_airport", "field_5612d8aa55fb4");
                            break;
                        case "Кол-во комнат:":
                            GenerateSqlToPostMeta(id, "number_of_rooms", secondCol);
                            GenerateSqlToPostMeta(id, "_number_of_rooms", "field_55ffbafb9c9ce");
                            break;
                        case "Расст до ближ. города:":
                            GenerateSqlToPostMeta(id, "distance_to_city", secondCol);
                            GenerateSqlToPostMeta(id, "_distance_to_city", "field_55f7dd4e4c747");
                            break;

                        case "Другие удобства:":

                            var str = secondCol;
                            var words = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var word in words)
                            {
                                var word2 = word.Trim();
                                switch (word2)
                                {
                                    case "wi-fi интернет":
                                        GenerateSqlToPostMeta(id, "feature_wi_fI", "a:1:{i:0;s:6:\"Active\";}");
                                        GenerateSqlToPostMeta(id, "_feature_wi_fI", "field_5620f83a492df");
                                        break;
                                }
                            }
                            
                            break;
                        
                            default:
                            break;
                    }

                    if (firstColumn.Count - 1 == i)
                    {
                        foreach (var p in description)
                        {
                            if (p.InnerText == "")
                                Console.WriteLine();
                            else
                            {
                                descr += (p.InnerText+"/n");
                            }
                        }
                    }
                }

                if (images != null)
                {
                    string[] imagesArr = new string[images.Count];
                    string imagesId = "";
                    for (var i = 0; i<images.Count; i++)
                    {
                        var imageUrl = images[i].OuterHtml.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                        var imageName = imageUrl[3].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        imagesArr[i] = id + imageName[6].Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];
                        imagesId += imagesArr[i] + ",";

                        try
                        {
                            var path = "D:/ms/photos/" + imageName[6];
                            DownloadImage(imageUrl[3], path);
                            GenerateSqlToPhotos(imagesArr[i], imageName[6], imageName[6], id);
                        }
                        catch (Exception)
                        {
                          //
                        }
                    }
                    GenerateSqlToPostMeta(id, "_thumbnail_id", imagesArr[0]);
                    GenerateSqlToPosts(id, descr, imagesId, title);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error");
            }
        }

        public static void MovingOnPages(int numberOfPages, string url)
        {
            //for (var i = 1; i <= numberOfPages; i++)
            for (var i = 2; i <= 2; i++)
            {
                var url2 =
                    $"http://www.grekodom.ru/RealtyObjects?multiType=null&multiRegion=null&type=undefined&subregion=undefined&span=undefined&distance=0&sortFilter=0&aim=0&squarefrom=0&squareto=&pricefrom=0&priceto=&roomF=0&roomT=&yearBuilt=0&area=0&seaView=false&pool=false&parking=false&furniture=false&heat=false&ds=0&page={i}";

                GetFullHtmlAsString(url2);

                var w1Client = new WebClient
                {
                    Proxy = null,
                    Encoding = Encode
                };

                var html1 = new HtmlDocument();

                html1.LoadHtml(w1Client.DownloadString(url2));
                GetId(html1);
            }
        }

        public static void GetId(HtmlDocument html)
        {

            var spans = html.DocumentNode.SelectNodes("//span[@class='area']");
            //var spans1 = html.DocumentNode.SelectNodes("//span[@class='area']");

            var counter = 0;

            foreach (var span in spans)
            {
                var id = (span.LastChild.InnerText).Remove(0, 1);             //ID
                var url3 = $"http://www.grekodom.ru/realtyobject/{id}";
                var w2Client = new WebClient
                {
                    Proxy = null,
                    Encoding = Encode
                };
                var html2 = new HtmlDocument();

                html2.LoadHtml(w2Client.DownloadString(url3));
                Console.WriteLine(++counter);

                GetMainInformation(html2, id);
            }
        }

        public static string GetFullHtmlAsString(string url)
        {
            Request = WebRequest.Create(url);
            Request.Proxy = null;
            Response = Request.GetResponse();
            using (var sReader = new StreamReader(Response.GetResponseStream(), Encode))
            {
                Console.WriteLine("Downloaded full HTML");
                return sReader.ReadToEnd();
            }
        }

        public static int GetPagesCount(HtmlDocument html)
        {
            var divs = html.DocumentNode.SelectSingleNode("//span[@class='col-md-6']");

            var str = divs.FirstChild.InnerText;
            var amountOfPosts = Convert.ToInt32(str.Remove(6));

            return amountOfPosts;
        }

        static void Main(string[] args)
        {
            //listOPosts = new List<Post>();

            Console.OutputEncoding = Encoding.GetEncoding(1251);                //Кодировка для русского вывода в консоль

            const string url = (@"http://www.grekodom.ru/realtyobjects");       //короткая версия ссылки (работает как и длинная)
            GetFullHtmlAsString(url);                                           //get HTML as String from url


            var wClient = new WebClient
            {
                Proxy = null,
                Encoding = Encode
            };

            var html = new HtmlDocument();
            html.LoadHtml(wClient.DownloadString(url));

            GetPagesCount(html);

            var numberOfPages = (GetPagesCount(html) / 15) + 1;                 //считаем количество страниц, ибо нефиг парсить лишнего

            Console.WriteLine("Number Of Pages = " + GetPagesCount(html));
            Console.WriteLine("Number Of Posts = " + numberOfPages);

            MovingOnPages(numberOfPages, url); //обход страниц сайта

            //SerializeToXml(listOPosts);

        }

    }

}
