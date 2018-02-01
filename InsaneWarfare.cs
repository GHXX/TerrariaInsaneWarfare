using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InsaneWarfare
{
    public class InsaneWarfare : Mod
    {
        public static Random rand = new Random();
        public InsaneWarfare()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };

        }
    }
}
