using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Gambler.Projectiles
{
	public class BubbleCardProj : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bubble");
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			Projectile.rotation += 0.02f;
			Projectile.velocity.X *= 0.98f;
			Projectile.damage += (Projectile.timeLeft % 20 == 0 && Projectile.timeLeft > 150) ? 1 : 0;

			if (Projectile.ai[1] <= 0f)
			{
				Projectile.velocity.Y -= 0.012f;
				if (Main.myPlayer == Projectile.owner)
				{
					if (!Main.mouseLeft && modPlayer.GamblerDeckInHand)
					{
						Vector2 newMove = Main.MouseWorld - Projectile.Center;
						newMove.Normalize();
						newMove *= 10f;
						Projectile.velocity = newMove;
						Projectile.ai[1] = 1f; ;
						Projectile.netUpdate = true;
					}
				}
			}
			else
			{
				Projectile.velocity.Y *= 0.98f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 217);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
			SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(44, 60 * 5); // Frostburn
			}
		}
	}
}