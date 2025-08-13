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
	public class SageBee : OrchidModShapeshifterShapeshift
	{
		public bool CanDash = false;
		public bool LateralMovement = false;
		public int DashCooldown = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Zombie125;
			Item.useTime = 40;
			Item.shootSpeed = 7.5f;
			Item.knockBack = 3f;
			Item.damage = 25;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 30;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
			MeleeSpeedRight = true;
			AutoReuseRight = true;
			GroundedWildshape = false;
		}

		public override void ShapeshiftAnchorOnShapeshiftFast(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			DashCooldown = 5;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 2;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			DashCooldown = 30;

			CanDash = false;
			LateralMovement = false;

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 4; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Honey)].noGravity = true;
			}

			foreach (Player otherPlayer in Main.player)
			{ // Covers nearby players in honey
				if (otherPlayer.Center.Distance(player.Center) < 160f && otherPlayer.active)
				{
					otherPlayer.AddBuff(BuffID.Honey, 300);
				}
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 4; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Honey)].noGravity = true;
			}

			foreach (Player otherPlayer in Main.player)
			{ // Covers nearby players in honey
				if (otherPlayer.Center.Distance(player.Center) < 160f && otherPlayer.active)
				{
					otherPlayer.AddBuff(BuffID.Honey, 300);
				}
			}
		}

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter);
		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageBeeProj>();
			Vector2 offset = new Vector2(Main.rand.NextFloat(-4f, 4f), 14f + Main.rand.NextFloat(-4f, 4f));
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - (projectile.Center + offset)).RotatedByRandom(MathHelper.ToRadians(3f)) * Item.shootSpeed;
			ShapeshifterNewProjectile(shapeshifter, projectile.Center + offset, velocity, projectileType, Item.damage * 2.5f, Item.crit, Item.knockBack, player.whoAmI);

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.RightCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.Item17, projectile.Center);
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageBeeProj>();
			int count = 2;
			foreach (Projectile proj in Main.projectile)
			{ // Counts active stingers to increase the amount of bees
				if (proj.active && proj.type == projectileType && proj.owner == Main.myPlayer && proj.ai[0] != -1)
				{
					count++;
				}
			}

			count = (int)Math.Floor(count * 0.66f);

			if (player.strongBees)
			{
				count += Main.rand.Next(2);
			}

			int damage = shapeshifter.GetShapeshifterDamage(Item.damage * 0.5f);
			for (int i = 0; i < count; i++)
			{
				Projectile newProjectile;
				if (player.strongBees && Main.rand.NextBool())
					newProjectile = ShapeshifterNewProjectile(shapeshifter, projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(6f), ProjectileID.GiantBee, Item.damage * 0.575f, Item.crit, 0f, player.whoAmI);
				else
					newProjectile = ShapeshifterNewProjectile(shapeshifter, projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(6f), ProjectileID.Bee, Item.damage * 0.5f, Item.crit, 0f, player.whoAmI);

				newProjectile.DamageType = ModContent.GetInstance<ShapeshifterDamageClass>();
			}

			// adjust shapeshift anchor fields
			anchor.LeftCLickCooldown = Item.useTime;
			anchor.RightCLickCooldown = Item.useTime;
			anchor.NeedNetUpdate = true;
			SoundEngine.PlaySound(SoundID.Item97, projectile.Center);

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Honey)].noGravity = true;
			}
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && CanDash && projectile.ai[0] <= 0 && DashCooldown <= 0;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// 8 dir input
			if (anchor.IsInputLeft && !anchor.IsInputRight)
			{
				anchor.Projectile.ai[1] = MathHelper.Pi * 1.5f; // Left
				if (anchor.IsInputUp && !anchor.IsInputDown)
				{
					anchor.Projectile.ai[1] += MathHelper.Pi * 0.25f; // Top Left
				}
				else if (!anchor.IsInputUp && anchor.IsInputDown)
				{
					anchor.Projectile.ai[1] -= MathHelper.Pi * 0.25f; // Bottom Left
				}
			}
			else if (!anchor.IsInputLeft && anchor.IsInputRight)
			{
				anchor.Projectile.ai[1] = MathHelper.Pi * 0.5f; // Right
				if (anchor.IsInputUp && !anchor.IsInputDown)
				{
					anchor.Projectile.ai[1] -= MathHelper.Pi * 0.25f; // Top Right
				}
				else if (!anchor.IsInputUp && anchor.IsInputDown)
				{
					anchor.Projectile.ai[1] += MathHelper.Pi * 0.25f; // Bottom Right
				}
			}
			else if (anchor.IsInputUp && !anchor.IsInputDown)
			{
				anchor.Projectile.ai[1] = 0f; // Up
			}
			else if (!anchor.IsInputUp && anchor.IsInputDown)
			{
				anchor.Projectile.ai[1] = MathHelper.Pi; // Down
			}
			else
			{ // Projectile Direction (no input)
				anchor.Projectile.ai[1] = MathHelper.Pi * (1f +  projectile.direction * 0.5f);
			}

			projectile.ai[2] = 4;
			projectile.ai[0] = 10;
			DashCooldown = 30;
			anchor.LeftCLickCooldown = Item.useTime;
			anchor.RightCLickCooldown = Item.useTime;
			anchor.NeedNetUpdate = true;
			CanDash = false;
			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);

			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 4; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Honey)].noGravity = true;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.noFallDmg = true;
			projectile.ai[2]--;
			DashCooldown--;

			GravityMult = 0.7f;
			if (anchor.IsInputDown) GravityMult += 0.3f;

			// ANIMATION

			if (anchor.Timespent % 4 == 0)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame == 4)
				{
					anchor.Frame = 0;
				}
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (anchor.IsInputJump && intendedVelocity.Y >= 1.2f)
			{ // Gliding
				intendedVelocity.Y = 1.2f;
				if (anchor.Timespent % 4 == 0)
				{
					anchor.Timespent += 2;
				}
			}

			if (projectile.ai[2] > 0)
			{ // Dashing
				intendedVelocity = Vector2.UnitY.RotatedBy(projectile.ai[1]) * -10f * speedMult * shapeshifter.ShapeshifterMoveSpeedMiscOverride;
				projectile.direction = intendedVelocity.X > 0 ? 1 : -1;
				projectile.spriteDirection = projectile.direction;

				if (Main.rand.NextBool())
				{
					Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Honey)].noGravity = true;
				}
			}
			else
			{
				if (anchor.IsInputLeft || anchor.IsInputRight)
				{ // Player is inputting a movement key
					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, shapeshifter, -3.5f, speedMult, 0.2f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, shapeshifter, 3.5f, speedMult, 0.2f);
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

				float intendedDistance = 32f;
				if (anchor.IsInputDown) intendedDistance -= 16f;
				if (IsGrounded(projectile, player, intendedDistance, anchor.IsInputDown, anchor.IsInputDown))
				{ // Pushes away from the ground
					CanDash = true;
					intendedVelocity.Y -= player.gravity * 2f;
					if (intendedVelocity.Y < -2f)
					{
						intendedVelocity.Y = -2f;
					}
				}
				else if (IsGrounded(projectile, player, intendedDistance + 2f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 1f && shapeshifter.ShapeshifterMoveSpeedDecelerate != 0f)
				{ // Locks up so the screen doesn't shake constantly
					CanDash = true;
					intendedVelocity.Y *= 0f;
				}


				if (projectile.ai[0] > 0)
				{ // Override animation during attack
					projectile.ai[0]--;
					if (projectile.ai[2] < -45)
					{
						projectile.direction = (int)projectile.ai[1];
						projectile.spriteDirection = projectile.direction;
					}
				}
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > 5)
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
			recipe.AddIngredient(ItemID.BeeWax, 14);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}