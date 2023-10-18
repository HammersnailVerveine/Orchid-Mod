using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Small
{
	public class CrimsonScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crimson Bolt");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 90;
			Projectile.scale = 1f;
			Projectile.penetrate = 2;
			this.projectileTrail = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(5))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 258);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale *= 1.3f;
				Main.dust[dust].noGravity = true;
			}
			
			if (Projectile.timeLeft < 40) {
				Projectile.velocity *= 0.9f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 258, 5, 8, true, 1.5f, 1f, 4f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 258, 5, 8, true, 1.5f, 1f, 2.5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, 75, 1, false, false);
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Projectile.friendly = false;
			Projectile.timeLeft = Projectile.timeLeft > 40 ? 40 : Projectile.timeLeft;
			
			if (modPlayer.shamanOrbSmall != ShamanOrbSmall.CRIMSON)
			{
				modPlayer.shamanOrbSmall = ShamanOrbSmall.CRIMSON;
				modPlayer.orbCountSmall = 0;
			}
			modPlayer.orbCountSmall++;

			if (modPlayer.orbCountSmall == 1)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 20, 0f, 0f, Mod.Find<ModProjectile>("CrimsonOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountSmall++;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 25, 0f, 0f, Mod.Find<ModProjectile>("CrimsonOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				}
			}
			if (modPlayer.orbCountSmall == 2)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 25, 0f, 0f, Mod.Find<ModProjectile>("CrimsonOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountSmall == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 15, player.position.Y - 20, 0f, 0f, Mod.Find<ModProjectile>("CrimsonOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountSmall > 3)
			{
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(10, true);
				player.statLife += 10;
				modPlayer.orbCountSmall = 0;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			return false;
		}
	}
}