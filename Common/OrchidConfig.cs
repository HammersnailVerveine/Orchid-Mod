using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace OrchidMod.Common
{
	public class OrchidClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]
		[BackgroundColor(128, 255, 128)]

		[DefaultValue(true)]
		public bool ShowClassTags { get; set; }

		[DefaultValue(false)]
		public bool ShowModTags { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool UseOldGuardianHammerUi { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool AltGuardianChargeSounds { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool BlockCancelChain { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool SwapPaviseImputs { get; set; }

		[DefaultValue(false)]
		[BackgroundColor(151, 120, 79)]
		public bool SwapGauntletImputs { get; set; }
	}

	public class OrchidServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool LoadCrossmodContentWithoutRequiredMods { get; set; }
	}
}