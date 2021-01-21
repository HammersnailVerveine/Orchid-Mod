using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles.Nature
{
    public class NatureSporeProjBloom : OrchidModAlchemistProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Nature Spore");
        } 
		
        public override void SafeSetDefaults() {
            projectile.width = 50;
            projectile.height = 50;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.alpha = 255;
			projectile.timeLeft = 60;
        }
		
		public override void AI() {
			if (projectile.timeLeft == 1) {
				projectile.friendly = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 44, 5, 5 + Main.rand.Next(3), true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
			}
        }
    }
}