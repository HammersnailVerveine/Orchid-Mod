using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Alchemist.Projectiles.Sigil
{
	public abstract class AlchemistSigil : OrchidModAlchemistProjectile
	{
		protected static AlchemistElement element;
		private int animDirection;

		protected abstract Texture2D GetOutlineTexture();

		public override void SafeSetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 18000; // 5 minutes
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public override void OnSpawn(IEntitySource source)
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
		}

		public override void AI()
		{
			Projectile.rotation += (0.05f * (0.2f - Math.Abs(Projectile.rotation)) + 0.001f) * animDirection;
			if (Math.Abs(Projectile.rotation) >= 0.2f)
			{
				Projectile.rotation = 0.2f * animDirection;
				animDirection *= -1;
			}
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D outlineTexture = GetOutlineTexture();
			if (outlineTexture == null) return;
			Color color = lightColor * (0.25f + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().Timer120 - 60) / 90f));
			color.R += 20;
			color.G += 20;
			color.B += 20;
			Vector2 offset = new Vector2(outlineTexture.Width / 2, outlineTexture.Height / 2);
			Main.spriteBatch.Draw(outlineTexture, Projectile.position + offset - Main.screenPosition, null, color, Projectile.rotation, offset, Projectile.scale, SpriteEffects.None, 0f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			return false;
		}
	}
}