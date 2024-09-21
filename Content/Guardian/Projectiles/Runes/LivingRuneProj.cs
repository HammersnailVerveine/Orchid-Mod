using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class LivingRuneProj : GuardianRuneProjectile
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
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void SafeOnSpawn(IEntitySource source)
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			texture = TextureAssets.Projectile[this.Type].Value;
		}

		public override bool SafeAI()
		{
			Spin(1.2f);

			if (guardian.modPlayer.Timer120 % 2 == 0)
			{
				OldPosition.Add(new Vector2(Projectile.Center.X, Projectile.Center.Y));
				OldRotation.Add(0f + Projectile.rotation);
				if (OldPosition.Count > 3)
					OldPosition.RemoveAt(0);
				if (OldRotation.Count > 3)
					OldRotation.RemoveAt(0);
			}

			if (Projectile.velocity.Y < 0.5f) Projectile.velocity.Y += 0.02f;
			Projectile.velocity.X *= 0.95f;
			Projectile.rotation += (0.05f * (0.2f - Math.Abs(Projectile.rotation)) + 0.001f) * animDirection;
			if (Math.Abs(Projectile.rotation) >= 0.2f)
			{
				Projectile.rotation = 0.2f * animDirection;
				animDirection *= -1;
			}

			if (Main.rand.NextBool(20))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DesertWater2);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}

			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (this.texture == null) return false;

			for (int i = 0; i < OldPosition.Count; i ++)
			{
				var color = Lighting.GetColor((int)(OldPosition[i].X / 16f), (int)(OldPosition[i].Y / 16f), Color.White) * 0.1f * i;
				var position = OldPosition[i] - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
				spriteBatch.Draw(texture, position, null, color, OldRotation[i], texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}

			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DesertWater2);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}
	}
}