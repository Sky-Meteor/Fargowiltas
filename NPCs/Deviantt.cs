﻿using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Items.Summons.Abom;
using Fargowiltas.Items.Summons.Deviantt;
using Fargowiltas.Projectiles;
using Fargowiltas.ShoppingBiomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.NPCs
{
    [AutoloadHead]
    public class Deviantt : ModNPC
    {
        private bool canSayDefeatQuote = true;
        private int defeatQuoteTimer = 900;
        private int trolling;

        //public override bool Autoload(ref string name)
        //{
        //    name = "Deviantt";
        //    return mod.Properties.Autoload;
        //}

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Deviantt");

            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<SkyBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<JungleBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);

            NPC.Happiness.SetNPCAffection<Mutant>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Abominationn>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate);

            NPCID.Sets.DebuffImmunitySets.Add(NPC.type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Suffocation,
                    BuffID.Lovestruck,
                    BuffID.Stinky
                }
            });
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.Bestiary.Deviantt")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = NPC.downedMoonlord ? 50 : 15;
            NPC.lifeMax = NPC.downedMoonlord ? 2500 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Angler;

            //if (GetInstance<FargoConfig>().CatchNPCs)
            //{
            //    Main.NPCCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)mod.ItemType("Deviantt");
            //}
                
            NPC.buffImmune[BuffID.Suffocation] = true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DevianttAlive"))
                return false;

            return GetInstance<FargoConfig>().Devi && !FargoGlobalNPC.AnyBossAlive() 
                && ((FargoWorld.DownedBools.TryGetValue("rareEnemy", out bool value) && value)
                || (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode")));
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void AI()
        {
            NPC.breath = 200;
            if (defeatQuoteTimer > 0)
                defeatQuoteTimer--;
            else
                canSayDefeatQuote = false;

            if (++trolling > 60 * 60)
            {
                trolling = -Main.rand.Next(30 * 60);

                DoALittleTrolling();
            }
        }

        void DoALittleTrolling()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            //no trolling when possible danger
            if (FargoGlobalNPC.AnyBossAlive()
                || Main.npc.Any(n => n.active && n.damage > 0 && !n.friendly && NPC.Distance(n.Center) < 1200)
                || NPC.life < NPC.lifeMax)
                return;

            if (NPC.ai[0] == 10) //actual attack anim
                return;

            Vector2 targetPos = default;

            const float maxRange = 600f;
            float targetDistance = maxRange;

            void TryUpdateTarget(Vector2 possibleTarget)
            {
                if (targetDistance > NPC.Distance(possibleTarget)
                    && Collision.CanHitLine(NPC.Center, 0, 0, possibleTarget, 0, 0))
                {
                    Tile tileBelow = Framing.GetTileSafely(possibleTarget + 32f * Vector2.UnitY);
                    if (tileBelow.HasUnactuatedTile && Main.tileSolid[tileBelow.TileType] && !Main.tileSolidTop[tileBelow.TileType])
                    {
                        targetPos = possibleTarget;
                        targetDistance = NPC.Distance(possibleTarget);
                    }
                }
            }

            //pick a target
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].friendly && Main.npc[i].townNPC
                    && Main.npc[i].life == Main.npc[i].lifeMax
                    && i != NPC.whoAmI)
                    TryUpdateTarget(Main.npc[i].Center);
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].ghost
                    && Main.player[i].statLife == Main.player[i].statLifeMax2)
                    TryUpdateTarget(Main.player[i].Center);
            }

            if (targetPos != default)
            {
                float distanceRatio = targetDistance / maxRange;

                //account for gravity
                targetPos.Y += 16f;
                targetPos.Y -= 20f * 3 * distanceRatio * distanceRatio;
                Vector2 vel = (8f + 12f * distanceRatio) * NPC.DirectionTo(targetPos);

                int type = Main.rand.NextBool() ? ProjectileID.LovePotion : ProjectileID.FoulPotion;
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, type, 0, 0f, Main.myPlayer);
                Main.projectile[p].npcProj = true;

                NPC.spriteDirection = NPC.direction = targetPos.X < NPC.Center.X ? -1 : 1;
                NPC.ai[0] = 10; //trick vanilla ai into thinking devi has just attacked, but dont actually attack
                NPC.ai[1] = NPCID.Sets.AttackTime[NPC.type] - 1; //sets localai[3] = 0 if exactly AttackTime
                NPC.localAI[3] = 300f; //counts up from zero and attacks at some threshold if left alone
                NPC.netUpdate = true;
            }
        }

        public override List<string> SetNPCNameList()
        {
            string[] names = { "Akira", "Remi", "Saku", "Seira", "Koi", "Elly", "Lori", "Calia", "Teri", "Artt", "Flan", "Shion", "Tewi" };

            return new List<string>(names);
        }

        public override string GetChat()
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode")
                && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived"))
            {
                ModLoader.GetMod("FargowiltasSouls").Call("GiveDevianttGifts");
                return Main.npcChatText = Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Gift");
            }

            if (Main.notTheBeesWorld)
            {
                string LocalizedText(string text) => Language.GetTextValue($"Mods.Fargowiltas.Dialogues.Devi.{text}");
                string text = LocalizedText("Bee");
                int max = Main.rand.Next(10, 50);
                for (int i = 0; i < max; i++)
                    text += Language.GetTextValue("Mods.Fargowiltas.UI.space") + LocalizedText(Main.rand.Next(new string[] { "HA", "HA", "HEE", "HOO", "HEH", "HAH" }));
                text += Language.GetTextValue("Mods.Fargowiltas.MessageInfo.BattleCry.!");
                return text;
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && Main.rand.NextBool())
            {
                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("EridanusArmor"))
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Eridanus");

                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("NekomiArmor"))
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Nekomi");
            }

            if (NPC.homeless && canSayDefeatQuote && Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi"))
            {
                canSayDefeatQuote = false;
                return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Defeat");
            }

            if (Main.rand.NextBool())
            {
                if (Main.LocalPlayer.stinky)
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Stinky");

                if (Main.LocalPlayer.loveStruck)
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Love", Main.rand.Next(2, 8));

                if (Main.bloodMoon)
                    return Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.BloodMoon");
            }

            List<string> dialogue = new List<string>
            {
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.1"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.2"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.3"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.4"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.5", Main.LocalPlayer.name),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.6"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.7"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.8"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.9"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.10"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.11"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.12"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.13"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.14"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.15"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.16"),
                Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.17"),
            };

            if (Main.hardMode)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.HardMode"));
            }

            int mutant = NPC.FindFirstNPC(NPCType<Mutant>());
            if (mutant != -1)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Mutant1", Main.npc[mutant].GivenName));
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Mutant2", Main.npc[mutant].GivenName));
            }

            int lumberjack = NPC.FindFirstNPC(NPCType<LumberJack>());
            if (lumberjack != -1)
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Lumber", Main.npc[lumberjack].GivenName));
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
            {
                dialogue.Add(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.Eternity"));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
            {
                button2 = Language.GetTextValue("Mods.Fargowiltas.UI.Help"); //(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived") ? "Help" : "Receive Gift";
            }
        }

        public const string ShopName = "Shop";

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = ShopName;
            }
            else if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
            {
                Main.npcChatText = Fargowiltas.dialogueTracker.GetDialogue(NPC.GivenName);
            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName);

            if (Fargowiltas.ModLoaded["FargowiltasSoulsDLC"] && TryFind("FargowiltasSoulsDLC", "PandorasBox", out ModItem pandorasBox))
            {
                npcShop.Add(new Item(pandorasBox.Type));
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
            {
                if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode") && TryFind("FargowiltasSouls", "EternityAdvisor", out ModItem advisor))
                {
                    npcShop.Add(new Item(advisor.Type) { shopCustomPrice = Item.buyPrice(copper: 10000) });
                }
            }

            npcShop
                .Add(new Item(ItemType<WormSnack>()) { shopCustomPrice = Item.buyPrice(copper: 20000) }, new Condition("Mods.Fargowiltas.Conditions.WormDown", () => FargoWorld.DownedBools["worm"]))
                .Add(new Item(ItemType<PinkSlimeCrown>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.PinkyDown", () => FargoWorld.DownedBools["pinky"]))
                .Add(new Item(ItemType<GoblinScrap>()) { shopCustomPrice = Item.buyPrice(copper: 10000) }, new Condition("Mods.Fargowiltas.Conditions.ScoutDown", () => FargoWorld.DownedBools["goblinScout"]))
                .Add(new Item(ItemType<Eggplant>()) { shopCustomPrice = Item.buyPrice(copper: 20000) }, new Condition("Mods.Fargowiltas.Conditions.DoctorDown", () => FargoWorld.DownedBools["doctorBones"]))
                .Add(new Item(ItemType<AttractiveOre>()) { shopCustomPrice = Item.buyPrice(copper: 30000) }, new Condition("Mods.Fargowiltas.Conditions.MinerDown", () => FargoWorld.DownedBools["undeadMiner"]))
                .Add(new Item(ItemType<HolyGrail>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.TimDown", () => FargoWorld.DownedBools["tim"]))
                .Add(new Item(ItemType<GnomeHat>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.GnomeDown", () => FargoWorld.DownedBools["gnome"]))
                .Add(new Item(ItemType<GoldenSlimeCrown>()) { shopCustomPrice = Item.buyPrice(copper: 600000) }, new Condition("Mods.Fargowiltas.Conditions.GoldSlimeDown", () => FargoWorld.DownedBools["goldenSlime"]))
                .Add(new Item(ItemType<SlimyLockBox>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.DungeonSlimeDown", () => NPC.downedBoss3 &&  FargoWorld.DownedBools["dungeonSlime"]))
                .Add(new Item(ItemType<AthenianIdol>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.MedusaDown", () => Main.hardMode && FargoWorld.DownedBools["medusa"]))
                .Add(new Item(ItemType<ClownLicense>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.ClownDown", () => Main.hardMode && FargoWorld.DownedBools["clown"]))
                .Add(new Item(ItemType<HeartChocolate>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.NymphDown", () => FargoWorld.DownedBools["nymph"]))
                .Add(new Item(ItemType<MothLamp>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.MothDown", () => Main.hardMode && FargoWorld.DownedBools["moth"]))
                .Add(new Item(ItemType<DilutedRainbowMatter>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.RainbowSlimeDown", () => Main.hardMode &&  FargoWorld.DownedBools["rainbowSlime"]))
                .Add(new Item(ItemType<CloudSnack>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.WyvernDown", () => Main.hardMode && FargoWorld.DownedBools["wyvern"]))
                .Add(new Item(ItemType<RuneOrb>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.RuneDown", () => Main.hardMode && FargoWorld.DownedBools["runeWizard"]))
                .Add(new Item(ItemType<SuspiciousLookingChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicDown", () => Main.hardMode && FargoWorld.DownedBools["mimic"]))
                .Add(new Item(ItemType<HallowChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicHallowDown", () => Main.hardMode && FargoWorld.DownedBools["mimicHallow"]))
                .Add(new Item(ItemType<CorruptChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicCorruptDown", () => Main.hardMode && (FargoWorld.DownedBools["mimicCorrupt"] || FargoWorld.DownedBools["mimicCrimson"])))
                .Add(new Item(ItemType<CrimsonChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicCrimsonDown", () => Main.hardMode && (FargoWorld.DownedBools["mimicCorrupt"] || FargoWorld.DownedBools["mimicCrimson"])))
                .Add(new Item(ItemType<JungleChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicJungleDown", () => Main.hardMode && FargoWorld.DownedBools["mimicJungle"]))
                .Add(new Item(ItemType<CoreoftheFrostCore>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.IceGolemDown", () => Main.hardMode && FargoWorld.DownedBools["iceGolem"]))
                .Add(new Item(ItemType<ForbiddenForbiddenFragment>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.SandDown", () => Main.hardMode && FargoWorld.DownedBools["sandElemental"]))
                .Add(new Item(ItemType<DemonicPlushie>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.DevilDown", () => NPC.downedMechBossAny && FargoWorld.DownedBools["redDevil"]))
                .Add(new Item(ItemType<SuspiciousLookingLure>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.BloodFishDown", () => FargoWorld.DownedBools["eyeFish"] || FargoWorld.DownedBools["zombieMerman"]))
                .Add(new Item(ItemType<BloodUrchin>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.BloodEelDown", () => Main.hardMode && FargoWorld.DownedBools["bloodEel"]))
                .Add(new Item(ItemType<HemoclawCrab>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.BloodGoblinDown", () => Main.hardMode && FargoWorld.DownedBools["goblinShark"]))
                .Add(new Item(ItemType<BloodSushiPlatter>()) { shopCustomPrice = Item.buyPrice(copper: 200000) }, new Condition("Mods.Fargowiltas.Conditions.BloodNautDown", () => Main.hardMode && FargoWorld.DownedBools["dreadnautilus"]))
                .Add(new Item(ItemType<ShadowflameIcon>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.SummonerDown", () => Main.hardMode && NPC.downedGoblins && FargoWorld.DownedBools["goblinSummoner"]))
                .Add(new Item(ItemType<PirateFlag>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.PirateDown", () => Main.hardMode && NPC.downedPirates && FargoWorld.DownedBools["pirateCaptain"]))
                .Add(new Item(ItemType<Pincushion>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.NailheadDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["nailhead"]))
                .Add(new Item(ItemType<MothronEgg>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.MothronDown", () => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && FargoWorld.DownedBools["mothron"]))
                .Add(new Item(ItemType<LeesHeadband>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.LeeDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["boneLee"]))
                .Add(new Item(ItemType<GrandCross>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.PaladinDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["paladin"]))
                .Add(new Item(ItemType<AmalgamatedSkull>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.SkeleGunDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["skeletonGun"]))
                .Add(new Item(ItemType<AmalgamatedSpirit>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.SkeleGunDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["skeletonMage"]))
            ;

            npcShop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (NPC.downedMoonlord)
            {
                damage = 80;
                knockback = 4f;
            }
            else if (Main.hardMode)
            {
                damage = 40;
                knockback = 4f;
            }
            else
            {
                damage = 20;
                knockback = 2f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = NPC.downedMoonlord ? 1 : 30;
            if (!NPC.downedMoonlord)
            {
                randExtraCooldown = 30;
            }
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = NPC.downedMoonlord ? ProjectileType<FakeHeartMarkDeviantt>() : ProjectileType<FakeHeartDeviantt>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            randomOffset = 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f, 0, default, 0.8f);
                }

                if (!Main.dedServ)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/DevianttGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/DevianttGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/DevianttGore1").Type);
                }
            }
            else
            {
                for (int k = 0; k < hit.Damage / NPC.lifeMax * 50.0; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 0, default, 0.6f);
                }
            }
        }

        public override void OnKill()
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && TryFind("FargowiltasSouls", "CosmosChampion", out ModNPC cosmosChamp) && NPC.AnyNPCs(cosmosChamp.Type))
                Item.NewItem(NPC.GetSource_Loot(), NPC.Hitbox, ItemType<Items.Tiles.WalkingRick>());
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived"))
            {
                Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
                Rectangle rectangle = NPC.frame;//new Rectangle(0, y3, texture2D13.Width, num156);
                Vector2 origin2 = rectangle.Size() / 2f;
                SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                Color color26 = Main.DiscoColor;
                color26.A = 0;

                //for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
                //{
                //    Color color27 = color26 * 0.5f;
                //    color27 *= (float)(NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type];
                //    Vector2 value4 = NPC.oldPos[i];
                //    float num165 = NPC.rotation; //NPC.oldRot[i];
                //    Main.EntitySpriteDraw(texture2D13, value4 + NPC.Size / 2f - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, NPC.scale, effects, 0);
                //}

                float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.5f + 1f;
                scale *= NPC.scale;
                Main.EntitySpriteDraw(texture2D13, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, NPC.rotation, origin2, scale, effects, 0);
            }
            //Main.EntitySpriteDraw(texture2D13, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, origin2, NPC.scale, effects, 0);
            return true;
        }
    }
}
