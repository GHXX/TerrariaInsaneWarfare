using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsaneWarfare
{
    class WeaponMissileLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches missiles after your enemies!");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5; // gun
            item.value = 30000;
            item.UseSound = SoundID.Item98;
            item.noMelee = true;
            item.ranged = true;
            item.rare = 5;  //rainbow :D
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ProjectileMissileLauncher"); //207 - chlorophyte
            item.shootSpeed = 6f;
            item.damage = 10;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
