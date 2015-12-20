using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWSim
{
    public enum AugmentType
    {
        Brutal,
        Piercing,
        Fierce,
        Ferocious,
        Grievous
    }

    public class Augment
    {
        public AugmentType Type;
        public int Level;

        public Augment(AugmentType type, int level)
        {
            Type = type;
            Level = level;
        }
    }
}
