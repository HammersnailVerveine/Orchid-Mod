using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
	public class LeafSlow : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Leaf Slow");
			// Description.SetDefault("Reduced Movement Speed");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity *= 0.85f;
		}
	}
}
