using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class SporeEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Spore Empowerment");
			// Description.SetDefault("Empowers the spore caller next use");
			Main.buffNoSave[Type] = true;
		}
	}
}