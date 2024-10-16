using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Buffs.Debuffs
{
	public class SageOwlDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 10;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ShapeshifterGlobalNPC>().SageOwlDebuff = true;
			Color color = Color.Aqua * 0.15f;
			Lighting.AddLight(npc.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}
	}
}