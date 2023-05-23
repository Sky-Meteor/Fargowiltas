﻿using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerDarkMage : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mysterious Energizer");
            // Tooltip.SetDefault("Formed after using 10 Really Forbidden Tomes\n'You feel like you've waited one thousand years'");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.rare = 1;
            Item.value = 100000;
        }
    }
}