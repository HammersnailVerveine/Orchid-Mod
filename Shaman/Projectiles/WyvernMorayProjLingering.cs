using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Effects;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class WyvernMorayProjLingering : OrchidModShamanProjectile
	{
		public bool Improved { get => projectile.ai[0] == 1; set => projectile.ai[0] = value.ToInt(); }
		public float Opacity { get => projectile.ai[1]; set => projectile.ai[1] = value; }

		public Color effectColor = new Color(113, 187, 162);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Dusts");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 100;
			projectile.height = 100;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.tileCollide = false;

		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Improved) damage += damage;
		}

		public override void AI()
		{
			//this.Opacity = 1 - (float)Math.Pow(MathHelper.Lerp(0.0f, 1.0f, Math.Abs(1 - (projectile.timeLeft / 90f))), 5f);
			this.Opacity = 1 - (float)Math.Pow(MathHelper.Lerp(0.0f, 1.0f, (1 - projectile.timeLeft / 180f)), 5f); // WTF!!!

			Lighting.AddLight(projectile.Center, effectColor.ToVector3() * 0.4f * this.Opacity);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			Texture2D texture = OrchidHelper.GetExtraTexture(11);

			Effect effect = EffectsManager.WyvernMorayLingeringEffect;
			effect.Parameters["time"].SetValue(Main.GlobalTime * 0.1f + projectile.position.X * 2);

			Color color = Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, effectColor * this.Opacity);
			Vector2 origin = texture.Size() * 0.5f;
			float scale = projectile.scale * 2.25f;

			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive, effect: effect);
			{
				for (int i = 0; i < 2; i++) spriteBatch.Draw(texture, drawPos, null, color, 0f, origin, scale, SpriteEffects.None, 0);
			}
			SetSpriteBatch(spriteBatch: spriteBatch);

			return false;
		}
	}
}