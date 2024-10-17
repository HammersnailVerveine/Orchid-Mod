using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Content.Shapeshifter.Dusts;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenTortoise : OrchidModShapeshifterShapeshift
	{
		public bool WasGliding = false;
		public bool Landed = false;
		public bool TouchedGround = false;
		public bool LateralMovement = false;
		public bool CanAscend = false;

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
			Item.damage = 19;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 28;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
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
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor)
		{
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS

			// ANIMATION

			if (anchor.Projectile.ai[0] != 0)
			{ // Override animation during left and right click attack
				projectile.direction = (int)anchor.Projectile.ai[1];
				projectile.spriteDirection = projectile.direction;

				if (anchor.Projectile.ai[0] > 0)
				{ // Left Click
					anchor.Projectile.ai[0]--;
					anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 7 : 8);
				}
				else
				{ // Right Click
					anchor.Projectile.ai[0]++;
					anchor.Frame = (anchor.Projectile.ai[0] < -5 && anchor.Projectile.ai[0] > -295 ? 6 : 5);
				}

				if (anchor.Projectile.ai[0] == 0)
				{ // Puts the animation back on track
					anchor.Frame = 0;
				}
			}
			else if (LateralMovement)
			{ // Player is moving left or right, cycle through frames
				if (anchor.Timespent % 6 == 0 && anchor.Timespent > 0)
				{
					anchor.Frame++;
					if (anchor.Frame == 5)
					{
						anchor.Frame = 1;
					}
				}
			}
			else
			{
				anchor.Timespent = 0;
				anchor.Frame = 0;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;

			intendedVelocity.Y += 0.2f;

			// Normal movement
			if (player.controlLeft || player.controlRight)
			{
				if (projectile.ai[0] < -5)
				{ // Prevents horizontal movement while blocking, but cancels the block
					projectile.ai[0] = -5;
				}
				else
				{
					if (player.controlLeft && !player.controlRight)
					{ // Left movement
						intendedVelocity.X -= 0.1f;
						if (intendedVelocity.X < -2f) intendedVelocity.X = -2f;
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (player.controlRight && !player.controlLeft)
					{ // Right movement
						intendedVelocity.X += 0.1f;
						if (intendedVelocity.X > 2f) intendedVelocity.X = 2f;
						projectile.direction = 1;
						projectile.spriteDirection = 1;
						LateralMovement = true;
					}
					else
					{
						LateralMovement = false;
						intendedVelocity.X *= 0.8f;
					}
				}
			}
			else
			{
				LateralMovement = false;
				intendedVelocity.X *= 0.8f;
			}

			FinalVelocityCalculations(intendedVelocity, projectile, player);

			// ATTACK

			if (IsLocalPlayer(player))
			{
				if (CanLeftClick(anchor) && !Landed)
				{ // Left click attack
					/*
					int projectileType = ModContent.ProjectileType<SageOwlProj>();
					Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(7.5f)) * Item.shootSpeed;
					int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
					Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, damage, Item.knockBack, player.whoAmI);
					newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
					newProjectile.netUpdate = true;
					*/

					anchor.LeftCLickCooldown = Item.useTime;
					anchor.Projectile.ai[0] = 10;
					anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
					anchor.NeedNetUpdate = true;

					anchor.Frame = 7;
					SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
				}

				if (CanRightClick(anchor))
				{ // Right click attack
					anchor.NeedNetUpdate = true;
					anchor.RightCLickCooldown = Item.useTime;
					anchor.Projectile.ai[0] = -300;
					anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
					projectile.velocity.X = 0f;
					SoundEngine.PlaySound(SoundID.Item37, projectile.Center);
				}
			}

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			if (anchor.OldPosition.Count > 3)
			{
				anchor.OldPosition.RemoveAt(0);
				anchor.OldRotation.RemoveAt(0);
				anchor.OldFrame.RemoveAt(0);
			}
		}

		public override void PreDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor)
		{
			if (projectile.ai[2] > 0)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float scalemult = (float)Math.Sin(projectile.ai[2] * 0.1046f) * 0.25f + 1f;
				spriteBatch.Draw(anchor.TextureShapeshift, drawPosition, drawRectangle, lightColor * 0.75f, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
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