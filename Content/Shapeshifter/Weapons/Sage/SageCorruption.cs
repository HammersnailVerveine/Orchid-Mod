using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageCorruption : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public bool CanDash = false;
		public float RightClickChargeBuffer = 0; // used to keep the left click charge damage multiplier on the right click ability active for 1 second after releasing the button
		public int RightClickChargeBufferTimer = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Zombie29;
			Item.useTime = 30;
			Item.shootSpeed = 5f;
			Item.knockBack = 5f;
			Item.damage = 13;
			ShapeshiftWidth = 28;
			ShapeshiftHeight = 20;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
			GroundedWildshape = false;
			AutoReuseLeft = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			CanDash = false;
			RightClickChargeBuffer = 0f;
			RightClickChargeBufferTimer = 0;
			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			anchor.LeftCLickCooldown = Item.useTime * 2f;

			LateralMovement = false;

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 10; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.CorruptGibs)].velocity.Y -= 1.25f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.CorruptGibs)].velocity.Y -= 1.25f;
			}
		}

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsLeftClick && (anchor.IsLeftClickRelease || AutoReuseLeft) && anchor.CanLeftClick && (!anchor.IsRightClick || anchor.ai[4] == 0f);

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsRightClick && anchor.ai[4] == 1f && anchor.CanRightClick && anchor.CanLeftClick;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageCorruptionProjBlast>();
			Vector2 velocity = Main.MouseWorld.X < projectile.Center.X ? -Vector2.UnitX : Vector2.UnitX;
			float chargeMult = (RightClickChargeBuffer / 300f);
			float damage = Item.damage * 3f + Item.damage * chargeMult * 12f; // damage increased based on the left click hold duration
			ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, damage, Item.crit, 0f, player.whoAmI, chargeMult);

			projectile.velocity = -velocity * (6f + 4f * (anchor.ai[0] / 300f)) * shapeshifter.GetShapeshifterMeleeSpeed(); // dash duration reduced by attack speed, therefore it multiplies the velocity
			projectile.velocity.Y = -2f - 2f * chargeMult;

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.RightCLickCooldown = Item.useTime * 5f;
			RightClickChargeBuffer = 0f;
			RightClickChargeBufferTimer = 0; 
			anchor.ai[0] = 0f;
			anchor.ai[2] = anchor.LeftCLickCooldown;
			anchor.ai[3] = velocity.X > 0f ? 1f : -1f;
			anchor.ai[4] = 0f;
			anchor.NeedNetUpdate = true;
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && CanDash && projectile.ai[0] <= 0;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// 8 dir input
			if (anchor.IsInputLeft && !anchor.IsInputRight)
			{
				projectile.ai[1] = MathHelper.Pi * 1.5f; // Left
				if (anchor.IsInputUp && !anchor.IsInputDown)
				{
					projectile.ai[1] += MathHelper.Pi * 0.25f; // Top Left
				}
				else if (!anchor.IsInputUp && anchor.IsInputDown)
				{
					projectile.ai[1] -= MathHelper.Pi * 0.25f; // Bottom Left
				}
			}
			else if (!anchor.IsInputLeft && anchor.IsInputRight)
			{
				projectile.ai[1] = MathHelper.Pi * 0.5f; // Right
				if (anchor.IsInputUp && !anchor.IsInputDown)
				{
					projectile.ai[1] -= MathHelper.Pi * 0.25f; // Top Right
				}
				else if (!anchor.IsInputUp && anchor.IsInputDown)
				{
					projectile.ai[1] += MathHelper.Pi * 0.25f; // Bottom Right
				}
			}
			else if (anchor.IsInputUp && !anchor.IsInputDown)
			{
				projectile.ai[1] = 0f; // Up
			}
			else if (!anchor.IsInputUp && anchor.IsInputDown)
			{
				projectile.ai[1] = MathHelper.Pi; // Down
			}
			else
			{ // Projectile Direction (no input)
				projectile.ai[1] = MathHelper.Pi * (1f + projectile.direction * 0.5f);
			}

			projectile.ai[2] = 8;
			projectile.ai[0] = 60;
			anchor.NeedNetUpdate = true;
			CanDash = false;
			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);

			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 4; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.CorruptGibs)].velocity.Y -= 1.25f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds the jump dash cooldown
			// ai[1] holds the jump dash direction
			// ai[2] holds the jump dash duration
			// anchor.ai[0] holds the left click hold duration
			// anchor.ai[1] holds a value used in the left click attack animation
			// anchor.ai[2] holds the right click attack animation duration
			// anchor.ai[3] holds the sprite direction while attacking
			// anchor.ai[4] is == 1 when the player released right click and can use the blast

			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.noFallDmg = true;

			projectile.ai[0]--;

			if (anchor.ai[0] > 0 && projectile.ai[2] <= 0)
			{
				speedMult -= speedMult * 0.33f * (anchor.ai[0] / 300);
			}

			if (anchor.IsRightClickRelease)
			{
				anchor.ai[4] = 1f;
			}

			if (RightClickChargeBufferTimer > 0)
			{ // Right click damage bonus based on left click charge stays active for 1 second after releasing the key
				RightClickChargeBufferTimer--;

				if (RightClickChargeBufferTimer <= 0)
				{
					RightClickChargeBuffer = 0f;
				}
			}

			// ANIMATION
			if (anchor.ai[2] > 0f)
			{ // attacking (right click)
				projectile.direction = (int)anchor.ai[3];
				projectile.spriteDirection = projectile.direction;

			    if (anchor.ai[2] > 20f)
				{
					anchor.Frame = 8;
				}
				else if (anchor.ai[2] > 10f)
				{
					anchor.Frame = 9;
				}
				else
				{
					anchor.Frame = 10;
				}

				anchor.ai[2]--;
			}
			else if (anchor.ai[0] > 0f)
			{ // attacking (left click)
				projectile.direction = (int)anchor.ai[3];
				projectile.spriteDirection = projectile.direction;

				if (anchor.Frame < 8)
				{
					anchor.Frame = 8;
					anchor.ai[1] = 1f;
				}

				if (anchor.Frame > 10)
				{
					anchor.Frame = 10;
					anchor.ai[1] = -1f;
				}

				if (anchor.Timespent % (anchor.ai[0] >= 300 ? 4 : (anchor.ai[0] >= 100 ? 5 : 6)) == 0)
				{
					anchor.Frame += (int)anchor.ai[1];

					if (anchor.Frame == 10)
					{
						anchor.ai[1] = -1;
					}

					if (anchor.Frame == 8)
					{
						anchor.ai[1] = 1;
					}
				}
			}
			else
			{ // Movement & Idle
				if (anchor.Frame >= 8)
				{
					anchor.Frame = 0;
				}

				if (anchor.Timespent % 5 == 0)
				{
					anchor.Frame++;

					if (anchor.Frame == 8)
					{
						anchor.Frame = 0;
					}
				}
			}

			// ATTACK


			if (anchor.IsLeftClick)
			{
				if (ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter))
				{
					SoundStyle soundStyle = SoundID.Item112;
					soundStyle.Pitch *= Main.rand.NextFloat(0.7f, 1.3f);
					SoundEngine.PlaySound(soundStyle, projectile.Center);
					anchor.LeftCLickCooldown = Item.useTime - (Item.useTime * 0.66f * (anchor.ai[0] / 300));

					// spawn projectile
					if (IsLocalPlayer(player))
					{
						Vector2 targetLocation = Main.MouseWorld;
						Vector2 offSet = targetLocation - projectile.Center;

						if (targetLocation.Distance(projectile.Center) > 480f)
						{ // 30 tiles range max
							targetLocation = projectile.Center + Vector2.Normalize(offSet) * 480f;
						}

						if (targetLocation.Distance(projectile.Center) < 80f)
						{ // 5 tiles range main
							targetLocation = projectile.Center + Vector2.Normalize(offSet) * 80f;
						}

						targetLocation += Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(0f, 16f);

						// projectile snaps to nearby NPCs
						offSet = (targetLocation - projectile.Center) / 60f;
						Vector2 finalTargetLocation = projectile.Center;
						for (int i = 0; i < 60; i++)
						{
							finalTargetLocation += TileCollideShapeshifter(finalTargetLocation, offSet, 16, 16, true, true, (int)player.gravDir);
						}

						int projectileType = ModContent.ProjectileType<SageCorruptionProj>();
						Vector2 offSetSpawn = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(16f, 64f);
						ShapeshifterNewProjectile(shapeshifter, projectile.Center + offSetSpawn, Vector2.Zero, projectileType, Item.damage * 0.3f, Item.crit, 0f, player.whoAmI, finalTargetLocation.X, finalTargetLocation.Y);

						anchor.ai[3] = targetLocation.X > projectile.Center.X ? 1f : -1f;
						anchor.NeedNetUpdate = true;
					}
				}

				// The attack speed gets gradually faster over time
				anchor.ai[0] += 1.2f * shapeshifter.GetShapeshifterMeleeSpeed();
				if (anchor.ai[0] > 300)
				{
					anchor.ai[0] = 300;
				}

				RightClickChargeBuffer = anchor.ai[0];
				RightClickChargeBufferTimer = 60;
			}
			else
			{
				anchor.ai[0] = 0f;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			bool isHovering = false;

			if (projectile.ai[2] < 0f)
			{
				if (projectile.ai[2] < -300)
				{
					Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
				}

				if (anchor.IsInputJump && projectile.ai[2] >= -300)
				{ // Gliding
					intendedVelocity.Y *= 0.8f;
					isHovering = true;
					if (Main.rand.NextBool(12))
					{
						Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CorruptGibs)].velocity *= 0.1f;
					}
				}
				else
				{
					GravityCalculations(ref intendedVelocity, player, shapeshifter);
				}

				projectile.ai[2]++;
				if (projectile.ai[2] >= 0)
				{
					projectile.ai[2] = 0f;
				}
			}
			else
			{
				GravityCalculations(ref intendedVelocity, player, shapeshifter);
			}

			if (projectile.ai[2] > 0)
			{ // Dashing
				intendedVelocity = Vector2.UnitY.RotatedBy(projectile.ai[1]) * -8f * speedMult * shapeshifter.ShapeshifterMoveSpeedMiscOverride;
				projectile.direction = intendedVelocity.X > 0 ? 1 : -1;
				projectile.spriteDirection = projectile.direction;

				projectile.ai[2]--;
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;

				if (projectile.ai[2] <= 0)
				{
					projectile.ai[2] = -310f;
				}
			}
			else if (anchor.ai[2] <= 0f)
			{
				if (anchor.IsInputLeft || anchor.IsInputRight)
				{ // Player is inputting a movement key
					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, shapeshifter, (isHovering ? -4f : -3.2f), speedMult, 0.2f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, shapeshifter, (isHovering ? 4f : 3.2f), speedMult, 0.2f);
						projectile.direction = 1;
						projectile.spriteDirection = 1;
						LateralMovement = true;
					}
					else
					{ // Both keys pressed = no movement
						LateralMovement = false;
						TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);
					}
				}
				else
				{ // no movement input
					LateralMovement = false;
					TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);
				}

				float maxSpeed = (isHovering ? 5f : 4f); 
				if (Math.Abs(intendedVelocity.X) > maxSpeed && LateralMovement)
				{ // helps descelerate after a blast
					intendedVelocity.X *= 0.7f;
					if (Math.Abs(intendedVelocity.X) < maxSpeed)
					{
						intendedVelocity.X = maxSpeed * Math.Sign(intendedVelocity.X);
					}
				}

				float intendedDistance = 32f;
				if (anchor.IsInputDown) intendedDistance -= 16f;
				if (IsGrounded(projectile, player, intendedDistance, anchor.IsInputDown, anchor.IsInputDown))
				{ // Pushes away from the ground
					CanDash = true;
					intendedVelocity.Y -= player.gravity * 1.8f;
					if (intendedVelocity.Y < -2f)
					{
						intendedVelocity.Y = -2f;
					}
				}
				else if (IsGrounded(projectile, player, intendedDistance + 2.5f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 1f)
				{ // Locks up so the screen doesn't shake constantly
					CanDash = true;
					intendedVelocity.Y *= 0f;
				}
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player);

			if (Main.rand.NextBool(12))
			{
				Vector2 pos = projectile.Center + new Vector2(0, 2);
				if (projectile.spriteDirection == -1)
				{
					pos.X -= 14;
				}
				Dust dust = Dust.NewDustDirect(pos, 12, 8, DustID.CorruptGibs);
				dust.velocity *= 0.1f;
				dust.scale = Main.rand.NextFloat(0.4f, 0.7f);
			}

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

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<ShapeshifterBlankEffigy>();
			recipe.AddIngredient(ItemID.RottenChunk, 8);
			recipe.AddIngredient(ItemID.VilePowder, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}