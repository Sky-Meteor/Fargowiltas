using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Fargowiltas.Projectiles
{
    public class LumberJaxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("LumberJaxe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 150;
            AIType = ProjectileID.CrystalBullet;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0, Projectile.velocity.Y * 0, ModContent.ProjectileType<Explosion>(), (int)(Projectile.damage * 1f), Projectile.knockBack, Projectile.owner);
        }
    }
}