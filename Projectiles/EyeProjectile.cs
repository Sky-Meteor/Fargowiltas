using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Projectiles
{
    public class EyeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("EyeProjectile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 13;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.npcProj = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.Bullet;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 60, -Projectile.velocity.X * 0.2f, -Projectile.velocity.Y * 0.2f, 100, Scale: 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;

                num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 60, -Projectile.velocity.X * 0.2f, -Projectile.velocity.Y * 0.2f, 100, Scale: 1f);
                Main.dust[num469].velocity *= 2f;
            }
        }
    }
}