using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Buffs
{
    public class WoodDrop : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wood Drop");
            Main.buffNoSave[Type] = true;
        }
    }
}