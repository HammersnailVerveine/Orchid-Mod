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
		public static int DummyProjectile(int proj, bool dummy) {
			if (dummy) {
				OrchidModGlobalProjectile modProjectile = Main.projectile[proj].GetGlobalProjectile<OrchidModGlobalProjectile>();
				modProjectile.gamblerDummyProj = true;
			}
			return proj;
		}
		
		public static void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, int cardType, bool dummy = false) {
			//shootBonusProjectiles(player, position, cardType);
			if (cardType == ItemType<Gambler.Weapons.Cards.ForestCard>()) {
				int rand = Main.rand.Next(3) + 1;
				int projType = ProjectileType<Gambler.Projectiles.ForestCardProj>();
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				for (int i = 0; i < rand; i++) {
					Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
					vel = vel * scale; 
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				}
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.HealingPotionCard>() && !dummy) {
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				modPlayer.gamblerShuffleCooldown -= (int)(modPlayer.gamblerShuffleCooldownMax / 5);
				if (modPlayer.gamblerShuffleCooldown < 0) modPlayer.gamblerShuffleCooldown = 0;
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(4, true);
				player.statLife += 4;	
				return;
			}
				
			if (cardType == ItemType<Gambler.Weapons.Cards.SnowCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.SnowCardProj>();
				Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
				Vector2 vel = new Vector2(0f, 0f);
				
				float absX = (float)Math.Sqrt((player.Center.X - target.X) * (player.Center.X - target.X));
				float absY = (float)Math.Sqrt((player.Center.Y - target.Y) * (player.Center.Y - target.Y));
				if (absX > absY) {
					vel.X = target.X < player.Center.X ? 1f : -1f;
				} else {
					vel.Y = target.Y < player.Center.Y ? 1f : -1f;
				}
				
				vel.Normalize();
				vel *= new Vector2(speedX, speedY).Length();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.HellCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.HellCardProj>();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.OceanCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.OceanCardProj>();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.SlimeCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.SlimeCardProj>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.KingSlimeCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.KingSlimeCardProj>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.EyeCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.EyeCardProj>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.QueenBeeCard>()) {
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				if (Main.mouseLeft && Main.mouseLeftRelease || modPlayer.gamblerJustSwitched) {
					int projType = ProjectileType<Gambler.Projectiles.QueenBeeCardProj>();
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
						{
							proj.Kill();
							break;
						} 
					}
					modPlayer.gamblerJustSwitched = false;
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.SlimeRainCard>()) {
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				if (Main.mouseLeft && Main.mouseLeftRelease || modPlayer.gamblerJustSwitched) {
					int projType = ProjectileType<Gambler.Projectiles.SlimeRainCardProj1>();
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
						{
							proj.Kill();
							break;
						} 
					}
					modPlayer.gamblerJustSwitched = false;
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.SapCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.SapCardProj>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.BrainCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.BrainCardProj>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					for (int i = 0; i < 3 ; i ++) {	
						int newProj = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
						Main.projectile[newProj].ai[1] = (float)(i);
						Main.projectile[newProj].ai[0] = (float)(i == 0 ? 300 : 0);
						Main.projectile[newProj].friendly = i == 0;
						Main.projectile[newProj].netUpdate = true;
					}
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.EaterCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.EaterCardProj1>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.SkeletronCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.SkeletronCardProj>();
				bool found = false;
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						found = true;
						break;
					} 
				}
				if (!found) {
					GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				} else {
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				}
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.IceChestCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.IceChestCardProj>();
				Vector2 newPos = Main.screenPosition + new Vector2((float)Main.mouseX - 8, (float)Main.mouseY);
				Vector2 offSet = new Vector2(0f, -15f);
				for (int i = 0; i < 50; i ++) {
					offSet = Collision.TileCollision(newPos, offSet, 14, 32, true, false, (int) player.gravDir);
					newPos += offSet;
					if (offSet.Y > -15f) {
						break;
					}
				}
				newPos.Y = player.position.Y - newPos.Y > Main.screenHeight / 2 ? player.position.Y - Main.screenHeight / 2 : newPos.Y;
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(newPos.X, newPos.Y, 0f, 12.5f, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 30);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.MushroomCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.MushroomCardProj>();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.ShuffleCard>()) {
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				int projType = 0;
				int rand = Main.rand.Next(4);
				switch (rand) {
					case 0:
						projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj1>();
						break;
					case 1:
						projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj2>();
						break;
					case 2:
						projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj3>();
						break;
					default:
						projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj4>();
						break;
				}
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
				vel = vel * scale; 
				int newProj = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.projectile[newProj].damage += OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) * 2;
				Main.projectile[newProj].damage += rand == 3 ? 2 : 0;
				Main.projectile[newProj].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(position + vel * 2f, rand < 2 ? 60 : 63, 5, 10, true, 1.5f, 0.5f, 3f);
				OrchidModProjectile.spawnDustCircle(position + vel * 5f, rand < 2 ? 60 : 63, 3, 5, true, 1.5f, 0.5f, 3f);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.BubbleCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.BubbleCardProj>();
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				Vector2 vel = new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(30));
				vel = vel * scale; 
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 86);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.GoldChestCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.GoldChestCardProj>();
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
				vel = vel * scale; 
				int newProj = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.projectile[newProj].ai[1] = Main.rand.Next(4);
				Main.projectile[newProj].netUpdate = true;
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 9);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.DesertCard>()) {
				int projType = ProjectileType<Gambler.Projectiles.DesertCardProj>();
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				vel = vel * scale; 
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.JungleCard>()) {
				Vector2 vel = new Vector2(speedX, speedY / 5f).RotatedByRandom(MathHelper.ToRadians(15));
				int projType = ProjectileType<Gambler.Projectiles.JungleCardProj>();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
			
			if (cardType == ItemType<Gambler.Weapons.Cards.EmbersCard>()) {
				Vector2 vel = new Vector2(speedX, speedY / 5f).RotatedByRandom(MathHelper.ToRadians(15));
				int projType = ProjectileType<Gambler.Projectiles.EmbersCardProj>();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
				return;
			}
		}
		
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
						int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, 15, 0f, player.whoAmI), dummy);
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
					if (dummyProj != dummy) {
						return;
					}
						
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
								int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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
							int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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
							int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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
							int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, heading.X, heading.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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
							int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, heading.X, heading.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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
							int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, heading.X, heading.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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
							int newProjectile = GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, projType, proj.damage, proj.knockBack, player.whoAmI), dummy);
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