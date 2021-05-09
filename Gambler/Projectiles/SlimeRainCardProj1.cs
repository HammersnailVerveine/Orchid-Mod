using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class SlimeRainCardProj1: OrchidModGamblerProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Cloud");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 54;
            projectile.height = 28;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.alpha = 126;
			projectile.timeLeft = 9999;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
			Main.projFrames[projectile.type] = 3;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			
			if (modPlayer.timer120 % 20 == 0) {
				projectile.frame += projectile.frame + 1 == 3 ? -2 : 1;
			}
			
			if (modPlayer.timer120 % 30 == 0) {
				int projType = ProjectileType<Gambler.Projectiles.SlimeRainCardProj2>();
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X + Main.rand.Next(projectile.width - 10) + 5, projectile.Center.Y, 0f, 5f, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
			}
			
			if (Main.rand.Next(15) == 0) {
				int Alpha = 175;
				Color newColor = new Color(0, 80, 255, 0);
				int dust = Dust.NewDust(projectile.position + Vector2.One * 6f, projectile.width, projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.7f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noLight = true;
            }
			
			if (Main.myPlayer == projectile.owner && !this.initialized) {
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.SlimeRainCard>() && modPlayer.GamblerDeckInHand) {
						Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo > 5f) {
							newMove *= 8f / distanceTo;
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
				} else {
					projectile.velocity *= 0f;
					projectile.timeLeft = this.initialized ? projectile.timeLeft : 600;
					this.initialized = true;
				}
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
    }
}