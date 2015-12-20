using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public class Stats
    {
        public int AttackRating;
        public int WeaponPower;

        public int PenRating;
        public int CritRating;
        public int CritPowerRating;
        public int HitRating;

        public double CombatPower;

        public double CritChance => (55.14 - 100.3 / (Math.Exp(CritRating / 790.3) + 1)) / 100.0;
        public double CritDamage => Math.Sqrt(5 * CritPowerRating + 625) / 100.0;

        public static double CalculateCombatPower(double ar, double wp) => ar < 5200
            ? (375 - 600 / (Math.Exp(ar / 1400) + 1)) * (1 + wp / 375)
            : 204.38 + .5471 * wp + (.00008 * wp + .0301) * ar;

        public static Stats operator +(Stats l, Stats r) => new Stats
        {
            AttackRating = l.AttackRating + r.AttackRating,
            WeaponPower = l.WeaponPower + r.WeaponPower,
            PenRating = l.PenRating + r.PenRating,
            CritRating = l.CritRating + r.CritRating,
            CritPowerRating = l.CritPowerRating + r.CritPowerRating,
            HitRating = l.HitRating + r.HitRating,
        };

        public static Stats ClassicDps => new Stats
        {
            AttackRating = 6200,
            PenRating = 966,
            CritPowerRating = 375,
            HitRating = 656
        };
        public static Stats CritWeapon => new Stats
        {
            WeaponPower = 510,
            CritRating = 336
        };
        public static Stats CritPowerWeapon => new Stats
        {
            WeaponPower = 510,
            CritPowerRating = 328
        };
        public static Stats PurpleAuxWeapon => new Stats
        {
            WeaponPower = 423
        };

        public override string ToString() => $"{AttackRating} AR, {WeaponPower} WP, {HitRating} Hit, {PenRating} Pen, {CritRating} CR, {CritPowerRating} CP";
    }
}
