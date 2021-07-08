using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class WyvernMorayProjLingering : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
            this.empowermentType = 2;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyvern Dusts");
        }
		
        public override void AI()
        {
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, Main.rand.Next(2) == 0 ? 15 : 16);
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust2].noGravity = true;
			// Main.dust[dust2].velocity.X /= 3;
			// Main.dust[dust2].velocity.Y *= 3;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}