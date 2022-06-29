using Fargowiltas.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;

namespace Fargowiltas
{
    internal class DevianttDialogueTracker
    {
        public static class HelpDialogueType
        {
            public static readonly byte BossOrEvent = 0;
            public static readonly byte Environment = 1;
            public static readonly byte Misc = 2;
        }

        public struct HelpDialogue
        {
            public readonly string Message;
            public readonly byte Type;
            public readonly Predicate<string> Predicate;

            public HelpDialogue(string message, byte type, Predicate<string> predicate)
            {
                Message = message;
                Type = type;
                Predicate = predicate;
            }

            public bool CanDisplay(string deviName) => Predicate(deviName);
        }

        public List<HelpDialogue> PossibleDialogue;
        private int lastDialogueType;

        public DevianttDialogueTracker()
        {
            PossibleDialogue = new List<HelpDialogue>();
        }

        public void AddDialogue(string message, byte type, Predicate<string> predicate)
        {
            PossibleDialogue.Add(new HelpDialogue(message, type, predicate));
        }

        public string GetDialogue(string deviName)
        {
            WeightedRandom<string> dialogueChooser = new WeightedRandom<string>();
            (List<HelpDialogue> sortedDialogue, int type) = SortDialogue(deviName);

            foreach (HelpDialogue dialogue in sortedDialogue)
            {
                dialogueChooser.Add(dialogue.Message);
            }

            lastDialogueType = type;
            return dialogueChooser;
        }

        private (List<HelpDialogue> sortedDialogue, int type) SortDialogue(string deviName)
        {
            List<HelpDialogue> sortedDialogue = new List<HelpDialogue>();
            int typeChoice = 0;
            int attempts = 0;
            while (true)
            {
                attempts++;
                typeChoice = Main.rand.Next(3);
                if (typeChoice != lastDialogueType || typeChoice == HelpDialogueType.Misc) // There's a lot more misc so allow repeats
                {
                    sortedDialogue = PossibleDialogue.Where((dialogue) => dialogue.Type == typeChoice && dialogue.CanDisplay(deviName)).ToList();

                    if (sortedDialogue.Count != 0)
                        break;
                }
                
                if (attempts == 100)
                {
                    typeChoice = HelpDialogueType.BossOrEvent;
                    sortedDialogue = PossibleDialogue.Where((dialogue) => dialogue.Type == typeChoice && dialogue.CanDisplay(deviName)).ToList();
                    break;
                }
            }

            return (sortedDialogue, typeChoice);
        }

        public void AddVanillaDialogue()
        {
            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.DownedMutant"),
                HelpDialogueType.BossOrEvent, (name) => (bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") ?? false));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.MutantSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => (bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom") ?? false) && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") ?? false));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.AbomSuggestion"),
                HelpDialogueType.Misc, (name) => (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedEridanus") && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom"));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.ChampionSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedMoonlord && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedEridanus"));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.MoonLordSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedAncientCultist && !NPC.downedMoonlord);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.CultistSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedFishron && !NPC.downedAncientCultist);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.FishronSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => FargoWorld.DownedBools["betsy"] && !NPC.downedFishron);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.BetsySuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedGolemBoss && !FargoWorld.DownedBools["betsy"]);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.GolemSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedPlantBoss && !NPC.downedGolemBoss);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.PlanteraSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss);

            //AddDialogue("Watch out when you break your fourth altar! It might attract the pirates, so be sure you're ready when you do it.", HelpDialogueType.BossOrEvent, (name) => Main.hardMode && !NPC.downedPirates);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.DestroyerSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => Main.hardMode && !NPC.downedMechBoss1);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.TwinsSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => Main.hardMode && !NPC.downedMechBoss2);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.PrimeSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => Main.hardMode && !NPC.downedMechBoss3);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.WallOfFleshSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi") && !Main.hardMode);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.DeviSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedBoss3 && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi"));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SkeletronSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedQueenBee && !NPC.downedBoss3);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.QueenBeeSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedBoss2 && !NPC.downedQueenBee);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.BrainSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedBoss1 && !NPC.downedBoss2 && WorldGen.crimson);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.EaterSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedBoss1 && !NPC.downedBoss2 && !WorldGen.crimson);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.GoblinCrimsonSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => !NPC.downedGoblins && WorldGen.crimson);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.GoblinCorruptSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => !NPC.downedGoblins && !WorldGen.crimson);

            // I added this because, if there isn't always dialogue available for a boss, the dialogue chooser self destructs
            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.EyeSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => NPC.downedSlimeKing && !NPC.downedBoss1);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SlimeSuggestion"),
                HelpDialogueType.BossOrEvent, (name) => !NPC.downedSlimeKing);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionMisc1"),
                HelpDialogueType.Misc, (name) => true);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionMisc2"),
                HelpDialogueType.Misc, (name) => true);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionMisc3"),
                HelpDialogueType.Misc, (name) => true);

            //AddDialogue("Just so you know, ammos are less effective. Only a bit of their damage contributes to your total output!",
            //    HelpDialogueType.Misc, (name) => Main.LocalPlayer.HeldItem.CountsAsClass(DamageClass.Ranged));

            //AddDialogue("Found any Top Hat Squirrels yet? Keep one in your inventory and maybe a special friend will show up!",
            //    HelpDialogueType.Misc, (name) => !NPC.AnyNPCs(ModContent.NPCType<Squirrel>()));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionMaxLife"),
                HelpDialogueType.Misc, (name) => Main.LocalPlayer.statLifeMax < 400);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionFish"),
                HelpDialogueType.Misc, (name) => !Main.hardMode);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionWater"),
                HelpDialogueType.Environment, (name) => !Main.LocalPlayer.accFlipper && !Main.LocalPlayer.gills && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("MutantAntibodies") ?? false));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionOnFire"),
                HelpDialogueType.Environment, (name) => !Main.LocalPlayer.fireWalk && !(Main.LocalPlayer.lavaMax > 0) && !Main.LocalPlayer.buffImmune[BuffID.OnFire] && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionSpace"),
                HelpDialogueType.Environment, (name) => !Main.LocalPlayer.buffImmune[BuffID.Suffocation] && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            //AddDialogue("The spirits of light and dark stopped by and they sounded pretty upset with you. Don't be too surprised if something happens to you for entering their territory!", HelpDialogueType.Environment, (name) => Main.hardMode && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            //AddDialogue("Why not go hunting for some rare monsters every once in a while? Plenty of treasure to be looted and all that.", HelpDialogueType.Misc, (name) => Main.hardMode);

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionUndergroundHallow"),
                HelpDialogueType.Environment, (name) => Main.hardMode && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            AddDialogue(Language.GetTextValue("Mods.Fargowiltas.Dialogues.Devi.SuggestionUndergroundJungle"),
                HelpDialogueType.Misc, (name) => Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.LocalPlayer.statLifeMax2 < 500);

            // This is much more possible than before because of how branching works, so I just decided to remove it.
            //AddDialogue("Ever tried out those 'enchantment' thingies? Try breaking a couple altars and see what you can make.",
            //    HelpDialogueType.Misc, (name) => Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
        }
    }
}
