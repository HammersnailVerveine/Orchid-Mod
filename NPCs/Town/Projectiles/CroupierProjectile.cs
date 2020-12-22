using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Town.Projectiles
{	
    public class CroupierProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chip");
        }
    }
}