using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;
using OrchidMod.Alchemist;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Light;
using OrchidMod.Alchemist.Weapons.Dark;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles
{	
    public class AlchemistProj : OrchidModAlchemistProjectile
    {
		public int nbElements = 0;
		public Item waterFlask = new Item();
		public Item fireFlask = new Item();
		public Item natureFlask = new Item();
		public Item airFlask = new Item();
		public Item lightFlask = new Item();
		public Item darkFlask = new Item();
		public OrchidModGlobalItem waterFlaskGlobal = null;
		public OrchidModGlobalItem fireFlaskGlobal = null;
		public OrchidModGlobalItem natureFlaskGlobal = null;
		public OrchidModGlobalItem airFlaskGlobal = null;
		public OrchidModGlobalItem lightFlaskGlobal = null;
		public OrchidModGlobalItem darkFlaskGlobal = null;
		public Color glowColor = new Color(0, 0, 0);
		public bool hitNPC = false;
		
		public bool noCatalyticSpawn = false;
		public int nbElementsNoExtract = 0;
		
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
			
			if (!this.hitNPC) {
				for (int l = 0; l < Main.npc.Length; l++) {  
					NPC target = Main.npc[l];
					if (projectile.Hitbox.Intersects(target.Hitbox))  {
						target.immune[projectile.owner] = 0;
					}
				}
			}
			
			if (this.waterFlaskGlobal != null) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.waterFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.fireFlaskGlobal != null) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.fireFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.natureFlaskGlobal != null) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.natureFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.airFlaskGlobal != null) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.airFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.lightFlaskGlobal != null) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.lightFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			if (this.darkFlaskGlobal != null) {
				if (Main.rand.Next(8) > this.nbElements) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.darkFlaskGlobal.alchemistRightClickDust);
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
			Item[] flasks = modPlayer.alchemistFlasks;
			
			this.projOwner = (projectile.owner == Main.myPlayer);
				
			this.nbElements = modPlayer.alchemistNbElements;
			projectile.damage = (int)(modPlayer.alchemistFlaskDamage * modPlayer.alchemistDamage + 5E-06f);
		
			if (elements[0]) {
				this.waterFlask = flasks[0];
				this.waterFlaskGlobal = this.waterFlask.GetGlobalItem<OrchidModGlobalItem>();
			}
				
			if (elements[1]) {
				this.fireFlask = flasks[1];
				this.fireFlaskGlobal = this.fireFlask.GetGlobalItem<OrchidModGlobalItem>();
			}
				
			if (elements[2]) {
				this.natureFlask = flasks[2];
				this.natureFlaskGlobal = this.natureFlask.GetGlobalItem<OrchidModGlobalItem>();
			}
				
			if (elements[3]) {
				this.airFlask = flasks[3];
				this.airFlaskGlobal = this.airFlask.GetGlobalItem<OrchidModGlobalItem>();
			}
				
			if (elements[4]) {
				this.lightFlask = flasks[4];
				this.lightFlaskGlobal = this.lightFlask.GetGlobalItem<OrchidModGlobalItem>();
			}
				
			if (elements[5]) {
				this.darkFlask = flasks[5];
				this.darkFlaskGlobal = this.darkFlask.GetGlobalItem<OrchidModGlobalItem>();
			}
				
			this.addVariousEffects();
			this.glowColor = new Color(modPlayer.alchemistColorR, modPlayer.alchemistColorG, modPlayer.alchemistColorB);
				
			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
				
			if (this.fireFlask.type == ItemType<GunpowderFlask>()) {
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
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.waterFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.natureFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.airFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.lightFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.darkFlaskGlobal);
			}
		}
		
		public void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer) {
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.waterFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.natureFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.airFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.lightFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.darkFlaskGlobal);
			}
		}
		
		public void KillThird(int timeLeft, Player player, OrchidModPlayer modPlayer) {
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.waterFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.natureFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.airFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.lightFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.darkFlaskGlobal);
			}
		}
		
		public void OnHitNPCFirst(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal) {
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.waterFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.natureFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.airFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.lightFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.darkFlaskGlobal);
			}
		}
		
		public void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal) {
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.waterFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.natureFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.airFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.lightFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.darkFlaskGlobal);
			}
		}
		
		public void OnHitNPCThird(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal) {
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.waterFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.natureFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.airFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.lightFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.darkFlaskGlobal);
			}
		}
				
		public void addVariousEffects() {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			this.nbElementsNoExtract += this.nbElements;
			
			int buffType = BuffType<Alchemist.Buffs.ReactiveVialsBuff>();
			if (player.HasBuff(buffType)) {
				projectile.damage = (int)(projectile.damage * 1.1f);
				player.ClearBuff(buffType);
			}
			
			if (this.fireFlaskGlobal != null) {
				fireFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.waterFlaskGlobal != null) {
				waterFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.natureFlaskGlobal != null) {
				natureFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.airFlaskGlobal != null) {
				airFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.lightFlaskGlobal != null) {
				lightFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
			
			if (this.darkFlaskGlobal != null) {
				darkFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
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
			
			if (this.waterFlaskGlobal != null) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.waterFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.fireFlaskGlobal != null) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.fireFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.natureFlaskGlobal != null) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.natureFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.airFlaskGlobal != null) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.airFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.lightFlaskGlobal != null) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.lightFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
			
			if (this.darkFlaskGlobal != null) {
				for (int i = 0 ; i < 8 - this.nbElements ; i ++ ) {
					if (Main.rand.Next(3) < 2) {
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.darkFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}	
		}
		
		public bool hasCloud() {
			return (this.airFlask.type == ItemType<CouldInAVial>() || this.airFlask.type == ItemType<FartInAVial>());
		}
    }
}