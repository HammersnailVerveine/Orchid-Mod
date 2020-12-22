using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class QueenBeeFlaskProj : OrchidModAlchemistProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bee");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 180;
        }
		
		public override void AI()
        {
			projectile.alpha += 3 + Main.rand.Next(3);
			if (projectile.alpha >= 255) {
				projectile.Kill();
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
            return false;
        }
    }
}