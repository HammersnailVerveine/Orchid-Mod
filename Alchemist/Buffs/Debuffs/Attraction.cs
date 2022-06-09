using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs.Debuffs
{
	public class Attraction : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Attraction");
			Description.SetDefault("Attracts nearby alchemist projectiles");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}
