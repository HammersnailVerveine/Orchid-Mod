using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class CrystalScepterProj2 : OrchidModShamanProjectile
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Beam");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 40;
            projectile.extraUpdates = 2;
			projectile.scale = 1f;
			projectile.alpha = 255;
            this.empowermentType = 5;
        }
		
        public override void AI()
        {
			if (Main.rand.Next(3) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 185);
				Main.dust[dust].velocity = projectile.velocity;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 62);
				Main.dust[dust2].velocity = projectile.velocity;
				Main.dust[dust2].scale = 1f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
			spawnDustCircle(Main.rand.Next(2) == 0 ? 185 : 62, 10);
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 10 ; i ++ )
			{
				double dustDeg = (i * (36)) + 5 - Main.rand.Next(10);
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width/4 - 1;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height/4 + 2;
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity *= 0.05f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}