using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Projectiles.Earth
{
	public class GemScepterProjectileAmber : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0) Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);

			if (Main.rand.NextBool(5))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			if (Projectile.timeLeft < 30 || Projectile.penetrate < 1)
			{
				Projectile.velocity *= 0.9f;
			}

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 1) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Projectile.timeLeft > 30) Projectile.timeLeft = 30;
			Projectile.penetrate = -1;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			int type = ModContent.ProjectileType<GemScepterBreak>();
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * 0.15f * (i + 1), OldRotation[i] + (float)Math.Pow(TimeSpent, 2.2) * 0.001f, TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * (0.2f + TimeSpent * 0.003f), SpriteEffects.None, 0f);
			}

			return false;
		}
	}
}
