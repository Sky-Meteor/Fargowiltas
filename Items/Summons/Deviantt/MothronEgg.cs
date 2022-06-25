using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class MothronEgg : BaseSummon
    {
        public override int NPCType => NPCID.Mothron;

        public override string NPCName => FargoUtils.IsChinese() ? "蛾怪" : "Mothron";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Mothron Egg");
            Tooltip.SetDefault("Summons Mothron" +
                               "\nOnly usable during Solar Eclipse");
        }

        public override bool CanUseItem(Player player)
        {
            return Main.eclipse;
        }
    }
}