using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class PlunderedBooty : BaseSummon
    {
        public override int NPCType => NPCID.PirateShip;

        public override string NPCName => FargoUtils.IsChinese() ? "荷兰飞盗船" : "Flying Dutchman";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Plundered Booty");
            Tooltip.SetDefault("Summons Flying Dutchman");
        }
    }
}