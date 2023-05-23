﻿using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class DevianttPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Deviantt Pants");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.vanity = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChumBucket)
                .AddIngredient(ItemID.SoulofFlight, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
