using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.General.Buffs.Debuffs
{
	public class ToolSetBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Miner's Demise");
			Description.SetDefault("Trap damage will not be reduced");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}