using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs.Thorium
{
	public class AquaBump : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Aqua Bump");
			// Description.SetDefault("Landing will hurt");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		// public override void Update(Player player, ref int buffIndex) {
		// if (player.velocity.Y == 0 && player.grappling[0] == 0 && player.jump == 0) {

		// }
		// }

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.velocity.Y == 0)
			{
				npc.SimpleStrikeNPC(50, npc.direction);
				npc.buffTime[buffIndex] = 0;
				buffIndex--;
			}
		}
	}
}
