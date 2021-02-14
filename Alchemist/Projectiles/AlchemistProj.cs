using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Alchemist;
using OrchidMod;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles
{	
    public class AlchemistProj : OrchidModAlchemistProjectile
    {
		private int nbElements = 0;
		private int waterFlask = 0;
		private int fireFlask = 0;
		private int natureFlask = 0;
		private int airFlask = 0;
		private int lightFlask = 0;
		private int darkFlask = 0;
		private int waterDust = -1;
		private int fireDust = -1;
		private int natureDust = -1;
		private int airDust = -1;
		private int lightDust = -1;
		private int darkDust = -1;
		private Color glowColor = new Color(0, 0, 0);
		private bool hitNPC = false;
		
		private bool noCatalyticSpawn = false;
		private int nbElementsNoExtract = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.penetrate = 2;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alchemical Solution");
        } 
		
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
			Texture2D texture = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/AlchemistProj_Glow");
			OrchidModProjectile.DrawProjectileGlowmask(projectile, spriteBatch, texture, glowColor);
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate --;
            return false;
        }
		
        public override void AI()
        {
			if (projectile.penetrate == 1) {
				Vector2 oldCenter = new Vector2 (projectile.Center.X, projectile.Center.Y);
				//projectile.tileCollide = false;
				projectile.penetrate = -1;
				projectile.width = 48;
				projectile.height = 48;
				projectile.timeLeft = 1;
				projectile.alpha = 255;
				projectile.Center = oldCenter;
			}
			
			if (!this.initialized) {
				this.initializeAlchemistProjectile();
				this.initialized = true;
			}
			
			if (this.waterDust != -1) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.waterDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.fireDust != -1) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.fireDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.natureDust != -1) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.natureDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.airDust != -1) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.airDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.lightDust != -1) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.lightDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.darkDust != -1) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.darkDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
		
		public void initializeAlchemistProjectile() {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			bool[] elements = modPlayer.alchemistElements;
			int[] flasks = modPlayer.alchemistFlasks;
			int[] dusts =  modPlayer.alchemistDusts;
			
			this.projOwner = (projectile.owner == Main.myPlayer);
				
			this.nbElements = modPlayer.alchemistNbElements;
			projectile.damage = (int)(modPlayer.alchemistFlaskDamage * modPlayer.alchemistDamage + 5E-06f);
		
			if (elements[0]) {
				this.waterFlask = flasks[0];
				this.waterDust = dusts[0];
			}
					
			if (elements[1]) {
				this.fireFlask = flasks[1];
				this.fireDust = dusts[1];
			}
				
			if (elements[2]) {
				this.natureFlask = flasks[2];
				this.natureDust = dusts[2];
			}
				
			if (elements[3]) {
				this.airFlask = flasks[3];
				this.airDust = dusts[3];
			}
				
			if (elements[4]) {
				this.lightFlask = flasks[4];
				this.lightDust = dusts[4];
			}
				
			if (elements[5]) {
				this.darkFlask = flasks[5];
				this.darkDust = dusts[5];
			}
				
			this.addVariousEffects();
			this.glowColor = new Color(modPlayer.alchemistColorR, modPlayer.alchemistColorG, modPlayer.alchemistColorB);
				
			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);	
			OrchidModAlchemistHelper.clearAlchemistDusts(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
				
			if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.GunpowderFlask>()) {
				this.noCatalyticSpawn = true;
			}
				
			if (this.nbElements > 2) {
				if (modPlayer.alchemistMeteor) {
					player.AddBuff((BuffType<Alchemist.Buffs.MeteorSpeed>()), 60 * 3);
				}
			}
		}
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			int soundNb = this.nbElements == 1 ? 1 : this.nbElements == 2 ? 2 : 3;
			switch (soundNb) {
				case 1:
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
					break;
				case 2:
					Main.PlaySound(13, (int)projectile.position.X, (int)projectile.position.Y, 0);
					break;
				default:
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 107);
					break;
			}
			
			this.spawnKillDusts(timeLeft);
			
			if (this.projOwner) {
				this.KillFirst(timeLeft, player, modPlayer);
				this.KillSecond(timeLeft, player, modPlayer);
				this.KillThird(timeLeft, player, modPlayer);
			}
        }	
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (this.projOwner) {
				this.hitNPC = true;
				OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
				OrchidModGlobalNPC modTargetGlobal = target.GetGlobalNPC<OrchidModGlobalNPC>();
				
				this.OnHitNPCFirst(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
				this.OnHitNPCSecond(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
				this.OnHitNPCThird(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
			}
		}
		
		
		
		public void KillFirst(int timeLeft, Player player, OrchidModPlayer modPlayer) {
			
		}
		
		public void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer) {
			
			if (this.waterFlask != 0) {
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.SeafoamVial>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(90)));
						int spawnProj = this.natureFlask == ItemType<Alchemist.Weapons.Nature.PoisonVial>() ? ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProjAlt>() : ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					int dmg = (int)((this.nbElements * 3 + 6) * modPlayer.alchemistDamage);
					int shoot = ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProj>();
					if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.PoisonVial>()) {
						dmg = (int)((this.nbElements * 5 + 8) * modPlayer.alchemistDamage);
						shoot = ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProj>();
					}
					nb = this.hasCloud() ? 2 : 1;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -2.5f).RotatedByRandom(MathHelper.ToRadians(30)));
						vel *= (float)(1 - (Main.rand.Next(10) / 10));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, shoot, dmg, 0.5f, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.WaterleafFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
							Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
							int dmg = (int)((this.nbElements + 10) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.DungeonFlask>()) {
					int dmg = (int)((8 + (this.nbElements * 4)) * modPlayer.alchemistDamage);
					int rand = this.nbElements + Main.rand.Next(2);
					for (int i = 0 ; i < rand ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Water.DungeonFlaskProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.BloodMoonFlask>()) {
					int dmg = (int)((1 + this.nbElements * 3) * modPlayer.alchemistDamage);
					int rand =  2 + this.nbElements + Main.rand.Next(2);
					for (int i = 0 ; i < rand ; i ++) {
						Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(180)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Water.BloodMoonFlaskProj>(), dmg, 0.5f * this.nbElements, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.SlimeFlask>()) {
					if (this.fireFlask != 0) {
						int type = ProjectileType<Alchemist.Projectiles.Water.SlimeFlaskProj>();
						int dmg = (int)((5 * this.nbElements) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, type, dmg, 0.5f, projectile.owner);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1f, 1f, 5f, true, true, false, 0, 0, true);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, true);
						Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y, 45);
					}
				}
			}
			
			if (this.fireFlask != 0) {
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.EmberVial>()) {
					int nb = 2 + Main.rand.Next(3);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.EmberVialProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					int dmg = (int)(8 * modPlayer.alchemistDamage);
					int rand = this.nbElements + Main.rand.Next(2);
					for (int i = 0 ; i < rand ; i ++) {
						Vector2 vel = (new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(60)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Fire.EmberVialProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.GunpowderFlask>()) {
					int dmg = (int)((5 * this.nbElements + 15) * modPlayer.alchemistDamage);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ProjectileType<Alchemist.Projectiles.Fire.GunpowderFlaskProj>(), dmg, 3f, projectile.owner, 0.0f, 0.0f);
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
				}
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.FireblossomFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
						
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 22) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
						
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 14) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
			}
			
			if (this.natureFlask != 0) {
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.LivingSapVial>()) {
					int dmg = (int)(this.nbElements * 3);
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.LivingSapVialProj>();
					Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, projectile.owner);
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(90)));
						spawnProj = ProjectileType<Alchemist.Projectiles.Nature.LivingSapVialProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.PoisonVial>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(90)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					int dmg = (int)((this.nbElements * 5 + 8) * modPlayer.alchemistDamage);
					nb = this.hasCloud() ? 2 : 1;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -2.5f).RotatedByRandom(MathHelper.ToRadians(30)));
						vel *= (float)(1 - (Main.rand.Next(10) / 10));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProj>(), dmg, 0.5f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.GlowingMushroomVial>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2	vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt2>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					bool spawnedMushroom = false;
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						int projType = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProj>();
						int projTypeAlt = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt>();
						if (proj.active == true && (proj.type == projType || proj.type == projTypeAlt) && proj.owner == projectile.owner) {
							spawnedMushroom = true;
							break;
						}
					}
					if (!spawnedMushroom) {
						int duration = (int)((this.nbElements + 3) * modPlayer.alchemistDamage);
						Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProj>(), duration, 0f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.SunflowerFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2	vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					if (this.waterFlask != 0) {
						int dmg = (int)((this.nbElements + 6) * modPlayer.alchemistDamage);
						Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.MoonglowFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 13) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.DaybloomFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 8) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>()) {
					if (!this.hitNPC) {
						float baseRange = 50f;
						int usedElements = this.nbElements > 3 ? 3 : this.nbElements;
						float distance = 20f + usedElements * baseRange;
						NPC attractiteTarget = null;
						for (int k = 0; k < 200; k++)
						{
							if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly){
								Vector2 newMove = Main.npc[k].Center - projectile.Center;
								float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
								if (distanceTo < distance) {
									distance = distanceTo;
									attractiteTarget = Main.npc[k];
								}
							}
						}
						if (attractiteTarget != null) {
							attractiteTarget.AddBuff(mod.BuffType("Attraction"), 60 * (this.nbElements * 3));
						}
					}
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 15) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, mod.ProjectileType("NatureSporeProj"), dmg, 0f, projectile.owner);
					}
					if (this.nbElements == 1) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 20, true, 1.5f, 1f, 3f);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 20, true, 1.5f, 1f, 3f, true, false);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 20, true, 1.5f, 1f, 3f, false, true);
					} else if (this.nbElements == 2) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 30, true, 1.5f, 1f, 8f);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 30, true, 1.5f, 1f, 8f, true, false);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 30, true, 1.5f, 1f, 8f, false, true);
					} else if (this.nbElements > 2) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 40, true, 1.5f, 1f, 13f);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 40, true, 1.5f, 1f, 13f, true, false);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 40, true, 1.5f, 1f, 13f, false, true);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.AttractiteFlask>()) {
					if (this.nbElements == 1) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f, true, false);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f, false, true);
					} else if (this.nbElements == 2) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f, true, false);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f, false, true);
					} else if (this.nbElements > 2) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f, true, false);
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f, false, true);
					}
					if (!this.hitNPC) {
						float baseRange = 50f;
						int usedElements = this.nbElements > 3 ? 3 : this.nbElements;
						float distance = 20f + usedElements * baseRange;
						NPC attractiteTarget = null;
						for (int k = 0; k < 200; k++)
						{
							if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly){
								Vector2 newMove = Main.npc[k].Center - projectile.Center;
								float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
								if (distanceTo < distance) {
									distance = distanceTo;
									attractiteTarget = Main.npc[k];
								}
							}
						}
						if (attractiteTarget != null) {
							attractiteTarget.AddBuff(mod.BuffType("Attraction"), 60 * (this.nbElements * 3));
						}
					}
				}
			}
			
			if (this.airFlask != 0) {
				if (this.hasCloud()) {
					int nb = 2 + Main.rand.Next(3);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -(float)((3 * this.nbElements) + Main.rand.Next(3))).RotatedByRandom(MathHelper.ToRadians(10)));
						int spawnProj = Main.rand.Next(3) == 0 ? ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>() : ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
						int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
						Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
						Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
						Main.projectile[smokeProj].ai[1] = this.glowColor.B;
					}
					if (this.waterFlask == ItemType<Alchemist.Weapons.Water.BloodMoonFlask>()) {
						for (int i = 0 ; i < nb ; i ++) {
							int dmg = (int)((1 + this.nbElements * 3) * modPlayer.alchemistDamage);
							Vector2 vel = (new Vector2(0f, -(float)((3 * this.nbElements) + Main.rand.Next(3))).RotatedByRandom(MathHelper.ToRadians(10)));
							int spawnProj = ProjectileType<Alchemist.Projectiles.Water.BloodMoonFlaskProj>();
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, projectile.owner);
						}
					}
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.ShadowChestFlask>()) {
					int dmg = (int)(((this.nbElements * 15) + 10) * modPlayer.alchemistDamage);
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.ShadowChestFlaskProj>();
					for (int i = 0 ; i < 4 ; i ++) {
						Vector2 vel = (new Vector2(0f, 5f * this.nbElements).RotatedBy(MathHelper.ToRadians(90 * i)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, projectile.owner);
					}
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.QueenBeeFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					if (this.fireFlask == 0) {
						for (int i = 0 ; i < nb ; i ++) {
							Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
							int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
						}
							
						int dmg = (int)((9 + (2 * this.nbElements)) * modPlayer.alchemistDamage);
						int rand = this.nbElements + Main.rand.Next(3) + 1;
						for (int i = 0 ; i < rand ; i ++) {
							Vector2 vel = ( new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
							if (player.strongBees && Main.rand.Next(2) == 0) 
									Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, 566, (int) (dmg * 1.15f), 0f, projectile.owner, 0f, 0f);
							else {
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, 181, dmg, 0f, projectile.owner, 0f, 0f);
							}
						}
					}
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.ShiverthornFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 10) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.DeathweedFlask>()) {
					int nb = 2 + Main.rand.Next(2);
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					for (int l = 0; l < Main.projectile.Length; l++) {  
						Projectile proj = Main.projectile[l];
						if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
							proj.Kill();
						}
					}
					nb = this.nbElements + this.nbElementsNoExtract;
					nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
					for (int i = 0 ; i < nb ; i ++) {
						Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
						int dmg = (int)((this.nbElements + 15) * modPlayer.alchemistDamage);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>(), dmg, 0f, projectile.owner);
					}
				}
			}
			
			if (this.lightFlask != 0) {
			}
			
			if (this.darkFlask != 0) {
			}
		}
		
		public void KillThird(int timeLeft, Player player, OrchidModPlayer modPlayer) {
			
		}
		
		public void OnHitNPCFirst(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal) {
		}
		
		public void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal) {
			if (this.waterFlask != 0) {
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.KingSlimeFlask>()) {
					if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f) {
						target.AddBuff(mod.BuffType("SlimeSlow"), 90 * (this.nbElements * 2));
					}
					if (Main.rand.Next(10) < this.nbElements && !this.noCatalyticSpawn) {
						int dmg = (int)((this.nbElements * 3 + 6) * modPlayer.alchemistDamage);
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubble>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>()) {
					if (this.fireFlask != 0) {
						int dmg = (int)(((this.nbElements * 15) + 50 ) * modPlayer.alchemistDamage);
						modTarget.spreadOilFire(target.Center, dmg, Main.player[projectile.owner]);
						Main.PlaySound(2, (int)target.position.X, (int)target.position.Y, 45);
					}
					modTarget.alchemistOil = 60 * 10;
						
					int rand = this.nbElements;
					rand += this.hasCloud() ? 2 : 0;
					if (Main.rand.Next(6) < rand && !this.noCatalyticSpawn) {
						int dmg = 0;
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
					modTarget.alchemistWater = 60 * 5;
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.HellOil>()) {
					if (this.fireFlask != 0) {
						int dmg = (int)(((this.nbElements * 20) + 60 ) * modPlayer.alchemistDamage);
						modTarget.spreadOilFire(target.Center, dmg, Main.player[projectile.owner]);
						Main.PlaySound(2, (int)target.position.X, (int)target.position.Y, 45);
					}
					modTarget.alchemistOil = 60 * 10;
						
					int rand = this.nbElements;
					rand += this.hasCloud() ? 2 : 0;
					if (Main.rand.Next(6) < rand && !this.noCatalyticSpawn) {
						int dmg = 0;
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
					modTarget.alchemistWater = 60 * 5;
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.SeafoamVial>()) {
					int rand = this.nbElements;
					rand += this.hasCloud() ? 2 : 0;
					if (Main.rand.Next(10) < rand && !this.noCatalyticSpawn) {
						int dmg = (int)((this.nbElements * 3 + 6) * modPlayer.alchemistDamage);
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.SeafoamBubble>();
						if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.PoisonVial>()) {
							dmg = (int)((this.nbElements * 5 + 8) * modPlayer.alchemistDamage);
							proj = ProjectileType<Alchemist.Projectiles.Reactive.PoisonBubble>();
						}
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.DungeonFlask>()) {
					int rand = this.nbElements;
					rand += this.hasCloud() ? 2 : 0;
					if (Main.rand.Next(10) < rand && !this.noCatalyticSpawn) {
						int dmg = (int)((8 + (this.nbElements * 3)) * modPlayer.alchemistDamage);
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.SpiritedBubble>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.WaterleafFlask>()) {
					if (modTarget.alchemistWater > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 7 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 33, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
			}
			
			if (this.fireFlask != 0) {
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.EmberVial>()) {
					target.AddBuff(BuffID.OnFire, (60 * 3 ) + (60 * (this.nbElements)));
				}
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.FireblossomFlask>()) {
					if (modTarget.alchemistFire > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 10 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 6, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
						}
					}
				}
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>()) {
					if (modTarget.alchemistFire > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 6 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 6, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
						}
					}
				}
			}
			
			if (this.natureFlask != 0) {
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.AttractiteFlask>()) {
					target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>(), 60 * (this.nbElements * 3));
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.PoisonVial>()) {
					target.AddBuff(BuffID.Poisoned, 60 * 5);
					int rand = this.nbElements;
					rand += this.hasCloud() ? 2 : 0;
					if (Main.rand.Next(10) < rand && !this.noCatalyticSpawn) {
						int dmg = (int)((this.nbElements * 5 + 8) * modPlayer.alchemistDamage);
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.PoisonBubble>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.MoonglowFlask>()) {
					if (modTarget.alchemistNature > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 9 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 163, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.DaybloomFlask>()) {
					if (modTarget.alchemistNature > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 5 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 163, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>()) {
					target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>(), 60 * (this.nbElements * 3));
					if (modTarget.alchemistNature > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 10 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 163, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.LivingSapVial>()) {
					int rand = this.nbElements;
					rand += this.hasCloud() ? 2 : 0;
					if (Main.rand.Next(10) < rand) {
						int dmg = 5 + this.nbElements * 5;
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.LivingSapBubble>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
				}
			}
			
			if (this.airFlask != 0) {
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.QueenBeeFlask>()) {
					int rand = this.nbElements;
					if (Main.rand.Next(10) < rand) {
						int dmg = (int)((9 + (2 * this.nbElements)) * modPlayer.alchemistDamage);
						int proj = ProjectileType<Alchemist.Projectiles.Reactive.AlchemistHive>();
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
					}
				}
				if (this.hasCloud()) {
					if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f) {
						target.velocity.Y = -((float) this.nbElements * 4);
					}
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.DeathweedFlask>()) {
					if (modTarget.alchemistAir > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 9 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 15, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.ShiverthornFlask>()) {
					if (modTarget.alchemistAir > 0) {
						Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y, 45);
						int dmg = 8 * this.nbElements;
						player.ApplyDamageToNPC(target, Main.DamageVar(dmg), 0f, Main.LocalPlayer.direction, true);
						for (int i = 0 ; i < 10 ; i ++) {
							int dust = Dust.NewDust(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, 15, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, default(Color), 3.5f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
							Main.dust[dust].scale *= 0.5f;
						}
					}
				}
			}
			
			if (this.lightFlask != 0) {
				if (this.lightFlask == ItemType<Alchemist.Weapons.Light.GlowingVial>()) {
					target.AddBuff(BuffID.Confused, 60 * (this.nbElements));
				}
			}
			
			if (this.darkFlask != 0) {
				if (this.darkFlask == ItemType<Alchemist.Weapons.Dark.EmoVial>()) {
					target.AddBuff(153, 60 * ((((int)(this.nbElements / 2)) == 0) ? 1 : ((int)(this.nbElements / 2)))); // Shadowflame
				}
			}
		}
		
		public void OnHitNPCThird(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal) {
			if (this.fireFlask != 0) {
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.EmberVial>()) {
					modTarget.alchemistFire = 60 * 5;
				}
			}
			
			if (this.waterFlask != 0) {
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>()) {
					modTarget.alchemistOil = 60 * 10;
					modTarget.alchemistWater = 60 * 5;
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.HellOil>()) {
					modTarget.alchemistOil = 60 * 10;
					modTarget.alchemistWater = 60 * 5;
				}
			}
		}
		
		
		public void addVariousEffects() {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			this.nbElementsNoExtract += this.nbElements;
			
			if (this.fireFlask != 0) {
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.FireblossomFlask>()) {
					this.nbElementsNoExtract --;
				}
				
				if (this.fireFlask == ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>()) {
					this.nbElementsNoExtract --;
				}
			}
			
			if (this.waterFlask != 0) {
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.WaterleafFlask>()) {
					this.nbElementsNoExtract --;
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>()) {
					if (this.fireFlask != 0) {
						projectile.damage += (int)(30 * modPlayer.alchemistDamage);
					}
				}
				if (this.waterFlask == ItemType<Alchemist.Weapons.Water.HellOil>()) {
					if (this.fireFlask != 0) {
						projectile.damage += (int)(50 * modPlayer.alchemistDamage);
					}
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.SunflowerFlask>()) {
					projectile.damage += (int)(3 * modPlayer.alchemistDamage);
				}
			}
			
			if (this.natureFlask != 0) {
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.MoonglowFlask>()) {
					this.nbElementsNoExtract --;
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.GlowingAttractiteFlask>()) {
					this.nbElementsNoExtract --;
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.AttractiteFlask>()) {
					this.nbElementsNoExtract --;
				}
				if (this.natureFlask == ItemType<Alchemist.Weapons.Nature.DaybloomFlask>()) {
					this.nbElementsNoExtract --;
				}
			}
			
			if (this.airFlask != 0) {
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.DeathweedFlask>()) {
					this.nbElementsNoExtract --;
				}
				if (this.airFlask == ItemType<Alchemist.Weapons.Air.ShiverthornFlask>()) {
					this.nbElementsNoExtract --;
				}
			}
			
			int buffType = BuffType<Alchemist.Buffs.ReactiveVialsBuff>();
			if (player.HasBuff(buffType)) {
				projectile.damage = (int)(projectile.damage * 1.1f);
				player.ClearBuff(buffType);
			}
		}
		
		public void spawnKillDusts(int timeLeft) {
			int rand = this.nbElements + Main.rand.Next(this.nbElements * 2);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			rand = 1 + Main.rand.Next(this.nbElements);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			rand = Main.rand.Next(this.nbElements);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			
			if (this.waterDust != -1) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.waterDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.fireDust != -1) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.fireDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.natureDust != -1) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.natureDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.airDust != -1) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.airDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.lightDust != -1) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.lightDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.darkDust != -1) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.darkDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}	
		}
		
		public bool hasCloud() {
			return (this.airFlask == ItemType<Alchemist.Weapons.Air.CouldInAVial>() || this.airFlask == ItemType<Alchemist.Weapons.Air.FartInAVial>());
		}
    }
}