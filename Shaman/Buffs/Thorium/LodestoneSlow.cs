using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Thorium
{
	public class LodestoneSlow : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lodestone Slow");
			Description.SetDefault("You feel Heavier");
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
			if (npc.velocity.Y != 0)
			{
				npc.velocity.Y += 0.06f;
			}
			npc.velocity.X *= 0.85f;
		}
	}
}
