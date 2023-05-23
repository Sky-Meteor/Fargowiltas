using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Tiles
{
    public class SiblingPylon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sibling Pylon");
            /* Tooltip.SetDefault(@"Teleport to another pylon when Mutant, Abominationn, and Deviantt are nearby
You can only place one per type"); */
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SiblingPylonTile>());
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 10));
        }
    }
}