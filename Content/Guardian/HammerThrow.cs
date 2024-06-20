using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

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

		public bool Ding = false;

		public bool WeakThrow() => Projectile.ai[0] == 1;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overPlayers.Add(index);

		public override void AltSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.timeLeft = 600;
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

				range = HammerItem.range;
				penetrate = HammerItem.penetrate;
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
				if (Projectile.ai[1] == 0) // Held
				{
					player.itemAnimation = 1;
					Projectile.timeLeft = 600;
					Projectile.spriteDirection = -player.direction;
					player.heldProj = Projectile.whoAmI;

					player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi + guardian.GuardianThrowCharge * 0.006f * Projectile.spriteDirection); // set arm position (90 degree offset since arm starts lowered)
					Vector2 armPosition = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - guardian.GuardianThrowCharge * 0.006f * Projectile.spriteDirection);
					Projectile.Center = armPosition - new Vector2((Projectile.width + 0.3f * guardian.GuardianThrowCharge + (float)Math.Sin(MathHelper.Pi / 210f * guardian.GuardianThrowCharge) * 10f) * player.direction * 0.4f, (Projectile.height - (Projectile.height * 0.007f)  * guardian.GuardianThrowCharge) * 0.4f);

					if (Main.MouseWorld.X > player.Center.X && player.direction != 1) player.ChangeDir(1);
					else if (Main.MouseWorld.X < player.Center.X && player.direction != -1) player.ChangeDir(-1);

					if (guardian.GuardianThrowCharge < 210) guardian.GuardianThrowCharge++;

					if (guardian.GuardianThrowCharge >= 180 && !Ding)
					{
						Ding = true;
						SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
					}

					if (!player.controlUseItem && player.whoAmI == Main.myPlayer)
					{
						Projectile.ai[1] = 1;
						Projectile.friendly = true;

						Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * HammerItem.Item.shootSpeed;

						if (guardian.ThrowLevel() < 4)
						{
							dir *= (0.3f * (guardian.ThrowLevel() + 2) / 3);
							Projectile.damage = (int)(Projectile.damage / 3f);
							Projectile.knockBack = (int)(Projectile.knockBack / 3f);
							Projectile.ai[0] = 1f;
						}

						Projectile.velocity = dir;
						Projectile.rotation = dir.ToRotation();
						Projectile.direction = Projectile.spriteDirection;
						Projectile.netUpdate = true;

						SoundEngine.PlaySound(HammerItem.Item.UseSound, player.Center);
						guardian.GuardianThrowCharge = 0;
					}
				}
				else // Thrown
				{
					if (HammerItem.ThrowAI(player, guardian, Projectile, WeakThrow()))
					{
						if (Projectile.timeLeft < 598 && HammerItem.tileCollide && range > 0) // Delay helps preventing the hammer from instantly despawning if launched from inside a tile
						{ // Hammer has a smaller hitbox for tilecollide stuff
							Vector2 collideVelocity = Collision.TileCollision(Projectile.Center - Vector2.One * 10f, Projectile.velocity, 20, 20, true, true, (int)player.gravDir);
							if (collideVelocity != Projectile.velocity)
							{
								OnTileCollide(Projectile.velocity);
							}
						}

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
								float mult = 10f;
								if (Projectile.timeLeft < 500) mult += (500 - Projectile.timeLeft) / 40f;
								vel *= mult;
								Projectile.velocity = vel;
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
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			range = -40;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (this.HammerTexture == null) return false;

			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);
			var position = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
			var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			float rotationBonus = 0f;
			if (Projectile.ai[1] == 0)
			{
				Player player = Main.player[Projectile.owner];
				OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
				rotationBonus += guardian.GuardianThrowCharge * 0.0065f * Projectile.spriteDirection;
			}

			spriteBatch.Draw(HammerTexture, position, null, color, Projectile.rotation + rotationBonus, HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);


			if (Projectile.ai[1] != 0)
			{
				for (int i = 0; i < OldPosition.Count; i++)
				{
					color = Lighting.GetColor((int)(OldPosition[i].X / 16f), (int)(OldPosition[i].Y / 16f), Color.White) * (((WeakThrow() ? 0.05f : 0.15f) * i));
					position = OldPosition[i] - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
					spriteBatch.Draw(HammerTexture, position, null, color, OldRotation[i], HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
				}
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
					HammerItem.OnThrowHitFirst(player, guardian, target, Projectile, hit.Knockback, hit.Crit, weak);
				}
				HammerItem.OnThrowHit(player, guardian, target, Projectile, hit.Knockback, hit.Crit, weak);
			}

			if (!penetrate)
			{
				range = -40;
			}
		}
	}
}