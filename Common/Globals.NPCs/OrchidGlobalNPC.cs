using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using OrchidMod.Content.Gambler.UI;

namespace OrchidMod.Common.Globals.NPCs
{
	public class OrchidGlobalNPC : GlobalNPC
	{
		public int GamblerDungeonCardCount = 0;
		public int ShamanBomb = 0;
		public int ShamanShroom = 0;
		public int ShamanSpearDamage = 0;

		public bool AlchemistHit = false;
		public bool GamblerHit = false;
		public bool ShamanWater = false;
		public bool ShamanWind = false;

		// ...

		public override bool InstancePerEntity => true;

		public override void ResetEffects(NPC npc)
		{
			ShamanBomb -= ShamanBomb > 0 ? 1 : 0;
			ShamanShroom -= ShamanShroom > 0 ? 1 : 0;
			ShamanSpearDamage -= ShamanSpearDamage > 0 ? 1 : 0;
			ShamanWind = false;
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
		{
			if (ShamanWater) modifiers.FinalDamage *= 1.05f;
			if (ShamanShroom > 0) modifiers.FinalDamage *= 1.1f;
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (ShamanBomb > 0)
			{
				if (Main.rand.NextBool(15))
				{
					var dust = Main.dust[Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Torch, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 3.5f)];
					dust.noGravity = true;
					dust.velocity *= 1.8f;
					dust.velocity.Y -= 0.5f;

					if (Main.rand.NextBool(4))
					{
						dust.noGravity = false;
						dust.scale *= 0.5f;
					}
				}

				Lighting.AddLight(npc.position, 0.5f, 0.2f, 0f);

				if (ShamanBomb == 1)
				{
					npc.SimpleStrikeNPC(500, npc.direction);

					for (int i = 0; i < 15; i++)
					{
						var dust = Main.dust[Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 6, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 3.5f)];
						dust.noGravity = true;
						dust.velocity *= 5f;
						dust.scale *= 1.5f;
					}

					SoundEngine.PlaySound(SoundID.Item, npc.Center); // Before 1.4 it was (2, pos, 45)
				}
			}

			if (ShamanShroom > 0 && Main.rand.NextBool(15))
			{
				var dust = Main.dust[Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 176, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 3.5f)];
				dust.noGravity = true;
				dust.velocity *= 1f;
				dust.scale *= 0.5f;
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (ShamanWind)
			{
				if (npc.lifeRegen > 0) npc.lifeRegen = 0;

				npc.lifeRegen -= 16;

				if (damage < 20) damage = 20;
			}
		}

		public override void OnKill(NPC npc)
		{
			if (this.AlchemistHit && Main.rand.NextBool(4))
			{
				Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<Content.Alchemist.Misc.Potency>());
			}
			if (this.GamblerHit && Main.rand.NextBool(4))
			{
				Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<Content.Gambler.Misc.Chip>());
			}
		}
	}
}