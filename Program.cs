using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net;
using System.Text;
using System.Reflection;

namespace adventofcode2021
{
    class Program
    {
        #nullable enable
        static void Main(string[] args)
        {
            var rootCmd = new RootCommand
            {
                new Option<string?>( "--input", "if provided, get input directly from the commandline" ),
                new Option<uint?>( "--year", "the year." ),
                new Option<uint?>( "--day", "the day." )
            };
            rootCmd.Handler = CommandHandler.Create<string, uint?, uint?, IConsole>(Solve);
            rootCmd.InvokeAsync(args);
        }

        public static void Solve(string input, uint? year, uint? day, IConsole console)
        {
            uint selectedYear = year.HasValue ? year.Value : (uint)DateTime.Now.Year;
            uint selectedDay = day.HasValue ? day.Value : (uint)DateTime.Now.Day;
            string rawInput = string.IsNullOrEmpty(input) ? rawInput = GetOrCacheAdventInput(selectedYear, selectedDay) : input;
            
            string puzzleTypeName = $"adventofcode2021.Day{selectedDay.ToString("D2")}";
            Type? puzzle = Type.GetType(puzzleTypeName);
            if (puzzle == null)
            {
                throw new Exception($"cannot find type for {puzzleTypeName}.");
            }

            string solverMethodName = "Solve";
            MethodInfo? solver = puzzle.GetMethod(solverMethodName, BindingFlags.Static | BindingFlags.Public);
            if (solver == null)
            {
                throw new Exception($"cannot find public, static method named {solverMethodName}");
            }

            //Invoke the puzzle solver method
            var solution = solver.Invoke(null, new object[] { rawInput });
        }
        #nullable disable
        
        public static string GetOrCacheAdventInput(uint year, uint day, int part=1)
        {
            var userDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AdventOfCode");
            var cacheDirectory = Path.Combine(userDirectory, $"{year}", $"{day}");
            Directory.CreateDirectory(cacheDirectory);

            var filePath = Path.Combine(cacheDirectory, $"part{part}");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            try
            {
                var sessionToken = File.ReadAllText(Path.Combine(userDirectory, $"aoc.session.token"));

                var site = $"https://adventofcode.com/{year}/day/{day}/input";
                HttpWebRequest request = WebRequest.Create(site) as HttpWebRequest;
                request.Headers.Add(HttpRequestHeader.Cookie, $"session={sessionToken}");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                WebHeaderCollection header = response.Headers;

                var encoding = ASCIIEncoding.ASCII;
                string responseText = string.Empty;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }
                File.WriteAllText(filePath, responseText);
                return responseText;
            }
            catch( Exception e)
            {
                Console.WriteLine($"An error occurred while fetching input: {e}");
            }
            return string.Empty;
        }
    }
}