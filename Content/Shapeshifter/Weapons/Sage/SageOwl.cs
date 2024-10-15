using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageOwl : OrchidModShapeshifterShapeshift
	{
		bool WasGliding = false;
		bool WasAscending = false;
		bool Landed = false;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Zombie111;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 22;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 30;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor)
		{
			Player owner = Main.player[projectile.owner];
			anchor.Frame = 2;
			projectile.direction = owner.direction;
			projectile.spriteDirection = owner.direction;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor)
		{
			Player owner = Main.player[projectile.owner];
			Vector2 intendedVelocity = projectile.velocity;
			owner.fallStart = (int)(owner.position.Y / 16f);
			owner.fallStart2 = (int)(owner.position.Y / 16f);
			anchor.Timespent++;

			owner.nightVision = true;

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

			bool grounded = false;
			for (int i = 0; i < 10; i++)
			{ // Checks if the player/projectile is within 2 tiles of the ground
				if (Collision.TileCollision(projectile.position + Vector2.UnitY * 3.2f * i, Vector2.UnitY * 3.2f, projectile.width, projectile.height, false, false, (int)owner.gravDir) != Vector2.UnitY * 3.2f)
				{
					grounded = true;
					break;
				}
			}

			if (grounded)
			{
				intendedVelocity.Y += 0.05f;
				if (anchor.Timespent < 0) anchor.Timespent = 0;
			}
			else
			{
				intendedVelocity.Y += 0.15f;
			}


			if (owner.controlDown || owner.controlUp || owner.controlJump)
			{ // Vertical movement
				if (anchor.Frame < 0) anchor.Frame = 0;
				if (owner.controlJump || owner.controlUp)
				{
					if (owner.controlJump && grounded)
					{ // Player is grounded and presses up. Propels the player upwards

					}
					else
					{ // Slowly glides down
						if (anchor.Frame == 4) anchor.Frame = 3;
						intendedVelocity.Y = 0.8f;
						WasGliding = true;
					}
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
				{
					intendedVelocity.Y = 0;
					WasGliding = false;
				}
				else if (grounded)
				{
					intendedVelocity.Y -= 1f;
					if (intendedVelocity.Y > -1f)
					{
						intendedVelocity.Y = -1f;
					}
				}
				else
				{
					intendedVelocity.Y = -1f;
				}
				SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
			}

			if (owner.controlLeft || owner.controlRight)
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

				if (owner.controlLeft && !owner.controlRight)
				{ // Left movement
					intendedVelocity.X -= 0.25f;
					if (intendedVelocity.X < -5f) intendedVelocity.X = -5f;
					projectile.direction = -1;
					projectile.spriteDirection = -1;
				}
				else if (owner.controlRight && !owner.controlLeft)
				{ // Right movement
					intendedVelocity.X += 0.25f;
					if (intendedVelocity.X > 5f) intendedVelocity.X = 5f;
					projectile.direction = 1;
					projectile.spriteDirection = 1;
				}
			}
			else
			{
				intendedVelocity.X *= 0.9f;
				if (Math.Abs(intendedVelocity.X) < 0.5f && grounded)
				{ // Player close to the ground and not moving = landing frame
					if (intendedVelocity.Y < 0.25f)
					{
						intendedVelocity.Y = 0.25f;
					}
					intendedVelocity.X *= 0.5f;
					anchor.Frame = 0;
					Landed = true;
				}
			}

			if (projectile.gfxOffY != 0 || owner.gfxOffY != 0)
			{ // fuck slopes all my homies hate slopes
				if (intendedVelocity.Y > -0.5f)
				{
					intendedVelocity.Y = -0.5f;
				}
				projectile.gfxOffY = 0;
				owner.gfxOffY = 0;
			}

			Vector2 finalVelocity = Vector2.Zero;
			intendedVelocity /= 10f;
			for (int i = 0; i < 10; i++)
			{
				finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, false, false, (int)owner.gravDir);
			}

			projectile.velocity = finalVelocity;

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