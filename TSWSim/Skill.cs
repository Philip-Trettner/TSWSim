using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public enum Special
    {
        None,

        Bombardment,
        DeadlyAim,
        BreachingShot,
        ShortFuse,
        FinalFuse,
    }

    public class Skill
    {
        public string Name;
        public double CombatPowerScaling;
        public double ProcScaling = 1.0; // 0.8 for 3 hits, 0.75 for 4 hits
        public int Hits = 1;
        public double Duration = 1.0;
        public double HitTimestep = 0.0;
        public Weapon Weapon;
        public Special Special = Special.None;
        public bool CountForEF = true;
        public bool IsConsumer = false;
    }
}
