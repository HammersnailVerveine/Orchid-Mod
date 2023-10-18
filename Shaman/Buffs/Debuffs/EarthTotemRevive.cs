using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
	public class EarthTotemRevive : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Earth Totem Revival");
			// Description.SetDefault("Cannot be revived by a shaman totem");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}