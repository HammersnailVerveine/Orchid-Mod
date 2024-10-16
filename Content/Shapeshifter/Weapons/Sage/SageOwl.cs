using Microsoft.Xna.Framework;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Content.Shapeshifter.Dusts;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageOwl : OrchidModShapeshifterShapeshift
	{
		public bool WasGliding = false;
		public bool Landed = false;
		public bool TouchedGround = false;
		public bool LateralMovement = false;
		public bool CanAscend = false;
		public int AscendTimer = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Zombie111;
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			Item.knockBack = 3f;
			Item.damage = 17;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 30;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 2;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

			WasGliding = false;
			Landed = false;
			TouchedGround = false;
			LateralMovement = false;
			CanAscend = false;
			AscendTimer = 0;

			for (int i = 0; i < 8; i++)
			{
				FeatherDust(projectile, 1);
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor)
		{
			for (int i = 0; i < 5; i++)
			{
				FeatherDust(projectile, 1);
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 intendedVelocity = projectile.velocity;
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			anchor.Timespent++;

			player.nightVision = true;

			// ANIMATION

			if (anchor.Timespent % 6 == 0 && anchor.Timespent > 0)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame == 2)
				{
					anchor.Timespent = -5;
				}

				if (anchor.Frame == 1)
				{
					anchor.Timespent = -3;
				}

				if (anchor.Frame == 7)
				{
					anchor.Frame = 1;
				}
			}

			// MOVEMENT

			if (AscendTimer > 0)
			{ // Player is ascending, prevent normal movement for a duration
				AscendTimer --;
				WasGliding = false;
				Landed = false;
				TouchedGround = false;
				LateralMovement = false;
				CanAscend = false;

				if ((anchor.Timespent + 1) % 6 != 0)
				{
					anchor.Timespent++;
				}

				if (AscendTimer > 85)
				{ // Ascend quickly
					if (anchor.Frame == 3)
					{
						intendedVelocity.Y -= 1f;
						if (intendedVelocity.Y > -1f)
						{
							intendedVelocity.Y = -1f;
						}
						SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
					}

					if (player.controlDown)
					{ // Control height a bit by pressing down
						AscendTimer--;
					}
				}
				else
				{ // Stay in place for a while after reaching max height
					if (AscendTimer == 85)
					{
						SoundEngine.PlaySound(Main.rand.NextBool() ? SoundID.Zombie110 : SoundID.Zombie111, projectile.Center);
					}
					intendedVelocity *= 0.8f;
				}
				FeatherDust(projectile, 30);
			}
			else
			{ // Normal movement
				if (!Landed)
				{ // Drops feathers while flying
					FeatherDust(projectile, 90);
				}

				bool grounded = false;
				for (int i = 0; i < 10; i++)
				{ // Checks if the player/projectile is within 2 tiles of the ground
					if (Collision.TileCollision(projectile.position + Vector2.UnitY * 3.2f * i, Vector2.UnitY * 3.2f, projectile.width, projectile.height, false, false, (int)player.gravDir) != Vector2.UnitY * 3.2f)
					{
						grounded = true;
						if (i == 0)
						{
							TouchedGround = true;
						}
						break;
					}
				}

				if (grounded)
				{
					intendedVelocity.Y += 0.05f;
					if (anchor.Timespent < 0) anchor.Timespent = 0;
					CanAscend = true;
				}
				else
				{
					TouchedGround = false;
					intendedVelocity.Y += 0.15f;
				}

				if (player.controlJump && CanAscend)
				{ // Check for an ascend input
					AscendTimer = 120;
					anchor.Timespent = 0;
					anchor.Frame = 2;
					intendedVelocity.Y = -2f;
					intendedVelocity.X = 0f;
					CanAscend = false;
					SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
					anchor.NeedNetUpdate = true;

					for (int i = 0; i < 5; i ++)
					{
						FeatherDust(projectile, 1);
					}
				}
				else
				{ // Normal movement
					if ((player.controlDown || player.controlUp || player.controlJump) && !TouchedGround)
					{ // Vertical movement (Deactivated if too close to the ground)
						if (anchor.Frame < 0) anchor.Frame = 0;
						if (player.controlJump || player.controlUp)
						{ // Slowly glides down
							if (anchor.Frame == 4) anchor.Frame = 3;
							intendedVelocity.Y = 0.8f;
							WasGliding = true;
						}
						else
						{ // Stops flapping the wings, falling down faster
							if (anchor.Frame == 4 || anchor.Frame == 3)
							{
								anchor.Frame = 2;
							}
							intendedVelocity.Y += 0.05f;
						}
					}
					else if (anchor.Frame == 3 || WasGliding)
					{ // Idle (slowly falling)
						if (WasGliding)
						{ // No vertical ascend after a glide
							intendedVelocity.Y = 0;
							WasGliding = false;
						}
						else if (grounded)
						{ // Pushes the player up more if near the ground while moving (helps with navigation)
							if (LateralMovement || anchor.Projectile.ai[0] > 0 || anchor.IsLeftClicking)
							{
								intendedVelocity.Y -= 1f;
								if (intendedVelocity.Y > -1f)
								{
									intendedVelocity.Y = -1f;
								}

							}
						}
						else
						{ // Else slowly flaps down
							intendedVelocity.Y = -1f;
						}
						SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
					}

					if (player.controlLeft || player.controlRight || anchor.Projectile.ai[0] > 0 || anchor.IsLeftClicking)
					{
						if (Landed)
						{ // Kickstart if the owl was landed
							Landed = false;
							intendedVelocity.Y = -2f;
						}

						if (grounded)
						{ // Helps staying a bit more over the ground while moving left and right
							/*
							if (intendedVelocity.Y > -0.1f)
							{
								intendedVelocity.Y = -0.1f;
							}
							*/
							if ((anchor.Timespent + 1) % 6 != 0)
							{
								anchor.Timespent++;
							}
						}

						if (player.controlLeft && !player.controlRight)
						{ // Left movement
							intendedVelocity.X -= 0.25f;
							if (intendedVelocity.X < -5f) intendedVelocity.X = -5f;
							projectile.direction = -1;
							projectile.spriteDirection = -1;
							LateralMovement = true;
						}
						else if (player.controlRight && !player.controlLeft)
						{ // Right movement
							intendedVelocity.X += 0.25f;
							if (intendedVelocity.X > 5f) intendedVelocity.X = 5f;
							projectile.direction = 1;
							projectile.spriteDirection = 1;
							LateralMovement = true;
						}
						else
						{
							LateralMovement = false;
							intendedVelocity.X *= 0.9f;
						}
					}
					else
					{
						LateralMovement = false;
						intendedVelocity.X *= 0.9f;
						if (Math.Abs(intendedVelocity.X) < 0.5f && TouchedGround)
						{ // Player close to the ground, not attacking and not moving = landing frame
							if (intendedVelocity.Y < 0.25f)
							{
								intendedVelocity.Y = 0.25f;
							}
							intendedVelocity.X *= 0.5f;
							anchor.Frame = 0;
							Landed = true;
						}
					}
				}
			}

			if (projectile.gfxOffY != 0 || player.gfxOffY != 0)
			{ // fuck slopes all my homies hate slopes
				if (intendedVelocity.Y > -0.5f)
				{
					intendedVelocity.Y = -0.5f;
				}
				projectile.gfxOffY = 0;
				player.gfxOffY = 0;
			}

			Vector2 finalVelocity = Vector2.Zero;
			intendedVelocity /= 10f;
			for (int i = 0; i < 10; i++)
			{
				finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, false, false, (int)player.gravDir);
			}

			projectile.velocity = finalVelocity;

			// ATTACK

			if (IsLocalPlayer(player))
			{
				if (Main.mouseLeft && (Main.mouseLeftRelease || AutoReuseLeft) && anchor.CanLeftClick && !Landed)
				{ // Left click attack
					int projectileType = ModContent.ProjectileType<SageOwlProj>();
					for (int i = 0; i < 3; i++)
					{
						Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(7.5f)) * Item.shootSpeed * (0.85f + i * 0.15f);
						int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
						Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, damage, Item.knockBack, player.whoAmI, 1f);
						newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
						newProjectile.netUpdate = true;
					}

					anchor.LeftCLickCooldown = Item.useTime;
					SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
					FeatherDust(projectile, 2);
					anchor.Projectile.ai[0] = 10;
					anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
					anchor.NeedNetUpdate = true;
				}
			}

			if (anchor.Projectile.ai[0] > 0)
			{ // Override animation during attack
				anchor.Projectile.ai[0]--;
				if (anchor.Projectile.ai[0] == 0)
				{
					anchor.Frame = 1;
				}
				else
				{
					anchor.Frame = 7;
					projectile.direction = (int)anchor.Projectile.ai[1];
					projectile.spriteDirection = projectile.direction;
				}
			}

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			if (anchor.OldPosition.Count > 5)
			{
				anchor.OldPosition.RemoveAt(0);
				anchor.OldRotation.RemoveAt(0);
				anchor.OldFrame.RemoveAt(0);
			}
		}

		public void FeatherDust(Projectile projectile, int rand = 1)
		{
			if (Main.rand.NextBool(rand))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<SageOwlDust>(), Scale: Main.rand.NextFloat(1.2f, 1.4f));
				dust.velocity *= 0.5f;
				dust.velocity.Y = 2f;
				dust.customData = Main.rand.Next(314);
			}
		}

		/*
		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor)
		{
			Player owner = Main.player[projectile.owner];
			Vector2 intendedVelocity = projectile.velocity;
			anchor.Timespent++;
			intendedVelocity.Y += 0.05f;

			if (anchor.Timespent % 6 == 0 && anchor.Timespent > 0)
			{
				anchor.Frame++;

				if (anchor.Frame == 2)
				{
					anchor.Timespent = -5;
				}

				if (anchor.Frame == 1)
				{
					anchor.Timespent = -3;
				}

				if (anchor.Frame == 7)
				{
					anchor.Frame = 1;
				}

				if (owner.controlUp && !owner.controlDown)
				{ // Up movement
					if (anchor.Frame < 0) anchor.Frame = 0;

					if (anchor.Frame == 3 || !WasAscending)
					{
						anchor.Frame = 3;
						SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
						intendedVelocity.Y = -5;
					}

					WasAscending = true;
					anchor.Timespent++;
				}
				else
				{
					if ((owner.controlDown || owner.controlJump) && !owner.controlUp)
					{ // Down movement
						if (anchor.Frame < 0) anchor.Frame = 0;
						if (owner.controlJump)
						{
							if (anchor.Frame == 4) anchor.Frame = 3;
							intendedVelocity.Y = 0.8f;
							WasGliding = true;
						}
						else
						{
							if (anchor.Frame == 4 || anchor.Frame == 3)
							{
								anchor.Frame = 2;
							}
							intendedVelocity.Y += 0.1f;
						}
					}
					else if (anchor.Frame == 3 || WasGliding)
					{ // Idle
						intendedVelocity.Y = -1;
						SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
						WasGliding = false;
					}
					WasAscending = false;
				}

				if (owner.controlLeft && !owner.controlRight)
				{ // Left movement
					intendedVelocity.X -= 0.75f;
					if (intendedVelocity.X < -5f) intendedVelocity.X = -5f;
					projectile.direction = -1;
					projectile.spriteDirection = -1;
				}
				else if (owner.controlRight && !owner.controlLeft)
				{ // Right movement
					intendedVelocity.X += 0.75f;
					if (intendedVelocity.X > 5f) intendedVelocity.X = 5f;
					projectile.direction = 1;
					projectile.spriteDirection = 1;
				}
				else
				{
					intendedVelocity.X *= 0.75f;
				}
			}

			Vector2 finalVelocity = Vector2.Zero;
			intendedVelocity /= 10f;
			for (int i = 0; i < 10; i++)
			{
				finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, false, false, (int)owner.gravDir);
			}

			projectile.velocity = finalVelocity;

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			if (anchor.OldPosition.Count > 5)
			{
				anchor.OldPosition.RemoveAt(0);
				anchor.OldRotation.RemoveAt(0);
				anchor.OldFrame.RemoveAt(0);
			}
		}
		*/
	}
}