using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class KingSlimeCardProj : OrchidModGamblerProjectile
    {
		private int baseDamage = 0;
		private int justHit = 0;
		private int velocityStuck = 0;
		private float oldPositionY = 0f;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 26;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 620;
			projectile.penetrate = -1;
			projectile.alpha = 64;
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 2;
			this.gamblingChipChance = 10;
        }
		
		public override void SafeAI()
        {
			
			if (!this.initialized) {
				this.baseDamage = projectile.damage;
				this.initialized = true;
				projectile.ai[1] = 1f;
			}
			
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			if (projectile.ai[1] == 2f && projectile.timeLeft % 10 == 0 && projectile.velocity.Y > 0f) {
				projectile.damage ++;
			}
			
			if (projectile.ai[1] == 0f || projectile.ai[1] == 2f) {
				projectile.velocity.Y += (projectile.wet || projectile.lavaWet || projectile.honeyWet) ? projectile.velocity.Y > -7.5f ? -0.5f : 0f : projectile.velocity.Y < 7.5f ? 0.4f : 0f;
			}
			projectile.frame = projectile.velocity.Y < 0f ? 1 : 0;
			this.justHit -= this.justHit > 0 ? 1 : 0;
			
			this.velocityStuck = projectile.Center.Y == oldPositionY ? this.velocityStuck +  1 : 0;
			this.oldPositionY = 0f + projectile.Center.Y;
			
			if (projectile.velocity.X > 6f) {
				projectile.velocity.X = 6f;
			}
			if (projectile.velocity.X < -6f) {
				projectile.velocity.X = -6f;
			}
			
			if (Main.myPlayer == projectile.owner) {
				if (velocityStuck >= 5) {
					projectile.velocity.Y = -5;
					this.velocityStuck = 0;
				}
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.KingSlimeCard>() && modPlayer.gamblerAttackInHand) {
					Vector2 newMove = new Vector2(Main.screenPosition.X + (float)Main.mouseX, projectile.Center.Y) - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f) {
						if ((float)(Main.screenPosition.X + Main.mouseX) > projectile.Center.X) {
							projectile.velocity.X += projectile.velocity.X < 6f ? this.justHit > 0 ? 0.25f : 0.35f : 0f; 
						} else {
							projectile.velocity.X -= projectile.velocity.X > -6f ? this.justHit > 0 ? 0.25f : 0.35f : 0f; 
						}
					} else {
						if (projectile.velocity.Length() > 0.01f) {
							projectile.velocity.X *= 0.8f;
						}
					}
					
					if (projectile.ai[1] == 1f) {
						projectile.velocity.Y = -10f;	
						if (projectile.Center.Y - 50f < (Main.screenPosition.Y + (float)Main.mouseY)) {
							projectile.ai[1] = 2f;
							projectile.netUpdate = true;
						}
					}
						
					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000) {
						projectile.netUpdate = true;
					}
				} else {
					projectile.Kill();
				}
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (projectile.velocity.Y > 0f) {
				projectile.velocity.Y = -10;
				projectile.ai[1] = 1f;
				if (this.baseDamage < projectile.damage) {
					if (this.baseDamage < projectile.damage - 6) {
						OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, false, true);
					}
					projectile.damage = this.baseDamage;
				}
			} else {
				projectile.velocity.Y = 1f;
				projectile.ai[1] = 0f;
			}
			if (projectile.velocity.X != oldVelocity.X) {
				projectile.velocity.X = -oldVelocity.X;
				projectile.velocity.Y = 0f;
			}
            return false;
        }
		
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
			Texture2D texture = ModContent.GetTexture("OrchidMod/Gambler/Projectiles/KingSlimeCardProj_Glow");
			OrchidModProjectile.DrawProjectileGlowmask(projectile, spriteBatch, texture, Color.White);
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			projectile.velocity.Y = -10;
			projectile.velocity.X *= 0.5f;
			this.justHit = 30;
			projectile.ai[1] = 1f;
			int projType = ProjectileType<Gambler.Projectiles.KingSlimeCardProj2>();
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, projectile.damage, 0, projectile.owner);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 59, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, false, true);
        }
		
		public override void Kill(int timeLeft) {
            for(int i = 0; i < 7; i++) {
			    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, 0.0f, 0.0f, 175, new Color(0, 80, 255, 0));
            }
		}
		
		public override void SafePostAI() {
			SlimePostAITrail();
		}
		
		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor) 
		{
			SlimePreDrawTrail(spriteBatch, lightColor);
			return true;
		}
		
		public void SlimePreDrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			float offSet = this.projectileTrailOffset + 0.5f;
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * offSet, projectile.height * offSet);
			Texture2D texture = ModContent.GetTexture("OrchidMod/Gambler/Projectiles/KingSlimeCardProj2");
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.3f);
			}
		}
		
		public void SlimePostAITrail() {
            for (int num46 = projectile.oldPos.Length - 1; num46 > 0; num46--)
            {
                projectile.oldPos[num46] = projectile.oldPos[num46 - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }
    }
}
