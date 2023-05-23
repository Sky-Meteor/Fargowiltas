﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Fargowiltas.Items.Summons.Abom
{
    public class WeatherBalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Weather Balloon");
            // Tooltip.SetDefault("Starts the Rain");
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

        public override bool CanUseItem(Player player)
        {
            return !Main.IsItRaining && !Main.IsItStorming;
        }
                
        public override bool? UseItem(Player player)
        {
            LanternNight.GenuineLanterns = false;
            LanternNight.ManualLanterns = false;

            //sets rain time to 12 hours
            int day = 86400;
            int hour = day / 24;
            Main.rainTime = hour * 12;
            Main.raining = true;
            Main.maxRaining = Main.cloudAlpha = 0.9f;

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
                Main.SyncRain();
            }
            FargoUtils.PrintText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.WeatherBalloon"), new Color(175, 75, 255));
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }
    }
}