using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class StarScouterScepterProjAltExplosion : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 200;
            this.empowermentType = 3;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbital Explosion");
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}