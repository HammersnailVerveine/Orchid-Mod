using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs.Debuffs
{
	public class FlashFreeze : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Frozen");
			Description.SetDefault("Reduced Movement Speed");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity *= (1f - ((npc.knockBackResist) * 0.3f));
		}
	}
}
