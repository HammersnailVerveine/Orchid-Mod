using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using OrchidMod;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public class AlchemistHiddenReactionHelper
	{
		public static void triggerAlchemistReaction(Mod mod, Player player, OrchidModPlayer modPlayer) {
			bool reaction = false;
			string floatingTextStr = "Failed reaction ...";
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingMushroomVial>(), player, modPlayer, mod) 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.KingSlimeFlask>(), player, modPlayer, mod)) {
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(25, true);
				player.statLife += 25;	
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 56);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Healing !";
			}
			
			if (modPlayer.alchemistNbElements == 2
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.FireblossomFlask>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod))
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>(), player, modPlayer, mod))) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 6);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				int dmg = OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod) ? (int)((14) * modPlayer.alchemistDamage) : (int)((22) * modPlayer.alchemistDamage); 
				for (int i = 0 ; i < 10 ; i ++) {
					Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>();
					int newSpore = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
					Main.projectile[newSpore].localAI[1] = 1f;
				}
				int nb = 4 + Main.rand.Next(3);
				for (int i = 0 ; i < nb ; i ++) {
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
				if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>(), player, modPlayer, mod)) {
					for (int i = 0 ; i < 5 ; i ++) {
						Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
						int newSpore = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
						Main.projectile[newSpore].localAI[1] = 1f;
					}
					for (int i = 0 ; i < 2 ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
				}
				floatingTextStr = "Fire Spores !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.WaterleafFlask>(), player, modPlayer, mod))
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>(), player, modPlayer, mod))) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 33);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				int dmg = (int)((10) * modPlayer.alchemistDamage); 
				for (int i = 0 ; i < 10 ; i ++) {
					Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>();
					int newSpore = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
					Main.projectile[newSpore].localAI[1] = 1f;
				}
				int nb = 4 + Main.rand.Next(3);
				for (int i = 0 ; i < nb ; i ++) {
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
				if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>(), player, modPlayer, mod)) {
					for (int i = 0 ; i < 5 ; i ++) {
						Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
						int newSpore = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
						Main.projectile[newSpore].localAI[1] = 1f;
					}
					for (int i = 0 ; i < 2 ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
				}
				floatingTextStr = "Water Spores !";
			}
			
			if (modPlayer.alchemistNbElements == 2
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.DeathweedFlask>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.ShiverthornFlask>(), player, modPlayer, mod))
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>(), player, modPlayer, mod))) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 16);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				int dmg = OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.DeathweedFlask>(), player, modPlayer, mod) ? (int)((15) * modPlayer.alchemistDamage) : (int)((10) * modPlayer.alchemistDamage); 
				for (int i = 0 ; i < 10 ; i ++) {
					Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>();
					int newSpore = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
					Main.projectile[newSpore].localAI[1] = 1f;
				}
				int nb = 4 + Main.rand.Next(3);
				for (int i = 0 ; i < nb ; i ++) {
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
				if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>(), player, modPlayer, mod)) {
					for (int i = 0 ; i < 5 ; i ++) {
						Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
						int newSpore = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
						Main.projectile[newSpore].localAI[1] = 1f;
					}
					for (int i = 0 ; i < 2 ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
				}
				floatingTextStr = "Air Spores !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.GlowingMushroomVial>(), player, modPlayer, mod)
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 5);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 56);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				player.QuickSpawnItem(mod.ItemType("MushroomThread"), 1);
				floatingTextStr = "Mushroom Thread !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.ShiverthornFlask>(), player, modPlayer, mod)
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.WaterleafFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff(109, 60 * 30); // Flipper
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 6);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Flipper !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.FireblossomFlask>(), player, modPlayer, mod) 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.WaterleafFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff(1, 60 * 30); // Obsidian Skin
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 33);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Flipper !";
			}
			
			if (modPlayer.alchemistNbElements == 3 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.DaybloomFlask>(), player, modPlayer, mod) 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod)
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))) {
				reaction = true;
				player.AddBuff(8, 60 * 30); // Featherfall
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 16);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Featherfall !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.MoonglowFlask>(), player, modPlayer, mod)
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff(10, 60 * 30); // Invisibility
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 15);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Invisiblity !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.DaybloomFlask>(), player, modPlayer, mod) 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff(12, 60 * 30); // Night Owl
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 15);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Night Owl !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.GunpowderFlask>(), player, modPlayer, mod)
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))) {
				reaction = true;
				player.jump = 1;
				player.velocity.Y = -15f;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 10);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 15);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				for(int i=0; i < 15; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 37);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.2f;
				}
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 14);
				floatingTextStr = "Propulsion !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.KingSlimeFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				for(int i=0; i < 10; i++)
				{
					int Alpha = 175;
					Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
					int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
				
				int dmg = (int)(12 * modPlayer.alchemistDamage);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubble>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, proj, dmg, 0f, player.whoAmI);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Slime Bubble !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.LivingSapVial>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				for(int i=0; i < 10; i++)
				{
					int Alpha = 175;
					Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
					int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
				
				int dmg = (int)(15 * modPlayer.alchemistDamage);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.LivingSapBubble>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, proj, dmg, 0f, player.whoAmI);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Sap Bubble !";
			}
			
			if (modPlayer.alchemistNbElements == 3 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>(), player, modPlayer, mod)
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.ShiverthornFlask>(), player, modPlayer, mod)
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.MoonglowFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff(107, 60 * 30); // Builder
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 30);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 25);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 15);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				floatingTextStr = "Builder !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.SeafoamVial>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				for(int i=0; i < 10; i++)
				{
					int Alpha = 175;
					Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
					int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
				
				int dmg = (int)(12 * modPlayer.alchemistDamage);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SeafoamBubble>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, proj, dmg, 0f, player.whoAmI);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Seafoam Bubble !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.HellOil>(), player, modPlayer, mod))) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 15);
				for(int i=0; i < 10; i++)
				{
					int Alpha = 175;
					Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
					int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
				
				int dmg = 0;
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, proj, dmg, 0f, player.whoAmI);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Oil Bubble !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.QueenBeeFlask>(), player, modPlayer, mod)
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.PoisonVial>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 25);
				
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 97);
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 16);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				int dmg = (int)((10) * modPlayer.alchemistDamage); 
				for (int i = 0 ; i < 10 ; i ++) {
					Vector2 vel = ( new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
					if (player.strongBees && Main.rand.Next(2) == 0) 
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 566, (int) (dmg * 1.15f), 0f, player.whoAmI);
					else {
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 181, dmg, 0f, player.whoAmI);
					}
				}
				for (int i = 0 ; i < 10 ; i ++) {
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
				floatingTextStr = "Bee Swarm !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.PoisonVial>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				for(int i=0; i < 10; i++)
				{
					int Alpha = 175;
					Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
					int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
				
				int dmg = (int)(18 * modPlayer.alchemistDamage);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.PoisonBubble>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, proj, dmg, 0f, player.whoAmI);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Poison Bubble !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.CouldInAVial>(), player, modPlayer, mod) || OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Air.FartInAVial>(), player, modPlayer, mod))
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.DungeonFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 20);
				for(int i=0; i < 10; i++)
				{
					int Alpha = 175;
					Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
					int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
				
				int dmg = (int)(18 * modPlayer.alchemistDamage);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SpiritedBubble>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, proj, dmg, 0f, player.whoAmI);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Spirited Bubble !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.SeafoamVial>(), player, modPlayer, mod) 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.PoisonVial>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 15);
				
				int dmg = (int)(18 * modPlayer.alchemistDamage);
				for (int i = 0 ; i < 7 ; i ++) {
					int proj = Main.rand.Next(2) == 0 ? ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProj>() : ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProj>();
					Projectile.NewProjectile(player.Center.X - 120 + i * 40, player.Center.Y, 0f, -(float)(3 + Main.rand.Next(4)) * 0.5f, proj, dmg, 0f, player.whoAmI);
				}
				
				for (int i = 0 ; i < 11 ; i ++) {
					Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(10)));
					int proj = Main.rand.Next(2) == 0 ? ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProjAlt>() : ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProjAlt>();
					Projectile.NewProjectile(player.Center.X - 150 + i * 30, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				}
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Bubbles !";
			}
			
			if (modPlayer.alchemistNbElements == 2 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Water.SlimeFlask>(), player, modPlayer, mod) 
			&& OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<Alchemist.Weapons.Nature.SunflowerFlask>(), player, modPlayer, mod)) {
				reaction = true;
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 15);
				
				int dmg = (int)(8 * modPlayer.alchemistDamage);
				for (int i = 0 ; i < 5 ; i ++) {
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(10)).RotatedBy(MathHelper.ToRadians(- 40 + (20 * i))));
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>(), dmg, 0f, player.whoAmI);
				}
				
				int nb = 5 + Main.rand.Next(4);
				for (int i = 0 ; i < nb ; i ++) {
					Vector2	vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
				floatingTextStr = "Sunflower Seeds !";
			}
			
			// end of reactions //
			
			Color floatingTextColor = reaction ? new Color(128, 255, 0) : new Color(255, 0, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);
			if (reaction == false) {
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 5);
				for(int i=0; i < 15; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 37);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.2f;
				}
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 6);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 16);
			}
			
			int colorRed = modPlayer.alchemistColorR;
			int colorGreen = modPlayer.alchemistColorG;
			int colorBlue = modPlayer.alchemistColorB;
			
			int rand = 2 + Main.rand.Next(3);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = 1 + Main.rand.Next(3);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = Main.rand.Next(2);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			
			OrchidModAlchemistHelper.clearAlchemistDusts(player, modPlayer, mod);
			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
			modPlayer.alchemistSelectUIDisplay = false;
		}
	}
}