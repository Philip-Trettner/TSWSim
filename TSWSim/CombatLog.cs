using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public class CombatLog
    {
        public double Duration;
        public List<Hit> Hits = new List<Hit>();

        public double PenPercentage => Hits.Sum(h => h.Penetrating ? 1.0 : 0.0) / Hits.Count;
        public double CritPercentage => Hits.Sum(h => h.Critical ? 1.0 : 0.0) / Hits.Count;

        public double TotalDamage => Hits.Sum(h => h.Damage);
        public double DPS => TotalDamage / Duration;

        public void Dump()
        {
            Console.WriteLine("=== COMBAT LOG ===");
            Console.WriteLine("Duration: " + Duration.ToString("F1") + "s");
            Console.WriteLine("Damage:   " + TotalDamage);
            Console.WriteLine("Pen %:    " + PenPercentage.ToString("P"));
            Console.WriteLine("Crit %:   " + CritPercentage.ToString("P"));
            Console.WriteLine("DPS:      " + DPS);
            Console.WriteLine($"{Hits.Count} hits:");
            foreach (var h in Hits)
                Console.WriteLine("  " + h);
        }
    }
}
