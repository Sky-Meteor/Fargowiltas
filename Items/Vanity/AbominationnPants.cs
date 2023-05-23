﻿using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class AbominationnPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Abominationn Pants");
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
                .AddIngredient(ItemID.TreeTrunks)
                .AddIngredient(ItemID.FallenTuxedoPants)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
