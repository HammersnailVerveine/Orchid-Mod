using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace OrchidMod.Content.Guardian
{
	public class HammerThrow : OrchidModProjectile
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public bool returning = false;
		public OrchidModGuardianHammer HammerItem;
		public Texture2D HammerTexture;

		public int range = 0;
		public bool penetrate;
		public bool hitTarget = false;
		public int dir;

		public bool WeakThrow() => Projectile.ai[0] == 1;

		public override void AltSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.timeLeft = 300;
			Projectile.alpha = 256;
			Projectile.tileCollide = false;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			Item item = player.inventory[player.selectedItem];

			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();

			if (item == null || !(item.ModItem is OrchidModGuardianHammer hammerItem))
			{
				Projectile.Kill();
				return;
			}
			else
			{
				HammerItem = hammerItem;
				HammerTexture = TextureAssets.Item[hammerItem.Item.type].Value;
				Projectile.width = HammerTexture.Width;
				Projectile.height = HammerTexture.Height;

				Projectile.position.X -= Projectile.width / 2;
				Projectile.position.Y -= Projectile.height / 2;

				this.range = HammerItem.range;
				this.penetrate = HammerItem.penetrate;
				Projectile.tileCollide = HammerItem.tileCollide;
			}

			dir = (Projectile.velocity.X > 0 ? 1 : -1);
		}
		
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();

			if (player.dead) Projectile.Kill();
			if (HammerItem != null)
			{
				if (HammerItem.ThrowAI(player, guardian, WeakThrow()))
				{

					if (WeakThrow())
						Projectile.rotation += 0.25f * dir;
					else
						Projectile.rotation += Projectile.velocity.Length() / 30f * (Projectile.velocity.X > 0 ? 1f : -1f) * 1.2f;

					OldPosition.Add(new Vector2(Projectile.Center.X, Projectile.Center.Y));
					OldRotation.Add(0f + Projectile.rotation);
					if (OldPosition.Count > 5)
						OldPosition.RemoveAt(0);
					if (OldRotation.Count > 5)
						OldRotation.RemoveAt(0);

					range--;

					if (range < 0)
					{
						float dist = Projectile.Center.Distance(player.Center);
						Vector2 vel = player.Center - Projectile.Center;
						vel.Normalize();
						if (range < -40)
						{
							vel *= 10f;
							Projectile.velocity = vel;
							Projectile.tileCollide = false;
						}
						else
						{
							vel *= 0.5f;
							Projectile.velocity += vel;
						}

						if (dist < 30f)
						{
							Projectile.Kill();
						}

						if (range < -100)
						{
							Projectile.friendly = false;
						}
					}
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			range = -40;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (this.HammerTexture == null) return false;

			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);
			var position = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
			var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

			spriteBatch.Draw(HammerTexture, position, null, color, Projectile.rotation, HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);

			for (int i = 0; i < OldPosition.Count; i ++)
			{
				color = Lighting.GetColor((int)(OldPosition[i].X / 16f), (int)(OldPosition[i].Y / 16f), Color.White) * (((WeakThrow() ? 0.05f : 0.15f) * i));
				position = OldPosition[i] - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
				spriteBatch.Draw(HammerTexture, position, null, color, OldRotation[i], HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}

			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			if (HammerItem != null)
			{
				bool weak = WeakThrow();
				if (!hitTarget)
				{
					if (!weak)
					{
						guardian.AddSlam(HammerItem.slamStacks);
						guardian.AddBlock(HammerItem.blockStacks);
					}
					hitTarget = true;
					HammerItem.OnThrowHitFirst(player, guardian, target, hit.Knockback, hit.Crit, weak);
				}
				HammerItem.OnThrowHit(player, guardian, target, hit.Knockback, hit.Crit, weak);
			}

			if (!penetrate)
			{
				range = -40;
			}
		}
	}
}