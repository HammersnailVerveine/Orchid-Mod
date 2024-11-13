using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs.Debuffs
{
	public class HockeyQuarterstaffDebuff : ModBuff
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
			foreach (NPC otherNPC in Main.npc)
			{
				if (npc.Center.Distance(otherNPC.Center) < (Math.Max(otherNPC.width, otherNPC.height) * 0.5f + Math.Max(otherNPC.width, otherNPC.height) * 0.5f + 16f) && npc != otherNPC && OrchidModProjectile.IsValidTarget(otherNPC))
				{
					Player player = npc.HasValidTarget ? Main.player[npc.target] : null;
					if (player == null)
					{ // Literally use any player, this does not matter
						foreach(Player pl in Main.player)
						{
							if (pl.active)
							{
								player = pl;
								break;
							}
						}
					}

					npc.velocity *= 0.5f;
					player.ApplyDamageToNPC(otherNPC, 500, npc.velocity.Length(), npc.velocity.X > 0 ? 1 : -1, false, ModContent.GetInstance<GuardianDamageClass>(), true);
					player.ApplyDamageToNPC(npc, 500, 0f, 1, false, ModContent.GetInstance<GuardianDamageClass>(), true);
					npc.RequestBuffRemoval(ModContent.BuffType<HockeyQuarterstaffDebuff>());
				}
			}

			if (npc.velocity.Length() < 8f)
			{
				npc.RequestBuffRemoval(ModContent.BuffType<HockeyQuarterstaffDebuff>());
			}
			else
			{
				Vector2 finalVelocity = Vector2.Zero;
				Vector2 intendedVelocity = npc.velocity / 10f;
				for (int i = 0; i < 10; i++)
				{
					finalVelocity += Collision.TileCollision(npc.position + finalVelocity, intendedVelocity, npc.width, npc.height);
				}

				if ((finalVelocity - npc.velocity).Length() > 8f)
				{
					Player player = npc.HasValidTarget ? Main.player[npc.target] : null;

					if (player == null)
					{ // Literally use any player, this does not matter
						foreach (Player pl in Main.player)
						{
							if (pl.active)
							{
								player = pl;
								break;
							}
						}
					}

					npc.velocity *= 0f;
					player.ApplyDamageToNPC(npc, 500, 0f, 1, false, ModContent.GetInstance<GuardianDamageClass>(), true);
					npc.RequestBuffRemoval(ModContent.BuffType<HockeyQuarterstaffDebuff>());
				}
			}
		}
	}
}