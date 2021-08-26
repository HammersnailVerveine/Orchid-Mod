using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Circle
{

	public class GraniteEnergyScepterOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Flame");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				projectile.active = false;
			}

			projectile.rotation += 0.1f;

			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.5f;
			}

			if (player.GetModPlayer<OrchidModPlayer>().shamanOrbCircle != ShamanOrbCircle.GRANITE || player.GetModPlayer<OrchidModPlayer>().orbCountCircle <= 0)
				projectile.Kill();

			if (projectile.timeLeft == 12960000)
			{
				startX = projectile.position.X - player.position.X + player.velocity.X;
				startY = projectile.position.Y - player.position.Y + player.velocity.Y;
			}
			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + startX;
			projectile.position.Y = player.position.Y + startY;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
			}
		}
	}
}
