using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class EaterCardProj1: OrchidModGamblerProjectile
    {
		private int bounceDelay = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eater Eye");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 620;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj2>();
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;
			projectile.rotation += 0.05f;
			
			if (Main.rand.Next(60) == 0) {
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, projType, 0, 0, projectile.owner);
			}
			
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
				Main.dust[dust].velocity *= 0f;
            }
			
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType)
				{
					if (proj.Hitbox.Intersects(projectile.Hitbox) && proj.damage > 0) {
						projectile.damage += 3;
						projectile.scale += 0.1f;
						projectile.width += 1;
						projectile.height += 1;
						Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y, 2);
						proj.Kill();
					}
				} 
			}

			if (Main.myPlayer == projectile.owner) {
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.EaterCard>() && modPlayer.gamblerAttackInHand) {
					if (this.bounceDelay <= 0) {
						Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo > 5f) {
							newMove *= 7f / distanceTo;
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
					projectile.Kill();
				}
			}
        }
		
		public override void Kill(int timeLeft) {
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj2>();
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == projectile.owner)
				{
					proj.Kill();
				} 
			}
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
			projType = ProjectileType<Gambler.Projectiles.EaterCardProj3>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, projectile.damage * 5, 0, projectile.owner), dummy);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 18, 5, 3 + Main.rand.Next(5), false, 1.5f, 1f, 7f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 18, 10, 5  + Main.rand.Next(5), false, 1f, 1f, 5f, true, true, false, 0, 0, true);
			Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y, 83);
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