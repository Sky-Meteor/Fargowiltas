using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerCultist : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Insanity Energizer");
            // Tooltip.SetDefault("Formed after using 10 Zealot's Madness\n'You're probably insane'");
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