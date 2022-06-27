using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace OrchidMod.Common
{
	public class OrchidConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("General")]

		[DefaultValue(true)]
		[Label("Tags?")]
		[Tooltip("$Help, my english is bed")]
		public bool ShowClassTags { get; set; }
	}
}