namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    [TestFixture]
    public class SolutionTest
    {
        private string[] urls = new string[] {"mysite.com/pictures/holidays.html",
                                          "www.codewars.com/users/GiacomoSorbi?ref=CodeWars",
                                          "www.microsoft.com/docs/index.htm#top",
                                          "mysite.com/very-long-url-to-make-a-silly-yet-meaningful-example/example.asp",
                                          "www.very-long-site_name-to-make-a-silly-yet-meaningful-example.com/users/giacomo-sorbi",
                                          "https://www.linkedin.com/in/giacomosorbi",
                                          "www.agcpartners.co.uk/",
                                          "www.agcpartners.co.uk",
                                          "https://www.agcpartners.co.uk/index.html",
                                          "http://www.agcpartners.co.uk"};

        private string[] seps = new string[] { " : ", " / ", " * ", " > ", " + ", " * ", " * ", " # ", " >>> ", " % " };


        private string[] anss = new string[] {"<a href=\"/\">HOME</a> : <a href=\"/pictures/\">PICTURES</a> : <span class=\"active\">HOLIDAYS</span>",
                                          "<a href=\"/\">HOME</a> / <a href=\"/users/\">USERS</a> / <span class=\"active\">GIACOMOSORBI</span>",
                                          "<a href=\"/\">HOME</a> * <span class=\"active\">DOCS</span>",
                                          "<a href=\"/\">HOME</a> > <a href=\"/very-long-url-to-make-a-silly-yet-meaningful-example/\">VLUMSYME</a> > <span class=\"active\">EXAMPLE</span>",
                                          "<a href=\"/\">HOME</a> + <a href=\"/users/\">USERS</a> + <span class=\"active\">GIACOMO SORBI</span>",
                                          "<a href=\"/\">HOME</a> * <a href=\"/in/\">IN</a> * <span class=\"active\">GIACOMOSORBI</span>",
                                          "<span class=\"active\">HOME</span>",
                                          "<span class=\"active\">HOME</span>",
                                          "<span class=\"active\">HOME</span>",
                                          "<span class=\"active\">HOME</span>"};
        [Test]
        public void ExampleTests()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"\nTest With: {urls[i]}");
                if (i == 5) Console.WriteLine("\nThe one used in the above test was my LinkedIn Profile; if you solved the kata this far and manage to get it, feel free to add me as a contact, message me about the language that you used and I will be glad to endorse you in that skill and possibly many others :)\n\n ");

                Assert.AreEqual(anss[i], Kata.GenerateBC(urls[i], seps[i]));
            }
        }

        [Test]
        public void RandomTests()
        {
            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                int docAndExt = random.Next(2);
                int bonus1 = random.Next(10) > 6 ? 1 : 0;
                int bonus2 = random.Next(10) > 6 ? 1 : 0;
                int[] config = new int[] { 1, 1, random.Next(2), random.Next(20, 50), docAndExt, docAndExt, bonus1, bonus2 };

                StringBuilder urlSB = new StringBuilder();

                // Protocol and site
                for (int urlPart = 0; urlPart < 2; urlPart++)
                {
                    for (int n = 0; n < config[urlPart]; n++)
                    {
                        urlSB.Append(table[urlPart][random.Next(table[urlPart].Length)]);
                    }
                }

                // Paths and words
                urlSB.Append("/");
                int tempLen = urlSB.Length;
                for (int urlPart = 2; urlPart < 4; urlPart++)
                {
                    for (int n = 0; n < config[urlPart]; n++)
                    {
                        urlSB.Append(table[urlPart][random.Next(table[urlPart].Length)]);
                        if (urlPart == 3)
                        {
                            if (urlSB.Length > config[urlPart] + tempLen) break;
                            urlSB.Append("-");
                        }
                        else
                        {
                            urlSB.Append("/");
                        }
                    }
                }

                // Files and extensions
                urlSB.Append("/");
                for (int urlPart = 4; urlPart < 6; urlPart++)
                {
                    for (int n = 0; n < config[urlPart]; n++)
                    {
                        urlSB.Append(table[urlPart][random.Next(table[urlPart].Length)]);
                    }
                }

                // Anchors and variables
                for (int urlPart = 6; urlPart < table.Length; urlPart++)
                {
                    for (int n = 0; n < config[urlPart]; n++)
                    {
                        urlSB.Append(table[urlPart][random.Next(table[urlPart].Length)]);
                    }
                }

                string url = urlSB.ToString();
                string sep = separators[random.Next(separators.Length)];
                string expected = Solution(url, sep);
                string actual = Kata.GenerateBC(url, sep);

                Console.WriteLine($"Using URL: {url}");
                Console.WriteLine($"Using Separator: '{sep}'\n");

                Assert.AreEqual(expected, actual);
            }
        }

        private static string[] separators = new string[] { " * ", " > ", " / ", " : ", " . ", " >>> ", " # ", " + ", " - ",
        " ; " };
        private static string[] siteprefixes = new string[] { "http://", "https://", "http://www.", "https://www.", "", "", "",
        "" };
        private static string[] sites = new string[] { "codewars.com", "google.ca", "facebook.fr", "linkedin.it", "github.com",
        "agcpartners.co.uk", "twitter.de", "pippi.pi" };
        private static string[] paths = new string[] { "pictures", "images", "profiles", "users",
        "pictures-you-wished-you-never-saw-but-you-cannot-unsee-now", "issues", "files", "games", "app", "wanted",
        "web", "most-downloaded", "most-viewed" };
        private static string[] words = new string[] { "the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at",
        "a", "bed", "uber", "cauterization", "pippi", "surfer", "insider", "kamehameha", "bladder", "skin",
        "transmutation", "meningitis", "paper", "research", "biotechnology", "bioengineering", "eurasian",
        "diplomatic", "immunity" };
        private static string[] documents = new string[] { "index", "funny", "giacomo-sorbi", "login", "test", "secret-page" };
        private static string[] extensions = new string[] { ".html", ".htm", ".asp", ".php" };
        private static string[] anchors = new string[] { "#top", "#bottom", "#images", "#info", "#conclusion", "#team", "#people",
        "#offers" };
        private static string[] parameters = new string[] { "?source=utm_pippi", "?hack=off", "?referral=CodeWars",
        "?order=desc&filter=adult", "?favourite=code", "?previous=normalSearch&output=full",
        "?rank=recent_first&hide=sold", "?sortBy=year" };

        private static string[][] table = new string[][] { siteprefixes, sites, paths, words, documents, extensions, anchors,
        parameters };

        private string Solution(string url, string separator)
        {
            List<string> output = new List<string>();
            List<string> paths = Regex.Replace(url, @"(.+:\/\/)?.+?(\/|$)([a-z\-\/]+)?(.+)?", "$3", RegexOptions.IgnoreCase).Split('/').ToList();
            paths.RemoveAll(x => x == "index" || string.IsNullOrWhiteSpace(x) || string.IsNullOrEmpty(x));

            var formatted = paths
              .Select((x, i) => i == paths.Count() - 1
                ? $"<span class=\"active\">{Acronymize(x)}</span>"
                : $"<a href=\"/{string.Join("/", Enumerable.Range(0, i + 1).Select(y => paths[y]))}/\">{Acronymize(x)}</a>");

            return string.Join(separator, new string[] { (paths.Count() == 0 ? "<span class=\"active\">HOME</span>" : "<a href=\"/\">HOME</a>") }.Concat(formatted));
        }

        private string Acronymize(string str)
        {
            return (str.Length <= 30
              ? Regex.Replace(str, "-", " ")
              : Regex.Replace(Regex.Replace(str, @"\b(the|of|in|from|by|with|and|or|for|to|at|a)\b", ""), @"\b(\w)\w*", "$1").Replace("-", "")).ToUpper();
        }
    }
}