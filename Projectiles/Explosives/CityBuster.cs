﻿using Fargowiltas.Tiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Projectiles.Explosives
{
    public class CityBuster : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("City Buster");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0;
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            Vector2 position = Projectile.Center;
            int radius = 64;     //bigger = boomer

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius * 2; y <= 0; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, yPosition];
                    if (tile == null)
                        continue;

                    if (!FargoGlobalProjectile.OkayToDestroyTile(tile) || FargoGlobalProjectile.TileIsLiterallyAir(tile))
                        continue;

                    FargoGlobalTile.ClearTileAndLiquid(xPosition, yPosition);
                }
            }

            Main.refreshMap = true;
        }
    }
}