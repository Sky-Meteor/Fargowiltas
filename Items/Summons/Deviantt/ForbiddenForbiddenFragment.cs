using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class ForbiddenForbiddenFragment : BaseSummon
    {
        public override int NPCType => NPCID.SandElemental;

        public override string NPCName => FargoUtils.IsChinese() ? "沙尘精" : "Sand Elemental";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Forbidden Forbidden Fragment");
            Tooltip.SetDefault("Summons Sand Elemental");
        }
    }
}