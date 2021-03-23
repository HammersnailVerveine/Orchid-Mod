using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler
{
	public class GamblerAttackHelper
	{
		public static void shootBonusProjectiles(Player player, Vector2 position, int cardType, bool dummy = false) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			if (modPlayer.gamblerSlimyLollipop) {
				OrchidModGlobalItem orchidItem = modPlayer.gamblerCardCurrent.GetGlobalItem<OrchidModGlobalItem>();
				if (orchidItem.gamblerCardSets.Contains("Slime") && Main.rand.Next(180) == 0) {
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					int rand = Main.rand.Next(3) + 1;
					int projType = ProjectileType<Gambler.Projectiles.SlimeRainCardProj2>();
					for (int i = 0; i < rand; i++) {
						Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
						Vector2 heading = target - player.position;
						heading.Normalize();
						heading *= new Vector2(0f, 5f).Length();
						Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(30));
						vel = vel * scale; 
						int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, 15, 0f, player.whoAmI), dummy);
						Main.projectile[newProjectile].ai[1] = 1f;
						Main.projectile[newProjectile].netUpdate = true;
					}
				}
			}
			
			for (int l = 0; l < Main.projectile.Length; l++)
			{  
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.owner == player.whoAmI) {
					bool dummyProj = proj.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					if (dummyProj == dummy) {
						if (proj.type == ProjectileType<Gambler.Projectiles.ForestCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 30;
								float scale = 1f - (Main.rand.NextFloat() * .3f);
								int rand = Main.rand.Next(3) + 1;
								int projType = ProjectileType<Gambler.Projectiles.ForestCardProj>();
								for (int i = 0; i < rand; i++) {
									Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
									Vector2 heading = target - proj.position;
									heading.Normalize();
									heading *= new Vector2(0f, 10f).Length();
									Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(30));
									vel = vel * scale; 
									int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
									Main.projectile[newProjectile].ai[1] = 1f;
									Main.projectile[newProjectile].netUpdate = true;
									if (i == 0) {
										OrchidModProjectile.spawnDustCircle(proj.Center, 31, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);	
									}
								}
							}
						}
						
						if (proj.type == ProjectileType<Gambler.Projectiles.JungleCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 30;
								int projType = ProjectileType<Gambler.Projectiles.JungleCardProj>();
								Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
								Vector2 heading = target - proj.position;
								heading.Normalize();
								heading *= new Vector2(0f, 10f).Length();
								Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(15));
								int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
								Main.projectile[newProjectile].localAI[1] = 1f;
								Main.projectile[newProjectile].netUpdate = true;
								OrchidModProjectile.spawnDustCircle(proj.Center - new Vector2(4, 4), 44, 10, 4, false, 1f, 1.5f, 5f, true, true, false, 0, 0, true);
							}
						}
						
						if (proj.type == ProjectileType<Gambler.Projectiles.DesertCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 10;
								float scale = 1f - (Main.rand.NextFloat() * .3f);
								int projType = ProjectileType<Gambler.Projectiles.DesertCardProj>();
								Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
								Vector2 heading = target - proj.position;
								heading.Normalize();
								heading *= new Vector2(0f, 8f).Length();
								Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(20));
								vel = vel * scale; 
								int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
								Main.projectile[newProjectile].ai[1] = 1f;
								Main.projectile[newProjectile].netUpdate = true;
								OrchidModProjectile.spawnDustCircle(proj.Center, 31, 5, 4, true, 1f, 1f, 5f, true, true, false, 0, 0, true);
							}
						}
						
						if (proj.type == ProjectileType<Gambler.Projectiles.OceanCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 50;
								int projType = ProjectileType<Gambler.Projectiles.OceanCardProj>();
								Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
								Vector2 heading = target - proj.position;
								heading.Normalize();
								heading *= new Vector2(0f, 5f).Length();
								int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, heading.X, heading.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
								Main.projectile[newProjectile].ai[1] = 1f;
								Main.projectile[newProjectile].netUpdate = true;
								OrchidModProjectile.spawnDustCircle(proj.Center, 31, 10, 10, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
							}
						}
						
						if (proj.type == ProjectileType<Gambler.Projectiles.HellCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 30;
								int projType = ProjectileType<Gambler.Projectiles.HellCardProj>();
								Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
								Vector2 heading = target - proj.position;
								heading.Normalize();
								heading *= new Vector2(0f, 15f).Length();
								int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, heading.X, heading.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
								Main.projectile[newProjectile].ai[1] = 1f;
								Main.projectile[newProjectile].netUpdate = true;
								OrchidModProjectile.spawnDustCircle(proj.Center, 6, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
							}
						}
						
						if (proj.type == ProjectileType<Gambler.Projectiles.MushroomCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 30;
								int projType = ProjectileType<Gambler.Projectiles.MushroomCardProj>();
								Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
								Vector2 heading = target - proj.position;
								heading.Normalize();
								heading *= new Vector2(0f, 10f).Length();
								int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, heading.X, heading.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
								Main.projectile[newProjectile].ai[1] = 1f;
								Main.projectile[newProjectile].netUpdate = true;
								OrchidModProjectile.spawnDustCircle(proj.Center, 172, 25, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
							}
						}
						
						if (proj.type == ProjectileType<Gambler.Projectiles.SnowCardProjAlt>())
						{
							OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
							if (modProjectile.gamblerInternalCooldown == 0) {
								modProjectile.gamblerInternalCooldown = 40;
								int projType = ProjectileType<Gambler.Projectiles.SnowCardProj>();
								Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
								Vector2 vel = new Vector2(0f, 0f);
								float absX = (float)Math.Sqrt((proj.Center.X - target.X) * (proj.Center.X - target.X));
								float absY = (float)Math.Sqrt((proj.Center.Y - target.Y) * (proj.Center.Y - target.Y));
								if (absX > absY) {
									vel.X = target.X < proj.Center.X ? 1f : -1f;
								} else {
									vel.Y = target.Y < proj.Center.Y ? 1f : -1f;
								}
								vel.Normalize();
								vel *= new Vector2(0f, 5f).Length();
								int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
								Main.projectile[newProjectile].ai[1] = 1f;
								Main.projectile[newProjectile].netUpdate = true;
								OrchidModProjectile.spawnDustCircle(proj.Center, 31, 25, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
							}
						}
					}
				}
			}	
		}
	}
}