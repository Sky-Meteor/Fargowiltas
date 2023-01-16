using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Fargowiltas
{
    public sealed class FargoConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("$Mods.Fargowiltas.Config.TownNPCsHeader")]
        [Label("$Mods.Fargowiltas.Config.Mutant")]
        [DefaultValue(true)]
        public bool Mutant;

        [Label("$Mods.Fargowiltas.Config.Abom")]
        [DefaultValue(true)]
        public bool Abom;

        [Label("$Mods.Fargowiltas.Config.Devi")]
        [DefaultValue(true)]
        public bool Devi;

        [Label("$Mods.Fargowiltas.Config.Lumber")]
        [DefaultValue(true)]
        public bool Lumber;

        [Label("$Mods.Fargowiltas.Config.Squirrel")]
        [DefaultValue(true)]
        public bool Squirrel;

        [Label("$Mods.Fargowiltas.Config.CatchNPCs")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CatchNPCs;

        [Label("$Mods.Fargowiltas.Config.NPCSales")]
        [DefaultValue(true)]
        public bool NPCSales;

        [Label("$Mods.Fargowiltas.Config.SaferBoundNPCs")]
        [DefaultValue(true)]
        public bool SaferBoundNPCs;

        [Header("$Mods.Fargowiltas.Config.SeasonsHeader")]
        [Label("$Mods.Fargowiltas.Config.Halloween")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections Halloween;

        [Label("$Mods.Fargowiltas.Config.Christmas")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections Christmas;

        [Header("$Mods.Fargowiltas.Config.SeedsHeader")]
        [Label("$Mods.Fargowiltas.Config.DrunkWorld")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections DrunkWorld;

        [Label("$Mods.Fargowiltas.Config.BeeWorld")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections BeeWorld;

        [Label("$Mods.Fargowiltas.Config.WorthyWorld")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections WorthyWorld;

        [Label("$Mods.Fargowiltas.Config.CelebrationWorld")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections CelebrationWorld;

        [Label("$Mods.Fargowiltas.Config.ConstantWorld")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections ConstantWorld;

        [Header("$Mods.Fargowiltas.Config.UnlimitedHeader")]
        [Label("$Mods.Fargowiltas.Config.UnlimitedAmmo")]
        [DefaultValue(true)]
        public bool UnlimitedAmmo;

        [Label("$Mods.Fargowiltas.Config.UnlimitedConsumableWeapons")]
        [DefaultValue(true)]
        public bool UnlimitedConsumableWeapons;

        [Label("$Mods.Fargowiltas.Config.UnlimitedPotionBuffsOn120")]
        [Tooltip("$Mods.Fargowiltas.Config.UnlimitedPotionBuffsOn120Tooltip")]
        [DefaultValue(true)]
        public bool UnlimitedPotionBuffsOn120;

        private const uint maxExtraBuffSlots = 99;
        [Header("$Mods.Fargowiltas.Config.MiscHeader")]
        [Label("$Mods.Fargowiltas.Config.ExtraBuffSlots")]
        [Range(0, maxExtraBuffSlots)]
        [DefaultValue(22)]
        [ReloadRequired]
        public uint ExtraBuffSlots;

        [Label("$Mods.Fargowiltas.Config.ExpandedTooltips")]
        [DefaultValue(true)]
        public bool ExpandedTooltips;

        [Label("$Mods.Fargowiltas.Config.AnglerQuestInstantReset")]
        [DefaultValue(true)]
        public bool AnglerQuestInstantReset;

        [Label("$Mods.Fargowiltas.Config.ExtraLures")]
        [DefaultValue(true)]
        public bool ExtraLures;

        [Label("$Mods.Fargowiltas.Config.StalkerMoneyTrough")]
        [DefaultValue(true)]
        public bool StalkerMoneyTrough;

        [Label("$Mods.Fargowiltas.Config.RottenEggs")]
        [DefaultValue(true)]
        public bool RottenEggs;

        [Label("$Mods.Fargowiltas.Config.BannerRecipes")]
        [DefaultValue(true)]
        public bool BannerRecipes;

        [Label("$Mods.Fargowiltas.Config.IncreaseMaxStack")]
        [DefaultValue(true)]
        public bool IncreaseMaxStack;

        [Label("$Mods.Fargowiltas.Config.ExtractSpeed")]
        [DefaultValue(true)]
        public bool ExtractSpeed;

        [Label("$Mods.Fargowiltas.Config.Fountains")]
        [DefaultValue(true)]
        public bool Fountains;

        [Label("$Mods.Fargowiltas.Config.BossZen")]
        [DefaultValue(true)]
        public bool BossZen;

        [Label("$Mods.Fargowiltas.Config.PiggyBankAcc")]
        [DefaultValue(true)]
        public bool PiggyBankAcc;

        [Label("$Mods.Fargowiltas.Config.FasterLavaFishing")]
        [DefaultValue(true)]
        public bool FasterLavaFishing;

        [Label("$Mods.Fargowiltas.Config.TorchGodEX")]
        [Tooltip("$Mods.Fargowiltas.Config.TorchGodEXTooltip")]
        [DefaultValue(true)]
        public bool TorchGodEX;

        [Label("$Mods.Fargowiltas.Config.DebuffOpacity")]
        [DefaultValue(1f)]
        [Slider]
        public float DebuffOpacity;

        [Label("$Mods.Fargowiltas.Config.DebuffFaderRatio")]
        [DefaultValue(0.75f)]
        [Slider]
        public float DebuffFaderRatio;

        //[Label("[i:1612] Debuff Display Duration Countdown")]
        //[DefaultValue(true)]
        //public bool DebuffCountdown;

        [Label("$Mods.Fargowiltas.Config.TransparentFriendlyProjectiles")]
        [DefaultValue(1f)]
        [Tooltip("$Mods.Fargowiltas.Config.TransparentFriendlyProjectilesTooltip")]
        [Slider]
        public float TransparentFriendlyProjectiles;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            ExtraBuffSlots = Utils.Clamp<uint>(ExtraBuffSlots, 0, maxExtraBuffSlots);
            TransparentFriendlyProjectiles = Utils.Clamp(TransparentFriendlyProjectiles, 0f, 1f);
        }
    }
}
