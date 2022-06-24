using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class HellShamanRodProj : OrchidModShamanProjectile
	{
		private const float length = 16 * 47; // 47 tiles
		private Vector2 startPosition;

		private ref float Progress => ref Projectile.ai[0]; // 2 -> 0 -> 2 -> 0 ...

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Leaf");

			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1801;
			Projectile.penetrate = -1;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(250, 250, 250);

		public override void OnSpawn()
		{
			Projectile.rotation = (float)Math.PI / 3f * Projectile.ai[1];
			startPosition = Projectile.position;

			Progress = 0;
		}

		public override void AI()
		{
			if (Progress >= 0 && Projectile.ai[1] == -1) Progress += Projectile.velocity.Length() / length;

			Projectile.friendly = Projectile.ai[1] == -1;
			Projectile.rotation -= 0.15f;
			Projectile.position = Vector2.SmoothStep(startPosition + Vector2.Normalize(Projectile.velocity) * length, startPosition, Math.Abs(1 - Progress));

			if (Projectile.ai[1] == 0)
			{
				SoundEngine.PlaySound(SoundID.Item65, Projectile.position);
				Projectile.ai[1] = -1;
				Projectile.timeLeft = 1800;
			}

			if (Projectile.ai[1] > 0 && Projectile.timeLeft % 60 == 0) Projectile.ai[1]--;

			if (Progress >= 2) Progress = 0;

			if (Projectile.ai[1] == -1 && Main.rand.Next(15) == 0)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Content.Dusts.LeafDust>())];
				dust.customData = 1;
				dust.scale *= Main.rand.NextFloat(1.25f, 1.75f);
				dust.velocity = new Vector2(Vector2.Normalize(Projectile.velocity).X * -0.33f, Main.rand.NextFloat(0.2f, 0.45f));
			}

			Lighting.AddLight(Projectile.Center, new Vector3(255 / 255f, 180 / 255f, 0 / 255f) * 0.35f);
		}

		public override void Kill(int timeLeft)
		{
				SoundEngine.PlaySound(SoundID.Item65, Projectile.position);

			for (int i = 0; i < 5; i++)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Content.Dusts.LeafDust>())];
				dust.customData = 1;
				dust.scale *= Main.rand.NextFloat(1.25f, 1.75f);
				dust.velocity = new Vector2(Vector2.Normalize(Projectile.velocity).X * 0.2f + Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(0.2f, 0.45f));
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (!target.boss && target.CanBeChasedBy() && target.knockBackResist > 0f)
			{
				target.AddBuff(24, 60 * 5);

				var owner = Main.player[Projectile.owner];

				if (modPlayer.GetNbShamanicBonds() >= 2)
				{
					target.AddBuff(ModContent.BuffType<Buffs.Debuffs.LeafSlow>(), 60 * 5);
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix); // 1.3 line above

			Texture2D radialGradient = OrchidAssets.GetExtraTexture(11).Value;
			spriteBatch.Draw(radialGradient, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 225, 0) * 0.35f, 0f, radialGradient.Size() * 0.5f, 0.75f * Projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix); // 1.3 line above

			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				float num = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color color = Color.Lerp(new Color(100, 100, 100), new Color(250, 250, 250), num) * num * 0.75f;
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale * num, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}