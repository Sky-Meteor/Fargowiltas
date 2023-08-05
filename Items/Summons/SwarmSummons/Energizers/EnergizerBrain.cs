using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerBrain : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brainy Energizer");
            // Tooltip.SetDefault("Formed after using 10 Brain Storms\n'You still feel dumb'");
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