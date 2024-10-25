using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Content.Shapeshifter.Dusts;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Utilities;
using System;
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
		public int Jumps = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Zombie111;
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			Item.knockBack = 1f;
			Item.damage = 19;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 30;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			MeleeSpeedLeft = true;
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
			/*
			int projectileType = ModContent.ProjectileType<SageOwlProj>();
			for (int i = 0; i < 3; i++)
			{
				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(7.5f)) * Item.shootSpeed * (0.85f + i * 0.15f);
				int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, damage, Item.knockBack, player.whoAmI);
				newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
				newProjectile.netUpdate = true;
			}
			*/

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
			FeatherDust(projectile, 2);
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter) && !Landed;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			/*
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
			*/

			// adjust shapeshift anchor fields
			anchor.RightCLickCooldown = Item.useTime * 4;
			//anchor.Projectile.ai[2] = 30;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);
			for (int i = 0; i < 3; i++)
			{
				FeatherDust(projectile, 2);
			}
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && Jumps > 0;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Projectile.ai[0] = -31;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS

			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.nightVision = true;
			player.noFallDmg = true;

			// ANIMATION

			if (anchor.Timespent % 6 == 0 && anchor.Timespent > 0 && anchor.Frame != 2)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame == 7)
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
			if (intendedVelocity.Y < 12f)
			{ // gravity
				intendedVelocity.Y += 0.185f;

				if (anchor.IsInputJump && intendedVelocity.Y >= 0.8f)
				{
					intendedVelocity.Y = 0.8f;
					anchor.Frame = 3;
				}
				else if (intendedVelocity.Y > 12f)
				{
					intendedVelocity.Y = 12f;
				}
			}

			float speedMult = player.moveSpeed;

			if (anchor.Projectile.ai[0] < -30)
			{ // Jump
				Jumps--;
				anchor.Frame = 3;
				anchor.Projectile.ai[0] = -30;
				intendedVelocity.Y = -8f;
				SoundEngine.PlaySound(SoundID.Item32, projectile.Center);

				for (int i = 0; i < 3; i++)
				{
					FeatherDust(projectile, 2);
				}

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left dash
					intendedVelocity.X = -4f * speedMult;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right dash
					intendedVelocity.X = 4f * speedMult;
				}
			}

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					intendedVelocity.X -= 0.2f * speedMult;
					if (intendedVelocity.X < -4f * speedMult) intendedVelocity.X = -4f * speedMult;
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					intendedVelocity.X += 0.2f * speedMult;
					if (intendedVelocity.X > 4f * speedMult) intendedVelocity.X = 4f * speedMult;
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

			if (Collision.TileCollision(projectile.position, Vector2.UnitY * 8f, projectile.width, projectile.height, anchor.IsInputDown, anchor.IsInputDown, (int)player.gravDir) != Vector2.UnitY * 8f)
			{ // Lands when near the ground
				Landed = true;
				anchor.Timespent = 0;
				if (LateralMovement)
				{ // Jumps when a movement input is done while landed
					Landed = false;
					Jumps = 3;
					if (intendedVelocity.Y > 0)
					{
						anchor.Frame = 3;
						anchor.Projectile.ai[0] = -30;
						intendedVelocity.Y = -5f;
						anchor.NeedNetUpdate = true;
						SoundEngine.PlaySound(SoundID.Item32, projectile.Center);

						for (int i = 0; i < 2; i++)
						{
							FeatherDust(projectile, 2);
						}
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
					projectile.direction = (int)anchor.Projectile.ai[1];
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

		public override void PreDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor)
		{

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
	}
}