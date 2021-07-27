using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Projectiles
{
	public class StaticQuartzHealerPro : ModProjectile
	{
		Mod thoriumMod = OrchidMod.ThoriumMod;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Scythe");
		}

		public override void SetDefaults()
		{
			projectile.width = 100;
			projectile.height = 100;
			projectile.aiStyle = 0;
			projectile.penetrate = 100;
			projectile.light = 0.2f;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ownerHitCheck = true;
			projectile.ignoreWater = true;
			projectile.timeLeft = 26;
		}

		public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
			npc.immune[projectile.owner] = 10;
			
			//TODO thorium
			if (thoriumMod != null) {
				if (projectile.penetrate > 99 && npc.CanBeChasedBy())
				{
					player.AddBuff(thoriumMod.BuffType("SoulEssence"), 1800, true);
					CombatText.NewText(npc.Hitbox, new Color(100, 255, 200), 1, false, true);
					
					FieldInfo fieldSoul = thoriumPlayer.GetType().GetField("soulEssence", BindingFlags.Public | BindingFlags.Instance);
					if (fieldSoul != null) {
						
						int healCharge = (int)(fieldSoul.GetValue(thoriumPlayer)) + 1;
						fieldSoul.SetValue(thoriumPlayer, healCharge);
					}
				}
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			hitDirection = target.Center.X < player.Center.X ? -1 : 1;
			
			//TODO thorium
			if (thoriumMod != null)
			{
				ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
				Type playerType = thoriumPlayer.GetType();
				FieldInfo critField = playerType.GetField("radiantCrit", BindingFlags.Public | BindingFlags.Instance);
				if (critField != null)
				{
					int healCrit = (int)critField.GetValue(thoriumPlayer);
					crit = Main.rand.Next(101) <= healCrit;
				}
				else
				{
					crit = false;
				}

				FieldInfo fieldWarlock = playerType.GetField("warlockSet", BindingFlags.Public | BindingFlags.Instance);
				if (fieldWarlock != null)
				{
					bool healWarlock = (bool)fieldWarlock.GetValue(thoriumPlayer);

					if (healWarlock && Main.rand.NextFloat() < 0.5f)
					{
						int shadowWispType = thoriumMod.ProjectileType("ShadowWisp");
						if (player.ownedProjectileCounts[shadowWispType] < 15)
						{
							Projectile.NewProjectile((int)target.Center.X, (int)target.Center.Y, 0f, -2f, shadowWispType, (int)(projectile.damage * 0.5f), 0, Main.myPlayer);
						}
					}
				}
				
				FieldInfo fieldIridescent = playerType.GetField("iridescentSet", BindingFlags.Public | BindingFlags.Instance);
				if (fieldIridescent != null)
				{
					bool healIridescent = (bool)fieldIridescent.GetValue(thoriumPlayer);

					if (healIridescent && Main.rand.NextFloat() < 0.15f)
					{
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 100, 1f, 0f);
						for (int k = 0; k < 20; k++)
						{
							int dust = Dust.NewDust(target.position, target.width, target.height, 87, Main.rand.Next((int)-6f, (int)6f), Main.rand.Next((int)-6f, (int)6f), 0, default(Color), 1.25f);
							Main.dust[dust].noGravity = true;
						}
						for (int k = 0; k < 10; k++)
						{
							int dust = Dust.NewDust(target.position, target.width, target.height, 91, Main.rand.Next((int)-2f, (int)2f), Main.rand.Next((int)-2f, (int)2f), 0, default(Color), 1.15f);
							Main.dust[dust].noGravity = true;
						}

						int healNoEffects = thoriumMod.ProjectileType("HealNoEffects");
						int heal = 0;
						if (target.type != NPCID.TargetDummy)
						{
							for (int k = 0; k < Main.maxPlayers; k++)
							{
								Player ally = Main.player[k];
								if (ally.active && ally != player && ally.statLife < ally.statLifeMax2 && ally.DistanceSQ(projectile.Center) < 500 * 500)
								{
									Projectile.NewProjectile(ally.Center, Vector2.Zero, healNoEffects, 0, 0, projectile.owner, heal, ally.whoAmI);
								}
							}
						}

						for (int u = 0; u < Main.maxNPCs; u++)
						{
							NPC enemyTarget = Main.npc[u];
							if (enemyTarget.CanBeChasedBy() && enemyTarget.DistanceSQ(player.Center) < 250 * 250)
							{
								enemyTarget.AddBuff(BuffID.Confused, 120, false);
							}
						}
					}
				}
			}
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			
			if (projectile.timeLeft < 10)
			{
				projectile.alpha += 30;
			}

			if (player.dead)
			{
				projectile.Kill();
			}

			int dir = (player.direction > 0).ToDirectionInt();
			projectile.rotation += dir * 0.25f;
			projectile.spriteDirection = dir;

			player.heldProj = projectile.whoAmI;
			projectile.Center = player.Center;
			projectile.gfxOffY = player.gfxOffY;
		}
	}
}
