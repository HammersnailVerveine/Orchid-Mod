using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
	public class HellHit : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Hellish Recovery");
			Description.SetDefault("Cannot be hit with depths weaver set projectiles");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}
