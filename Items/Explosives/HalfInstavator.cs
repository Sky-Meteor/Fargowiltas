using Fargowiltas.Projectiles.Explosives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Explosives
{
    public class HalfInstavator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Half Instavator");
            Tooltip.SetDefault("Drops a bomb that creates half a hellevator instantly\nDo not use if any important building is below");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 32;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            //Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HalfInstaProj>();
            Item.shootSpeed = 5f;
        }

        /*public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Dynamite, 10)
                .AddIngredient(ItemID.Torch, 25)
                .AddIngredient(ItemID.FallenStar)
                .AddTile(TileID.Anvils)
                .Register();
        }*/
    }
}