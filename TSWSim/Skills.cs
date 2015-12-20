using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public static class Skills
    {

        public static Skill Striker => new Skill
        {
            Name = "Striker",
            CombatPowerScaling = 0.725,
            Weapon = Weapon.Shotgun
        };

        public static Skill OutForAKill => new Skill
        {
            Name = "Out for a Kill",
            CombatPowerScaling = 3.03,
            Weapon = Weapon.Shotgun,
            IsConsumer = true
        };

        public static Skill Timber => new Skill
        {
            Name = "Timber",
            CombatPowerScaling = 1.885,
            Weapon = Weapon.Aux
        };

        public static Skill Shootout => new Skill
        {
            Name = "Shootout",
            CombatPowerScaling = 0.96,
            Hits = 5,
            Duration = 2.5,
            HitTimestep = .5,
            ProcScaling = 0.9, // ???
            Weapon = Weapon.Pistol,
            IsConsumer = true
        };

        public static Skill Bombardment => new Skill
        {
            Name = "Bombardment",
            Hits = 0,
            CombatPowerScaling = 1.735,
            CountForEF = false,
            Special = Special.Bombardment,
            Weapon = Weapon.Shotgun
        };

        public static Skill BreachingShot => new Skill
        {
            Name = "Breaching Shot",
            Hits = 0,
            CountForEF = false,
            Special = Special.BreachingShot,
            Weapon = Weapon.Shotgun
        };
        public static Skill DeadlyAim => new Skill
        {
            Name = "Deadly Aim",
            Hits = 0,
            CountForEF = false,
            Special = Special.DeadlyAim,
            Weapon = Weapon.Pistol
        };
        public static Skill ShortFuseExternal => new Skill
        {
            Name = "Short Fuse",
            Hits = 0,
            CountForEF = false,
            Special = Special.ShortFuse,
            Weapon = Weapon.Elemental
        };
        public static Skill FinalFuseExternal => new Skill
        {
            Name = "Short Fuse",
            Hits = 0,
            CountForEF = false,
            Special = Special.FinalFuse,
            Weapon = Weapon.Elemental
        };

        public static readonly List<Skill> All = new List<Skill>
        {
            Striker,
            OutForAKill,
            Shootout,
            Bombardment
        };
    }
}
