using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Buffs.Debuffs
{
	public class WardenSpiderDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed -= 0.25f;

			if (Main.rand.NextBool(3))
			{
				Dust.NewDustDirect(player.position, player.width, player.height, DustID.Web).velocity *= 0.25f;
			}
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ShapeshifterGlobalNPC>().WardenSpiderDebuff = true;
			if (npc.knockBackResist > 0f)
			{
				npc.velocity.X *= 0.9f;
			}

			if (Main.rand.NextBool(3))
			{
				Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Web).velocity *= 0.25f;
			}
		}
	}
}