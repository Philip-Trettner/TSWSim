using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    /// <summary>
    /// Dmg simulator, also encapsulates the scenario
    /// </summary>
    public class Simulator
    {
        public double BaseDmgVariation = 0.1; // uniform
        public double PenetrationDamage = 0.49;
        public double PenetrationBaseChance = 0.25; // until we know how to link pen rating to %


        public int ExposedStacks = 10;
        public double BreachingBonus = .24;

        public Stats BaseStats = Stats.ClassicDps;
        public Dictionary<Weapon, Stats> WeaponStats = new Dictionary<Weapon, Stats>();
        public Dictionary<Weapon, Signet> WeaponSignets = new Dictionary<Weapon, Signet>();
        public List<Signet> TalismanSignets = new List<Signet>();

        public List<Skill> Rotation = new List<Skill>();
        public List<Passive> Passives = new List<Passive>();
        public int RotationRepeatIdx = 0;

        public void AddWeapon(Weapon weapon, Signet signet, Stats stats)
        {
            WeaponStats.Add(weapon, BaseStats + stats);
            WeaponSignets.Add(weapon, signet);
        }

        public void DumpSetup()
        {
            Console.WriteLine("=== SETUP ===");
            var weps = WeaponStats.Keys.ToArray();
            for (int i = 0; i < weps.Length; ++i)
            {
                Console.WriteLine($"Weapon {i + 1}: {weps[i]} with {WeaponSignets[weps[i]]}");
                Console.WriteLine($"  {WeaponStats[weps[i]]}");
            }
            Console.WriteLine("Other Signets/Effects:");
            foreach (var signet in TalismanSignets)
                Console.WriteLine("  " + signet);
            Console.WriteLine("Passives:");
            foreach (var passive in Passives)
                Console.WriteLine("  " + passive);
            Console.WriteLine("Rotation:");
            foreach (var skill in Rotation)
                Console.WriteLine("  " + skill.Name);
            Console.WriteLine();
        }

        public CombatLog Simulate(double duration, Random random)
        {
            var log = new CombatLog { Duration = duration };

            // calc combat powers
            foreach (var stats in WeaponStats.Values)
                stats.CombatPower = Stats.CalculateCombatPower(stats.AttackRating, stats.WeaponPower);

            // passives
            var hasBrawler = Passives.Contains(Passive.Brawler);
            var hasSealTheDeal = Passives.Contains(Passive.SealTheDeal);
            var hasElementalForce = Passives.Contains(Passive.ElementalForce);
            var hasDeadOnTarget = Passives.Contains(Passive.DeadOnTarget);

            // passive counters
            var elementalForce = 0;

            // effects
            var breachingShot = new Effect();
            var deadlyAim = new Effect();
            var shortFuse = new Effect();
            var finalFuse = new Effect();

            // Step sim
            var skillIdx = 0;
            var time = 0.0;
            while (time < duration)
            {
                // get current skill
                var skill = Rotation[skillIdx];
                // advance skill idx
                ++skillIdx;
                if (skillIdx >= Rotation.Count)
                    skillIdx = RotationRepeatIdx;

                // calc hits
                var hittime = time;
                var stats = skill.Hits > 0 ? WeaponStats[skill.Weapon] : null;
                for (var h = 0; h < skill.Hits; ++h)
                {
                    Debug.Assert(stats != null, "stats != null");

                    // base hit
                    var dmgvar = 1 + (random.NextDouble() * 2 - 1) * BaseDmgVariation;
                    var hit = new Hit
                    {
                        Time = hittime,
                        Skill = skill,
                        Damage = stats.CombatPower * skill.CombatPowerScaling * dmgvar
                    };

                    // expose
                    hit.Damage *= 1 + ExposedStacks * 0.03;

                    // additive bonus dmg
                    {
                        var additive = 0.0;
                        if (hasSealTheDeal && skill.IsConsumer) additive += .15; // 15% from seal the deal
                        if (hasDeadOnTarget && skill.Weapon == Weapon.Shotgun) additive += .10; // 10% from dead on target 
                        // TODO: lethality
                        // TODO: twist the knife
                        // TODO: signets
                        if (finalFuse.Affects(hittime)) additive += .25;
                        if (shortFuse.Affects(hittime)) additive += .15;

                        hit.Damage *= 1 + additive;
                    }

                    // critical 
                    {
                        // TODO: Mad skills
                        var critChance = stats.CritChance;
                        if (hasSealTheDeal && skill.IsConsumer) critChance += .05; // 5% from seal the deal
                        if (deadlyAim.Affects(hittime)) critChance += .40; // 40% from deadly aim

                        // check crit
                        critChance /= skill.ProcScaling;
                        hit.Critical = random.NextDouble() < critChance || elementalForce == 7;

                        if (hit.Critical)
                        {
                            var critDamage = stats.CritDamage;
                            // TODO: CiB
                            // TODO: Laceration
                            if (hasBrawler) critDamage += .15; // 15% from brawler
                            hit.Damage *= 1 + critDamage;
                        }
                    }

                    // penetrating
                    {
                        var penChance = PenetrationBaseChance;
                        // TODO: iron maiden
                        if (breachingShot.Affects(hittime)) penChance += .45; // 45% from breaching shot

                        // check pen
                        penChance /= skill.ProcScaling;
                        hit.Penetrating = random.NextDouble() < penChance;

                        if (hit.Penetrating)
                        {
                            var penDamage = PenetrationDamage + BreachingBonus;
                            hit.Damage *= 1 + penDamage;
                        }
                    }

                    // TODO: procs

                    // Finalize hit
                    hit.Damage = Math.Round(hit.Damage);
                    log.Hits.Add(hit);

                    // advance hit steps
                    hittime += skill.HitTimestep;
                }

                // special
                if (skill.Special != Special.None)
                    switch (skill.Special)
                    {
                        case Special.Bombardment:
                            break;

                        case Special.DeadlyAim:
                            deadlyAim.StartTime = time;
                            deadlyAim.Duration = 10;
                            break;
                        case Special.BreachingShot:
                            breachingShot.StartTime = time;
                            breachingShot.Duration = 8;
                            break;
                        case Special.ShortFuse:
                            shortFuse.StartTime = time;
                            shortFuse.Duration = 10;
                            break;
                        case Special.FinalFuse:
                            finalFuse.StartTime = time;
                            finalFuse.Duration = 10;
                            break;

                        default: throw new NotImplementedException();
                    }

                // advance elemental force
                if (hasElementalForce && skill.CountForEF)
                    elementalForce = (elementalForce + 1) % 8;

                // advance time
                time += skill.Duration;
            }
            return log;
        }
    }
}
