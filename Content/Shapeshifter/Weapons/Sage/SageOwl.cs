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

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageOwl : OrchidModShapeshifterShapeshift
	{
		public bool Landed = false;
		public bool LateralMovement = false;
		public bool CanDash = false;
		public int DashCooldown = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 23, 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Zombie111;
			Item.useTime = 35;
			Item.shootSpeed = 8f;
			Item.knockBack = 1f;
			Item.damage = 9;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 30;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
			GroundedWildshape = false;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 2;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			DashCooldown = 30;

			Landed = false;
			CanDash = false;
			LateralMovement = false;

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

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && !Landed;

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int damage = Item.damage;
			int cooldown = Item.useTime;

			/*
			if (AscendTimer > -60)
			{ // More damage and attack speed while hovering
				damage += 3;
				cooldown -= 15;
			}
			*/

			int projectileType = ModContent.ProjectileType<SageOwlProj>();
			for (int i = 0; i < 2; i++)
			{
				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(7.5f)) * Item.shootSpeed * (0.85f + i * 0.15f);
				ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, damage, Item.crit, Item.knockBack, player.whoAmI);
			}

			anchor.LeftCLickCooldown = cooldown;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
			FeatherDust(projectile, 2);
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter) && !Landed;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			projectile.velocity *= 0f;
			int projectileType = ModContent.ProjectileType<SageOwlProjAlt>(); ;
			Vector2 position = projectile.Center;
			position.Y += 90;
			Projectile.NewProjectile(Item.GetSource_FromAI(), position, Vector2.Zero, projectileType, 0, 0f, player.whoAmI);

			foreach (NPC npc in Main.npc)
			{ // Applies the ability debuff to all valid NPCs
				if (OrchidModProjectile.IsValidTarget(npc))
				{
					float angle = (npc.Center - projectile.Center).ToRotation();
					if (npc.Center.Distance(projectile.Center) < 480f && angle > MathHelper.Pi * 0.25f && angle < MathHelper.Pi * 0.75f)
					{
						npc.AddBuff(ModContent.BuffType<SageOwlDebuff>(), 600);
					}
				}
			}

			// adjust shapeshift anchor fields
			anchor.RightCLickCooldown = Item.useTime * 4;
			anchor.Projectile.ai[2] = 20;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);
			for (int i = 0; i < 3; i++)
			{
				FeatherDust(projectile, 2);
			}
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && CanDash && projectile.ai[0] <= 0 && DashCooldown <= 0;

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

			if (!Landed)
			{ // Drops feathers while flying
			}

			// ANIMATION

			if (Landed)
			{
				anchor.Frame = 0;
			}
			else
			{ // Animation frames
				FeatherDust(projectile, 120);

				if (anchor.Frame < 1 || anchor.Frame > 6)
				{
					anchor.Frame = 1;
				}

				if (anchor.Timespent % 5 == 0 && anchor.Timespent > 0)
				{
					anchor.Frame++;

					if (anchor.Frame <= 1)
					{
						anchor.Frame = 1;
						anchor.Timespent = -3;
					}

					if (anchor.Frame == 2)
					{
						anchor.Timespent = -1;
					}

					if (anchor.Frame == 7)
					{
						anchor.Frame = 1;
						anchor.Timespent = -3;
					}
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
			}
			else
			{
				if (anchor.IsInputLeft || anchor.IsInputRight)
				{ // Player is inputting a movement key
					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, -3f, speedMult, 0.2f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, 3f, speedMult, 0.2f);
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
					intendedVelocity.Y -= player.gravity * 2f;
					if (intendedVelocity.Y < -2f)
					{
						intendedVelocity.Y = -2f;
					}
				}
				else if (IsGrounded(projectile, player, intendedDistance + 2f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 1f)
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
				if (anchor.OldPosition.Count > 4)
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override void ShapeshiftPreDraw(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor)
		{
			if (projectile.ai[2] > 0)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float scalemult = (float)Math.Sin(projectile.ai[2] * 0.157f) * 0.25f + 1f;
				spriteBatch.Draw(anchor.TextureShapeshift, drawPosition, drawRectangle, lightColor * 0.75f, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
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

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}