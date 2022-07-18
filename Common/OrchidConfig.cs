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
		[Label("Class Tags")]
		[Tooltip("$Enables/Disables the display of Orchid Mod Class tags.")]
		public bool ShowClassTags { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		[Label("Cross-Mod Tags?")]
		[Tooltip("$Enables/Disables the display of Orchid Mod Cross-Content tags.")]
		public bool LoadCrossmodContentWithoutRequiredMods { get; set; }
	}
}