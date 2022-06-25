using Terraria.ID;

namespace Fargowiltas.Items.Summons.Abom
{
    public class BatteredClub : BaseSummon
    {
        public override int NPCType => NPCID.DD2OgreT2;

        public override string NPCName => FargoUtils.IsChinese() ? "食人魔" : "Ogre";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Battered Club");
            Tooltip.SetDefault("Summons the Ogre");
        }
    }
}