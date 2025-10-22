using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using OrchidMod.Content.Alchemist.Misc;
using OrchidMod.Content.Gambler.Misc;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Buffs.Debuffs;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using OrchidMod.Common.ModSystems;

namespace OrchidMod.Common.Global.NPCs
{
	public class OrchidGlobalNPC : GlobalNPC
	{
		public int GamblerDungeonCardCount = 0;
		public int ShamanBomb = 0;
		public int ShamanShroom = 0;
		public int ShamanSpearDamage = 0;
		public int StarKOOwner = -1;
		public Vector2 ForcedVelocity = Vector2.Zero;

		public bool AlchemistHit = false;
		public bool GamblerHit = false;
		public bool GuardianHit = false;
		public bool ShamanHit = false;
		public bool ShapeshifterHit = false;
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

		public override void PostAI(NPC npc)
		{
			if (npc.HasBuff<HockeyQuarterstaffDebuff>())
			{
				npc.DiscourageDespawn(2);
				if (npc.life < 2000 && ((Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Center - npc.Center) * new Vector2(1f, 1.2f)).Length() > 1000f)
				{
					Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, npc.velocity, ModContent.ProjectileType<StarKO>(), 0, 0, ai0: npc.whoAmI);
					if (StarKOOwner >= 0 && Main.player[StarKOOwner].active && !Main.player[StarKOOwner].dead && Collision.TileCollision(Main.player[StarKOOwner].Center, Vector2.UnitY * 1000, 1, 1) == Vector2.UnitY * 1000) npc.Center = Main.player[StarKOOwner].Center - Vector2.UnitY * 1000;
					if (Main.netMode != NetmodeID.MultiplayerClient) npc.StrikeInstantKill();
				}
				//if (StarKOTimer == 1) StarKOOwner = -1;
				//StarKOTimer--;
			}
		}

		/*public override void DrawEffects(NPC npc, ref Color drawColor)
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
				var dust = Main.dust[Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.BubbleBurst_Blue, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 3.5f)];
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
		}*/

		public override void OnKill(NPC npc)
		{
			if (AlchemistHit && Main.rand.NextBool(10))
			{
				Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<Potency>());
			}

			if (GamblerHit && Main.rand.NextBool(10))
			{
				Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<Chip>());
			}

			if (GuardianHit && !npc.SpawnedFromStatue && OrchidMiscModSystem.SlamDropCooldown >= 300)
			{ // Slam pickups drop logic (every 10 sec, not if there are more than 2 nearby slams)
				OrchidMiscModSystem.SlamDropCooldown = 0;
				int slamType = ModContent.ItemType<Slam>();
				int count = 0;
				foreach (Item item in Main.item)
				{
					if (item.type == slamType && item.Center.Distance(npc.Center) < 160f && item.active)
					{
						count++;
						if (count == 3)
						{
							break;
						}
					}
				}

				if (count < 3)
				{
					Item.NewItem(npc.GetSource_Death(), npc.getRect(), slamType);
				}
			}
		}
	}
}