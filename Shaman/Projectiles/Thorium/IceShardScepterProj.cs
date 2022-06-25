using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class IceShardScepterProj : OrchidModShamanProjectile
	{
		private Vector2 storeVelocity;
		private int storeDamage;
		private float dustScale = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Bolt");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 80;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.alpha += 30;

			if (Projectile.timeLeft == 80)
			{
				storeVelocity = Projectile.velocity;
				storeDamage = Projectile.damage;
			}

			if (Projectile.timeLeft > 35)
			{
				Projectile.velocity *= 0f;
				Projectile.damage = 0;
				dustScale += 0.0195f;
			}

			if (Projectile.timeLeft == 35)
			{
				Projectile.damage = storeDamage;
				Projectile.velocity = storeVelocity;
				Projectile.extraUpdates = 1;

				OrchidModProjectile.spawnDustCircle(Projectile.Center, 92, 20, 5, true, 1.5f, 1f, 1f, true, true, false, 0, 0, true);
				for (int i = 0; i < 5; i++)
				{
					int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 92);
					Main.dust[index].scale = 1.5f;
					Main.dust[index].velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(20));
					Main.dust[index].noGravity = true;
				}
			}

			for (int i = 0; i < 3; i++)
			{
				Vector2 Position = Projectile.position;
				int index2 = Dust.NewDust(Position, Projectile.width, Projectile.height, 92);
				Main.dust[index2].scale = (float)90 * 0.010f + dustScale / 3;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
			}

			if (!this.initialized)
			{
				this.initialized = true;
				Player player = Main.player[Projectile.owner];
				OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
				int newCrit = 10 * modPlayer.GetNbShamanicBonds() + (int)player.GetCritChance<ShamanDamageClass>() + player.inventory[player.selectedItem].crit;
				OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				modProjectile.baseCritChance = newCrit;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 92);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity *= 2f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Main.rand.NextBool(3)) target.AddBuff((44), 3 * 60); // Frostburn
		}
	}
}