using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
    public class AbyssPrecinctProjExplosion : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 200;
            projectile.height = 200;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 200;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Explosion");
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}