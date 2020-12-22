using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles
{
    public class AlchemistSmoke1: OrchidModAlchemistProjectile
    {
		public Color glowColor = new Color(255, 255, 255);
		public float rotationspeed = 0f;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alchemical Smoke");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 30;
            projectile.height = 28;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			projectile.alpha = 64;
			projectile.tileCollide = false;
        }
		
		public override Color? GetAlpha(Color lightColor)
        {
           // return this.glowColor;
           return Main.myPlayer == projectile.owner ? this.glowColor : Color.White;
        }
		
		public override void AI()
        {
			projectile.scale *= 0.98f;
			projectile.velocity *= 0.95f;
			this.rotationspeed *= 0.99f;
			projectile.alpha += 1 + Main.rand.Next(2);
			projectile.rotation += this.rotationspeed;
			if (projectile.timeLeft == 180) {
				this.glowColor.R = (byte)projectile.localAI[0];
				this.glowColor.G = (byte)projectile.localAI[1];
				this.glowColor.B = (byte)projectile.ai[1];
				this.rotationspeed = (float)((1 + Main.rand.Next(3)) / 10f);
				this.rotationspeed *= Main.rand.Next(2) == 0 ? -1 : 1;
			}
			this.glowColor.A = (byte)(255 - projectile.alpha);
			if (projectile.alpha >= 196) {
				projectile.Kill();
			}
			if (Main.rand.Next(30) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Dusts.WhiteDust>(), 0.0f, 0.0f, 0, this.glowColor);
				Main.dust[dust].velocity *= 0.5f;;
				Main.dust[dust].noGravity = true;
			}
        }
		
		// public override bool OnTileCollide(Vector2 oldVelocity)
        // {
            // if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            // if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			// projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
            // return false;
        // }
		
		public override void Kill(int timeLeft) {
            for (int i = 0; i < 2; i++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Dusts.WhiteDust>(), 0.0f, 0.0f, 0, this.glowColor);
				Main.dust[dust].velocity *= 0.5f;;
				Main.dust[dust].noGravity = true;
            }
		}
    }
}