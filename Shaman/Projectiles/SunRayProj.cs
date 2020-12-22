using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class SunRayProj : OrchidModShamanProjectile
    {
		public int sizeBonus = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.timeLeft = 45;
			projectile.penetrate = 100;
            projectile.extraUpdates = 5;			
			projectile.ignoreWater = true; 
			projectile.alpha = 255;
            this.empowermentType = 1;
            this.empowermentLevel = 4;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun beam");
        } 
		
        public override void AI()
		{
			if (projectile.timeLeft == 45)
			{
				for (int i =0 ; i < Main.player[projectile.owner].GetModPlayer<OrchidModPlayer>().getNbShamanicBonds() ; i ++) {
					sizeBonus += 2;
					projectile.damage += 5;
				}
				projectile.netUpdate = true;
			}
			
			++projectile.localAI[0];
			if ((double) projectile.localAI[0] <= 7.0)
				return;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				Vector2 Position = projectile.position - projectile.velocity * ((float) index1 * 0.25f);
				projectile.alpha = (int) byte.MaxValue;
				int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 169, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = (float) (Main.rand.Next(70, 110) * 0.013f) + ((sizeBonus) * 0.15f);
				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].noGravity = true;	
          }
		  
		    if (projectile.timeLeft % 5 == 0)
				spawnDustCircle(169, - (int)(- 10 - projectile.timeLeft));
        }
		
		public override void Kill(int timeLeft)
		{
		}
		
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 30; i ++ )
			{
				double dustDeg = (double) projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width/2;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height/2;
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity = projectile.velocity / 2;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 1.5f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff((24), 5 * 60); // On fire
		}
    }
}