using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
    public class PoisonBubble : AlchemistProjReactive
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.scale = 1f;
			projectile.alpha = 64;
			this.spawnTimeLeft = 600;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poison Bubble");
        } 
		
		public override void SafeAI()
        {
			projectile.velocity.Y *= 0.95f;
			projectile.velocity.X *= 0.99f;
			projectile.rotation += 0.02f;
			
			if (Main.rand.Next(20) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
            }
		}
		
		public override void Despawn() {
            for(int i=0; i<5; i++)
            {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
		}
		
		public override void SafeKill(int timeLeft, Player player, OrchidModPlayer modPlayer)
        {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 85);
			int proj = ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProj>();
			int dmg = projectile.damage;
            for(int i=0; i<8; i++)
            {
				Vector2 perturbedSpeed = new Vector2(0f, 12.5f).RotatedBy(MathHelper.ToRadians(22.5f + i * 45));
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0.5f, projectile.owner, 0.0f, 0.0f);
            }
			
			for(int i=0; i<3; i++)
            {
				Vector2 perturbedSpeed = new Vector2(0f, 12.5f).RotatedByRandom(MathHelper.ToRadians(180));
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0.5f, projectile.owner, 0.0f, 0.0f);
            }
        }
    }
}