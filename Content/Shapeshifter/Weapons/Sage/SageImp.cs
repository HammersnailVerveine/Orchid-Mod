using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageImp : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public bool CanDash = false;
		public int FastAttack = 0;
		public int FastAttackTimer = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 1, 55, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Zombie29;
			Item.useTime = 35;
			Item.shootSpeed = 2f;
			Item.knockBack = 3f;
			Item.damage = 61;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 26;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
			GroundedWildshape = false;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			CanDash = false;
			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

			LateralMovement = false;

			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 10; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Torch)].velocity *= 0.75f;
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
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Torch)].velocity *= 0.75f;
			}
		}

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageImpProj>();
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(FastAttack > 0 ? 0.2f : 0f) * Item.shootSpeed;
			ShapeshifterNewProjectile(shapeshifter, projectile.Center + new Vector2(0f, 2f), velocity, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);

			SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, projectile.Center);

			anchor.LeftCLickCooldown = Item.useTime;
			projectile.ai[0] = 15;

			if (FastAttack > 0)
			{
				FastAttack--;
				anchor.LeftCLickCooldown /= 3f;
				projectile.ai[0] /= 3f;
			}

			projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 position = projectile.Center;
			Vector2 offSet = Main.MouseWorld - projectile.Center;

			// Spawn the wall at the correct position, on the ground below the player cursor, up to 10 tiles away
			if (offSet.Length() > 160f)
			{
				offSet = Vector2.Normalize(offSet) * 160f;
			}

			for (int i = 0; i < 10; i++)
			{
				position += Collision.TileCollision(position, offSet * 0.1f, 2, 2, true, true, (int)player.gravDir);
			}
			
			for (int i = 0; i < 75; i++)
			{
				position += Collision.TileCollision(position, Vector2.UnitY * 15f, 18, 2, false, false, (int)player.gravDir);
			}

			position.Y -= 78; // half the wall height

			// Delete existing walls
			int projectileType = ModContent.ProjectileType<SageImpProjAlt>();
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.type == projectileType && proj.owner == player.whoAmI)
				{
					proj.Kill();
				}
			}

			ShapeshifterNewProjectile(shapeshifter, position, Vector2.Zero, projectileType, Item.damage * 2f, Item.crit, 0f, player.whoAmI);

			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);

			anchor.RightCLickCooldown = 60;
			projectile.ai[0] = 15;
			projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => player.controlJump && CanDash && projectile.ai[2] <= 0;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			float rotation = MathHelper.Pi * (1f + projectile.direction * 0.5f);

			// 8 dir input
			if (anchor.IsInputLeft && !anchor.IsInputRight)
			{
				rotation = MathHelper.Pi * 1.5f; // Left
				if (anchor.IsInputUp && !anchor.IsInputDown)
				{
					rotation += MathHelper.Pi * 0.25f; // Top Left
				}
				else if (!anchor.IsInputUp && anchor.IsInputDown)
				{
					rotation -= MathHelper.Pi * 0.25f; // Bottom Left
				}
			}
			else if (!anchor.IsInputLeft && anchor.IsInputRight)
			{
				rotation = MathHelper.Pi * 0.5f; // Right
				if (anchor.IsInputUp && !anchor.IsInputDown)
				{
					rotation -= MathHelper.Pi * 0.25f; // Top Right
				}
				else if (!anchor.IsInputUp && anchor.IsInputDown)
				{
					rotation += MathHelper.Pi * 0.25f; // Bottom Right
				}
			}
			else if (anchor.IsInputUp && !anchor.IsInputDown)
			{
				rotation = 0f; // Up
			}
			else if (!anchor.IsInputUp && anchor.IsInputDown)
			{
				rotation = MathHelper.Pi; // Down
			}

			anchor.LeftCLickCooldown = Item.useTime * 4f;
			anchor.NeedNetUpdate = true;
			CanDash = false;

			Vector2 position = projectile.position;
			Vector2 offSet = Vector2.UnitY.RotatedBy(rotation) * -6f * GetSpeedMult(player, shapeshifter, anchor);

			// helps with dush spawn sync in mp
			ShapeshifterNewProjectile(shapeshifter, projectile.Center, offSet, ModContent.ProjectileType<SageImpDash>(), 0, 0, 0, player.whoAmI);

			for (int i = 0; i < 32; i++)
			{
				position += Collision.TileCollision(position, offSet, projectile.width, projectile.height, true, true, (int)player.gravDir);
			}

			anchor.Teleport(position);
			projectile.position = position;
			projectile.velocity = offSet;
			projectile.velocity *= 0.75f;
			anchor.NeedNetUpdate = true;
			anchor.LeftCLickCooldown = Item.useTime;
			projectile.ai[2] = 30;
			Main.SetCameraLerp(0.1f, 5);
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] is used as a timer for attack animations
			// ai[1] is used to flip the sprite in the correct direction while attacking
			// ai[2] is used as a cooldown for the dash (jump)

			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.noFallDmg = true;

			projectile.ai[2]--;
			projectile.ai[0]--;
			FastAttackTimer--;

			if (FastAttackTimer <= 0)
			{
				FastAttack = 0;
			}

			GravityMult = 0.85f;
			if (anchor.IsInputDown) GravityMult += 0.15f;

			// ANIMATION

			if (anchor.Timespent % 5 == 0 && anchor.Timespent > 0)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame == 0)
				{
					anchor.Timespent = -5;
				}

				if (anchor.Frame == 1)
				{
					anchor.Timespent = -3;
				}

				if (anchor.Frame == 6)
				{
					anchor.Frame = 1;
				}
			}

			// ATTACK

			if (anchor.Projectile.ai[0] >= 0f)
			{ // Override animation during attack
				if (anchor.Projectile.ai[0] == 0)
				{
					anchor.Frame = 1;
				}
				else if (anchor.Projectile.ai[0] > 0)
				{
					anchor.Frame = 6;
				}
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			if (anchor.IsInputJump && intendedVelocity.Y >= 1.2f)
			{ // Gliding
				intendedVelocity.Y = 1.2f;
				if (anchor.Timespent % 4 == 0)
				{
					anchor.Timespent += 2;
				}
			}

			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, -3.2f, speedMult, 0.2f);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, 3.2f, speedMult, 0.2f);
					projectile.direction = 1;
					projectile.spriteDirection = 1;
					LateralMovement = true;
				}
				else
				{ // Both keys pressed = no movement
					LateralMovement = false;
					intendedVelocity.X *= 0.7f;
				}
			}
			else
			{ // no movement input
				LateralMovement = false;
				intendedVelocity.X *= 0.7f;
			}

			float intendedDistance = 32f;
			if (anchor.IsInputDown) intendedDistance -= 16f;
			if (IsGrounded(projectile, player, intendedDistance, anchor.IsInputDown, anchor.IsInputDown))
			{ // Pushes away from the ground
				CanDash = true;
				intendedVelocity.Y -= player.gravity * 1.4f;
				if (intendedVelocity.Y < -2.5f)
				{
					intendedVelocity.Y = -2.5f;
				}
			}
			else if (IsGrounded(projectile, player, intendedDistance + 2f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 1f)
			{ // Locks up so the screen doesn't shake constantly
				CanDash = true;
				intendedVelocity.Y *= 0f;
			}

			if (projectile.ai[0] >= 0)
			{ // Override direction during attack
				projectile.direction = (int)projectile.ai[1];
				projectile.spriteDirection = projectile.direction;
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
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}