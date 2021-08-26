using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class MagmaScepterProjExplosion : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 150;
			projectile.height = 150;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 200;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.BuffType("Singed")), 5 * 60);
			}
		}
	}
}