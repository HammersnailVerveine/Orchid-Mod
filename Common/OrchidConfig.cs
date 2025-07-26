﻿using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace OrchidMod.Common
{
	public class OrchidClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]
		[DefaultValue(true)]
		public bool ShowClassTags { get; set; }

		[DefaultValue(false)]
		public bool ShowModTags { get; set; }

		[Header("Guardian")]
		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool GuardianUseOldHammerUi { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool GuardianAltChargeSounds { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool GuardianBlockCancelChain { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool GuardianSwapPaviseImputs { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool GuardianSwapGauntletImputs { get; set; }

		[Header("Shapeshifter")]
		[DefaultValue(false)]
		[BackgroundColor(100, 175, 150)]
		public bool ShapeshifterUseHairColor { get; set; }

		[Increment(1)]
		[Range(0, 60)]
		[DefaultValue(10)]
		[BackgroundColor(100, 175, 150)]
		public int ShapeshifterHookDelay { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(100, 175, 150)]
		public bool ShapeshifterHookDashRelease { get; set; }
	}

	public class OrchidServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool LoadCrossmodContentWithoutRequiredMods { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(150, 0, 50)]
		[ReloadRequired]
		public bool EnableContentAlchemist { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(150, 0, 50)]
		[ReloadRequired]
		public bool EnableContentGambler { get; set; }

		[DefaultValue(true)]
		[BackgroundColor(150, 0, 50)]
		[ReloadRequired]
		public bool EnableContentShapeshifter { get; set; }
	}
}