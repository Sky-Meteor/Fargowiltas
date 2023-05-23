﻿using Fargowiltas.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Projectiles.Explosives
{
    public class InstatrackProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Instatrack");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            Projectile.aiStyle = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            // All the way across
            for (int x = 1; x <= Main.maxTilesX; x++)
            {
                // Six down, last is platforms
                for (int y = -5; y <= 0; y++)
                {
                    int xPosition = x;
                    int yPosition = (int)(y + position.Y / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, yPosition];

                    if (tile == null)
                        continue;

                    if (!FargoGlobalProjectile.OkayToDestroyTile(tile))
                        continue;

                    if (y == 0)
                    {
                        FargoGlobalTile.ClearEverything(xPosition, yPosition, false);
                        // Spawn platforms
                        WorldGen.PlaceTile(xPosition, yPosition, TileID.MinecartTrack);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, xPosition, yPosition, 1);
                    }
                    else
                    {
                        if (!FargoGlobalProjectile.TileIsLiterallyAir(tile))
                            FargoGlobalTile.ClearEverything(xPosition, yPosition);
                    }
                }
            }
        }
    }
}