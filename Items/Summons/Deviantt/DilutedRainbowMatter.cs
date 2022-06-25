using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class DilutedRainbowMatter : BaseSummon
    {
        public override int NPCType => NPCID.RainbowSlime;

        public override string NPCName => FargoUtils.IsChinese() ? "彩虹史莱姆" : "Rainbow Slime";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Diluted Rainbow Matter");
            Tooltip.SetDefault("Summons Rainbow Slime");
        }
    }
}