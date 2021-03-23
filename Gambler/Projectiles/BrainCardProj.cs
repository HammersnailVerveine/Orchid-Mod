using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class BrainCardProj: OrchidModGamblerProjectile
    {
		private int bounceDelay = 0;
		private double dustVal = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 30;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 750;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 50;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;
			
			if (projectile.ai[1] == 0 && projectile.timeLeft == 750) {
				this.gamblingChipChance = 0;
			}
			
            if (Main.rand.Next(60 - (projectile.ai[0] > 0f ? 50 : 0)) == 0) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
				Main.dust[dust].velocity *= 0f;
            }
			
			if (projectile.ai[0] > 0f) {
				projectile.alpha -= projectile.alpha > 0 ? 4 : 0;
				if (Main.rand.Next(60) == 0) {
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60);
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale *= 1.5f;
					Main.dust[dust].noLight = true;
					Main.dust[dust].noGravity = true;
				}
			} else {
				projectile.alpha += projectile.alpha < 196 ? 4 : 0;
			}

			if (Main.myPlayer == projectile.owner) {	
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.BrainCard>()) {
					Vector2 dustDist = projectile.Center - player.Center;
					float minDistance = (float)Math.Sqrt(dustDist.X * dustDist.X + dustDist.Y * dustDist.Y);
					if (minDistance < 100f) {
						projectile.friendly = false;
						projectile.alpha += projectile.alpha < 196 ? 8 : 0;
					} else {
						projectile.friendly = projectile.ai[0] > 0f;
					}
					
					if (projectile.ai[1] == 0f) {
						if (this.bounceDelay <= 0) {
							if (modPlayer.timer120 % 2 == 0) {
								this.spawnDust(60, 100);
							}
							this.dustVal --;
							
							Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							
							if (distanceTo > 5f) {
								newMove *= 10f / distanceTo;
								projectile.velocity = newMove;
							} else {
								if (projectile.velocity.Length() > 0f) {
									projectile.velocity *= 0f;
								}
							}
							int velocityXBy1000 = (int)(newMove.X * 1000f);
							int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
							int velocityYBy1000 = (int)(newMove.Y * 1000f);
							int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);

							if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000) {
								projectile.netUpdate = true;
							}
						}
					} else {
						Vector2 newMove = new Vector2(0f, 0f);
						bool found = false;
						int projType = ProjectileType<Gambler.Projectiles.BrainCardProj>();
						for (int l = 0; l < Main.projectile.Length; l++) {  
							Projectile proj = Main.projectile[l];
							if (proj.active && proj.type == projType && proj.owner == player.whoAmI && proj.ai[1] == 0f)
							{
								found = true;
								newMove = proj.position - new Vector2(player.Center.X - (int)(projectile.width / 2), player.Center.Y);
								break;
							} 
						}
						if (found) {
							int rotationVal = projectile.ai[1] == 1f ? 90 : projectile.ai[1] == 2f ?  180 : 270;
							projectile.position = new Vector2(player.Center.X - (int)(projectile.width / 2), player.Center.Y) + newMove.RotatedBy(rotationVal);
						} else {
							projectile.Kill();
						}
					}
				} else {
					projectile.Kill();
				}
			}
        }
		
		public void spawnDust(int dustType, int distToCenter) {
			Player player = Main.player[projectile.owner];
			for (int i = 0 ; i < 3 ; i ++ ) {
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);
						 
				float posX = player.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = player.Center.Y - (int)(Math.Sin(rad) * distToCenter);
						
				Vector2 dustPosition = new Vector2(posX, posY);
						
				int index2 = Dust.NewDust(dustPosition, 0, 0, dustType);
						
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}
		
		public override void Kill(int timeLeft) {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			projectile.friendly = false;
			this.gamblingChipChance = 50;
			int projType = ProjectileType<Gambler.Projectiles.BrainCardProj2>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, projectile.damage, 0, projectile.owner), dummy);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 5, 10, 5  + Main.rand.Next(5), false, 1f, 1f, 5f, true, true, false, 0, 0, true);
			Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y, 83);
			projectile.ai[0] = 0f;
			target.AddBuff(BuffID.Confused, 60 * 3);
			bool skipped = false;
			bool switched = false;
			projType = ProjectileType<Gambler.Projectiles.BrainCardProj>();
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					proj.damage += 25;
					if (proj.ai[1] != projectile.ai[1]) {
						if ((Main.rand.Next(2) == 0 || skipped) && !switched) {
							proj.ai[0] = 300f;
							switched = true;
							proj.friendly = true;
							proj.netUpdate = true;
							OrchidModProjectile.spawnDustCircle(proj.Center, 60, 10, 5  + Main.rand.Next(5), true, 1.5f, 1f, 5f, true, true, false, 0, 0, true, true);
						} else {
							skipped = true;
						}
					}
				} 
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			this.bounceDelay = 15;
            return false;
        }
    }
}