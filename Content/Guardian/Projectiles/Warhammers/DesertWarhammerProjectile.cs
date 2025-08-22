using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Warhammers
{
	public class DesertWarhammerProjectile : OrchidModGuardianProjectile
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 240;
			Projectile.penetrate = 2;
            Main.projFrames[Projectile.type] = 6;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.frame = Math.Abs(Main.rand.Next(6) - Main.rand.Next(6));
		}

		public override void AI()
		{
			Projectile.rotation += Projectile.velocity.X * 0.15f;
			Projectile.velocity.X *= 0.99f;
			if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y < 0) Projectile.velocity.Y *= 0.99f;
			OldPosition.Add(new Vector2(Projectile.Center.X, Projectile.Center.Y));
			OldRotation.Add(0f + Projectile.rotation);
			if (OldPosition.Count > 3)
				OldPosition.RemoveAt(0);
			if (OldRotation.Count > 3)
				OldRotation.RemoveAt(0);
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.frame != 5)
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.Gold : DustID.Dirt, Projectile.velocity.X, -1);
					dust.velocity *= 0.5f;
				}
			else
				for (int i = 0; i < 6; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmber, Projectile.velocity.X, -1, 100, Scale:0.5f);
					dust.velocity *= 0.8f;
				}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate < 2) return true;
			Projectile.penetrate--;
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X *= -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = Math.Max(-oldVelocity.Y,-5);
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			for (int i = 0; i < OldPosition.Count; i++)
				{
					var color = Lighting.GetColor((int)(OldPosition[i].X / 16f), (int)(OldPosition[i].Y / 16f), Color.White) * Math.Min(Projectile.velocity.Length() * 0.05f, 0.2f) * i;
					var position = OldPosition[i] - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;

					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, position, new Rectangle(0, Projectile.frame * 16, 14, 14), color, OldRotation[i], new Vector2(7), Projectile.scale, SpriteEffects.None, 0f);
				}
			return true;
		}
	}
}