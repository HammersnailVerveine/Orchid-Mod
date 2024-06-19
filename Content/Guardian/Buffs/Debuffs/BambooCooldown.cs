using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs.Debuffs
{
	public class BambooCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}