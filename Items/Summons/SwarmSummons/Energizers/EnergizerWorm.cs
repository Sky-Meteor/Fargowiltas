using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerWorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wormy Energizer");
            // Tooltip.SetDefault("Formed after using 10 Worm Chickens\n'It's a little squishy'");
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