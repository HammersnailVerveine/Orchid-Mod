using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class JungleRage : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			DisplayName.SetDefault("Jungle Rage");
			Description.SetDefault("Effectiveness of your shamanic fire and earth bonds greatly increased");
		}
	}
}