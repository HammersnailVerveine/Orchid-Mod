using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs
{
	public class GuardianGitBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.3f;
		}
	}
}