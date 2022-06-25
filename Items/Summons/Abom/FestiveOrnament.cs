using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Abom
{
    public class FestiveOrnament : BaseSummon
    {
        public override int NPCType => NPCID.Everscream;

        public override string NPCName => FargoUtils.IsChinese() ? "常绿尖叫怪" : "Everscream";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Festive Ornament");
            Tooltip.SetDefault("Summons Everscream" +
                               "\nOnly usable at night");
        }

        public override bool CanUseItem(Player player) => !Main.dayTime;
    }
}