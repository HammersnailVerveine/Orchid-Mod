using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class HellShamanRodProj : OrchidModShamanProjectile
	{
		private const float length = 16 * 47; // 47 tiles
		private Vector2 startPosition;

		private ref float Progress => ref projectile.ai[0]; // 2 -> 0 -> 2 -> 0 ...

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Leaf");

			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
		} 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1801;
            projectile.penetrate = -1;

			empowermentType = 4;
			empowermentLevel = 2;
			spiritPollLoad = 0;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(250, 250, 250);

		public override void OnSpawn()
		{
			projectile.rotation = (float)Math.PI / 3f * projectile.ai[1];
			startPosition = projectile.position;

			Progress = 0;
		}

		public override void AI()
        {
			if (Progress >= 0 && projectile.ai[1] == -1) Progress += projectile.velocity.Length() / length;

			projectile.friendly = projectile.ai[1] == -1;
			projectile.rotation -= 0.15f;
			projectile.position = Vector2.SmoothStep(startPosition + Vector2.Normalize(projectile.velocity) * length, startPosition, Math.Abs(1 - Progress));

			if (projectile.ai[1] == 0)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 65);
				projectile.ai[1] = -1;
				projectile.timeLeft = 1800;
			}

			if (projectile.ai[1] > 0 && projectile.timeLeft % 60 == 0) projectile.ai[1]--;

			if (Progress >= 2) Progress = 0;

			if (projectile.ai[1] == -1 && Main.rand.Next(15) == 0)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.LeafDust>())];
				dust.customData = 1;
				dust.scale *= Main.rand.NextFloat(1.25f, 1.75f);
				dust.velocity = new Vector2(Vector2.Normalize(projectile.velocity).X * -0.33f, Main.rand.NextFloat(0.2f, 0.45f));
			}

			Lighting.AddLight(projectile.Center, new Vector3(255 / 255f, 180 / 255f, 0 / 255f) * 0.35f);
		}
		
		public override void Kill(int timeLeft)
        {
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 65);

			for (int i = 0; i < 5; i++)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.LeafDust>())];
				dust.customData = 1;
				dust.scale *= Main.rand.NextFloat(1.25f, 1.75f);
				dust.velocity = new Vector2(Vector2.Normalize(projectile.velocity).X * 0.2f + Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(0.2f, 0.45f));
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (!target.boss && target.CanBeChasedBy() && target.knockBackResist > 0f)
			{
				target.AddBuff(24, 60 * 5);

				var owner = Main.player[projectile.owner];

				if (OrchidModShamanHelper.getNbShamanicBonds(owner, owner.GetModPlayer<OrchidModPlayer>(), mod) >= 2)
				{
					target.AddBuff(ModContent.BuffType<Buffs.Debuffs.LeafSlow>(), 60 * 5);
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D radialGradient = Effects.EffectsManager.RadialGradientTexture;
			spriteBatch.Draw(radialGradient, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, new Color(255, 225, 0) * 0.35f, 0f, radialGradient.Size() * 0.5f, 0.75f * projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);

			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				float num = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Color color = Color.Lerp(new Color(100, 100, 100), new Color(250, 250, 250), num) * num * 0.75f;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.oldRot[k], drawOrigin, projectile.scale * num, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}