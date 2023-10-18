using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class JungleRage : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			// DisplayName.SetDefault("Jungle Rage");
			// Description.SetDefault("20% increased shamanic damage");
		}
	}
}