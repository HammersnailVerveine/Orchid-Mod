using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class SkeletronCardProj : OrchidModGamblerProjectile
    {
		private int bounceDelay = 0;
		private double dustVal = 0;
		private int projectilePoll = 10;
		private int fireProj = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Might");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 28;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 620;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
			Main.projFrames[projectile.type] = 2;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;
			
			if (projectile.ai[1] == 0f) {
				projectile.friendly = true;
				projectile.rotation += 0.25f;
				projectile.frame = 1;
			} else {
				projectile.friendly = false;
				projectile.rotation = 0f;
				projectile.frame = 0;
			}
			projectile.frame = (int)projectile.ai[1];

			if (Main.myPlayer == projectile.owner) {
				if (modPlayer.timer120 % 2 == 0) {
					this.spawnDust(59, 250);
				}
				this.dustVal --;
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.SkeletronCard>() && modPlayer.gamblerAttackInHand) {
					if (this.bounceDelay <= 0) {
						Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo > 5f) {
							newMove *= 2f / distanceTo;
							projectile.velocity = newMove;
						} else {
							if (projectile.velocity.Length() > 0f) {
								projectile.velocity *= 0f;
							}
						}
						
						if (distanceTo > 250f) {
							if (projectile.ai[1] == 0f) {
								projectile.ai[1] = 1f;
								projectile.netUpdate = true;
							}
							if (modPlayer.timer120 % (int)(60 / this.projectilePoll) == 0) {
								this.fireProj ++;
							}
							
							if (this.fireProj == 5) {
								Vector2 projMove = newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
								projMove *= 10f / distanceTo;
								int projType = ProjectileType<Gambler.Projectiles.SkeletronCardProjAlt>();
								bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
								GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projMove.X, projMove.Y, projType, (int)(projectile.damage * 2), projectile.knockBack, projectile.owner), dummy);
								Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 8);
								this.projectilePoll -= this.projectilePoll - 1 <= 0 ? 0 : 1;
								this.fireProj = 0;
							}
						} else {
							if (projectile.ai[1] == 1f) {
								projectile.ai[1] = 0f;
								projectile.netUpdate = true;
								this.fireProj = 0;
							}
							if (modPlayer.timer120 % 30 == 0) {
								this.projectilePoll += this.projectilePoll + 1 > 10 ? 0 : 1;
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
					projectile.Kill();
				}
			}
        }
		
		public override void SafePostAI()
        {
            for (int num46 = projectile.oldPos.Length - 5; num46 > 0; num46--)
            {
                projectile.oldPos[num46] = projectile.oldPos[num46 - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }
		
		public override void SafePreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.ai[1] == 1f && projectile.rotation == 0f) {
				Texture2D flameTexture = ModContent.GetTexture("OrchidMod/Gambler/Projectiles/SkeletronCardProj_Glow");
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 1f, projectile.height * 1f);
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					drawPos.X += Main.rand.Next(6) - 3 - Main.player[projectile.owner].velocity.X;
					drawPos.Y += Main.rand.Next(6) - 3 - Main.player[projectile.owner].velocity.Y;
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k*5) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(flameTexture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.3f);
				}
			}
		}
		
		public void spawnDust(int dustType, int distToCenter) {
			Player player = Main.player[projectile.owner];
			for (int i = 0 ; i < 3 ; i ++ ) {
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);
						 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter);
						
				Vector2 dustPosition = new Vector2(posX, posY);
						
				int index2 = Dust.NewDust(dustPosition, 0, 0, dustType);
						
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
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