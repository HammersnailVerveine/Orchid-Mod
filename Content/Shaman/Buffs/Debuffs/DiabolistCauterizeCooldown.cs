using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs.Debuffs
{
	public class DiabolistCauterizeCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Cauterized");
			// Description.SetDefault("Cannot trigger a diabolist cauterization again");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}