using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class HolyGrail : BaseSummon
    {
        public override int NPCType => NPCID.Tim;

        public override string NPCName => LocalizedName("Tim");

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Holy Grail");
            Tooltip.SetDefault("Summons Tim" +
                               "\nOnly usable at night or underground");
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Fargowiltas:AnyGoldBar", 3)
                .AddIngredient(ItemID.Ruby, 3)
                .AddIngredient(ItemID.WineGlass)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}