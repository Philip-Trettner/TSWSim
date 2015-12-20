using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TSWSim.Tests
{
    [TestFixture]
    public class Misc
    {
        private readonly Simulator sim = new Simulator();

        public Misc()
        {
            sim.BaseStats = Stats.ClassicDps;

            sim.TalismanSignets.Add(Signet.Laceration);
            sim.TalismanSignets.Add(Signet.ConeyIslandBand);

            sim.AddWeapon(Weapon.Shotgun, Signet.Abuse, Stats.CritWeapon);
            sim.AddWeapon(Weapon.Pistol, Signet.Aggression, Stats.CritPowerWeapon);
            sim.AddWeapon(Weapon.Aux, Signet.Augur, Stats.PurpleAuxWeapon);

            sim.Passives.AddRange(new[]
            {
                Passive.ElementalForce,
                Passive.Brawler,
                Passive.DeadOnTarget,
                Passive.IronMaiden,
                Passive.Lethality,
                Passive.TwistTheKnife,
                Passive.SealTheDeal,
            });

            sim.Rotation.AddRange(new[]
            {
                Skills.Bombardment,
                // EF-0
                Skills.Striker,
                Skills.Striker,
                Skills.Striker,
                Skills.Striker,
                // - buffs
                Skills.DeadlyAim,
                Skills.BreachingShot,
                Skills.ShortFuseExternal,
                // ---
                Skills.Striker,
                Skills.OutForAKill,
                Skills.Timber,
                Skills.Shootout,
                // EF-0
                Skills.OutForAKill,
                Skills.Striker,
                Skills.Striker,
                Skills.Striker,
                Skills.Striker,
                Skills.Striker,
                Skills.OutForAKill,
                Skills.Shootout,
                // EF-0
            });
        }

        [Test]
        public void SingleRotation()
        {
            var r = new Random();
            var log = sim.Simulate(20.0, r);
            Assert.GreaterOrEqual(log.Hits.Count, 20);
            sim.DumpSetup();
            log.Dump();

            // force crit EF
            foreach (var hit in log.Hits.Where(hit => hit.Skill == Skills.Shootout))
                Assert.That(hit.Critical);
        }

        [Test]
        public void LotsOfRotations()
        {
            var r = new Random();
            var dps = new List<double>();
            const int rots = 50000;
            for (var i = 0; i < rots; ++i)
                dps.Add(sim.Simulate(20.0, r).DPS);
            dps.Sort();
            Console.WriteLine("Total # of rotations: " + rots);
            Console.WriteLine("DPS Average: " + (int)(dps.Sum() / dps.Count));
            Console.WriteLine("  Min: " + (int)dps.First());
            Console.WriteLine("  10%: " + (int)dps[dps.Count * 1 / 10]);
            Console.WriteLine("  Med: " + (int)dps[dps.Count / 2]);
            Console.WriteLine("  90%: " + (int)dps[dps.Count * 9 / 10]);
            Console.WriteLine("  Max: " + (int)dps.Last());
        }
    }
}
