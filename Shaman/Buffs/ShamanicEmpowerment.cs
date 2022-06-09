using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class ShamanicEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shamanic Accuracy");
			Description.SetDefault("The source of your shamanic attacks will stay in position better");
		}
	}
}