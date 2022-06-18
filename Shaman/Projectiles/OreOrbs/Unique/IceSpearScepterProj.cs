using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IceSpearScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 30;
			Projectile.penetrate = 1;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spear");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 3 == 0)
				Projectile.frame++;
			if (Projectile.frame == 4)
				Projectile.frame = 0;

			if (Projectile.timeLeft == 30)
			{
				for (int i = 0; i < 3; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67);
					Main.dust[dust].velocity = -Projectile.velocity / 5;
					Main.dust[dust].scale = 1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = false;
				}
			}

			if (Main.rand.Next(6) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.ICE)
			{
				modPlayer.shamanOrbUnique = ShamanOrbUnique.ICE;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique++;
			//modPlayer.sendOrbCountPackets();

			if (modPlayer.orbCountUnique == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("IceSpearOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 3;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("IceSpearOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
				player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				//modPlayer.sendOrbCountPackets();
			}

			if (modPlayer.orbCountUnique == 10)
			{

				modPlayer.orbCountUnique = 0;

				float speedX = 1f;
				int angle = 22;
				int dmg = (int)(Projectile.damage + 35);

				if (Projectile.velocity.X < 0)
				{
					speedX = -speedX;
					angle = -angle;
				}

				Vector2 spearVelocity = (new Vector2(speedX, 0f).RotatedBy(MathHelper.ToRadians(angle)));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, spearVelocity.X, spearVelocity.Y, Mod.Find<ModProjectile>("IceSpearProj").Type, dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
			}
		}
	}
}