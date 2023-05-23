using System;
using System.Linq;
using System.Collections.Generic;
using Fargowiltas.Buffs;
using Fargowiltas.Items.Summons.SwarmSummons.Energizers;
using Fargowiltas.Items.Tiles;
////using Fargowiltas.Items.Vanity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Fargowiltas.Items.Explosives;

namespace Fargowiltas.NPCs
{
    public class FargoGlobalNPC : GlobalNPC
    {
        internal static int[] Bosses = { 
            NPCID.KingSlime,
            NPCID.EyeofCthulhu,
            //NPCID.EaterofWorldsHead,
            NPCID.BrainofCthulhu,
            NPCID.QueenBee,
            NPCID.SkeletronHead,
            NPCID.QueenSlimeBoss,
            NPCID.TheDestroyer,
            NPCID.SkeletronPrime,
            NPCID.Retinazer,
            NPCID.Spazmatism,
            NPCID.Plantera,
            NPCID.Golem,
            NPCID.DukeFishron,
            NPCID.HallowBoss,
            NPCID.CultistBoss,
            NPCID.MoonLordCore,
            NPCID.MartianSaucerCore,
            NPCID.Pumpking,
            NPCID.IceQueen,
            NPCID.DD2Betsy,
            NPCID.DD2OgreT3,
            NPCID.IceGolem,
            NPCID.SandElemental,
            NPCID.Paladin,
            NPCID.Everscream,
            NPCID.MourningWood,
            NPCID.SantaNK1,
            NPCID.HeadlessHorseman,
            NPCID.PirateShip 
        };

        public static int LastWoFIndex = -1;
        public static int WoFDirection = 0;

        internal bool PillarSpawn = true;
        internal bool SwarmActive;
        internal bool PandoraActive;
        internal bool NoLoot = false;
        //internal bool DestroyerSwarm = false;

        public static int eaterBoss = -1;
        public static int brainBoss = -1;
        public static int plantBoss = -1;
        public static int beeBoss = -1;

        public override bool InstancePerEntity => true;

        //        public override void SetDefaults(NPC npc)
        //        {
        //            if (GetInstance<FargoConfig>().CatchNPCs)
        //            {
        //                if (npc.townNPC && npc.type < NPCID.Count && npc.type != NPCID.OldMan)
        //                {
        //                    Main.npcCatchable[npc.type] = true;
        //                    npc.catchItem = npc.type == NPCID.DD2Bartender ? (short)mod.ItemType("Tavernkeep") : (short)mod.ItemType(NPCID.GetUniqueKey(npc.type).Replace("Terraria ", string.Empty));
        //                }

        //                if (npc.type == NPCID.SkeletonMerchant)
        //                {
        //                    Main.npcCatchable[npc.type] = true;
        //                    npc.catchItem = (short)mod.ItemType("SkeletonMerchant");
        //                }
        //            }
        //        }

        public override bool CanHitNPC(NPC npc, NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            if (target.dontTakeDamage && target.type == NPCType<Squirrel>())
                return false;
            
            if (target.friendly && GetInstance<FargoConfig>().SaferBoundNPCs && (target.type == NPCID.BoundGoblin || target.type == NPCID.BoundMechanic || target.type == NPCID.BoundWizard || target.type == NPCID.BartenderUnconscious || target.type == NPCID.GolferRescue))
                return false;
            
            return base.CanHitNPC(npc, target);
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.boss)
            {
                //TODO: boss is apparently not defined here
                //boss = npc.whoAmI;
            }

            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                    eaterBoss = npc.whoAmI;
                    break;

                case NPCID.BrainofCthulhu:
                    brainBoss = npc.whoAmI;
                    break;

                case NPCID.Plantera:
                    plantBoss = npc.whoAmI;
                    break;

                case NPCID.QueenBee:
                    beeBoss = npc.whoAmI;
                    break;

                //                case NPCID.TheDestroyer:
                //                    if (SwarmActive)
                //                    {
                //                        if (npc.ai[0] == 0)
                //                        {
                //                            if (Main.netMode == NetmodeID.MultiplayerClient)
                //                                return false;

                //                            for (int i = 0; i < Main.maxNPCs; i++) //purge segments i shouldn't have
                //                            {
                //                                if (Main.npc[i].active && (Main.npc[i].type == NPCID.TheDestroyerBody || Main.npc[i].type == NPCID.TheDestroyerTail) && Main.npc[i].realLife == npc.whoAmI)
                //                                {
                //                                    npc.life = 0;
                //                                    npc.HitEffect();
                //                                    npc.active = false;
                //                                    if (Main.netMode == NetmodeID.Server)
                //                                        NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                //                                }
                //                            }

                //                            npc.lifeMax /= 4;
                //                            if (npc.life > npc.lifeMax)
                //                                npc.life = npc.lifeMax;
                //                            npc.ai[3] = npc.whoAmI;
                //                            npc.realLife = npc.whoAmI;
                //                            int prev = npc.whoAmI;
                //                            int bodySegments = 9;
                //                            for (int j = 0; j < bodySegments; j++)
                //                            {
                //                                int type = NPCID.TheDestroyerBody;
                //                                if (j == bodySegments - 1)
                //                                {
                //                                    type = NPCID.TheDestroyerTail;
                //                                }

                //                                int n = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + npc.height), type, npc.whoAmI);
                //                                Main.npc[n].ai[3] = npc.whoAmI;
                //                                Main.npc[n].realLife = npc.whoAmI;
                //                                Main.npc[n].ai[1] = prev;
                //                                Main.npc[prev].ai[0] = n;
                //                                Main.npc[n].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;
                //                                if (Main.netMode == NetmodeID.Server)
                //                                    NetMessage.SendData(MessageID.SyncNPC, number: n);
                //                                prev = n;
                //                            }
                //                            if (Main.netMode == NetmodeID.Server)
                //                            {
                //                                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                //                                var netMessage = mod.GetPacket();
                //                                netMessage.Write((byte)4);
                //                                netMessage.Write(npc.whoAmI);
                //                                netMessage.Write(npc.lifeMax);
                //                                netMessage.Send();
                //                            }
                //                            return false;
                //                        }
                //                        else if (Main.netMode != NetmodeID.MultiplayerClient)
                //                        {
                //                            int count = 0;
                //                            for (int i = 0; i < Main.maxNPCs; i++) //confirm i have exactly the right number of segments behind me
                //                            {
                //                                if (Main.npc[i].active && (Main.npc[i].type == NPCID.TheDestroyerBody || Main.npc[i].type == NPCID.TheDestroyerTail) && Main.npc[i].realLife == npc.whoAmI)
                //                                {
                //                                    count++;
                //                                    if (count > 9)
                //                                        break;
                //                                }
                //                            }

                //                            if (count != 9) //if not exactly the right pieces, die
                //                            {
                //                                npc.life = 0;
                //                                npc.HitEffect();
                //                                npc.active = false;
                //                                if (Main.netMode == NetmodeID.Server)
                //                                {
                //                                    //NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("head killed by wrong count, " + count.ToString()), Color.White);
                //                                    NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                //                                }
                //                                else
                //                                {
                //                                    //Main.NewText("head killed by wrong count, " + count.ToString());
                //                                }
                //                            }
                //                        }
                //                    }
                //                    break;

