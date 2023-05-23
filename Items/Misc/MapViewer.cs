﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace Fargowiltas.Items.Misc
{
    public class MapViewer : ModItem
    {
        public override string Texture => "Terraria/Images/Map_4";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Ancient Master's Map of the Lost King's Great Ancestors");
            // Tooltip.SetDefault("Reveals the map");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Text == "Reveals the map")
                {
                    tooltips.Remove(tooltips[i]);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        tooltips.Insert(i, new TooltipLine(Mod, "MapViewerTooltip", Language.GetTextValue("Mods.Fargowiltas.ItemTooltip.MapSingle")));
                    }
                    else
                    {
                        tooltips.Insert(i, new TooltipLine(Mod, "MapViewerTooltip", Language.GetTextValue("Mods.Fargowiltas.ItemTooltip.MapMulti")));
                    }
                    break;
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.White;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (WorldGen.InWorld(i, j))
                        {
                            Main.Map.Update(i, j, 255);
                        }
                    }
                }

                Main.refreshMap = true;
            }
            else
            {
                Point center = Main.LocalPlayer.Center.ToTileCoordinates();
                int range = 300;
                for (int i = center.X - range / 2; i < center.X + range / 2; i++)
                {
                    for (int j = center.Y - range / 2; j < center.Y + range / 2; j++)
                    {
                        if (WorldGen.InWorld(i, j))
                        {
                            Main.Map.Update(i, j, 255);
                        }
                    }
                }

                Main.refreshMap = true;
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TrifoldMap)
                .AddIngredient(ItemID.Goggles)
                .AddIngredient(ItemID.Sunglasses)
                .AddIngredient(ItemID.SuspiciousLookingEye)
                .AddIngredient(ItemID.MechanicalEye)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}