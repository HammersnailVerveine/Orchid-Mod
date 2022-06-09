using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{

	public class BloomingReactive : ModProjectile
	{
		float startX = 0;
		float startY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower");
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			// projectile.alpha = 64;
			Main.projFrames[Projectile.type] = 9;
		}

		// public override Color? GetAlpha(Color lightColor)
		// {
		// return Color.White;
		// }

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			Projectile.rotation += 0.02f;

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}
			if (modPlayer.alchemistFlower != Projectile.ai[1])
			{
				Projectile.ai[1] = modPlayer.alchemistFlower;
				Projectile.netUpdate = true;
			}
			Projectile.frame = (int)Projectile.ai[1];

			if (Projectile.ai[1] == 0)
				Projectile.active = false;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}
			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(80) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Content.Dusts.BloomingDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Content.Dusts.BloomingDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
			}
		}
	}
}
