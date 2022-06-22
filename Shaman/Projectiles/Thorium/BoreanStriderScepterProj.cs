using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Interfaces;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BoreanStriderScepterProj : OrchidModShamanProjectile, IDrawAdditive
	{
		public static readonly SoundStyle PoofSound = new(OrchidAssets.SoundsPath + "Poof") { PitchRange = (0.9f, 1f) };
		public static readonly Color EffectColor = new(69, 144, 225);

		// ...

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borean Egg");

			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.timeLeft = 60;
		}

		/* [SP]
		public override void OnSpawn()
		{
			var trail = new Content.Trails.RoundedTrail(target: Projectile, length: 16 * 10, width: (p) => 10 * (1 - p), color: (p) => EffectColor * (1 - p) * 0.5f, smoothness: 25);
			PrimitiveTrailSystem.NewTrail(trail);
		}
		*/

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, EffectColor.ToVector3() * 0.35f);

			if (Main.rand.Next(7) == 0)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.2f;
				dust.noLight = true;
				dust.velocity = Projectile.velocity;
			}

			Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
			if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;

			Projectile.rotation += Math.Sign(Projectile.velocity.X) * 0.1f;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (Projectile.soundDelay == 0)
			{
				SoundEngine.PlaySound(PoofSound, Projectile.Center);
			}
			Projectile.soundDelay = 10;

			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			for (int i = 0; i < nbBonds; i++)
			{
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X / (Main.rand.Next(3) + 2), -3f).RotatedByRandom(MathHelper.ToRadians(30));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("BoreanStriderScepterProjAlt").Type, (int)(Projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
			}

			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.5f;
				dust.noLight = true;
				dust.velocity = new Vector2(0, Main.rand.NextFloat(2f, 4f)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
			}

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<BoreanStriderScepterKillProj>(), 0, 0.0f, player.whoAmI, Math.Sign(Projectile.velocity.X) * 0.1f, Projectile.rotation);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("Freezing").Type), 2 * 60);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			var texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);

			// Trail
			for (int k = 1; k < Projectile.oldPos.Length; k++)
			{
				Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height / 2);
				float progress = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPosTrail = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size * 0.5f + new Vector2(0f, Projectile.gfxOffY);
				Color color = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), new Color(118, 184, 231)) * progress * 0.6f;

				spriteBatch.Draw(texture, drawPosTrail, rectangle, color, Projectile.rotation + progress, origin, Projectile.scale * progress, SpriteEffects.None, 0f);
			}

			// Projectile
			{
				int height = texture.Height / 2;
				float rotation = Projectile.rotation + MathHelper.PiOver2;

				spriteBatch.Draw(texture, drawPos, new Rectangle(0, height, texture.Width, height), EffectColor * 0.75f, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, height), Projectile.GetAlpha(lightColor), rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}

			return false;
		}

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidAssets.GetExtraTexture(14).Value;
			var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

			spriteBatch.Draw(texture, drawPos, null, EffectColor * 0.5f, Projectile.timeLeft * 0.05f, texture.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.None, 0);
		}
	}

	public class BoreanStriderScepterKillProj : OrchidModShamanProjectile, IDrawAdditive
	{
		private static readonly int _timeLeft = 20;

		public override string Texture => "OrchidMod/Shaman/Projectiles/Thorium/BoreanStriderScepterProj";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borean Egg");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.timeLeft = _timeLeft;
			Projectile.damage = 0;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void OnSpawn()
		{
			Projectile.rotation = Projectile.ai[1];
		}

		public override void AI()
		{
			float progress = 1 - Projectile.timeLeft / (float)_timeLeft;

			Projectile.scale = MathUtils.MultiLerp<float>(MathHelper.Lerp, progress, 1, 1.3f, 1.1f, 0.5f, 0.2f, 0f);
			Projectile.rotation += Projectile.ai[0];

			Lighting.AddLight(Projectile.Center, BoreanStriderScepterProj.EffectColor.ToVector3() * 0.35f * (1 - progress));
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			var texture = TextureAssets.Projectile[Projectile.type].Value;
			int height = texture.Height / 2;
			float rotation = Projectile.rotation + MathHelper.PiOver2;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);

			spriteBatch.Draw(texture, drawPos, new Rectangle(0, height, texture.Width, height), BoreanStriderScepterProj.EffectColor * 0.75f, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, height), Projectile.GetAlpha(lightColor), rotation, origin, Projectile.scale, SpriteEffects.None, 0);

			return false;
		}

		public override bool? CanCutTiles() => false;
		public override bool? CanDamage()/* Suggestion: Return null instead of false */ => false;

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidAssets.GetExtraTexture(14).Value;
			var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

			spriteBatch.Draw(texture, drawPos, null, BoreanStriderScepterProj.EffectColor * 0.5f, Projectile.timeLeft * 0.05f, texture.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.None, 0);
		}
	}
}