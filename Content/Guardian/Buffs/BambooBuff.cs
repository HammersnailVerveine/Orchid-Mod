using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs
{
	public class BambooBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense *= 1.5f;
		}
	}
}