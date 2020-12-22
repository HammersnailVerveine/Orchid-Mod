using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Alchemist.Buffs.Debuffs
{
    public class SlimeSlow : ModBuff
	{
		public override void SetDefaults() {
			DisplayName.SetDefault("Slime Slow");
			Description.SetDefault("Reduces Movement Speed");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		// public override void Update(Player player, ref int buffIndex) {
			// if (player.velocity.Y == 0 && player.grappling[0] == 0 && player.jump == 0) {
				
			// }
		// }

		public override void Update(NPC npc, ref int buffIndex) {
			npc.velocity.X *= 0.85f;
			bool collide = Collision.TileCollision(npc.position, new Vector2(0f, npc.velocity.Y), npc.width, npc.height, true, false, 1) != new Vector2(0f, npc.velocity.Y);
			if (collide && npc.velocity.Y > 3f) {
				int dmg = (int)(3 * npc.velocity.Y);
				npc.StrikeNPCNoInteraction(dmg > 50 ? 50 : dmg, 0f, 0);
				npc.velocity.Y = - npc.velocity.Y * 0.75f;
			}
		}
	}
}
