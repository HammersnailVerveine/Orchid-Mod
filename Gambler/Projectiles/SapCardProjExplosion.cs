using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Projectiles
{
	public class SapCardProjExplosion : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			this.gamblingChipChance = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}

		public override void OnSpawn()
		{
			OrchidHelper.SpawnDustCircle(
				center: Projectile.Center,
				radius: 70,
				count: 25,
				type: (index) => 102,
				onSpawn: (dust, index, angleFromCenter) =>
				{
					dust.alpha = 50;
					dust.velocity = new Vector2(Main.rand.NextFloat(1, 2.5f), 0).RotatedBy(angleFromCenter);
					dust.scale *= 0.5f;
				}
			);
		}

		public override void AI()
		{
			OrchidModProjectile.resetIFrames(Projectile);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(BuffID.Poisoned, 60 * 5);
			}
		}
	}
}