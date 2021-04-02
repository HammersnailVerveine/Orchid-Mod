using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class CorruptionFlaskProj : OrchidModAlchemistProjectile
    {
		private double dustVal = 0;
		private int sporeType = 0;
		private int sporeDamage = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 28;
            projectile.height = 26;
            projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			projectile.scale = 1f;
			projectile.penetrate = -1;
			projectile.friendly = false;
			Main.projFrames[projectile.type] = 7;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom");
        }
		
        public override void AI()
        {
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			projectile.velocity.Y += 0.01f;
			
			if (projectile.timeLeft == 1) {
				projectile.timeLeft = projectile.damage;
				projectile.frame ++;
				if (projectile.frame >= 7) projectile.Kill();
			}
			
			int range = 50;
			if (this.sporeType == 0) {
				for (int l = 0; l < Main.projectile.Length; l++) {  
					Projectile proj = Main.projectile[l];
					if (proj.active && Main.rand.Next(2) == 0)  {
						float offsetXProj = proj.Center.X - projectile.Center.X;
						float offsetYProj = proj.Center.Y - projectile.Center.Y;
						float distanceProj = (float)Math.Sqrt(offsetXProj * offsetXProj + offsetYProj * offsetYProj);
						if (distanceProj < (float)range) {
							if (proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>()) {
								sporeType = 6;
								this.sporeDamage = proj.damage;
								break;
							}
							
							if (proj.type == ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>()) {
								sporeType = 59;
								this.sporeDamage = proj.damage;
								break;
							}
							
							if (proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>()) {
								sporeType = 61;
								this.sporeDamage = proj.damage;
								break;
							}
							
							if (proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>()) {
								sporeType = 63;
								this.sporeDamage = proj.damage;
								break;
							}
						}
					}
				}
			} else if (modPlayer.timer120 % 2 == 0) {
					this.spawnDust(sporeType, range);
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity *= 0f;
            return false;
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
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = projectile.velocity.X == 0 ? 1.5f :(float) Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}
    }
}