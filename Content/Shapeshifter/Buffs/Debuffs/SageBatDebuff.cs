using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Buffs.Debuffs
{
	public class SageBatDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.CanBeRemovedByNetMessage[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ShapeshifterGlobalNPC>().SageBatDebuff = true;
			Color color = new Color(255, 182, 0) * 0.3f;
			Lighting.AddLight(npc.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}
	}
}