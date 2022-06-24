using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Circle
{

	public class ReviverofSoulsFlame : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		float hoverY = 0;
		bool hoverD = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Flame");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 26;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 7;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
				switch (Main.rand.Next(5))
				{
					case 1:
						Projectile.scale = 1.1f;
						break;
					case 2:
						Projectile.scale = 1.2f;
						break;
					case 3:
						Projectile.scale = 1.3f;
						break;
					case 4:
						Projectile.scale = 1.4f;
						break;
					case 5:
						Projectile.scale = 1.5f;
						break;
				}
			else
				Projectile.scale = 1f;
			if (Main.time % 5 == 0)
				Projectile.frame++;
			if (Projectile.frame == 7)
				Projectile.frame = 0;

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 % 60 == 0)
				hoverD = !hoverD;

			if (hoverD == false)
				hoverY -= 0.3f;
			else hoverY += 0.3f;

			if (modPlayer.shamanOrbCircle != ShamanOrbCircle.REVIVER || modPlayer.orbCountCircle <= 0)
				Projectile.Kill();

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X + player.velocity.X;
				startY = Projectile.position.Y - player.position.Y + player.velocity.Y;
			}
			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY - hoverY;

			if (Main.player[Projectile.owner].FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
			{
				if (Main.rand.NextBool(10))
				{
					int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
					Main.dust[dust2].velocity *= 2f;
					Main.dust[dust2].scale = 1.5f;
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].noLight = true;
				}
			}
		}

		public override void SafePostAI()
		{
			for (int num46 = Projectile.oldPos.Length - 5; num46 > 0; num46--)
			{
				Projectile.oldPos[num46] = Projectile.oldPos[num46 - 1];
			}
			Projectile.oldPos[0] = Projectile.position;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D flameTexture = ModContent.Request<Texture2D>("OrchidMod/Shaman/Projectiles/OreOrbs/Circle/ReviverOfSoulsFlameTexture").Value;
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 1f, Projectile.height * 1f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				drawPos.X += Main.rand.Next(6) - 3 - Main.player[Projectile.owner].velocity.X;
				drawPos.Y += Main.rand.Next(6) - 3 - Main.player[Projectile.owner].velocity.Y;
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k * 5) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(flameTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.3f);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
			}
		}
	}
}

