using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class GeodeScepterProj : OrchidModShamanProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Cluster");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 45;
			this.projectileTrail = true;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			SoundEngine.PlaySound(SoundID.Item14);

			for (int i = 0; i < 3 + modPlayer.GetNbShamanicBonds(); i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(40));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("GeodeScepterProjAlt").Type, (int)(Projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
			}

			for (int i = 0; i < 10; i++)
			{

				int dustType = 60;
				switch (Main.rand.Next(3))
				{
					case 0:
						dustType = 60;
						break;
					case 1:
						dustType = 59;
						break;
					case 2:
						dustType = 62;
						break;
					default:
						break;
				}

				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale *= 1.5f * ((Main.rand.Next(20) + 5) / 10);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer) { }
	}
}