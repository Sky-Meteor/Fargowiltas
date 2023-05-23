﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace Fargowiltas.Items.Tiles
{
    public class CrucibleCosmos : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crucible of the Cosmos");
            // Tooltip.SetDefault("Functions as every crafting station\n'It seems to be hiding magnificent power'");
            //DisplayName.AddTranslation(GameCulture.Chinese, "宇宙坩埚");
            //Tooltip.AddTranslation(GameCulture.Chinese, "'它似乎隐藏着巨大的力量'\n包含几乎所有制作环境");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.Mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.OverrideColor = new Color?(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.rare = 10;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(2);
            Item.createTile = ModContent.TileType<CrucibleCosmosSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MultitaskCenter>())
                .AddIngredient(ModContent.ItemType<ElementalAssembler>())
				.AddIngredient(ModContent.ItemType<GoldenDippingVat>())
                .AddRecipeGroup("Fargowiltas:AnyForge")
                .AddRecipeGroup("Fargowiltas:AnyHMAnvil")
                .AddRecipeGroup("Fargowiltas:AnyBookcase")
                .AddIngredient(ItemID.CrystalBall)
                .AddIngredient(ItemID.Autohammer)
                .AddIngredient(ItemID.BlendOMatic)
                .AddIngredient(ItemID.MeatGrinder)
                .AddIngredient(ItemID.SteampunkBoiler)
                .AddIngredient(ItemID.LesionStation)
                .AddIngredient(ItemID.FleshCloningVaat)
                .AddIngredient(ItemID.LihzahrdFurnace)
                .AddIngredient(ItemID.LunarCraftingStation)
                .AddIngredient(ItemID.LunarBar, 25)
                .AddTile(TileID.DemonAltar)
                .Register();

            if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage))
            {
                CreateRecipe()
                    .AddIngredient(magicStorage.Find<ModItem>("CombinedStations4Item").Type)
                    .AddIngredient(ItemID.LunarBar, 25)
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }
        }
    }
}