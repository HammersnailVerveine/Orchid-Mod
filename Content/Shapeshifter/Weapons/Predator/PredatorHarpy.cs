using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Dusts;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorHarpy : OrchidModShapeshifterShapeshift
	{
		public bool Landed = false;
		public bool LateralMovement = false;
		public bool RightClickLanding = false; 
		public bool Reinforced = false; 
		public int Jumps = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 65, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Critter;
			Item.useTime = 35;
			Item.shootSpeed = 17.5f;
			Item.knockBack = 0.5f;
			Item.damage = 19;
			Item.crit = 6;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 26;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			GravityMult = 0.9f;
			GroundedWildshape = false;
		}

		public override Color GetColorGlow(ref bool drawPlayerAsAdditive, Color lightColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			return base.GetColorGlow(ref drawPlayerAsAdditive, lightColor, projectile, anchor, player, shapeshifter) * 0.5f;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

			Landed = false;
			LateralMovement = false;
			Jumps = 0;
			RightClickLanding = false;
			Reinforced = false; 

			for (int i = 0; i < 8; i++)
			{
				FeatherDust(projectile, 1);
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				FeatherDust(projectile, 1);
			}
		}

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Reinforced)
			{
				Reinforced = false;
				int projectileType = ModContent.ProjectileType<PredatorHarpyProjAlt>();
				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed * 1.2f;
				ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage * 2.5f, Item.crit, Item.knockBack * 5f, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item65, projectile.Center);
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
					int projectileType = ModContent.ProjectileType<PredatorHarpyProj>();
					Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f + 10 * i)) * (Item.shootSpeed - 5 * i);
					ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);
				}
				SoundEngine.PlaySound(SoundID.Item64, projectile.Center);
			}

			anchor.LeftCLickCooldown = Item.useTime;
			projectile.ai[0] = 10;
			projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			FeatherDust(projectile, 2);
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => Main.mouseRight && (Main.mouseRightRelease || AutoReuseRight) && anchor.CanRightClick && !Landed && RightClickLanding;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// adjust shapeshift anchor fields
			projectile.velocity.Y = 0f;
			anchor.RightCLickCooldown = Item.useTime * 4;
			projectile.ai[2] = 30;
			anchor.NeedNetUpdate = true;
			RightClickLanding = false;
			Reinforced = true;
			
			if (anchor.LeftCLickCooldown < 10)
			{
				anchor.LeftCLickCooldown = 10;
			}

			if (anchor.IsInputLeft && !anchor.IsInputRight)
			{ // Left movement
				projectile.direction = -1;
			}
			else if (anchor.IsInputRight && !anchor.IsInputLeft)
			{ // Right movement
				projectile.direction = 1;
			}

			projectile.ai[1] = projectile.direction;

			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);
			for (int i = 0; i < 4; i++)
			{
				FeatherDust(projectile, 2);
			}
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && Jumps > 0;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Projectile.ai[0] = -31;
			anchor.NeedNetUpdate = true;
			Reinforced = true;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS

			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.nightVision = true;
			player.noFallDmg = true;

			GravityMult = 0.7f;
			if (anchor.IsInputDown) GravityMult += 0.3f;

			// ANIMATION

			if (anchor.Timespent % 6 == 0 && anchor.Timespent > 0 && anchor.Frame != 2)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame >= 7)
				{
					anchor.Frame = 1;
				}
			}

			if (Landed)
			{
				anchor.Timespent = 0;
				anchor.Frame = 0;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (anchor.IsInputJump && intendedVelocity.Y >= 0.8f)
			{ // Gliding
				intendedVelocity.Y = 0.8f;
				anchor.Frame = 3;
			}

			if (anchor.Projectile.ai[0] < -30)
			{ // Jump
				Jumps--;
				anchor.Frame = 3;
				anchor.Projectile.ai[0] = -30;
				TryJump(ref intendedVelocity, 7f, player, shapeshifter, anchor);
				SoundEngine.PlaySound(SoundID.Item32, projectile.Center);

				for (int i = 0; i < 2; i++)
				{
					FeatherDust(projectile, 2);
				}

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left dash
					intendedVelocity.X = -3.5f * speedMult;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right dash
					intendedVelocity.X = 3.5f * speedMult;
				}
			}

			// Normal movement
			if (projectile.ai[2] > 0)
			{ // Right click Dash
				if (projectile.ai[2] > 27f && !IsLocalPlayer(player))
				{
					projectile.direction = (int)projectile.ai[1];
				}

				anchor.Frame = 1;
				anchor.Timespent = 0;
				intendedVelocity.X = 7f * speedMult * projectile.direction * shapeshifter.ShapeshifterMoveSpeedMiscOverride;
				if (intendedVelocity.Y > 0) intendedVelocity.Y = 0f;

				projectile.ai[2]--;
				if (projectile.ai[2] % 5 == 0 && IsLocalPlayer(player))
				{
					int projectileType = ModContent.ProjectileType<PredatorHarpyProj>();
					Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(7.5f)) * Item.shootSpeed * Main.rand.NextFloat(0.8f, 1.2f);
					ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage * 0.6f, Item.crit, Item.knockBack, player.whoAmI);
					SoundEngine.PlaySound(SoundID.Item63, projectile.Center);
				}

				FeatherDust(projectile, 8);

				if (Collision.TileCollision(projectile.position, Vector2.UnitX * 10f * speedMult * projectile.direction, projectile.width, projectile.height, true, true, (int)player.gravDir) != Vector2.UnitX * 10f * speedMult * projectile.direction)
				{
					projectile.ai[2] = 0;

					for (int i = 0; i < 6; i++)
					{
						FeatherDust(projectile);
					}
				}
			}
			else if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, -3.5f, speedMult, 0.2f);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, 3.5f, speedMult, 0.2f);
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

			if (IsGrounded(projectile, player, 8f, anchor.IsInputDown, anchor.IsInputDown))
			{ // Lands when near the groun
				RightClickLanding = true;
				Landed = true;
				anchor.Timespent = 0;
				if (LateralMovement || anchor.IsLeftClick)
				{ // Jumps when a movement input is done while landed
					Landed = false;
					Jumps = 3;
					if (intendedVelocity.Y > 0)
					{
						anchor.Frame = 3;
						anchor.Projectile.ai[0] = -30;
						TryJump(ref intendedVelocity, 7f, player, shapeshifter, anchor);
						anchor.NeedNetUpdate = true;
						SoundEngine.PlaySound(SoundID.Item32, projectile.Center);

						FeatherDust(projectile, 2);
					}
				}
			}
			else
			{
				Landed = false;
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player);

			// ATTACK

			if (anchor.Projectile.ai[0] > -30)
			{ // Override animation during attack
				anchor.Projectile.ai[0]--;
				if (anchor.Projectile.ai[0] == 0)
				{
					anchor.Frame = 1;
				}
				else if (anchor.Projectile.ai[0] > 0)
				{
					anchor.Frame = 7;
					projectile.direction = (int)projectile.ai[1];
					projectile.spriteDirection = projectile.direction;
				}
				else if (anchor.Projectile.ai[0] < -30)
				{
					anchor.Projectile.ai[0] = -30;
				}
			}

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

		public void FeatherDust(Projectile projectile, int rand = 1)
		{
			if (Main.rand.NextBool(rand))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<PredatorHarpyDust>(), Scale: Main.rand.NextFloat(1.2f, 1.4f));
				dust.velocity *= 0.5f;
				dust.velocity.Y = 2f;
				dust.customData = Main.rand.Next(314);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<ShapeshifterBlankEffigy>();
			recipe.AddIngredient(ItemID.Feather, 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}