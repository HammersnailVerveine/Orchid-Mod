using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Thorium
{
	public class OpalEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Opal Empowerment");
			// Description.SetDefault("Shamanic critical strike damage increased by 5");
		}
	}
}