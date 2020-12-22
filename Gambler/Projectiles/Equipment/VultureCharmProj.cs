using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Projectiles.Equipment
{
	public class VultureCharmProj : OrchidModGamblerProjectile
	{
		private bool rapidFade = false;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vulture Feather");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 180;
            projectile.penetrate = -1;
			this.gamblingChipChance = 5;
			this.projectileTrail = true;
		}
		
        public override void SafeAI()
        {	
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.direction = projectile.spriteDirection;
			projectile.velocity *= 0.98f;
			projectile.alpha += projectile.timeLeft < 120 ? projectile.timeLeft < 60 ? 3 : 2 : 0;
			projectile.alpha += this.rapidFade ? 3 : 0;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.velocity.X *= 0.01f;
			projectile.velocity.Y *= 0.01f;
			projectile.timeLeft /= 2;
			rapidFade = true;
			projectile.tileCollide = false;
            return false;
        }
	}
}