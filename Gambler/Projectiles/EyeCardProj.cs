using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class EyeCardProj: OrchidModGamblerProjectile
    {
		private double dustVal = 0;
		private int cooldown = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 32;
            projectile.height = 22;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 9999;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			// Main.projFrames[projectile.type] = 2;
			this.gamblingChipChance = 5;
			this.projectileTrail = true;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.direction = projectile.spriteDirection;
			
			this.cooldown -= this.cooldown > 0 ? 1 : 0;
			
			if (projectile.ai[1] == 1) {
				projectile.friendly = true;
				projectile.velocity *= 0.975f;
				if (projectile.velocity.Length() < 3f) {
					projectile.friendly = false;
					projectile.ai[1] = 0;
					this.cooldown = 40;
				}
			}
			
			if (Main.myPlayer == projectile.owner) {
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.EyeCard>() && modPlayer.GamblerDeckInHand) {
					Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX - 8, (float)Main.mouseY) - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (projectile.ai[1] == 0) {
						AdjustMagnitude(ref newMove, distanceTo > 300f && this.cooldown <= 0);
						projectile.velocity = (5 * projectile.velocity + newMove);
						AdjustMagnitude(ref projectile.velocity, distanceTo > 300f && this.cooldown <= 0);
					}
						
					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(newMove.Y * 1000f);
					int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000) {
						projectile.netUpdate = true;
					}
				} else {
					if (projectile.ai[1] == 0) {
						projectile.Kill();
					}
				}
			}
			
			if (modPlayer.timer120 % 2 == 0 && projectile.ai[1] == 0) {
				this.spawnDust(35, 300);
			}
			this.dustVal --;
        }
		
		public void spawnDust(int dustType, int distToCenter) {
			for (int i = 0 ; i < 3 ; i ++ ) {
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);
						 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + projectile.velocity.X - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + projectile.velocity.Y - 4;
						
				Vector2 dustPosition = new Vector2(posX, posY);
						
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
						
				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            return false;
        }
		
		private void AdjustMagnitude(ref Vector2 vector, bool dash)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			float init = dash ? 12f : 0.01f;
			if (magnitude > init)
			{
				vector *= init / magnitude;
			}
			projectile.ai[1] = dash ? 1f : 0f;
		}
    }
}