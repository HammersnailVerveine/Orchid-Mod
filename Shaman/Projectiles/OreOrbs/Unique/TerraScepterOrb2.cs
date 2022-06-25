using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class TerraScepterOrb2 : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Scepter Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 10;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 % 5 == 0)
				Projectile.frame++;
			if (Projectile.frame == 10)
				Projectile.frame = 0;

			if (player.GetModPlayer<OrchidShaman>().orbCountUnique == 0 || player.GetModPlayer<OrchidShaman>().orbCountUnique > 14 || player.GetModPlayer<OrchidShaman>().shamanOrbUnique != ShamanOrbUnique.TERRA)
				Projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidShaman>().orbCountUnique;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}
			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(16) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 157);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.2f;

			}
			if (Main.rand.Next(16) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.2f;
			}

		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 157);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 5f;
			}

			Player player = Main.player[Projectile.owner];
			int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(50 + 20 * orbsNumber);
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique < 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, (Main.rand.Next(20) - 10) * 1f, -5f, Mod.Find<ModProjectile>("TerraScepterOrbHoming2").Type, dmg, 0f, Projectile.owner, 0f, 0f);
		}
	}
}
