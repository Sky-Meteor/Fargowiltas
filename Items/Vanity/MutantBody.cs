﻿using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class MutantBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mutant Body");
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
                .AddIngredient(ItemID.SkeletronMask)
                .AddIngredient(ItemID.DestroyerMask)
                .AddIngredient(ItemID.SkeletronPrimeMask)
                .AddIngredient(ItemID.TwinMask)
                .AddIngredient(ItemID.GolemMask)
                .AddIngredient(ItemID.DukeFishronMask)
                .AddIngredient(ItemID.FairyQueenMask)
                .AddIngredient(ItemID.BossMaskMoonlord)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
