using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Tiles
{
    public class RegalStatue : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Regal Statue");
            // Tooltip.SetDefault("Town NPCs respawn extremely quickly when nearby");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<RegalStatueSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.KingStatue)
                .AddIngredient(ItemID.QueenStatue)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}