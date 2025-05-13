using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenSpider : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public bool JumpRelease = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.NPCHit29;
			Item.useTime = 30;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 26;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			GroundedWildshape = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}


		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 position = projectile.Center;
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f)) * Item.shootSpeed * Main.rand.NextFloat(0.8f, 1.2f) / 15f;

			bool foundTarget = false;
			for (int i = 0; i < 15; i++)
			{
				position += Collision.TileCollision(position, offSet, 2, 2, true, true, (int)player.gravDir);
				foreach (NPC npc in Main.npc)
				{
					if (OrchidModProjectile.IsValidTarget(npc))
					{
						if (position.Distance(npc.Center) < npc.width + 32f) // if the NPC is close to the projectile path, snaps to it.
						{
							foundTarget = true;
							position = npc.Center;
							break;
						}
					}
				}

				if (foundTarget)
				{
					break;
				}
			}

			int projectileType = ModContent.ProjectileType<WardenSpiderProj>();
			ShapeshifterNewProjectile(shapeshifter, Item.GetSource_FromAI(), position, offSet * 0.001f, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);

			SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack, projectile.Center);

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			anchor.Frame = 7;
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<WardenSpiderWeb>();
			Vector2 distance = Main.MouseWorld - projectile.Center;
			float ai0 = distance.Length() > 300f ? 0f : (30f - distance.Length() / 10f);
			Vector2 velocity = Vector2.Normalize(distance) * 10f;
			ShapeshifterNewProjectile(shapeshifter, Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, Item.damage, Item.crit, Item.knockBack * 3f, player.whoAmI, ai0);

			anchor.NeedNetUpdate = true;
			anchor.RightCLickCooldown = Item.useTime * 2.5f;
			anchor.Projectile.ai[0] = 15;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			projectile.velocity.X = 0f;
			SoundEngine.PlaySound(SoundID.Item17, projectile.Center);
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS & ANIMATION

			Tile tile = Framing.GetTileSafely((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f));
			bool walled = tile.WallType != 0;

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded || walled);

			foreach (Projectile webProjectile in Main.projectile)
			{
				if (webProjectile.ai[0] > 45 && webProjectile.active && projectile.Center.Distance(webProjectile.Center) < 48f)
				{
					walled = true;
				} 
			}

			if (walled)
			{ // Cancels fall damage when walled
				player.fallStart = (int)(player.position.Y / 16f);
				player.fallStart2 = (int)(player.position.Y / 16f);

				if (anchor.IsInputJump)
				{ // Prevents jumping immediately after detaching of a wall by pressing space
					walled = false;
					JumpRelease = false;
				}
			}

			if (anchor.Projectile.ai[0] != 0)
			{ // Override animation during left and right click attack
				projectile.direction = (int)anchor.Projectile.ai[1];
				projectile.spriteDirection = projectile.direction;

				if (anchor.Projectile.ai[0] > 0)
				{ // Left Click
					anchor.Projectile.ai[0]--;
					anchor.Frame = walled ? 10 : anchor.Projectile.ai[0] < 6 ? 5 : 4;

					if (anchor.Projectile.ai[0] < 0)
					{
						anchor.Projectile.ai[0] = 0;
					}
				}

				if (anchor.Projectile.ai[0] == 0)
				{ // Puts the animation back on track
					anchor.Frame = walled ? 6 : 0;
				}
			}
			else if (walled)
			{ // Attached to a wall
				if (anchor.Frame < 6)
				{
					anchor.Frame = 6;
				}

				if (projectile.velocity.Length() > 1f)
				{ // is moving
					if (anchor.Timespent % 4 == 0 && anchor.Timespent > 0)
					{
						anchor.Frame++;
						if (anchor.Frame > 9)
						{
							anchor.Frame = 6;
						}
					}
				}
				else
				{
					anchor.Timespent = 0;
					anchor.Frame = 6;
				}
			}
			else
			{ // Grounded animations
				if (anchor.Frame > 3)
				{
					anchor.Frame = 0;
				}

				if (LateralMovement)
				{ // Player is moving left or right, cycle through frames
					if (anchor.Timespent % 4 == 0 && anchor.Timespent > 0)
					{
						anchor.Frame++;
						if (anchor.Frame == 4)
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

				if (!IsGrounded(projectile, player, 8f))
				{
					anchor.Timespent = 0;
					anchor.Frame = 5;
				}
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			bool horizontalMovement = false;

			if (walled)
			{ // Can walk on walls
				if ((anchor.IsInputUp || anchor.IsInputDown))
				{ // Player is inputting a movement key and didn't just start blocking
					if (anchor.IsInputUp && !anchor.IsInputDown)
					{ // Up movement
						TryAccelerate(ref intendedVelocity, LateralMovement ? -2.264f : -3.2f, speedMult, LateralMovement ? 0.4245f : 0.6f, Yaxis: true);
						horizontalMovement = true;
					}
					else if (!anchor.IsInputUp && anchor.IsInputDown)
					{ // Down movement
						TryAccelerate(ref intendedVelocity, LateralMovement ? 2.264f : 3.2f, speedMult, LateralMovement ? 0.4245f : 0.6f, Yaxis: true);
						horizontalMovement = true;
					}
					else
					{ // Both keys pressed = no movement
						intendedVelocity.Y *= 0.5f;
					}
				}
				else
				{ // no movement input
					intendedVelocity.Y *= 0.5f;
				}
			}
			else
			{ // Falling

				if (anchor.IsInputJump)
				{ // Jump while no charge ready
					if (grounded && JumpRelease)
					{
						JumpRelease = false;
						intendedVelocity.Y = -9.5f;
					}
				}
				else
				{
					JumpRelease = true;
				}

				GravityCalculations(ref intendedVelocity, player);
			}

			// Normal movement
			if ((anchor.IsInputLeft || anchor.IsInputRight))
			{ // Player is inputting a movement key and didn't just start blocking
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, horizontalMovement ? -2.25f : -3.2f, speedMult, (horizontalMovement ? 0.25f : 0.4f) * (walled ? 1.5f : 1f));
					projectile.direction = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, horizontalMovement ? 2.25f : 3.2f, speedMult, (horizontalMovement ? 0.25f : 0.4f) * (walled ? 1.5f : 1f));
					projectile.direction = 1;
					LateralMovement = true;
				}
				else
				{ // Both keys pressed = no movement
					LateralMovement = false;
					intendedVelocity.X *= walled ? 0.5f : 0.7f;
				}
			}
			else
			{ // no movement input
				LateralMovement = false;
				intendedVelocity.X *= walled ? 0.5f : 0.7f;
			}

			if (walled)
			{
				if (intendedVelocity.Length() > 0.1f)
				{
					projectile.rotation = projectile.velocity.ToRotation();
				}
				projectile.spriteDirection = 1;
			}
			else
			{
				projectile.rotation = 0f;
				projectile.spriteDirection = projectile.direction;
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true, forceFallThrough: walled);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > 4)
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}
	}
}