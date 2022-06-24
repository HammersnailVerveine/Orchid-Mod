using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class HoneyProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey Magic");
		}

		public override void AI()
		{
			if ((Projectile.velocity.X > 0.001f || Projectile.velocity.X < -0.001f) || (Projectile.velocity.Y > 0.001f || Projectile.velocity.Y < -0.001f))
			{
				Projectile.rotation += 0.23f;
				Projectile.velocity.Y += 0.1f;
			}

			// if (Main.rand.Next(20) < projectile.timeLeft) {
			// Vector2 Position = projectile.position;
			// int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 153);
			// Main.dust[index2].scale = 1f;
			// Main.dust[index2].velocity *= 1f;
			// Main.dust[index2].noGravity = true;
			// }

			if (Main.rand.Next(6) == 0)
			{
				Vector2 Position = Projectile.position;
				int index2 = Dust.NewDust(Position, Projectile.width, Projectile.height, 152);
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 1f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if ((Projectile.velocity.X > 0.001f || Projectile.velocity.X < -0.001f) || (Projectile.velocity.Y > 0.001f || Projectile.velocity.Y < -0.001f))
			{
				Projectile.position += Projectile.velocity * 3;
			}

			Projectile.velocity *= 0f;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(10);

			for (int i = 0; i < 13; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].noGravity = true;
			}

			for (int i = 0; i < modPlayer.GetNbShamanicBonds(); i++)
			{
				if (Main.rand.Next(4) == 0)
				{
					if (player.strongBees && Main.rand.Next(2) == 0)
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 3 - Main.rand.Next(6), 3 - Main.rand.Next(6), 566, (int)(dmg * 1.15f), 0f, Projectile.owner, 0f, 0f);
					else
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 3 - Main.rand.Next(6), 3 - Main.rand.Next(6), 181, dmg, 0f, Projectile.owner, 0f, 0f);
					}
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.HONEY)
			{
				modPlayer.shamanOrbUnique = ShamanOrbUnique.HONEY;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique++;
			//modPlayer.sendOrbCountPackets();

			if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("HoneyOrb1").Type, 0, 0, Projectile.owner, 0f, 0f);
				player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				//modPlayer.sendOrbCountPackets();
			}

			if (modPlayer.orbCountUnique == 5)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("HoneyOrb1").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 10)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("HoneyOrb2").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("HoneyOrb3").Type, 0, 0, Projectile.owner, 0f, 0f);
		}
	}
}