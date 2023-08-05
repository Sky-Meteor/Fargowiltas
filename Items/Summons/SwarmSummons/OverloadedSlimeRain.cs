using Fargowiltas.Items.Summons.Abom;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons
{
    public class OverloadedSlimeRain : ModItem
    {
        public override string Texture => "Fargowiltas/Items/Summons/Abom/SlimyBarometer";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slimy Stormcaller");
            // Tooltip.SetDefault("Summons an Overloaded Slime Rain\nUse again to stop the event");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 1000;
            Item.rare = 1;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 4;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            if (FargoWorld.OverloadedSlimeRain)
            {
                // cancel it
                Main.StopSlimeRain();
                FargoWorld.OverloadedSlimeRain = false;
            }
            else
            {
                if (Main.netMode != 1)
                {
                    
                    Main.StartSlimeRain();
                    Main.slimeWarningDelay = 1;
                    Main.slimeWarningTime = 1;
                    Main.slimeRainKillCount = -10000; 
                }
                else
                {
                    NetMessage.SendData(61, -1, -1, null, player.whoAmI, -1f);
                }

                FargoWorld.OverloadedSlimeRain = true;
                SoundEngine.PlaySound(SoundID.Roar, player.position);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SlimyBarometer>())
                .AddIngredient(null, "Overloader", 10)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}