                //                case NPCID.TheDestroyerBody:
                //                case NPCID.TheDestroyerTail:
                //                    if (SwarmActive)// && Main.netMode != NetmodeID.MultiplayerClient)
                //                    {
                //                        //kill if real life is invalid
                //                        if (!(npc.realLife > -1 && npc.realLife < Main.maxNPCs && Main.npc[npc.realLife].active && Main.npc[npc.realLife].type == NPCID.TheDestroyer))
                //                        {
                //                            //Main.NewText("body realLife invalid, die");
                //                            npc.life = 0;
                //                            npc.HitEffect();
                //                            npc.active = false;
                //                            if (Main.netMode == NetmodeID.Server)
                //                                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                //                            return false;
                //                        }

                //                        int prev = npc.whoAmI;
                //                        int segment = (int)npc.ai[1];
                //                        int i = 0;
                //                        const int maxLength = 9;
                //                        for (; i < maxLength; i++) //iterate upwards along destroyer's body
                //                        {
                //                            if (segment > -1 && segment < Main.maxNPCs && Main.npc[segment].active && Main.npc[segment].type == NPCID.TheDestroyerBody
                //                                && Main.npc[segment].ai[3] == npc.ai[3] && Main.npc[segment].ai[0] == Main.npc[prev].whoAmI)
                //                            {
                //                                prev = segment;
                //                                segment = (int)Main.npc[segment].ai[1]; //continue if next is a valid BODY segment
                //                            }
                //                            else
                //                            {
                //                                break; //stop otherwise (this includes if head is found early, which is okay!)
                //                            }
                //                        }

                //                        //if last segment seen is indeed destroyer head
                //                        if (segment > -1 && segment < Main.maxNPCs && Main.npc[segment].active && Main.npc[segment].type == NPCID.TheDestroyer)
                //                        {
                //                            if (i == maxLength && npc.type != NPCID.TheDestroyerTail) //i am the furthest possible segment, become tail
                //                            {
                //                                //Main.NewText("body: become tail");
                //                                npc.type = NPCID.TheDestroyerTail;
                //                                npc.ai[0] = 0f;
                //                                npc.ai[2] = 0f;
                //                                npc.localAI[0] = 0f;
                //                                npc.localAI[1] = 0f;
                //                                npc.localAI[2] = 0f;
                //                                npc.localAI[3] = 0f;
                //                                if (Main.netMode == NetmodeID.Server)
                //                                    NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                //                            }
                //                        }
                //                        else //last segment seen isn't destroyer head, die
                //                        {
                //                            //Main.NewText("body killed by wrong lead");
                //                            npc.life = 0;
                //                            npc.HitEffect();
                //                            npc.active = false;
                //                            if (Main.netMode == NetmodeID.Server)
                //                                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
                //                            return false;
                //                        }
                //                    }
                //                    break;

                //                case NPCID.BlueSlime:
                //                    if (FargoWorld.OverloadedSlimeRain && npc.netID == NPCID.GreenSlime)
                //                    {
                //                        int[] slimes = { NPCID.BlueSlime, NPCID.RedSlime, NPCID.PurpleSlime, NPCID.YellowSlime, NPCID.BlackSlime, NPCID.JungleSlime };

                //                        npc.SetDefaults(slimes[Main.rand.Next(slimes.Length)]);

                //                        if (Main.netMode == NetmodeID.Server)
                //                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
                //                    }

                //                    break;

                default:
                    break;
            }

