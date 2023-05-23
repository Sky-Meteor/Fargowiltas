using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.Abom
{
    public class RunawayProbe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Runaway Probe");
            // Tooltip.SetDefault("Starts the Martian invasion");
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

        public override bool? UseItem(Player player)
        {
            Main.StartInvasion(InvasionID.MartianMadness);
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }
    }
}