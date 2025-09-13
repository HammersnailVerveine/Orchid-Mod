using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GuardianHammerAnchor : OrchidModGuardianAnchor
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public bool returning = false;
		public OrchidModGuardianHammer HammerItem;
		public Texture2D HammerTexture;

		public int range = 0;
		public int HitCount = 0;
		public bool penetrate;
		public bool WeakHit = false;
		public bool NeedNetUpdate = false;
		public int dir;
		public int hitboxOffset;

		public int BlockDuration = 0;

		public bool Ding = false;

		public bool WeakThrow => Projectile.ai[0] == 1;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overPlayers.Add(index);

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			Item item = player.inventory[player.selectedItem];


			if (item == null || !(item.ModItem is OrchidModGuardianHammer hammerItem))
			{
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.Kill();
				}
				return;
			}
			else
			{
				HammerItem = hammerItem;
				HammerTexture = TextureAssets.Item[hammerItem.Item.type].Value;
				//Projectile.width = (int)(HammerTexture.Width * hammerItem.Item.scale);
				//Projectile.height = (int)(HammerTexture.Height * hammerItem.Item.scale);
				hitboxOffset = (int)(HammerTexture.Width * hammerItem.Item.scale / 2f);
				DrawOriginOffsetX = DrawOriginOffsetY = hitboxOffset;

				Projectile.scale = hammerItem.Item.scale;

				//Projectile.position.X -= Projectile.width / 2;
				//Projectile.position.Y -= Projectile.height / 2;

				range = HammerItem.Range;
				penetrate = HammerItem.Penetrate;
				Projectile.netUpdate = true;
				Projectile.localNPCHitCooldown = hammerItem.HitCooldown;
			}

			dir = (Projectile.velocity.X > 0 ? 1 : -1);
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();

			if (HammerItem != null)
			{
				if (NeedNetUpdate)
				{
					NeedNetUpdate = false;
					Projectile.netUpdate = true;
				}

				Projectile.rotation += Projectile.velocity.Length() / 45f * dir;

				if (BlockDuration != 0)
				{ // Blocking
					if (BlockDuration <= HammerItem.BlockDuration * HammerItem.Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianBlockDuration)
					{ // hammers only starts to slow down 15 frames after being thrown
						Projectile.rotation += 0.25f * dir;

						if (BlockDuration > 0)
						{
							Projectile.velocity *= 0.9f;
							Projectile.timeLeft++;
						}
						else
						{
							float dist = Projectile.Center.Distance(player.Center);
							Vector2 vel = Vector2.Normalize(player.Center - Projectile.Center) * HammerItem.ReturnSpeed * BlockDuration * 0.2f;
							Projectile.velocity = -vel;

							if (dist < 30f && player.whoAmI == Main.myPlayer)
							{
								Projectile.Kill();
							}
						}
					}

					OldPosition.Add(new Vector2(Projectile.Center.X, Projectile.Center.Y));
					OldRotation.Add(0f + Projectile.rotation);
					if (OldPosition.Count > 10)
						OldPosition.RemoveAt(0);
					if (OldRotation.Count > 10)
						OldRotation.RemoveAt(0);

					BlockDuration--;

					if (BlockDuration == 0)
					{
						BlockDuration = -1;
					}
				}
				else if (Projectile.ai[1] <= 0) // Held
				{
					if (player.dead || player.HeldItem.ModItem is not OrchidModGuardianHammer)
					{
						if (Projectile.owner == Main.myPlayer)
						{
							Projectile.Kill();
						}
					}
					else
					{
						if (Projectile.owner == Main.myPlayer)
						{
							if (Main.MouseWorld.X > player.Center.X && player.direction != 1) player.ChangeDir(1);
							else if (Main.MouseWorld.X < player.Center.X && player.direction != -1) player.ChangeDir(-1);
						}

						player.itemAnimation = 1;
						Projectile.timeLeft = 600;
						Projectile.spriteDirection = -player.direction;
						player.heldProj = Projectile.whoAmI;

						if (guardian.GuardianItemCharge >= 180f && !Ding)
						{
							Ding = true;
							if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, player.Center);
							else SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
						}

						if (Projectile.ai[1] == 0)
						{
							if (WeakHit)
							{ // Projectiles just did a weak charge swing, kill it
								Projectile.Kill();
								return;
							}

							player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi + guardian.GuardianItemCharge * 0.006f * Projectile.spriteDirection); // set arm position (90 degree offset since arm starts lowered)
							Vector2 armPosition = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - guardian.GuardianItemCharge * 0.006f * Projectile.spriteDirection) - (new Vector2(player.Center.X, player.Center.Y) - new Vector2(player.Center.X, player.Center.Y).Floor());
							Projectile.Center = armPosition - new Vector2((hitboxOffset * 2 + 0.3f * guardian.GuardianItemCharge + (float)Math.Sin(MathHelper.Pi / 210f * guardian.GuardianItemCharge) * 10f) * player.direction * 0.4f, (hitboxOffset * 2 - hitboxOffset * 0.014f * guardian.GuardianItemCharge) * 0.4f);

							if (guardian.GuardianItemCharge < 210f)
							{
								guardian.GuardianItemCharge += 30f / HammerItem.Item.useTime * player.GetTotalAttackSpeed(DamageClass.Melee);

								if (guardian.GuardianItemCharge > 210f) guardian.GuardianItemCharge = 210f;
							}

							if (player.whoAmI == Main.myPlayer)
							{
								if (!player.controlUseItem)
								{
									if (guardian.GuardianItemCharge > 10f)
									{
										Projectile.ai[1] = 1;

										Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * HammerItem.Item.shootSpeed;

										if (guardian.ThrowLevel() < 4)
										{
											dir *= (0.3f * (guardian.ThrowLevel() + 2) / 3);
											Projectile.damage = (int)(Projectile.damage * 0.75f);
											Projectile.knockBack = (int)(Projectile.knockBack / 3f);
											Projectile.ai[0] = 1f;
										}

										Projectile.velocity = dir;
										Projectile.rotation = dir.ToRotation();
										Projectile.direction = Projectile.spriteDirection;
										Projectile.netUpdate = true;

										guardian.GuardianItemCharge = 0;
									}
									else
									{
										Projectile.ai[1] = -61f;
										Projectile.netUpdate = true;
									}
								}
								else if (Main.mouseRight)
								{
									Projectile.ai[1] = -60f;
									Projectile.netUpdate = true;
								}
							}
						}
						else
						{
							if (Projectile.ai[1] < -60f) // Makes easier to sync the behaviour after a weak slam
							{
								Projectile.ai[1] = -60f;
								WeakHit = true;
								guardian.GuardianItemCharge = 0;
							}

							if (Projectile.ai[1] == -60f)
							{ // First frame of the swing
								SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
								Projectile.friendly = true;
								ResetHitStatus(false);
								HammerItem.OnSwing(player, guardian, Projectile, guardian.GuardianItemCharge >= 180f);
							}

							Projectile.velocity = Vector2.UnitX * 0.001f * player.direction; // So enemies are KBd in the right direction

							float SwingOffset = (float)Math.Sin(MathHelper.Pi / 60f * Projectile.ai[1]);
							Vector2 arm = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - (guardian.GuardianItemCharge * 0.006f) * Projectile.spriteDirection);
							player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi + (guardian.GuardianItemCharge * 0.006f + SwingOffset * (3f + guardian.GuardianItemCharge * 0.006f)) * Projectile.spriteDirection);
							Vector2 armPosition = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - (guardian.GuardianItemCharge * 0.006f + SwingOffset * (3f + guardian.GuardianItemCharge * 0.006f)) * Projectile.spriteDirection) - (new Vector2(player.Center.X, player.Center.Y) - new Vector2(player.Center.X, player.Center.Y).Floor());
							Projectile.Center = armPosition - new Vector2((hitboxOffset * 2 + 0.3f * guardian.GuardianItemCharge + (float)Math.Sin(MathHelper.Pi / 210f * guardian.GuardianItemCharge) * 10f) * player.direction * 0.4f + (armPosition.X - arm.X) * (2.5f + hitboxOffset * 0.07f), (armPosition.Y - arm.Y) * -(1.1f + hitboxOffset * 0.03f) + (210f - guardian.GuardianItemCharge) * 0.075f);

							float toAdd = 30f / HammerItem.Item.useTime * HammerItem.SwingSpeed * player.GetTotalAttackSpeed(DamageClass.Melee);
							if (Projectile.ai[1] < -40) Projectile.ai[1] += toAdd * 1.5f;
							else
							{
								Projectile.ai[1] += toAdd * 0.66f;
								Projectile.friendly = false;
							}

							if (Projectile.ai[1] >= 0f)
							{
								Projectile.ai[1] = 0f;
								Projectile.friendly = false;
							}
						}

					}
				}
				else // Thrown
				{
					if (HammerItem.ThrowAI(player, guardian, Projectile, WeakThrow))
					{
						if (Projectile.timeLeft < 598 && range > 0) // Delay helps preventing the hammer from instantly despawning if launched from inside a tile
						Projectile.tileCollide = HammerItem.TileCollide;
						else Projectile.tileCollide = false;

						if (range == HammerItem.Range)
						{ // First frame of the throw
							SoundEngine.PlaySound(HammerItem.Item.UseSound, player.Center);
							ResetHitStatus(!WeakThrow);
							Projectile.friendly = true;
							HammerItem.OnThrow(player, guardian, Projectile, WeakThrow);
						}

						if (WeakThrow)
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
							Vector2 vel = Vector2.Normalize(player.Center - Projectile.Center) * HammerItem.ReturnSpeed;

							if (range < -30)
							{
								vel *= 1 - (30 - range) * 0.15f;
								Projectile.velocity = -vel;
							}
							else
							{
								vel *= 0.5f;
								Projectile.velocity += vel;
							}

							if (dist < 30f && player.whoAmI == Main.myPlayer)
							{
								Projectile.Kill();
							}

							if (range < -60)
							{
								Projectile.friendly = false;
							}
						}
					}
				}

				HammerItem.ExtraAI(player, guardian, Projectile);
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.X -= hitboxOffset;
			hitbox.Y -= hitboxOffset;
			hitbox.Width += hitboxOffset * 2;
			hitbox.Height += hitboxOffset * 2;
		}

		public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.ai[1] < 0) // Less damage for melee hits
			{
				if (Main.LocalPlayer.GetModPlayer<OrchidGuardian>().GuardianItemCharge < 180f)
				{
					modifiers.FinalDamage *= 0.5f;
				}
				else
				{
					modifiers.FinalDamage *= 0.75f;
				}
			}

			if (HammerItem != null && Projectile.ai[1] > 0)
			{
				if (!HammerItem.Penetrate && target.lifeMax > 5)
				{
					modifiers.FinalDamage *= 1f - 0.25f * HitCount;
					HitCount++;
					if (HitCount > 3)
					{
						HitCount = 3;
					}
				}
			}
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (Projectile.ai[1] < 0) // Less damage for melee hits
			{
				if (Main.LocalPlayer.GetModPlayer<OrchidGuardian>().GuardianItemCharge < 180f)
				{
					modifiers.FinalDamage *= 0.5f;
				}
				else
				{
					modifiers.FinalDamage *= 0.75f;
				}
			}

			if (HammerItem != null && Projectile.ai[1] > 0)
			{
				if (!HammerItem.Penetrate)
				{
					modifiers.FinalDamage *= 1f - 0.25f * HitCount;
					HitCount++;
					if (HitCount > 3)
					{
						HitCount = 3;
					}
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			if (HammerItem.TileBounce)
			{
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
			}
			else range = -40;
			Player player = Main.player[Projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			HammerItem.OnThrowTileCollide(player, guardian, Projectile, oldVelocity);
			return false;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (Projectile.ai[1] > 0)
			{ // Throw
				bool weak = WeakThrow;
				if (FirstHit)
				{
					if (!weak)
					{
						guardian.AddSlam(HammerItem.SlamStacks);
						guardian.AddGuard(HammerItem.GuardStacks);
					}
					HammerItem.OnThrowHitFirst(player, guardian, target, Projectile, hit.Knockback, hit.Crit, weak);
				}
				HammerItem.OnThrowHit(player, guardian, target, Projectile, hit.Knockback, hit.Crit, weak);

				if (!penetrate && target.lifeMax > 5)
				{
					range = -40;
					Projectile.netUpdate = true;
				}
			}
			else
			{ // Melee Swing
				bool fullyCharged = guardian.GuardianItemCharge >= 180f;
				if (FirstHit)
				{
					HammerItem.OnMeleeHitFirst(player, guardian, target, Projectile, hit.Knockback, hit.Crit, fullyCharged);
					if (guardian.GuardianItemCharge > 0f)
					{
						guardian.GuardianItemCharge += 60f * HammerItem.SwingChargeGain * player.GetTotalAttackSpeed(DamageClass.Melee);
						if (guardian.GuardianItemCharge > 210f)
						{
							guardian.GuardianItemCharge = 210f;
						}
					}
				}
				HammerItem.OnMeleeHit(player, guardian, target, Projectile, hit.Knockback, hit.Crit, fullyCharged);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(HammerItem.Item.type);
			writer.Write(range);
			writer.Write(BlockDuration);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Item item = new Item();
			int itemtype = reader.ReadInt32();
			range = reader.ReadInt32();
			BlockDuration = reader.ReadInt32();

			if (HammerItem == null)
			{
				item.SetDefaults(itemtype);
				if (item.ModItem is OrchidModGuardianHammer hammerItem)
				{
					HammerItem = hammerItem;

					/*if (Main.netMode != NetmodeID.Server)
					{
						HammerTexture = TextureAssets.Item[hammerItem.Item.type].Value;
						Projectile.width = (int)(HammerTexture.Width * hammerItem.Item.scale);
						Projectile.height = (int)(HammerTexture.Height * hammerItem.Item.scale);
					}*/

					Projectile.scale = hammerItem.Item.scale;

					//Projectile.position.X -= Projectile.width / 2;
					//Projectile.position.Y -= Projectile.height / 2;

					range = HammerItem.Range;
					penetrate = HammerItem.Penetrate;
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (HammerTexture == null) return false;
			Player player = Main.player[Projectile.owner];
			float rotationBonus = 0f;

			SpriteEffects effect;
			if (Projectile.spriteDirection == 1)
			{
				effect = SpriteEffects.FlipHorizontally;
				rotationBonus += MathHelper.PiOver2;
			}
			else
			{
				effect = SpriteEffects.None;
				rotationBonus -= MathHelper.PiOver2;
			}

			Vector2 posproj = Projectile.Center;
			//float rotaproj = Projectile.rotation;
			if (player.gravDir == -1)
			{
				if (Projectile.ai[1] <= 0)
				{
					posproj.Y = (player.Bottom.Floor() + player.position.Floor()).Y - posproj.Y;
				}
				//rotaproj += MathHelper.Pi;
				if (effect == SpriteEffects.None) effect = SpriteEffects.FlipHorizontally;
				else effect = SpriteEffects.None;
			}

			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);
			var position = posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY;

			if (Projectile.ai[1] == 0)
			{
				OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
				rotationBonus += guardian.GuardianItemCharge * 0.0065f * player.gravDir * Projectile.spriteDirection;
			}

			if (Projectile.ai[1] < 0)
			{
				OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
				float SwingOffset = (float)Math.Sin(MathHelper.Pi / 60f * Projectile.ai[1]);
				rotationBonus += (guardian.GuardianItemCharge * 0.0065f + SwingOffset * (3.5f + guardian.GuardianItemCharge * 0.006f)) * player.gravDir * Projectile.spriteDirection;
			}

			if (BlockDuration != 0)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				for (int i = 0; i < OldPosition.Count; i++)
				{
					Vector2 drawPositionTrail = OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
					spriteBatch.Draw(HammerTexture, drawPositionTrail, null, lightColor * 0.04f * (i + 1), OldRotation[i], HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
				}

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}

			spriteBatch.Draw(HammerTexture, position, null, color, Projectile.rotation + rotationBonus, HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);


			if (Projectile.ai[1] != 0)
			{
				for (int i = 0; i < OldPosition.Count; i++)
				{
					color = Lighting.GetColor((int)(OldPosition[i].X / 16f), (int)(OldPosition[i].Y / 16f), Color.White) * (((WeakThrow ? 0.05f : 0.15f) * i));
					position = OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY;

					spriteBatch.Draw(HammerTexture, position, null, color, OldRotation[i] + rotationBonus, HammerTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
				}
			}

			return false;
		}
	}
}