            return true;
        }

        public override void AI(NPC npc)
        {
            // Wack ghost saucers begone
            if (FargoWorld.OverloadMartians && npc.type == NPCID.MartianSaucerCore && npc.dontTakeDamage)
            {
                npc.dontTakeDamage = false;
            }

            //if (Fargowiltas.SwarmActive && Fargowiltas.ModLoaded["ThoriumMod"])
            //{
            //    Mod thorium = ModLoader.GetMod("ThoriumMod");

            //    if (npc.type == thorium.NPCType("BoreanStriderPopped") || npc.type == thorium.NPCType("FallenDeathBeholder2") || npc.type == thorium.NPCType("LichHeadless") || npc.type == thorium.NPCType("AbyssionReleased"))
            //    {
            //        SwarmActive = true;
            //    }
            //}
        }

        public override void PostAI(NPC npc)
        {
            if (SwarmActive && npc.type == NPCID.Golem)
                npc.dontTakeDamage = false; //always vulnerable in swarm
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            //TODO: fix shops i didn't touch these at all

            Player player = Main.LocalPlayer;

            if (GetInstance<FargoConfig>().NPCSales)
            {
                void AddItem(ref int next, int itemID, int customPrice = -1)
                {
                    if (next >= 40)
                        return;
                    /*
                    shop.item[next].SetDefaults(itemID);
                    if (customPrice != -1)
                        shop.item[next].shopCustomPrice = customPrice;
                    */
                    next++;
                }
                /*
                switch (npc.type)
                {
                    case NPCID.PartyGirl:
                        if (BirthdayParty.PartyIsUp)
                        {
                            AddItem(ref nextSlot, ItemID.SliceOfCake);
                        }
                        break;

                    case NPCID.Clothier:
                        AddItem(ref nextSlot, ItemID.PharaohsMask, Item.buyPrice(gold: 1));
                        AddItem(ref nextSlot, ItemID.PharaohsRobe, Item.buyPrice(gold: 1));

                        if (player.anglerQuestsFinished >= 10)
                        {
                            AddItem(ref nextSlot, ItemID.AnglerHat);

                            if (player.anglerQuestsFinished >= 15)
                            {
                                AddItem(ref nextSlot, ItemID.AnglerVest);

                                if (player.anglerQuestsFinished >= 20)
                                {
                                    AddItem(ref nextSlot, ItemID.AnglerPants);
                                }
                            }
                        }

                        AddItem(ref nextSlot, ItemID.BlueBrick, Item.buyPrice(silver: 1));
                        AddItem(ref nextSlot, ItemType<UnsafeBlueBrickWall>(), Item.buyPrice(copper: 25));
                        AddItem(ref nextSlot, ItemType<UnsafeBlueSlabWall>(), Item.buyPrice(copper: 25));
                        AddItem(ref nextSlot, ItemType<UnsafeBlueTileWall>(), Item.buyPrice(copper: 25));

                        AddItem(ref nextSlot, ItemID.GreenBrick, Item.buyPrice(silver: 1));
                        AddItem(ref nextSlot, ItemType<UnsafeGreenBrickWall>(), Item.buyPrice(copper: 25));
                        AddItem(ref nextSlot, ItemType<UnsafeGreenSlabWall>(), Item.buyPrice(copper: 25));
                        AddItem(ref nextSlot, ItemType<UnsafeGreenTileWall>(), Item.buyPrice(copper: 25));

                        AddItem(ref nextSlot, ItemID.PinkBrick, Item.buyPrice(silver: 1));
                        AddItem(ref nextSlot, ItemType<UnsafePinkBrickWall>(), Item.buyPrice(copper: 25));
                        AddItem(ref nextSlot, ItemType<UnsafePinkSlabWall>(), Item.buyPrice(copper: 25));
                        AddItem(ref nextSlot, ItemType<UnsafePinkTileWall>(), Item.buyPrice(copper: 25));

                        if (Main.LocalPlayer.inventory.Any(i => !i.IsAir && i.useAmmo == ItemID.Bone))
                        {
                            AddItem(ref nextSlot, ItemType<Items.Ammos.BrittleBone>());
                        }
                        break;

                    case NPCID.Merchant:
                        if (player.anglerQuestsFinished >= 5)
                        {
                            AddItem(ref nextSlot, ItemID.FuzzyCarrot);

                            if (player.anglerQuestsFinished >= 10)
                            {
                                AddItem(ref nextSlot, ItemID.AnglerEarring);
                                AddItem(ref nextSlot, ItemID.HighTestFishingLine);
                                AddItem(ref nextSlot, ItemID.TackleBox);
                                AddItem(ref nextSlot, ItemID.GoldenBugNet);
                                AddItem(ref nextSlot, ItemID.FishHook);

                                if (Main.hardMode)
                                {
                                    AddItem(ref nextSlot, ItemID.FinWings);
                                    AddItem(ref nextSlot, ItemID.SuperAbsorbantSponge);
                                    AddItem(ref nextSlot, ItemID.BottomlessBucket);

                                    if (player.anglerQuestsFinished >= 25)
                                    {
                                        AddItem(ref nextSlot, ItemID.HotlineFishingHook);

                                        if (player.anglerQuestsFinished >= 30)
                                        {
                                            AddItem(ref nextSlot, ItemID.GoldenFishingRod);
                                        }
                                    }
                                }
                            }
                        }

                        if (Main.LocalPlayer.inventory.Any(i => !i.IsAir && i.useAmmo == AmmoID.Dart))
                        {
                            AddItem(ref nextSlot, ItemID.Seed, 3);
                        }
                        break;

                    case NPCID.Painter:

                        if (player.ZoneDungeon)
                        {
                            nextSlot = 15;

                            AddItem(ref nextSlot, ItemID.BloodMoonRising);
                            AddItem(ref nextSlot, ItemID.BoneWarp);
                            AddItem(ref nextSlot, ItemID.TheCreationoftheGuide);
                            AddItem(ref nextSlot, ItemID.TheCursedMan);
                            AddItem(ref nextSlot, ItemID.TheDestroyer);
                            AddItem(ref nextSlot, ItemID.Dryadisque);
                            AddItem(ref nextSlot, ItemID.TheEyeSeestheEnd);
                            AddItem(ref nextSlot, ItemID.FacingtheCerebralMastermind);
                            AddItem(ref nextSlot, ItemID.GloryoftheFire);
                            AddItem(ref nextSlot, ItemID.GoblinsPlayingPoker);
                            AddItem(ref nextSlot, ItemID.GreatWave);
                            AddItem(ref nextSlot, ItemID.TheGuardiansGaze);
                            AddItem(ref nextSlot, ItemID.TheHangedMan);
                            AddItem(ref nextSlot, ItemID.Impact);
                            AddItem(ref nextSlot, ItemID.ThePersistencyofEyes);
                            AddItem(ref nextSlot, ItemID.PoweredbyBirds);
                            AddItem(ref nextSlot, ItemID.TheScreamer);
                            AddItem(ref nextSlot, ItemID.SkellingtonJSkellingsworth);
                            AddItem(ref nextSlot, ItemID.SparkyPainting);
                            AddItem(ref nextSlot, ItemID.SomethingEvilisWatchingYou);
                            AddItem(ref nextSlot, ItemID.StarryNight);
                            AddItem(ref nextSlot, ItemID.TrioSuperHeroes);
                            AddItem(ref nextSlot, ItemID.TheTwinsHaveAwoken);
                            AddItem(ref nextSlot, ItemID.UnicornCrossingtheHallows);
                        }
                        else if (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight)
                        {
                            nextSlot = 19;

                            AddItem(ref nextSlot, ItemID.AmericanExplosive);
                            AddItem(ref nextSlot, ItemID.CrownoDevoursHisLunch);
                            AddItem(ref nextSlot, ItemID.Discover);
                            AddItem(ref nextSlot, ItemID.FatherofSomeone);
                            AddItem(ref nextSlot, ItemID.FindingGold);
                            AddItem(ref nextSlot, ItemID.GloriousNight);
                            AddItem(ref nextSlot, ItemID.GuidePicasso);
                            AddItem(ref nextSlot, ItemID.Land);
                            AddItem(ref nextSlot, ItemID.TheMerchant);
                            AddItem(ref nextSlot, ItemID.NurseLisa);
                            AddItem(ref nextSlot, ItemID.OldMiner);
                            AddItem(ref nextSlot, ItemID.RareEnchantment);
                            AddItem(ref nextSlot, ItemID.Sunflowers);
                            AddItem(ref nextSlot, ItemID.TerrarianGothic);
                            AddItem(ref nextSlot, ItemID.Waldo);
                        }
                        else if (player.ZoneUnderworldHeight)
                        {
                            nextSlot = 19;

                            AddItem(ref nextSlot, ItemID.DarkSoulReaper);
                            AddItem(ref nextSlot, ItemID.Darkness);
                            AddItem(ref nextSlot, ItemID.DemonsEye);
                            AddItem(ref nextSlot, ItemID.FlowingMagma);
                            AddItem(ref nextSlot, ItemID.HandEarth);
                            AddItem(ref nextSlot, ItemID.ImpFace);
                            AddItem(ref nextSlot, ItemID.LakeofFire);
                            AddItem(ref nextSlot, ItemID.LivingGore);
                            AddItem(ref nextSlot, ItemID.OminousPresence);
                            AddItem(ref nextSlot, ItemID.ShiningMoon);
                            AddItem(ref nextSlot, ItemID.Skelehead);
                            AddItem(ref nextSlot, ItemID.TrappedGhost);
                        }
                        //deserttt

                        break;

                    case NPCID.Demolitionist:
                        AddItem(ref nextSlot, ItemType<BoomShuriken>(), Item.buyPrice(0, 0, 1, 25));
                        if (Main.hardMode)
                        {
                            AddItem(ref nextSlot, ItemID.CopperOre);
                            AddItem(ref nextSlot, ItemID.TinOre);
                            AddItem(ref nextSlot, ItemID.IronOre);
                            AddItem(ref nextSlot, ItemID.LeadOre);
                            AddItem(ref nextSlot, ItemID.SilverOre);
                            AddItem(ref nextSlot, ItemID.TungstenOre);
                            AddItem(ref nextSlot, ItemID.GoldOre);
                            AddItem(ref nextSlot, ItemID.PlatinumOre);
                        }
                        if (NPC.downedPlantBoss)
                        {
                            AddItem(ref nextSlot, ItemID.Meteorite);
                            AddItem(ref nextSlot, ItemID.DemoniteOre);
                            AddItem(ref nextSlot, ItemID.CrimtaneOre);
                            AddItem(ref nextSlot, ItemID.Hellstone);
                        }
                        if (NPC.downedMoonlord)
                        {
                            AddItem(ref nextSlot, ItemID.CobaltOre);
                            AddItem(ref nextSlot, ItemID.PalladiumOre);
                            AddItem(ref nextSlot, ItemID.MythrilOre);
                            AddItem(ref nextSlot, ItemID.OrichalcumOre);
                            AddItem(ref nextSlot, ItemID.AdamantiteOre);
                            AddItem(ref nextSlot, ItemID.TitaniumOre);
                            AddItem(ref nextSlot, ItemID.ChlorophyteOre);
                        }

                        break;

                    case NPCID.WitchDoctor:
                        if (NPC.downedBoss3)
                        {
                            bool alreadySellsTable = false;
                            foreach(Item item in shop.item)
                            {
                                if (!item.IsAir && item.type == ItemID.BewitchingTable)
                                {
                                    alreadySellsTable = true;
                                    break;
                                }
                            }

                            if (!alreadySellsTable)
                                AddItem(ref nextSlot, ItemID.BewitchingTable);
                        }
                        break;

                    case NPCID.Steampunker:
                        AddItem(ref nextSlot, WorldGen.crimson ? ItemID.PurpleSolution : ItemID.RedSolution);
                        break;

                    case NPCID.DyeTrader:
                        FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
                        if (modPlayer.FirstDyeIngredients["RedHusk"])
                        {
                            AddItem(ref nextSlot, ItemID.RedHusk);
                        }
                        if (modPlayer.FirstDyeIngredients["OrangeBloodroot"])
                        {
                            AddItem(ref nextSlot, ItemID.OrangeBloodroot);
                        }
                        if (modPlayer.FirstDyeIngredients["YellowMarigold"])
                        {
                            AddItem(ref nextSlot, ItemID.YellowMarigold);
                        }
                        if (modPlayer.FirstDyeIngredients["LimeKelp"])
                        {
                            AddItem(ref nextSlot, ItemID.LimeKelp);
                        }
                        if (modPlayer.FirstDyeIngredients["GreenMushroom"])
                        {
                            AddItem(ref nextSlot, ItemID.GreenMushroom);
                        }
                        if (modPlayer.FirstDyeIngredients["TealMushroom"])
                        {
                            AddItem(ref nextSlot, ItemID.TealMushroom);
                        }
                        if (modPlayer.FirstDyeIngredients["CyanHusk"])
                        {
                            AddItem(ref nextSlot, ItemID.CyanHusk);
                        }
                        if (modPlayer.FirstDyeIngredients["SkyBlueFlower"])
                        {
                            AddItem(ref nextSlot, ItemID.SkyBlueFlower);
                        }
                        if (modPlayer.FirstDyeIngredients["BlueBerries"])
                        {
                            AddItem(ref nextSlot, ItemID.BlueBerries);
                        }
                        if (modPlayer.FirstDyeIngredients["PurpleMucos"])
                        {
                            AddItem(ref nextSlot, ItemID.PurpleMucos);
                        }
                        if (modPlayer.FirstDyeIngredients["VioletHusk"])
                        {
                            AddItem(ref nextSlot, ItemID.VioletHusk);
                        }
                        if (modPlayer.FirstDyeIngredients["PinkPricklyPear"])
                        {
                            AddItem(ref nextSlot, ItemID.PinkPricklyPear);
                        }
                        if (modPlayer.FirstDyeIngredients["BlackInk"])
                        {
                            AddItem(ref nextSlot, ItemID.BlackInk);
                        }

                        break;

                    case NPCID.Dryad:
                        if (Main.hardMode)
                        {
                            AddItem(ref nextSlot, ItemID.NaturesGift, Item.buyPrice(gold: 20));
                            AddItem(ref nextSlot, ItemID.JungleRose, Item.buyPrice(gold: 10));

                            AddItem(ref nextSlot, ItemID.StrangePlant1, Item.buyPrice(gold: 5));
                            AddItem(ref nextSlot, ItemID.StrangePlant2, Item.buyPrice(gold: 5));
                            AddItem(ref nextSlot, ItemID.StrangePlant3, Item.buyPrice(gold: 5));
                            AddItem(ref nextSlot, ItemID.StrangePlant4, Item.buyPrice(gold: 5));
                        }
                        break;

                    case NPCID.Wizard:
                        if (NPC.downedGolemBoss)
                            AddItem(ref nextSlot, ItemID.SuperManaPotion);
                        break;
                    
                }
                */
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            FargoPlayer fargoPlayer = player.GetFargoPlayer();

            if (fargoPlayer.BattleCry)
            {
                spawnRate = (int)(spawnRate * 0.1);
                maxSpawns = (int)(maxSpawns * 10f);
            }

            if (fargoPlayer.CalmingCry)
            {
                spawnRate = (int)(spawnRate * 10f);
                maxSpawns = (int)(maxSpawns * 0.1);
            }

            if ((FargoWorld.OverloadGoblins || FargoWorld.OverloadPirates) && player.position.X > Main.invasionX * 16.0 - 3000 && player.position.X < Main.invasionX * 16.0 + 3000)
            {
                if (FargoWorld.OverloadGoblins)
                {
                    spawnRate = (int)(spawnRate * 0.2);
                    maxSpawns = (int)(maxSpawns * 10f);
                }
                else if (FargoWorld.OverloadPirates)
                {
                    spawnRate = (int)(spawnRate * 0.2);
                    maxSpawns = (int)(maxSpawns * 30f);
                }
            }

            if (FargoWorld.OverloadPumpkinMoon || FargoWorld.OverloadFrostMoon)
            {
                spawnRate = (int)(spawnRate * 0.2);
                maxSpawns = (int)(maxSpawns * 10f);
            }
            else if (FargoWorld.OverloadMartians)
            {
                spawnRate = (int)(spawnRate * 0.2);
                maxSpawns = (int)(maxSpawns * 30f);
            }

            if (AnyBossAlive() && GetInstance<FargoConfig>().BossZen && player.Distance(Main.npc[boss].Center) < 6000)
            {
                maxSpawns = 0;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player player = Main.LocalPlayer;

            if (FargoWorld.OverloadGoblins && player.position.X > Main.invasionX * 16.0 - 3000 && player.position.X < Main.invasionX * 16.0 + 3000)
            {
                // Literally nothing in the pool in the invasion so set everything to custom
                pool[NPCID.GoblinSummoner] = 1f;
                pool[NPCID.GoblinArcher] = 3f;
                pool[NPCID.GoblinPeon] = 5f;
                pool[NPCID.GoblinSorcerer] = 3f;
                pool[NPCID.GoblinWarrior] = 5f;
                pool[NPCID.GoblinThief] = 5f;
                pool[NPCID.GoblinScout] = 3f;
            }
            else if (FargoWorld.OverloadPirates && player.position.X > Main.invasionX * 16.0 - 3000 && player.position.X < Main.invasionX * 16.0 + 3000)
            {
                // Literally nothing in the pool in the invasion so set everything to custom
                if (NPC.CountNPCS(NPCID.PirateShip) < 4)
                {
                    pool[NPCID.PirateShip] = .5f;
                }

                pool[NPCID.Parrot] = 2f;
                pool[NPCID.PirateCaptain] = 1f;
                pool[NPCID.PirateCrossbower] = 3f;
                pool[NPCID.PirateCorsair] = 5f;
                pool[NPCID.PirateDeadeye] = 4f;
                pool[NPCID.PirateDeckhand] = 5f;
            }

            else if (FargoWorld.OverloadPumpkinMoon)
            {
                pool[NPCID.Pumpking] = 4f;
                pool[NPCID.MourningWood] = 4f;
                pool[NPCID.HeadlessHorseman] = 3f;
                pool[NPCID.Scarecrow1] = .5f;
                pool[NPCID.Scarecrow2] = .5f;
                pool[NPCID.Scarecrow3] = .5f;
                pool[NPCID.Scarecrow4] = .5f;
                pool[NPCID.Scarecrow5] = .5f;
                pool[NPCID.Scarecrow6] = .5f;
                pool[NPCID.Scarecrow7] = .5f;
                pool[NPCID.Scarecrow8] = .5f;
                pool[NPCID.Scarecrow9] = .5f;
                pool[NPCID.Scarecrow10] = .5f;
                pool[NPCID.Hellhound] = 3f;
                pool[NPCID.Poltergeist] = 3f;
                pool[NPCID.Splinterling] = 3f;
            }
            else if (FargoWorld.OverloadFrostMoon)
            {
                pool[NPCID.IceQueen] = 5f;
                pool[NPCID.Everscream] = 5f;
                pool[NPCID.SantaNK1] = 5f;
                pool[NPCID.ZombieElf] = 1f;
                pool[NPCID.ZombieElfBeard] = 1f;
                pool[NPCID.ZombieElfGirl] = 1f;
                pool[NPCID.GingerbreadMan] = 2f;
                pool[NPCID.ElfArcher] = 2f;
                pool[NPCID.Nutcracker] = 3f;
                pool[NPCID.ElfCopter] = 3f;
                pool[NPCID.Flocko] = 2f;
                pool[NPCID.Yeti] = 4f;
                pool[NPCID.PresentMimic] = 2f;
                pool[NPCID.Krampus] = 4f;
            }
            else if (FargoWorld.OverloadMartians)
            {
                pool[NPCID.MartianSaucerCore] = 1f;
                pool[NPCID.Scutlix] = 3f;
                pool[NPCID.ScutlixRider] = 2f;
                pool[NPCID.MartianWalker] = 3f;
                pool[NPCID.MartianDrone] = 2f;
                pool[NPCID.GigaZapper] = 1f;
                pool[NPCID.MartianEngineer] = 2f;
                pool[NPCID.MartianOfficer] = 2f;
                pool[NPCID.RayGunner] = 1f;
                pool[NPCID.GrayGrunt] = 1f;
                pool[NPCID.BrainScrambler] = 1f;
            }
        }

        public override bool PreKill(NPC npc)
        {
            if (NoLoot)
            {
                return false;
            }

            if (Fargowiltas.SwarmActive && (npc.type == NPCID.BlueSlime || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.Creeper || (npc.type >= NPCID.PirateCorsair && npc.type <= NPCID.PirateCrossbower)))
            {
                return false;
            }

            if (SwarmActive && Fargowiltas.SwarmActive && Main.netMode != NetmodeID.MultiplayerClient)
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        Swarm(npc, NPCID.KingSlime, NPCID.BlueSlime, ItemID.KingSlimeBossBag, ItemID.KingSlimeTrophy,  ItemType<EnergizerSlime>());
                        break;

                    case NPCID.EyeofCthulhu:
                        Swarm(npc, NPCID.EyeofCthulhu, NPCID.ServantofCthulhu, ItemID.EyeOfCthulhuBossBag, ItemID.EyeofCthulhuTrophy, ItemType<EnergizerEye>());
                        break;

                    case NPCID.EaterofWorldsHead:
                        Swarm(npc, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail, ItemID.EaterOfWorldsBossBag, ItemID.EaterofWorldsTrophy, ItemType<EnergizerWorm>());
                        break;

                    case NPCID.BrainofCthulhu:
                        Swarm(npc, NPCID.BrainofCthulhu, NPCID.Creeper, ItemID.BrainOfCthulhuBossBag, ItemID.BrainofCthulhuTrophy, ItemType<EnergizerBrain>());
                        break;

                    case NPCID.DD2DarkMageT1:
                        Swarm(npc, NPCID.DD2DarkMageT1, -1, ItemID.DefenderMedal, ItemID.BossTrophyDarkmage, ItemType<EnergizerDarkMage>());
                        break;

                    case NPCID.Deerclops:
                        Swarm(npc, NPCID.Deerclops, -1, ItemID.DeerclopsBossBag, ItemID.DeerclopsTrophy, ItemType<EnergizerDeer>());
                        break;

                    case NPCID.QueenBee:
                        Swarm(npc, NPCID.QueenBee, NPCID.BeeSmall, ItemID.QueenBeeBossBag, ItemID.QueenBeeTrophy, ItemType<EnergizerBee>());
                        break;

                    case NPCID.SkeletronHead:
                        Swarm(npc, NPCID.SkeletronHead, -1, ItemID.SkeletronBossBag, ItemID.SkeletronTrophy, ItemType<EnergizerSkele>());
                        break;

                    case NPCID.WallofFlesh:
                        Swarm(npc, NPCID.WallofFlesh, NPCID.TheHungry, ItemID.WallOfFleshBossBag, ItemID.WallofFleshTrophy, ItemType<EnergizerWall>());
                        break;

                    case NPCID.QueenSlimeBoss:
                        Swarm(npc, NPCID.QueenSlimeBoss, NPCID.QueenSlimeMinionPink, ItemID.QueenSlimeBossBag, ItemID.QueenSlimeTrophy, ItemType<EnergizerQueenSlime>());
                        break;

                    case NPCID.TheDestroyer:
                        Swarm(npc, NPCID.TheDestroyer, NPCID.Probe, ItemID.DestroyerBossBag, ItemID.DestroyerTrophy, ItemType<EnergizerDestroy>());
                        break;

                    case NPCID.Retinazer:
                        Swarm(npc, NPCID.Retinazer, -1, ItemID.TwinsBossBag, ItemID.RetinazerTrophy, ItemType<EnergizerTwins>());
                        break;

                    case NPCID.Spazmatism:
                        Swarm(npc, NPCID.Spazmatism, -1, -1, ItemID.SpazmatismTrophy, -1);
                        break;

                    case NPCID.SkeletronPrime:
                        Swarm(npc, NPCID.SkeletronPrime, -1, ItemID.SkeletronPrimeBossBag, ItemID.SkeletronPrimeTrophy, ItemType<EnergizerPrime>());
                        break;

                    case NPCID.Plantera:
                        Swarm(npc, NPCID.Plantera, NPCID.PlanterasHook, ItemID.PlanteraBossBag, ItemID.PlanteraTrophy, ItemType<EnergizerPlant>());
                        break;

                    case NPCID.Golem:
                        Swarm(npc, NPCID.Golem, NPCID.GolemHeadFree, ItemID.GolemBossBag, ItemID.GolemTrophy, ItemType<EnergizerGolem>());
                        break;

                    case NPCID.DD2Betsy:
                        Swarm(npc, NPCID.DD2Betsy, NPCID.DD2WyvernT3, ItemID.BossBagBetsy, ItemID.BossTrophyBetsy, ItemType<EnergizerBetsy>());
                        break;

                    case NPCID.DukeFishron:
                        Swarm(npc, NPCID.DukeFishron, NPCID.Sharkron, ItemID.FishronBossBag, ItemID.DukeFishronTrophy, ItemType<EnergizerFish>());
                        break;

                    case NPCID.HallowBoss:
                        Swarm(npc, NPCID.HallowBoss, -1, ItemID.FairyQueenBossBag, ItemID.FairyQueenTrophy, ItemType<EnergizerEmpress>());
                        break;

                    case NPCID.CultistBoss:
                        Swarm(npc, NPCID.CultistBoss, -1, ItemID.CultistBossBag, ItemID.AncientCultistTrophy, ItemType<EnergizerCultist>());
                        break;

                    case NPCID.MoonLordCore:
                        Swarm(npc, NPCID.MoonLordCore, NPCID.MoonLordFreeEye, ItemID.MoonLordBossBag, ItemID.MoonLordTrophy, ItemType<EnergizerMoon>());
                        break;

                    case NPCID.DungeonGuardian:
                        Swarm(npc, NPCID.DungeonGuardian, -1, -1, ItemID.BoneKey, ItemType<EnergizerDG>());
                        break;
                }




                //if (Fargowiltas.ModLoaded["ThoriumMod"])
                //{
                //    Mod thorium = ModLoader.GetMod("ThoriumMod");

                //    if (npc.type == thorium.NPCType("TheGrandThunderBirdv2"))
                //    {
                //        Swarm(npc, thorium.NPCType("TheGrandThunderBirdv2"), thorium.NPCType("Hatchling"), thorium.ItemType("ThunderBirdBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("QueenJelly"))
                //    {
                //        Swarm(npc, thorium.NPCType("QueenJelly"), thorium.NPCType("ZealousJelly"), thorium.ItemType("JellyFishBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("GraniteEnergyStorm"))
                //    {
                //        Swarm(npc, thorium.NPCType("GraniteEnergyStorm"), thorium.NPCType("EncroachingEnergy"), thorium.ItemType("GraniteBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("TheBuriedWarrior"))
                //    {
                //        Swarm(npc, thorium.NPCType("TheBuriedWarrior"), -1, thorium.ItemType("HeroBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("Viscount"))
                //    {
                //        Swarm(npc, thorium.NPCType("Viscount"), -1, thorium.ItemType("CountBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("ThePrimeScouter"))
                //    {
                //        Swarm(npc, thorium.NPCType("ThePrimeScouter"), -1, thorium.ItemType("ScouterBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("BoreanStriderPopped"))
                //    {
                //        Swarm(npc, thorium.NPCType("BoreanStrider"), thorium.ItemType("BoreanMyte1"), thorium.ItemType("BoreanBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("FallenDeathBeholder2"))
                //    {
                //        Swarm(npc, thorium.NPCType("FallenDeathBeholder"), thorium.ItemType("EnemyBeholder"), thorium.ItemType("BeholderBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("LichHeadless"))
                //    {
                //        Swarm(npc, thorium.NPCType("Lich"), -1, thorium.ItemType("LichBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("AbyssionReleased"))
                //    {
                //        Swarm(npc, thorium.NPCType("Abyssion"), thorium.NPCType("AbyssalSpawn"), thorium.ItemType("AbyssionBag"), -1, string.Empty);
                //    }
                //    else if (npc.type == thorium.NPCType("RealityBreaker"))
                //    {
                //        Swarm(npc, thorium.NPCType("Aquaius"), thorium.NPCType("AquaiusBubble"), thorium.ItemType("RagBag"), -1, string.Empty);
                //        Swarm(npc, thorium.NPCType("Omnicide"), -1, -1, -1, string.Empty);
                //        Swarm(npc, thorium.NPCType("SlagFury"), -1, -1, -1, string.Empty);
                //    }
                //}

                return false;
            }

            if (!PandoraActive)
            {
                return true;
            }

            return false;
        }

        public override void OnKill(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Painter:
                    if (NPC.AnyNPCs(NPCID.MoonLordCore))
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemType<EchPainting>());
                    break;

                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    if (!DD2Event.Ongoing)
                    {
                        if (Main.rand.NextBool(14))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.BossMaskOgre);

                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] { ItemID.ApprenticeScarf, ItemID.SquireShield, ItemID.HuntressBuckler, ItemID.MonkBelt, ItemID.DD2SquireDemonSword, ItemID.MonkStaffT1, ItemID.MonkStaffT2, ItemID.BookStaff, ItemID.DD2PhoenixBow, ItemID.DD2PetGhost }));

                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.GoldCoin, Main.rand.Next(4, 7));
                    }
                    break;

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    if (!DD2Event.Ongoing)
                    {
                        if (Main.rand.NextBool(14))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.BossMaskDarkMage);

                        if (Main.rand.NextBool(10))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.NextBool() ? ItemID.WarTable : ItemID.WarTableBanner);

                        if (Main.rand.NextBool(6))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] { ItemID.DD2PetGato, ItemID.DD2PetDragon }));
                    }
                    break;

                case NPCID.HeadlessHorseman:
                    if (!Main.dayTime && !Main.pumpkinMoon)
                    {
                        if (Main.rand.NextBool(20))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.JackOLanternMask);
                    }
                    break;

                case NPCID.MourningWood:
                    if (!Main.dayTime && !Main.pumpkinMoon)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.SpookyWood, 30);

                        if (Main.rand.NextBool(3))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                                ItemID.SpookyHook,
                                ItemID.SpookyTwig,
                                ItemID.StakeLauncher,
                                ItemID.CursedSapling,
                                ItemID.NecromanticScroll,
                                Main.expertMode ? ItemID.WitchBroom : ItemID.SpookyWood
                            }));
                    }
                    break;

                case NPCID.Pumpking:
                    if (!Main.dayTime && !Main.pumpkinMoon)
                    {
                        if (Main.rand.NextBool(3))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                                ItemID.TheHorsemansBlade,
                                ItemID.BatScepter,
                                ItemID.BlackFairyDust,
                                ItemID.SpiderEgg,
                                ItemID.RavenStaff,
                                ItemID.CandyCornRifle,
                                ItemID.JackOLanternLauncher,
                                ItemID.ScytheWhip
                            }));
                    }
                    break;

                case NPCID.Everscream:
                    if (!Main.dayTime && !Main.snowMoon)
                    {
                        if (Main.rand.NextBool(3))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                                ItemID.ChristmasTreeSword,
                                ItemID.ChristmasHook,
                                ItemID.Razorpine,
                                ItemID.FestiveWings
                            }));
                    }
                    break;

                case NPCID.SantaNK1:
                    if (!Main.dayTime && !Main.snowMoon)
                    {
                        if (Main.rand.NextBool(3))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                                ItemID.ElfMelter,
                                ItemID.ChainGun
                            }));
                    }
                    break;

                case NPCID.IceQueen:
                    if (!Main.dayTime && !Main.snowMoon)
                    {
                        if (Main.rand.NextBool(3))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                                ItemID.BlizzardStaff,
                                ItemID.SnowmanCannon,
                                ItemID.NorthPole,
                                ItemID.BabyGrinchMischiefWhistle,
                                ItemID.ReindeerBells
                            }));
                    }
                    break;

                default:
                    break;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.ZombieEskimo:
                case NPCID.ArmedZombieEskimo:
                case NPCID.Penguin:
                case NPCID.IceSlime:
                case NPCID.SpikedIceSlime:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ItemID.EskimoHood, ItemID.EskimoCoat, ItemID.EskimoPants));
                    break;

                case NPCID.GreekSkeleton:
                    npcLoot.RemoveWhere(rule => rule is CommonDrop drop && (drop.itemId == ItemID.GladiatorHelmet || drop.itemId == ItemID.GladiatorBreastplate || drop.itemId == ItemID.GladiatorLeggings));
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ItemID.GladiatorHelmet, ItemID.GladiatorBreastplate, ItemID.GladiatorLeggings));
                    break;

                case NPCID.Merchant:
                    npcLoot.Add(ItemDropRule.Common(ItemID.MiningShirt, 8));
                    npcLoot.Add(ItemDropRule.Common(ItemID.MiningPants, 8));
                    break;

                case NPCID.Nurse:
                    npcLoot.Add(ItemDropRule.Common(ItemID.LifeCrystal, 5));
                    break;

                case NPCID.Demolitionist:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Dynamite, 2, 5, 5));
                    break;

                case NPCID.Dryad:
                    npcLoot.Add(ItemDropRule.Common(ItemID.HerbBag, 3));
                    break;

                case NPCID.DD2Bartender:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Ale, 2, 4, 4));
                    break;

                case NPCID.ArmsDealer:
                    npcLoot.Add(ItemDropRule.Common(ItemID.NanoBullet, 4, 30, 30));
                    break;

                case NPCID.Clothier:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Skull, 20));
                    break;

                case NPCID.Mechanic:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Wire, 5, 40, 40));
                    break;

                case NPCID.Wizard:
                    npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar, 5, 5, 5));
                    break;

                case NPCID.TaxCollector:
                    npcLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 8, 10, 10));
                    break;

                case NPCID.Truffle:
                    npcLoot.Add(ItemDropRule.Common(ItemID.MushroomStatue, 8));
                    break;

                case NPCID.Angler:
                    npcLoot.Add(ItemDropRule.OneFromOptions(2, ItemID.OldShoe, ItemID.TinCan, ItemID.FishingSeaweed));
                    break;


                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DefenderMedal, 1, 20, 20));
                    break;

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DefenderMedal, 1, 5, 5));
                    break;

                case NPCID.Raven:
                    npcLoot.Add(ItemDropRule.Common(ItemID.GoodieBag));
                    break;

                case NPCID.SlimeRibbonRed:
                case NPCID.SlimeRibbonGreen:
                case NPCID.SlimeRibbonWhite:
                case NPCID.SlimeRibbonYellow:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Present));
                    break;

                case NPCID.BloodZombie:
                    npcLoot.Add(ItemDropRule.OneFromOptions(200, ItemID.BladedGlove, ItemID.BloodyMachete));
                    break;

                case NPCID.Clown:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Bananarang));
                    break;

                case NPCID.MoonLordCore:
                    npcLoot.Add(ItemDropRule.Common(ItemID.MoonLordLegs, 100));
                    break;
            }

            base.ModifyNPCLoot(npc, npcLoot);
        }


        public override bool CheckDead(NPC npc)
        {
            // Lumber Jaxe
            if (npc.FindBuffIndex(ModContent.BuffType<WoodDrop>()) != -1)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.Wood, Main.rand.Next(10, 30));
            }

            switch (npc.type)
            {
                // Avoid lunar event with cultist summon
                case NPCID.CultistBoss:
                    if (!PillarSpawn)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc2 = Main.npc[i];
                            NPC.LunarApocalypseIsUp = false;

                            if (npc2.type == NPCID.LunarTowerNebula || npc2.type == NPCID.LunarTowerSolar || npc2.type == NPCID.LunarTowerStardust || npc2.type == NPCID.LunarTowerVortex)
                            {
                                NPC.TowerActiveSolar = true;
                                npc2.active = false;
                            }

                            NPC.TowerActiveSolar = false;
                        }
                    }

                    break;

                case NPCID.GiantWormHead:
                case NPCID.DiggerHead:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "worm");
                    break;

                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, "ogre");
                    break;

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, "darkMage");
                    break;

                case NPCID.Clown:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "clown");

                    break;

                case NPCID.BlueSlime:
                    if (npc.netID == NPCID.Pinky)
                    {

                        FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "pinky");

                    }
                    break;

                case NPCID.UndeadMiner:

                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "undeadMiner");
                    break;

                case NPCID.Tim:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "tim");
                    break;

                case NPCID.DoctorBones:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "doctorBones");
                    break;

                case NPCID.Mimic:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimic");
                    break;

                case NPCID.WyvernHead:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "wyvern");
                    break;

                case NPCID.RuneWizard:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "runeWizard");
                    break;

                case NPCID.Nymph:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "nymph");
                    break;

                case NPCID.Moth:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "moth");
                    break;

                case NPCID.RainbowSlime:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "rainbowSlime");
                    break;

                case NPCID.Paladin:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "paladin");
                    break;

                case NPCID.Medusa:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "medusa");
                    break;

                case NPCID.IceGolem:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "iceGolem");
                    break;

                case NPCID.SandElemental:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "sandElemental");
                    break;

                case NPCID.Nailhead:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "nailhead");
                    break;

                case NPCID.Mothron:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3, "rareEnemy", "mothron");
                    break;

                case NPCID.BigMimicCorruption:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicCorrupt");
                    break;

                case NPCID.BigMimicHallow:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicHallow");
                    break;

                case NPCID.BigMimicCrimson:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicCrimson");
                    break;

                case NPCID.BigMimicJungle:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicJungle");
                    break;

                case NPCID.GoblinSummoner:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode && NPC.downedGoblins, "rareEnemy", "goblinSummoner");
                    break;

                case NPCID.PirateShip:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, NPC.downedPirates, "flyingDutchman");
                    break;

                case NPCID.DungeonSlime:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedBoss3, "rareEnemy", "dungeonSlime");
                    break;

                case NPCID.PirateCaptain:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode && NPC.downedPirates, "rareEnemy", "pirateCaptain");
                    break;

                case NPCID.SkeletonSniper:
                case NPCID.TacticalSkeleton:
                case NPCID.SkeletonCommando:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "skeletonGun");
                    break;

                case NPCID.Necromancer:
                case NPCID.NecromancerArmored:
                case NPCID.DiabolistRed:
                case NPCID.DiabolistWhite:
                case NPCID.RaggedCaster:
                case NPCID.RaggedCasterOpenCoat:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "skeletonMage");
                    break;

                case NPCID.BoneLee:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "boneLee");
                    break;

                case NPCID.HeadlessHorseman:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, "headlessHorseman");
                    break;

                case NPCID.Pumpking:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, NPC.downedHalloweenKing, "pumpking");
                    break;

                case NPCID.MourningWood:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, NPC.downedHalloweenTree, "mourningWood");
                    break;

                case NPCID.IceQueen:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, NPC.downedChristmasIceQueen, "iceQueen");
                    break;

                case NPCID.SantaNK1:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, NPC.downedChristmasSantank, "santank");
                    break;

                case NPCID.Everscream:
                    FargoUtils.TryDowned(npc, "Abominationn", Color.Orange, NPC.downedChristmasTree, "everscream");
                    break;

                case NPCID.ZombieMerman:
                case NPCID.EyeballFlyingFish:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "zombieMerman", "eyeFish");
                    break;

                case NPCID.BloodEelHead:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "bloodEel");
                    break;

                case NPCID.GoblinShark:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "goblinShark");
                    break;

                case NPCID.BloodNautilus:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "dreadnautilus");
                    break;

                case NPCID.Gnome:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "gnome");
                    break;

                case NPCID.RedDevil:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "redDevil");
                    break;

                case NPCID.GoldenSlime:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "goldenSlime");
                    break;

                case NPCID.GoblinScout:
                    FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", "goblinScout");
                    break;

                default:
                    break;
            }

            if (Fargowiltas.ModRareEnemies.ContainsKey(npc.type))
            {
                FargoUtils.TryDowned(npc, "Deviantt", Color.HotPink, "rareEnemy", Fargowiltas.ModRareEnemies[npc.type]);

            }

            if (npc.type == NPCID.DD2Betsy && !PandoraActive)
            {
                FargoUtils.PrintText(Language.GetTextValue("Announcement.HasBeenDefeated_Single", Language.GetTextValue("NPCName.DD2Betsy")), new Color(175, 75, 0));
                FargoWorld.DownedBools["betsy"] = true;
            }

            if (npc.boss)
            {
                FargoWorld.DownedBools["boss"] = true;
            }

            return true;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (GetInstance<FargoConfig>().RottenEggs && projectile.type == ProjectileID.RottenEgg && npc.townNPC)
            {
                modifiers.FinalDamage *= 20;
                //damage *= 20;
            }
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            // No angler check enables luiafk compatibility
            if (GetInstance<FargoConfig>().AnglerQuestInstantReset && Main.anglerQuestFinished)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.AnglerQuestSwap();
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // Broadcast swap request to server
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)3);
                    netMessage.Send();
                }
            }
        }

        private void SpawnBoss(NPC npc, int boss)
        {
            int spawn;

            if (SwarmActive)
            {
                if (npc.type == NPCID.WallofFlesh)
                {
                    NPC currentWoF = Main.npc[LastWoFIndex];
                    int startingPos = (int)currentWoF.position.X;
                    spawn = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), startingPos + (400 * WoFDirection), (int)currentWoF.position.Y, NPCID.WallofFlesh, 0);
                    if (spawn != Main.maxNPCs)
                    {
                        Main.npc[spawn].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;
                        LastWoFIndex = spawn;
                    }
                }
                else
                {
                    spawn = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npc.position.X + Main.rand.Next(-1000, 1000), (int)npc.position.Y + Main.rand.Next(-400, -100), boss);

                    if (spawn != Main.maxNPCs)
                    {
                        Main.npc[spawn].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;
                        NetMessage.SendData(MessageID.SyncNPC, number: boss);
                    }
                }
            }
            else
            {
                // Pandora
                int random;

                do
                {
                    random = Main.rand.Next(Bosses);
                }
                while (NPC.CountNPCS(random) >= 4);

                spawn = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npc.position.X + Main.rand.Next(-1000, 1000), (int)npc.position.Y + Main.rand.Next(-400, -100), random);
                if (spawn != Main.maxNPCs)
                {
                    Main.npc[spawn].GetGlobalNPC<FargoGlobalNPC>().PandoraActive = true;
                    NetMessage.SendData(MessageID.SyncNPC, number: random);
                }
            }
        }

        private void Swarm(NPC npc, int boss, int minion, int bossbag, int trophy, int reward)
        {
            if (bossbag >= 0 && bossbag != ItemID.DefenderMedal)
            {
                npc.DropItemInstanced(npc.Center, npc.Size, bossbag);
            }
            else if (bossbag >= 0 && bossbag == ItemID.DefenderMedal)
            {
                npc.DropItemInstanced(npc.Center, npc.Size, bossbag, 5);
            }

            int count = 0;
            if (SwarmActive)
            {
                count = NPC.CountNPCS(boss) - 1; // Since this one is about to be dead
            }
            else
            {
                // Pandora
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Array.IndexOf(Bosses, Main.npc[i].type) > -1)
                    {
                        count++;
                    }
                }
            }

            int missing = Fargowiltas.SwarmSpawned - count;

            Fargowiltas.SwarmKills++;

            // Drop swarm reward every 100 kills
            if (Fargowiltas.SwarmKills % 100 == 0 && reward > 0)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, reward);
            }

            //drop trphy every 10 killa
            if (Fargowiltas.SwarmKills % 10 == 0 && trophy != -1)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, trophy);
            }

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.SwarmKilled", Fargowiltas.SwarmKills), new Color(206, 12, 15));
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.SwarmTotal", Fargowiltas.SwarmTotal), new Color(206, 12, 15));
            }
            else
            {
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.SwarmKilled", Fargowiltas.SwarmKills), new Color(206, 12, 15));
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.SwarmTotal", Fargowiltas.SwarmTotal), new Color(206, 12, 15));
            }

            if (minion != -1 && NPC.CountNPCS(minion) >= Fargowiltas.SwarmSpawned)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == minion)
                    {
                        Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                        //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                    }
                }
            }

            // If theres still more to spawn
            if (Fargowiltas.SwarmKills <= Fargowiltas.SwarmTotal - Fargowiltas.SwarmSpawned)
            {
                int spawned = 0;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    // Count NPCs
                    int num = 0;
                    for (int j = 0; j < Main.maxNPCs; j++)
                    {
                        if (Main.npc[j].active)
                        {
                            num++;
                        }
                    }

                    // Kill a minion and spawn boss if too many npcs
                    if (num >= Main.maxNPCs)
                    {
                        if (SwarmActive && minion > 0 && i < Main.maxNPCs - 1)
                        {
                            if (Main.npc[i].type == minion)
                            {
                                Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                                //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                            }
                        }
                        else
                        {
                            // Pandora
                            if (Array.IndexOf(Bosses, Main.npc[i].type) == -1 && !Main.npc[i].boss)
                            {
                                Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                                //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                            }
                        }
                    }

                    SpawnBoss(npc, boss);
                    spawned++;

                    if (spawned <= missing)
                    {
                        continue;
                    }

                    break;
                }
            }
            else if (Fargowiltas.SwarmKills >= Fargowiltas.SwarmTotal)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.SwarmDefeated"), new Color(206, 12, 15));
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.SwarmDefeated"), new Color(206, 12, 15));
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC kill = Main.npc[i];
                    if (kill.active && !kill.friendly && kill.type != NPCID.LunarTowerNebula && kill.type != NPCID.LunarTowerSolar && kill.type != NPCID.LunarTowerStardust && kill.type != NPCID.LunarTowerVortex)
                    {
                        Main.npc[i].GetGlobalNPC<FargoGlobalNPC>().NoLoot = true;
                        Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                        //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                    }
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Fargowiltas.SwarmActive = false;
                    LastWoFIndex = -1;
                    WoFDirection = 0;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData);
                }
            }

            // Make sure theres enough left to beat it
            else
            {
                // Spawn more if needed
                if (count >= Fargowiltas.SwarmSpawned || Fargowiltas.SwarmTotal <= 20)
                {
                    return;
                }

                int extraSpawn = 0;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    // Count NPCs
                    int num = 0;
                    for (int j = 0; j < Main.maxNPCs; j++)
                    {
                        if (Main.npc[j].active)
                        {
                            num++;
                        }
                    }

                    // Kill a minion and spawn boss if too many NPCs
                    if (num >= Main.maxNPCs)
                    {
                        if (SwarmActive && minion > 0 && i < Main.maxNPCs - 1)
                        {
                            if (Main.npc[i].type == minion)
                            {
                                Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                                //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                            }
                        }
                        else
                        {
                            // Pandora
                            if (Array.IndexOf(Bosses, Main.npc[i].type) == -1 && !Main.npc[i].boss)
                            {
                                Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                                //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                            }
                        }
                    }

                    SpawnBoss(npc, boss);
                    extraSpawn++;

                    if (extraSpawn < 5)
                    {
                        continue;
                    }

                    break;
                }
            }
        }

        public static void SpawnWalls(Player player)
        {
            int startingPos;

            if (LastWoFIndex == -1)
            {
                startingPos = (int)player.position.X;
            }
            else
            {
                startingPos = (int)Main.npc[LastWoFIndex].position.X;
            }

            Vector2 pos = player.position;

            if (WoFDirection == 0)
            {
                //1 is to the right, -1 is left
                WoFDirection = ((player.position.X / 16) > (Main.maxTilesX / 2)) ? 1 : -1;
            }

            int wof = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), startingPos + (400 * WoFDirection), (int)pos.Y, NPCID.WallofFlesh, 0);
            Main.npc[wof].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;

            LastWoFIndex = wof;
        }

        public static bool SpecificBossIsAlive(ref int bossID, int bossType)
        {
            if (bossID != -1)
            {
                if (Main.npc[bossID].active && Main.npc[bossID].type == bossType)
                {
                    return true;
                }
                else
                {
                    bossID = -1;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static int boss = -1;

        public static bool AnyBossAlive()
        {
            if (boss == -1)
                return false;

            NPC npc = Main.npc[boss];

            if (npc.active && npc.type != NPCID.MartianSaucerCore && (npc.boss || npc.type == NPCID.EaterofWorldsHead))
                return true;
            boss = -1;
            return false;
        }
    }
}