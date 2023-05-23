﻿using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Fargowiltas.Items.Summons.Abom
{
    public class ForbiddenScarab : ModItem
    {
        //private static MethodInfo startSandstormMethod;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Forbidden Scarab");
            // Tooltip.SetDefault("Starts a Sandstorm");
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
            return !Sandstorm.Happening;
        }

        public override bool? UseItem(Player player)
        {
            Sandstorm.StartSandstorm();

            NetMessage.SendData(MessageID.WorldData);
            FargoUtils.PrintText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.ForbiddenScarab"), new Color(175, 75, 255));
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }
    }
}