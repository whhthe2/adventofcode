﻿using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net;
using System.Text;
namespace adventofcode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var rootCmd = new RootCommand
            {
                new Option<string>( "--input", "if provided, get input directly from the commandline" ),
                new Option<int>( "--year", "the year. Must also provide -d."),
                new Option<int>( "--day", "the day. Must also provide -y" )
            };*/

            var input = GetOrCacheAdventInput(2021, 5);

            var testInput = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";

            Day5.Part1(input, 1000);

        }

        public static string GetOrCacheAdventInput(int year, int day, int part=1)
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