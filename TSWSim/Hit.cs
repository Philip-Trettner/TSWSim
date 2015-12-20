using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public class Hit
    {
        public double Damage;
        public double Time;
        public Skill Skill;
        public bool Critical;
        public bool Penetrating;

        public string HitType => Critical ? Penetrating ? "CritPen" : "Crit" : Penetrating ? "Pen" : "Normal";
        public override string ToString() => $"[{Time.ToString("F1")}s] {Skill.Name} hits for {Damage} ({HitType})";
    }
}
