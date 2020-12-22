using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class CrystalScepterProj : OrchidModShamanProjectile
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
            projectile.aiStyle = 29;
			projectile.timeLeft = 40;
            projectile.extraUpdates = 2;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 100;
            this.empowermentType = 5;
            this.empowermentLevel = 3;
            this.spiritPollLoad = 0;
        }
		
        public override void AI()
        {
			if (projectile.timeLeft == 40) {
				spawnDustCircle(Main.rand.Next(2) == 0 ? 185 : 62, 20);
			} else if (projectile.timeLeft == 38) {
				spawnDustCircle(Main.rand.Next(2) == 0 ? 185 : 62, 15);
			}
			
			if (Main.rand.Next(40) == 0 && projectile.timeLeft < 30 && projectile.timeLeft > 10) {
				spawnDustCircle(Main.rand.Next(2) == 0 ? 185 : 62, 10);
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X / 3, projectile.velocity.Y / 3).RotatedByRandom(MathHelper.ToRadians(360));
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CrystalScepterProj2"), (int)(projectile.damage * 0.6), (float)(projectile.knockBack * 0.35), projectile.owner, 0f, 0f);
			}
			
			if (projectile.timeLeft == 0 ) {
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				
				for (int i = 0; i < 2 + modPlayer.getNbShamanicBonds(); i++)
				{
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X / 3, projectile.velocity.Y / 3).RotatedByRandom(MathHelper.ToRadians(360));
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CrystalScepterProj2"), (int)(projectile.damage * 0.6), (float)(projectile.knockBack * 0.35), projectile.owner, 0f, 0f);
				}
				
				spawnDustCircle(185, 20);
				spawnDustCircle(62, 30);
			}
			
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
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ )
			{
				double dustDeg = (i * (18)) + 5 - Main.rand.Next(10);
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter);
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter);
				
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