﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.Abom
{
    public class Anemometer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Anemometer");
            // Tooltip.SetDefault("Starts a Windy Day");
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
            return Main.windSpeedTarget <= 0.4f; //wind threshold
        }

        public override bool? UseItem(Player player)
        {
            Main.windSpeedTarget = Main.windSpeedCurrent = 0.8f; //40mph?

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
            FargoUtils.PrintText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.Anemometer"), new Color(175, 75, 255));
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }
    }
}