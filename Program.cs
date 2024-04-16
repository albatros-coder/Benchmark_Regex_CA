using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Benchmark_Regex_CA
{
    public class UrlBenchmark
    {
        public string Text { get; }

        public UrlBenchmark()
        {
            Text = @"Within this text, you can find various URLs. For instance, there are links such as http://example.com and https://www.example.org/documents/report.docx. Additionally, URLs like http://samplewebsite.com, https://samplepage.org, http://www.example.net/path/to/file.pdf, and https://www.testdomain.com/article?id=123 are included. Furthermore, links like https://www.testsite.net/directory/subdirectory/page, http://testdomain.org, https://example.edu/research/papers/2023/study.pdf, and http://www.samplesite.com/blog/post-1 are present. Lastly, you can also come across URLs such as https://www.example.com/path/to/page1, https://demodomain.net, http://www.example.org/resources/images/image1.jpg, and https://www.samplewebsite.net/products?category=electronics. The text also contains links like https://example.gov/public/documents/report.docx, http://www.samplecompany.com/about-us, https://www.example.com/services/web-design, http://testblog.net/2023/04/16/new-post, https://www.example.org/events/conference2023, and http://samplesite.edu/faculty/john-doe. In addition, URLs such as https://example.net/support/faq, http://www.samplestore.com/products/item1, https://www.testcompany.com/careers/openings, and http://examplefoundation.org/grants/apply are included. Moreover, the passage features links like https://www.example.gov/laws/act123, http://sampleuniversity.edu/admissions, https://www.example.org/membership/join, http://testcharity.net/donate, and https://www.examplecorp.com/investors/reports. URLs such as http://www.sampleschool.edu/programs/mathematics, https://example.com/blog/categories/technology, http://www.testnews.com/local/city-council-meeting, https://www.example.net/shop/cart, and http://sampleart.org/exhibitions/current are also present. Finally, the text includes a variety of URLs like https://example.gov/taxes/forms/1040, http://www.samplehospital.com/departments/cardiology, https://www.example.edu/library/catalog, http://testtheater.org/shows/upcoming, https://www.examplemuseum.net/collections/ancient-art, http://samplepark.gov/trails/hiking, https://www.example.com/recipes/desserts/cookies, http://www.testrestaurant.com/menu/lunch, https://example.net/travel/destinations/europe, and http://samplehotel.com/reservations/book.";
        }

        [Benchmark]
        public List<string> CustomAlgorithmApproach()
        {
            List<string> foundUrls = new List<string>();
            string[] words = Text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (IsUrl(word))
                {
                    foundUrls.Add(word);
                }
            }

            return foundUrls;
        }

        [Benchmark]
        public List<string> RegexApproach()
        {
            string regexPattern = @"(?<protocol>https?)://(?<domain>[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+)(?<path>/[^\s]*)?";
            Regex regex = new Regex(regexPattern);
            MatchCollection matches = regex.Matches(Text);

            List<string> urls = new List<string>();
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }

        private bool IsUrl(string word)
        {
            return word.StartsWith("http://") || word.StartsWith("https://");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var benchmark = new UrlBenchmark();

            BenchmarkRunner.Run<UrlBenchmark>();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            List<string> customUrls = benchmark.CustomAlgorithmApproach();
            string customTxtPath = Path.Combine(desktopPath, "customalgorithm.txt");
            File.WriteAllLines(customTxtPath, customUrls);

            List<string> regexUrls = benchmark.RegexApproach();
            string regexTxtPath = Path.Combine(desktopPath, "regexapproach.txt");
            File.WriteAllLines(regexTxtPath, regexUrls);

            Console.WriteLine("Custom urls saved to " + customTxtPath);
            Console.WriteLine("Regex urls saved to " + regexTxtPath);
        }
    }
}
