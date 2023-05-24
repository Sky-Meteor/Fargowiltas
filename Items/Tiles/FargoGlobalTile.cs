using Fargowiltas.Items.Tiles;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Tiles
{
    public class FargoGlobalTile : GlobalTile
    {
        public override int[] AdjTiles(int type)
        {
            if (type == TileID.HeavyWorkBench)
            {
                int[] adjTiles = new int[] { TileID.WorkBenches, TileID.HeavyWorkBench };

                return adjTiles;
            }

            //if (type == ModContent.TileType<CrucibleCosmosSheet>())
            //{
            //    Main.LocalPlayer.adjHoney = true;
            //    Main.LocalPlayer.adjLava = true;
            //}

            return base.AdjTiles(type);
        }

        public override void MouseOver(int i, int j, int type)
        {
            if (type == TileID.Extractinator)
            {
                Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().extractSpeed = true;
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (WorldGen.gen)
            {
                return;
            }

            if (type == TileID.Trees && !fail && !(FargoWorld.DownedBools.TryGetValue("lumberjack", out bool down) && down))
            {
                //if (!FargoWorld.DownedBools["lumberjack"])
                //{
                //    NPC.NewNPC(NPC.GetSource_TownSpawn, );

                //    FargoWorld.DownedBools["lumberjack"] = true;
                //}
                FargoWorld.WoodChopped++;

                if (FargoWorld.WoodChopped > 500)
                {
                    FargoWorld.DownedBools["lumberjack"] = true;
                }
            }
        }

        private static uint LastTorchUpdate;
        private readonly int[] TorchesToReplace = new int[]
        {
            //13,   //bone, but there's never a penalty for using this, so its ok to place and not remove
            7,      //demon, but this never gives a bonus for some reason
            20,     //hallow
            18,     //corrupt
            19,     //crimson
            9,      //ice
            21,     //jungle
            16,     //desert
            17,     //coral - not actually on the default torch rotation for some reason???
            0,      //regular torch
        };

        private enum TorchStyle : int
        {
            None = 0,
            Bone = 13,
            Demon = 7,
            Hallow = 20,
            Corrupt = 18,
            Crimson = 19,
            Ice = 9,
            Jungle = 21,
            Desert = 16,
            Coral = 17
        };

        public override void NearbyEffects(int i, int j, int type, bool closer)
        {
            if (closer && TileID.Sets.Torch[type] && !Main.dedServ
                && Main.LocalPlayer.UsingBiomeTorches
                && (LastTorchUpdate < Main.GameUpdateCount - 60 || LastTorchUpdate == Main.GameUpdateCount))
            {
                //check for == is so that all torches can update on the same tick
                LastTorchUpdate = Main.GameUpdateCount;

                if (ModContent.GetInstance<FargoConfig>().TorchGodEX
                    && Main.LocalPlayer.ShoppingZone_BelowSurface //torch luck only applies underground
                    && !Main.LocalPlayer.ZoneDungeon && !Main.LocalPlayer.ZoneLihzhardTemple //torch luck doesnt apply here
                    )
                {
                    int torch = Framing.GetTileSafely(i, j).TileFrameY / 22;

                    //PLEASE don't ask me anything about torch luck logic.
                    bool replaceTorch = TorchesToReplace.Contains(torch);
                    if (replaceTorch)
                    {
                        if ((torch == (int)TorchStyle.Hallow && Main.LocalPlayer.ZoneHallow)
                            || (torch == (int)TorchStyle.Corrupt && Main.LocalPlayer.ZoneCorrupt)
                            || (torch == (int)TorchStyle.Crimson && Main.LocalPlayer.ZoneCrimson)
                            || (torch == (int)TorchStyle.Desert && (Main.LocalPlayer.ZoneDesert || Main.LocalPlayer.ZoneUndergroundDesert))
                            || (torch == (int)TorchStyle.Jungle && Main.LocalPlayer.ZoneJungle)
                            || (torch == (int)TorchStyle.Coral && Main.LocalPlayer.ZoneBeach)
                            )
                        {
                            replaceTorch = false;
                        }
                    }

                    if (replaceTorch)
                    {
                        /*int correctTorch = Main.LocalPlayer.BiomeTorchPlaceStyle(0);
                        if (correctTorch == (int)TorchStyle.Demon)
                            correctTorch = (int)TorchStyle.Bone; //because bone gives bonus in hell but demon doesnt????
                        else if (Main.LocalPlayer.ZoneBeach)
                            correctTorch = (int)TorchStyle.Coral;
                        else if (correctTorch == (int)TorchStyle.None)
                            correctTorch = (int)TorchStyle.Bone; //bone gives bonus in general but torch god recommends normal

                        if (torch != correctTorch && TorchesToReplace.Contains(torch))
                        {
                            WorldGen.KillTile(i, j, noItem: true);
                            WorldGen.PlaceTile(i, j, TileID.Torches, false, false, Main.LocalPlayer.whoAmI, correctTorch);
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, i, j, TileID.Torches);
                        }*/
                    }
                }
            }
        }

        internal static void DestroyChest(int x, int y)
        {
            int chestType = 1;

            int chest = Chest.FindChest(x, y);
            if (chest != -1)
            {
                for (int i = 0; i < 40; i++)
                {
                    Main.chest[chest].item[i] = new Item();
                }

                Main.chest[chest] = null;

                if (Main.tile[x, y].TileType == TileID.Containers2)
                {
                    chestType = 5;
                }

                if (Main.tile[x, y].TileType >= TileID.Count)
                {
                    chestType = 101;
                }
            }

            for (int i = x; i < x + 2; i++)
            {
                for (int j = y; j < y + 2; j++)
                {
                    Main.tile[i, j].TileType = 0;
                    //Main.tile[i, j].sTileHeader = 0;
                    Main.tile[i, j].TileFrameX = 0;
                    Main.tile[i, j].TileFrameY = 0;
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                if (chest != -1)
                {
                    NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, chestType, x, y, 0f, chest, Main.tile[x, y].TileType);
                }

                NetMessage.SendTileSquare(-1, x, y, 3);
            }
        }

        internal static Point16 FindChestTopLeft(int x, int y, bool destroy)
        {
            Tile tile = Main.tile[x, y];
            if (TileID.Sets.BasicChest[tile.TileType])
            {
                TileObjectData data = TileObjectData.GetTileData(tile.TileType, 0);
                x -= tile.TileFrameX / 18 % data.Width;
                y -= tile.TileFrameY / 18 % data.Height;

                if (destroy)
                {
                    DestroyChest(x, y);
                }

                return new Point16(x, y);
            }

            return Point16.NegativeOne;
        }

        internal static void ClearTileAndLiquid(int x, int y, bool sendData = true)
        {
            FindChestTopLeft(x, y, true);

            Tile tile = Main.tile[x, y];
            bool hadLiquid = tile.LiquidAmount != 0;
            WorldGen.KillTile(x, y, noItem: true);

            tile.Clear(TileDataType.Tile);
            tile.Clear(TileDataType.Liquid);

            //tile.lava(false);
            //tile.honey(false);

            if (Main.netMode == NetmodeID.Server)
            {
                if (hadLiquid)
                    NetMessage.sendWater(x, y);
                if (sendData)
                    NetMessage.SendTileSquare(-1, x, y, 1);
            }
        }

        internal static void ClearEverything(int x, int y, bool sendData = true)
        {
            FindChestTopLeft(x, y, true);

            Tile tile = Main.tile[x, y];
            bool hadLiquid = tile.LiquidAmount != 0;
            WorldGen.KillTile(x, y, noItem: true);
            tile.ClearEverything();

            //tile.lava(false);
            //tile.honey(false);

            if (Main.netMode == NetmodeID.Server)
            {
                if (hadLiquid)
                    NetMessage.sendWater(x, y);
                if (sendData)
                    NetMessage.SendTileSquare(-1, x, y, 1);
            }
        }
    }
}