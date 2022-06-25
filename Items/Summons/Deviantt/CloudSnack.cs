using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class CloudSnack : BaseSummon
    {
        public override int NPCType => NPCID.WyvernHead;

        public override string NPCName => FargoUtils.IsChinese() ? "飞龙" : "Wyvern";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cloud Snack");
            Tooltip.SetDefault("Summons Wyvern");
        }
    }
}