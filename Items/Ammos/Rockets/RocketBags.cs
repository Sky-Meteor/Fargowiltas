using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Ammos.Rockets
{
    public abstract class RocketBag : BaseAmmo
    {
        public abstract int RocketProjectile { get; }
        public abstract int SnowmanProjectile { get; }
        public abstract int GrenadeProjectile { get; }
        public abstract int MineProjectile { get; }

        public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            switch (weapon.type)
            {
                case ItemID.RocketLauncher: type = RocketProjectile; break;
                case ItemID.SnowmanCannon: type = SnowmanProjectile; break;
                case ItemID.GrenadeLauncher: type = GrenadeProjectile; break;
                case ItemID.ProximityMineLauncher: type = MineProjectile; break;
                default: break;
            }
        }
    }

    class Rocket1Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.RocketI;
        public override int RocketProjectile => ProjectileID.RocketI;
        public override int SnowmanProjectile => ProjectileID.RocketSnowmanI;
        public override int GrenadeProjectile => ProjectileID.GrenadeI;
        public override int MineProjectile => ProjectileID.ProximityMineI;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Rocket I Bag");
        }
    }

    class Rocket2Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.RocketII;
        public override int RocketProjectile => ProjectileID.RocketII;
        public override int SnowmanProjectile => ProjectileID.RocketSnowmanII;
        public override int GrenadeProjectile => ProjectileID.GrenadeII;
        public override int MineProjectile => ProjectileID.ProximityMineII;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Rocket II Bag");
        }
    }

    class Rocket3Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.RocketIII;
        public override int RocketProjectile => ProjectileID.RocketIII;
        public override int SnowmanProjectile => ProjectileID.RocketSnowmanIII;
        public override int GrenadeProjectile => ProjectileID.GrenadeIII;
        public override int MineProjectile => ProjectileID.ProximityMineIII;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Rocket III Bag");
        }
    }

    class Rocket4Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.RocketIV;
        public override int RocketProjectile => ProjectileID.RocketIV;
        public override int SnowmanProjectile => ProjectileID.RocketSnowmanIV;
        public override int GrenadeProjectile => ProjectileID.GrenadeIV;
        public override int MineProjectile => ProjectileID.ProximityMineIV;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Rocket IV Bag");
        }
    }

    class ClusterRocket1Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.ClusterRocketI;
        public override int RocketProjectile => ProjectileID.ClusterRocketI;
        public override int SnowmanProjectile => ProjectileID.ClusterSnowmanRocketI;
        public override int GrenadeProjectile => ProjectileID.ClusterGrenadeI;
        public override int MineProjectile => ProjectileID.ClusterMineI;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Cluster Rocket I Bag");
        }
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class ClusterRocket2Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.ClusterRocketII;
        public override int RocketProjectile => ProjectileID.ClusterRocketII;
        public override int SnowmanProjectile => ProjectileID.ClusterSnowmanRocketII;
        public override int GrenadeProjectile => ProjectileID.ClusterGrenadeII;
        public override int MineProjectile => ProjectileID.ClusterMineII;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Cluster Rocket II Bag");
        }
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class DryRocketBag : RocketBag
    {
        public override int AmmunitionItem => ItemID.DryRocket;
        public override int RocketProjectile => ProjectileID.DryRocket;
        public override int SnowmanProjectile => ProjectileID.DrySnowmanRocket;
        public override int GrenadeProjectile => ProjectileID.DryGrenade;
        public override int MineProjectile => ProjectileID.DryMine;
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class WetRocketBag : RocketBag
    {
        public override int AmmunitionItem => ItemID.WetRocket;
        public override int RocketProjectile => ProjectileID.WetRocket;
        public override int SnowmanProjectile => ProjectileID.WetSnowmanRocket;
        public override int GrenadeProjectile => ProjectileID.WetGrenade;
        public override int MineProjectile => ProjectileID.WetMine;
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class LavaRocketBag : RocketBag
    {
        public override int AmmunitionItem => ItemID.LavaRocket;
        public override int RocketProjectile => ProjectileID.LavaRocket;
        public override int SnowmanProjectile => ProjectileID.LavaSnowmanRocket;
        public override int GrenadeProjectile => ProjectileID.LavaGrenade;
        public override int MineProjectile => ProjectileID.LavaMine;
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class HoneyRocketBag : RocketBag
    {
        public override int AmmunitionItem => ItemID.HoneyRocket;
        public override int RocketProjectile => ProjectileID.HoneyRocket;
        public override int SnowmanProjectile => ProjectileID.HoneySnowmanRocket;
        public override int GrenadeProjectile => ProjectileID.HoneyGrenade;
        public override int MineProjectile => ProjectileID.HoneyMine;
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class MiniNuke1Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.MiniNukeI;
        public override int RocketProjectile => ProjectileID.MiniNukeRocketI;
        public override int SnowmanProjectile => ProjectileID.MiniNukeSnowmanRocketI;
        public override int GrenadeProjectile => ProjectileID.MiniNukeGrenadeI;
        public override int MineProjectile => ProjectileID.MiniNukeMineI;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Mini Nuke I Bag");
        }
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }

    class MiniNuke2Bag : RocketBag
    {
        public override int AmmunitionItem => ItemID.MiniNukeII;
        public override int RocketProjectile => ProjectileID.MiniNukeRocketII;
        public override int SnowmanProjectile => ProjectileID.MiniNukeSnowmanRocketII;
        public override int GrenadeProjectile => ProjectileID.MiniNukeGrenadeII;
        public override int MineProjectile => ProjectileID.MiniNukeMineII;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Endless Mini Nuke II Bag");
        }
        public override string Texture => "Fargowiltas/Items/Placeholder";
    }
}
