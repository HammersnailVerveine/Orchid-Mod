using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Graphics;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class WyvernMorayProjLingering : OrchidModShamanProjectile
	{
		public bool Improved { get => Projectile.ai[0] == 1; set => Projectile.ai[0] = value.ToInt(); }
		public float Opacity { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }

		public Color effectColor = new Color(113, 187, 162);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wyvern Dusts");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Improved) modifiers.FinalDamage *= 2f;
		}

		public override void AI()
		{
			//this.Opacity = 1 - (float)Math.Pow(MathHelper.Lerp(0.0f, 1.0f, Math.Abs(1 - (projectile.timeLeft / 90f))), 5f);
			this.Opacity = 1 - (float)Math.Pow(MathHelper.Lerp(0.0f, 1.0f, (1 - Projectile.timeLeft / 180f)), 5f); // WTF!!!

			Lighting.AddLight(Projectile.Center, effectColor.ToVector3() * 0.4f * this.Opacity);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Texture2D texture = OrchidAssets.GetExtraTexture(11).Value;

			Effect effect = OrchidAssets.GetEffect("WyvernMorayLingering").Value;
			effect.Parameters["Time"].SetValue(Main.GlobalTimeWrappedHourly * 0.1f + Projectile.position.X * 2);

			Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, effectColor * this.Opacity);
			Vector2 origin = texture.Size() * 0.5f;
			float scale = Projectile.scale * 2.25f;

			SpriteBatchInfo sbInfo = new(spriteBatch);

			spriteBatch.End();
			sbInfo.Begin(spriteBatch, SpriteSortMode.Immediate, BlendState.Additive, effect);

			for (int i = 0; i < 2; i++) spriteBatch.Draw(texture, drawPos, null, color, 0f, origin, scale, SpriteEffects.None, 0);

			spriteBatch.End();
			sbInfo.Begin(spriteBatch);

			return false;
		}
	}
}