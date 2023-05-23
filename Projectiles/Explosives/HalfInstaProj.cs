using Fargowiltas.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Projectiles.Explosives
{
    public class HalfInstaProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Half Instavator");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            Projectile.aiStyle = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 170;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            //cavern height plus halfway to hell
            int yEndpoint = (int)(Main.rockLayer + (Main.maxTilesY - 200 - Main.rockLayer) / 2);

            // Five across
            for (int x = -2; x <= 2; x++)
            {
                for (int y = (int)(1 + position.Y / 16.0f); y <= yEndpoint; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, y];

                    if (tile == null)
                        continue;

                    if (!FargoGlobalProjectile.OkayToDestroyTile(tile))
                        continue;

                    FargoGlobalTile.ClearEverything(xPosition, y, false);

                    if (x == 0)
                    {
                        WorldGen.PlaceTile(xPosition, y, TileID.Rope);
                    }

                    NetMessage.SendTileSquare(-1, xPosition, y, 1);
                }
            }
        }
    }
}