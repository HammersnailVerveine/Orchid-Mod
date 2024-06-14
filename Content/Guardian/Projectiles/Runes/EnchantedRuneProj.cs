using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class EnchantedRuneProj : GuardianRuneProjectile
	{
		private int animDirection;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public Texture2D texture;

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Enchanted Sparkle");
		}

		public override void SafeOnSpawn(IEntitySource source)
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
			Projectile.frame = Distance > 100f ? 0 : 1;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			texture = TextureAssets.Projectile[this.Type].Value;
		}

		public override bool SafeAI()
		{
			Spin(Projectile.frame == 0 ? 1.2f : -1.7f);
			Projectile.rotation += (Projectile.frame == 0 ? 0.11f : 0.16f) * animDirection;

			if (guardian.modPlayer.timer120 % 2 == 0)
			{
				OldPosition.Add(new Vector2(Projectile.Center.X, Projectile.Center.Y));
				OldRotation.Add(0f + Projectile.rotation);
				if (OldPosition.Count > 5)
					OldPosition.RemoveAt(0);
				if (OldRotation.Count > 5)
					OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(2))
			{
				int type = Main.rand.Next(3);
				if (type == 0) type = DustID.Enchanted_Gold;
				else type = Projectile.frame == 0 ? DustID.MagicMirror : DustID.Enchanted_Pink;
				int dust = Dust.NewDust(Projectile.Center - new Vector2(5, 5), 10, 10, type);
				Main.dust[dust].velocity *= 0.2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				if (type != DustID.Enchanted_Gold)
					Main.dust[dust].scale *= 1.5f;
			}

			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (this.texture == null) return false;

			Rectangle rect = texture.Bounds;
			rect.Height /= 2;
			rect.Y += Projectile.frame * rect.Height;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				var color = Lighting.GetColor((int)(OldPosition[i].X / 16f), (int)(OldPosition[i].Y / 16f), Color.White) * 0.2f * i;
				var position = OldPosition[i] - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
				spriteBatch.Draw(texture, position, rect, color, OldRotation[i], rect.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}

			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int type = Main.rand.Next(3);
				if (type == 0) type = DustID.Enchanted_Gold;
				else type = Projectile.frame == 0 ? DustID.MagicMirror : DustID.Enchanted_Pink;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				if (type != DustID.Enchanted_Gold)
					Main.dust[dust].scale *= 1.5f;
			}
		}
	}
}