using Terraria.ID;

namespace Fargowiltas.Items.Summons.Abom
{
    public class BetsyEgg : BaseSummon
    {
        public override int NPCType => NPCID.DD2Betsy;

        public override string NPCName => FargoUtils.IsChinese() ? "双足翼龙" : "Betsy";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Dragon's Egg");
            Tooltip.SetDefault("Summons Betsy");
        }
    }
}