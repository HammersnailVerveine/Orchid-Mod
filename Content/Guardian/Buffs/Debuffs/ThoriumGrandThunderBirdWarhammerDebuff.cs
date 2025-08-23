using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs.Debuffs
{
	public class ThoriumGrandThunderBirdWarhammerDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (Main.rand.NextBool(15))
			{
				if (Main.rand.NextBool())
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Teleporter, Scale: 0.5f + Main.rand.NextFloat(0.75f));
					if (Main.rand.NextBool())
					{
						dust.noGravity = true;
						dust.scale *= 1.5f;
					}
				}
				else
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, Scale: 0.4f + Main.rand.NextFloat(0.25f));
					dust.velocity *= 0.5f;
					if (Main.rand.NextBool())
					{
						dust.noGravity = true;
						dust.scale *= 1.5f;
					}
				}
			}
		}
	}
}