using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace OrchidMod.Common
{
	public class OrchidClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]

		[DefaultValue(true)]
		[Label("Show tags")]
		[Tooltip("$Enables/Disables the display class tags")]
		public bool ShowClassTags { get; set; }
	}

	public class OrchidServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]
		[DefaultValue(true)]
		[ReloadRequired]
		[Label("Load Crossmod-Content without required mods")]
		[Tooltip("$Enables/Disables loading Crossmod-Content without required mods")]
		public bool LoadCrossmodContentWithoutRequiredMods { get; set; }
	}
}