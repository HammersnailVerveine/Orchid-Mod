using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class QueenBeeCardProj: OrchidModGamblerProjectile
    {
		private bool positiveX = false;
		private bool positiveY = false;
		private int bounceDelay = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hive");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 24;
            projectile.height = 30;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1200;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
			this.projectileTrail = true;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = modPlayer.gamblerCardCurrent.type;
			
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;
			
			bool spawnBees = false;
			if (positiveX == true) {
				if (projectile.velocity.X < 0f) {
					this.positiveX = false;
					spawnBees = true;
				}
			} else {
				if (projectile.velocity.X > 0f) {
					this.positiveX = true;
					spawnBees = true;
				}
			}
			
			if (positiveY == true) {
				if (projectile.velocity.Y < 0f) {
					this.positiveY = false;
					spawnBees = true;
				}
			} else {
				if (projectile.velocity.Y > 0f) {
					this.positiveY = true;
					spawnBees = true;
				}
			}
	
			if (Main.rand.Next(3) == 0 && ((spawnBees && Main.rand.Next(2) == 0) || Main.rand.Next(50) == 0)) {
				int rand = Main.rand.Next(2) + 1;
				for (int i = 0 ; i < rand ; i ++) {
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
					if (player.strongBees && Main.rand.Next(2) == 0) 
							Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, 566, (int) (projectile.damage * 1.15f), 0f, projectile.owner, 0f, 0f);
					else {
						int newProj = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, 181, projectile.damage, 0f, projectile.owner, 0f, 0f);
						OrchidModGlobalProjectile modProjectile = Main.projectile[newProj].GetGlobalProjectile<OrchidModGlobalProjectile>();
						modProjectile.gamblerProjectile = true;
						modProjectile.baseCritChance = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().baseCritChance;
					}
				}
			}

			if (Main.myPlayer == projectile.owner && modPlayer.gamblerAttackInHand) {
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.QueenBeeCard>() && this.bounceDelay <= 0) {
					Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f) {
						newMove *= 10f / distanceTo;
						projectile.velocity = newMove;
						projectile.netUpdate = true;
					} else {
						if (projectile.velocity.Length() > 0f) {
							projectile.velocity *= 0f;
							projectile.netUpdate = true;
						}
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
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			this.bounceDelay = 15;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 153);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
        }
    }
}