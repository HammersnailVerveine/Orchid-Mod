using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.Equipment.Viscount
{
    public class ViscountOrbSound : OrchidModShamanProjectile
    {
		public int timer = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1200;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 10;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sound Bubble");
        } 
		
		public override void AI()
        {
			projectile.rotation += 0.1f;
			
			this.timer = this.timer > 90 ? 1 : this.timer + 1;
			if (this.timer % 30 == 0) {
                spawnDustCircle(89, (int)(3 * (this.timer / 30)));
            }		
			
			Player player = Main.player[projectile.owner];
			Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 50f && projectile.position.X < player.position.X + player.width && projectile.position.X + projectile.width > player.position.X && projectile.position.Y < player.position.Y + player.height && projectile.position.Y + projectile.height > player.position.Y) {
				if (projectile.owner == Main.myPlayer && !Main.LocalPlayer.moonLeech) {
					OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
					modPlayer.shamanFireTimer += modPlayer.shamanFireTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanWaterTimer += modPlayer.shamanWaterTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanAirTimer += modPlayer.shamanAirTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanEarthTimer += modPlayer.shamanEarthTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanSpiritTimer += modPlayer.shamanSpiritTimer == 0 ? 0 : 3 * 60;
					projectile.Kill();
				}
			}
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 15 ; i ++ ) {
				double deg = (i * (54 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1.2f;
				Main.dust[index2].scale = 1.2f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 89);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].noGravity = true;
            }
        }
    }
}