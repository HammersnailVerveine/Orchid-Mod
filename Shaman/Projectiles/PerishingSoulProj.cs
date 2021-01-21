using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Projectiles
{
    public class PerishingSoulProj : OrchidModShamanProjectile
    {
		private Vector2 storeVelocity;
		private int storeDamage;
		private float dustScale = 0;
		private bool dustSpawned;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perishing Slash");
        } 
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
			projectile.timeLeft = 160;
            projectile.friendly = true;
            projectile.tileCollide = true;
			aiType = ProjectileID.Bullet;
            this.empowermentType = 1;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
		{
			projectile.alpha += 30;
			
			if (projectile.timeLeft == 160) {
				storeVelocity = projectile.velocity;
				storeDamage = projectile.damage;
			}
			if (projectile.timeLeft > 35) {
				projectile.velocity *= 0f;
				projectile.damage = 0;
				dustScale += 0.0195f;
				dustSpawned = false;

				if (Main.player[projectile.owner].GetModPlayer<OrchidModPlayer>().getNbShamanicBonds() > 3) {
					projectile.timeLeft --;
					dustScale += 0.0195f;
					projectile.netUpdate = true;
				}
			}
			
			if (projectile.timeLeft == 80 || projectile.timeLeft == 115) {
				int dustDist = 20;
				if (projectile.timeLeft == 115)
					dustDist = 10;
					
				spawnDustCircle(6, dustDist);
				projectile.netUpdate = true;
			}
			
			if (projectile.timeLeft <= 35) {
				projectile.damage = storeDamage;
				projectile.velocity = storeVelocity;
				projectile.extraUpdates = 1;
				projectile.netUpdate = true;
				
				if (dustSpawned == false) 
				{
					dustSpawned = true;
					spawnDustCircle(6, 20);
					spawnDustCircle(6, 30);
				}
			}
			for (int i = 0 ; i < 3 ; i ++) {
				Vector2 Position = projectile.position;
				int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 6);
				Main.dust[index2].scale = (float) 90 * 0.010f + dustScale/3;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ )
			{
				double dustDeg = (double) projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width/2 + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height/2 + 4;
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity *= 0.2f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 1.5f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
		{
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity *= 5f;
            }
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.rand.Next(4) == 0) target.AddBuff((24), 5 * 60);
		}
	}
}