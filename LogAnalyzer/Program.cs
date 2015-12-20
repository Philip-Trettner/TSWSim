using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = @"C:\Projekte\TSWSim\TSWSim\Data\log-bloodlines.txt";
            var rx = new Regex("^\\[..:..:..\\] Your Bloodline hits \\(Normal\\) Practice Demon for (\\d+) magical damage\\. \\(Normal\\)$");
            var csvlines = new List<string>();
            foreach (var line in File.ReadAllLines(filename))
            {
                var m = rx.Match(line);
                if (m.Success)
                {
                    var dmg = int.Parse(m.Groups[1].Value);
                    Console.WriteLine(dmg);
                    csvlines.Add(dmg.ToString());
                }
            }
            File.WriteAllLines(filename + ".csv", csvlines);
        }
    }
}
