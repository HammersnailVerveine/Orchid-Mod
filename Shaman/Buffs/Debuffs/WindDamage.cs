using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
	public class WindDamage : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wind Damage");
			Description.SetDefault("Kills");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity *= 0.05f;
			npc.GetGlobalNPC<OrchidGlobalNPC>().shamanWind = true;
		}
	}
}
