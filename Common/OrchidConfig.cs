using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace OrchidMod.Common
{
	public class OrchidConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public override bool Autoload(ref string name) => true;

		[Header("General")]

		[DefaultValue(true)]
		[Label("Show tags")]
		[Tooltip("$Enables/Disables the display сlass tags")]
		public bool ShowClassTags { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		[Label("Load Crossmod-Content without required mods")]
		[Tooltip("$Enables/Disables loading Crossmod-Content without required mods")]
		public bool LoadCrossmodContentWithoutRequiredMods { get; set; }
	}
}