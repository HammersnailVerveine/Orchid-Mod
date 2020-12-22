using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class PiratesGloryProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.timeLeft = 42;
            projectile.extraUpdates = 5;		
			projectile.ignoreWater = true;
			projectile.alpha = 255;
            this.empowermentType = 2;
            this.empowermentLevel = 3;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pirate's Magic");
        } 
		
        public override void AI()
		{
			++projectile.localAI[0];
			if ((double) projectile.localAI[0] <= 7.0)
				return;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				projectile.alpha = (int) byte.MaxValue;
				int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = (float) Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity = projectile.velocity / 4;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].noGravity = true;	
          }
		  
		    if (projectile.timeLeft % 7 == 0)
				spawnDustCircle(127, 20);
			if ((projectile.timeLeft - 5) % 7 == 0)
				spawnDustCircle(127, 10);
        }
		
		public override void Kill(int timeLeft)
		{
			spawnDustCircle(127, 10);
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ ) {
					
				double deg = (double) projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = (float) Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			spawnDustCircle(127, 20);
			if (modPlayer.getNbShamanicBonds() > 2) 
				target.AddBuff((72), 5 * 60); // Midas
		}
    }
}