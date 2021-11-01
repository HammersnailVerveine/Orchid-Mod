using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Interfaces;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BoreanStriderScepterProj : OrchidModShamanProjectile, IDrawAdditive
	{
		public static readonly Color EffectColor = new Color(69, 144, 225);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borean Egg");

			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.timeLeft = 60;
		}

		public override void OnSpawn()
		{
			var trail = new Content.Trails.RoundedTrail(target: projectile, length: 16 * 10, width: (p) => 10 * (1 - p), color: (p) => EffectColor * (1 - p) * 0.5f, smoothness: 25);
			PrimitiveTrailSystem.NewTrail(trail);
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, EffectColor.ToVector3() * 0.35f);

			if (Main.rand.Next(7) == 0)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.2f;
				dust.noLight = true;
				dust.velocity = projectile.velocity;
			}

			projectile.velocity.Y = projectile.velocity.Y + 0.1f;
			if (projectile.velocity.Y > 16f) projectile.velocity.Y = 16f;

			projectile.rotation += Math.Sign(projectile.velocity.X) * 0.1f;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (projectile.soundDelay == 0)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Poof").WithPitchVariance(Main.rand.NextFloat(0.9f, 1f)), projectile.Center);
			}
			projectile.soundDelay = 10;

			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			for (int i = 0; i < nbBonds; i++)
			{
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X / (Main.rand.Next(3) + 2), -3f).RotatedByRandom(MathHelper.ToRadians(30));
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("BoreanStriderScepterProjAlt"), (int)(projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
			}

			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.5f;
				dust.noLight = true;
				dust.velocity = new Vector2(0, Main.rand.NextFloat(2f, 4f)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
			}

			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<BoreanStriderScepterKillProj>(), 0, 0.0f, player.whoAmI, Math.Sign(projectile.velocity.X) * 0.1f, projectile.rotation);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.BuffType("Freezing")), 2 * 60);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			var texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);

			// Trail
			for (int k = 1; k < projectile.oldPos.Length; k++)
			{
				Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height / 2);
				float progress = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Vector2 drawPosTrail = projectile.oldPos[k] - Main.screenPosition + projectile.Size * 0.5f + new Vector2(0f, projectile.gfxOffY);
				Color color = Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16), new Color(118, 184, 231)) * progress * 0.6f;

				spriteBatch.Draw(texture, drawPosTrail, rectangle, color, projectile.rotation + progress, origin, projectile.scale * progress, SpriteEffects.None, 0f);
			}

			// Projectile
			{
				int height = texture.Height / 2;
				float rotation = projectile.rotation + MathHelper.PiOver2;

				spriteBatch.Draw(texture, drawPos, new Rectangle(0, height, texture.Width, height), EffectColor * 0.75f, rotation, origin, projectile.scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, height), projectile.GetAlpha(lightColor), rotation, origin, projectile.scale, SpriteEffects.None, 0);
			}

			return false;
		}

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidHelper.GetExtraTexture(14);
			var drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);

			spriteBatch.Draw(texture, drawPos, null, EffectColor * 0.5f, projectile.timeLeft * 0.05f, texture.Size() * 0.5f, projectile.scale * 0.8f, SpriteEffects.None, 0);
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
			projectile.width = 2;
			projectile.height = 2;
			projectile.timeLeft = _timeLeft;
			projectile.damage = 0;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void OnSpawn()
		{
			projectile.rotation = projectile.ai[1];
		}

		public override void AI()
		{
			float progress = 1 - projectile.timeLeft / (float)_timeLeft;

			projectile.scale = OrchidHelper.GradientValue<float>(MathHelper.Lerp, progress, 1, 1.3f, 1.1f, 0.5f, 0.2f, 0f);
			projectile.rotation += projectile.ai[0];

			Lighting.AddLight(projectile.Center, BoreanStriderScepterProj.EffectColor.ToVector3() * 0.35f * (1 - progress));
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			var texture = Main.projectileTexture[projectile.type];
			int height = texture.Height / 2;
			float rotation = projectile.rotation + MathHelper.PiOver2;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);

			spriteBatch.Draw(texture, drawPos, new Rectangle(0, height, texture.Width, height), BoreanStriderScepterProj.EffectColor * 0.75f, rotation, origin, projectile.scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, height), projectile.GetAlpha(lightColor), rotation, origin, projectile.scale, SpriteEffects.None, 0);

			return false;
		}

		public override bool? CanCutTiles() => false;
		public override bool CanDamage() => false;

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidHelper.GetExtraTexture(14);
			var drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);

			spriteBatch.Draw(texture, drawPos, null, BoreanStriderScepterProj.EffectColor * 0.5f, projectile.timeLeft * 0.05f, texture.Size() * 0.5f, projectile.scale * 0.8f, SpriteEffects.None, 0);
		}
	}
}