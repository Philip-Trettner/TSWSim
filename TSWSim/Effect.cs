using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public class Effect
    {
        public Skill Skill;
        public double StartTime;
        public double Duration;

        public bool Affects(double time) => StartTime <= time && time <= StartTime + Duration;
    }
}
