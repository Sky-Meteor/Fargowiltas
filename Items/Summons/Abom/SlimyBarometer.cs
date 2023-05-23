using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.Abom
{
    public class SlimyBarometer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slimy Barometer");
            // Tooltip.SetDefault("Starts the Slime Rain");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.slimeRain;
        }

        public override bool? UseItem(Player player)
        {
            Main.StartSlimeRain();
            Main.slimeWarningDelay = 1;
            Main.slimeWarningTime = 1;
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }
    }
}