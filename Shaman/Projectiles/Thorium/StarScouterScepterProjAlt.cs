using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Dusts.Thorium;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class StarScouterScepterProjAlt : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 90;
			projectile.scale = 1f;
            this.empowermentType = 3;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbital Bomb");
        }
		
		public override void AI()
        {
			if (Main.rand.Next(5) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 62, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			
			for (int i = 1 ; i < 3 ; i ++) {
				spawnDustCircle(62, i * 10);
			}
			
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 91);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, mod.ProjectileType("StarScouterScepterProjAltExplosion"), projectile.damage, 0.0f, projectile.owner, 0.0f, 0.0f);
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 10 ; i ++ )
			{
				double dustDeg = (double)(i * (36));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width/4;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height/4;
				
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