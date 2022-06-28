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
		[Label("Tags?")]
		[Tooltip("$Help, my english is bed")]
		public bool ShowClassTags { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		[Label("Crossmod?")]
		[Tooltip("$Help, my english is bed")]
		public bool LoadCrossmodContentWithoutRequiredMods { get; set; }
	}
}