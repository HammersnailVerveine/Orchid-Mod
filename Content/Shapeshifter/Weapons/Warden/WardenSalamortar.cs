using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenSalamortar : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public int Heat = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 2, 15, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.NPCHit33;
			Item.useTime = 20;
			Item.shootSpeed = 16f;
			Item.knockBack = 5f;
			Item.damage = 47;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 26;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			GroundedWildshape = true;
			AutoReuseRight = true;
		}

		public override void ShapeshiftAnchorOnShapeshiftFast(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.ai[1] = 60;
			projectile.ai[1] = 1;
			Heat = 1;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;
			Heat = 0;

			for (int i = 0; i < 6; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 6; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch);
				dust.scale *= Main.rand.NextFloat(1.5f, 2f);
				dust.velocity *= Main.rand.NextFloat(1.5f, 2f);
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch);
				dust.scale *= Main.rand.NextFloat(1.5f, 2f);
				dust.velocity *= Main.rand.NextFloat(1.5f, 2f);
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && (!ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter) || !anchor.IsRightClick);

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<WardenSalamortarProj>();
			Vector2 offset = Main.MouseWorld - projectile.Center;
			if (Math.Abs(offset.X) > 480f) offset.X = 480f * Math.Sign(offset.X);
			if (offset.Y < -240f) offset.Y = -240f;
			float ai1 = offset.Y / 20f;
			Vector2 velocity = new Vector2((offset.X + Main.rand.NextFloat(-16f, 16f)) / 60f, - 15f + Main.rand.NextFloat(-2f, 2f));
			ShapeshifterNewProjectile(shapeshifter, projectile.position + new Vector2(projectile.width * 0.5f - 4, 3f), velocity, projectileType, Item.damage * 0.2f, Item.crit, Item.knockBack, player.whoAmI, ai1 : ai1);
			anchor.LeftCLickCooldown = Item.useTime - Main.rand.Next(6);

			projectile.ai[2] = 0;
			anchor.NeedNetUpdate = true;
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsRightClick && (Main.mouseRightRelease || AutoReuseRight) && anchor.CanRightClick && anchor.CanLeftClick && Heat > 0;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<WardenSalamortarProjBig>();
			Vector2 offset = Main.MouseWorld - projectile.Center;
			if (Math.Abs(offset.X) > 480f) offset.X = 480f * Math.Sign(offset.X);
			if (Math.Abs(offset.Y) > 240f) offset.Y = 240f * Math.Sign(offset.Y);
			Vector2 velocity = new Vector2(offset.X / 60f, -20f);
			float ai1 = offset.Y / 30f;
			ShapeshifterNewProjectile(shapeshifter, projectile.position + new Vector2(projectile.width * 0.5f - 4, 3f), velocity, projectileType, Item.damage * (0.4f + Heat * 0.1f), Item.crit, Item.knockBack * 2f, player.whoAmI, ai1: ai1, ai2: Heat);
			anchor.RightCLickCooldown = Item.useTime * 3;

			projectile.ai[0] = 0;
			projectile.ai[1] = 0;
			projectile.ai[2] = 0;
			anchor.NeedNetUpdate = true;
		}

		public override void ShapeshiftBuffs(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			player.lavaImmune = true;
			player.fireWalk = true;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds the timer spent running in a single direction (positive or negative depending on dir)
			// ai[1] helps syncing Heat stacks animations properly
			// ai[2] holds the timer spent immobile - the salamander gets colder and loses heat stacks
			// anchor.ai[0] is == 1 if the player was just into lava (used to launch upwards when leaving it) 
			// anchor.ai[1] is a timer so the player doesn't spam lava launches in tight spaces

			// MISC EFFECTS & ANIMATION

			anchor.ai[1]++;
			bool grounded = IsGrounded(projectile, player, 8f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

			if (Math.Abs(projectile.velocity.X) == 0f)
			{
				projectile.ai[0] = 0;
				projectile.ai[2]++;

				if (projectile.ai[2] >= 600 && projectile.ai[2] % 120 == 0 && projectile.ai[1] > 0)
				{
					projectile.ai[1]--;
				}
			}
			else
			{
				projectile.ai[2] = 0;
			}

			// flickering fire light
			float lightMult = 0.5f + (float)(Math.Sin(anchor.Timespent * 0.04851f) * 0.1f) + (float)(Math.Sin(anchor.Timespent * 0.123f) * 0.05f) + (float)(Math.Sin(anchor.Timespent * 0.31461f) * 0.02f) + (float)(Math.Sin(anchor.Timespent * 0.07124f) * 0.03f);
			Lighting.AddLight(projectile.Center, 0.1f * Heat * lightMult, 0.05f * Heat * lightMult, 0f);

			// Heat dependent dusts

			if (Heat == 1)
			{
				if (Main.rand.NextBool(18))
				{
					Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 0), projectile.width + 8, 8, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(0.75f, 1f);
					dust.velocity.Y = Main.rand.NextFloat(-1f , -0.5f);
					dust.velocity.X *= 0.1f;
				}
			}

			if (Heat == 2)
			{
				if (Main.rand.NextBool(15))
				{
					Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 0), projectile.width + 8, 8, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(0.8f, 1.1f);
					dust.velocity.Y = Main.rand.NextFloat(-1f , -0.5f);
					dust.velocity.X *= 0.1f;
				}

				if (Main.rand.NextBool(8))
				{
					Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 0), projectile.width + 8, 8, DustID.Torch);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.velocity.Y = Main.rand.NextFloat(-1f , -0.5f);
					dust.noGravity = true;
					dust.noLight = true;
				}
			}

			if (Heat == 3)
			{
				if (Main.rand.NextBool(12))
				{
					Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 0), projectile.width + 8, 8, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(0.8f, 1.1f);
					dust.velocity.Y = Main.rand.NextFloat(-1f , -0.5f);
					dust.velocity.X *= 0.1f;
				}

				if (Main.rand.NextBool(6))
				{
					Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 0), projectile.width + 8, 8, DustID.Torch);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.velocity.Y = Main.rand.NextFloat(-1f , -0.5f);
					dust.noGravity = true;
					dust.noLight = true;
				}
			}

			// increases ai[0] when moving left or right & grants heat stacks periodically
			if (projectile.velocity.X > 0)
			{
				if (projectile.ai[0] < 0f)
				{
					projectile.ai[0] = 0f;
				}

				projectile.ai[0]++;
			}

			if (projectile.velocity.X < 0)
			{
				if (projectile.ai[0] > 0f)
				{
					projectile.ai[0] = 0f;
				}

				projectile.ai[0]--;
			}

			if (projectile.ai[0] % 90 == 0 && projectile.ai[0] != 0)
			{
				if (projectile.ai[1] == 3 && Math.Abs(projectile.ai[0]) == 90)
				{ // "hack" so that the visuals trigger
					Heat = 2;
					projectile.ai[1] = 2; 
				}

				if (projectile.ai[1] < 3) 
				{
					projectile.ai[1]++;
				}
			}

			if (Math.Abs(projectile.ai[0]) >= 90)
			{ // increases movement speed after running in the same direction for 90 frames
				speedMult *= 1.2f + Heat * 0.1f;
			}

			// Heat stacks gain sound & dusts / gores
			if (Heat != projectile.ai[1])
			{
				for (int i = 0; i < 7; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
				}

				for (int i = 0; i < 5; i++)
				{
					Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
					gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
					gore.velocity += projectile.velocity * Main.rand.NextFloat(0.5f, 1f);
				}

				if (Heat > projectile.ai[1])
				{
					SoundEngine.PlaySound(SoundID.LiquidsWaterLava, projectile.Center);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.LiquidsHoneyLava, projectile.Center);
					for (int i = 0; i < 8; i++)
					{
						Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Torch)].noGravity = true;
					}
				}

				Heat = (int)projectile.ai[1];
			}

			// Grounded animations

			// Player is moving left or right, cycle through frames
			if (grounded && Math.Abs(projectile.velocity.X) > 0.1f && projectile.velocity.Y >= 0f)
			{
				if (anchor.Timespent % (Math.Abs(projectile.ai[0]) >= 90 ? 3 : 4) == 0)
				{
					anchor.Frame++;
				}

				if (anchor.Frame > (7 + Heat * 8) || anchor.Frame < (2 + Heat * 8))
				{ // puts the animation back on track while moving
					anchor.Frame = 2 + Heat * 8;
				}
			}
			else if (!grounded)
			{ // falling frame
				anchor.Frame = 1 + Heat * 8;
			}
			else
			{
				anchor.Frame = 0 + Heat * 8;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;

			// Falling & Jumping
			if (anchor.IsInputJump)
			{ // Jump
				if (grounded)
				{
					TryJump(ref intendedVelocity, 10f, player, shapeshifter, anchor, true);
				}
			}

			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (player.lavaWet && intendedVelocity.Y > -20f)
			{
				projectile.ai[1] = 3;
				intendedVelocity.Y -= 0.35f;
				anchor.ai[0] = 1f;
				anchor.RightCLickCooldown = 120;
				anchor.LeftCLickCooldown = 30;
			}

			if (!player.lavaWet && anchor.ai[0] == 1f)
			{ // shoots out of lava
				anchor.ai[0] = 0f;
				if (anchor.ai[1] >= 60)
				{
					anchor.ai[1] = 0f;
					SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, projectile.Center);
					intendedVelocity.Y = anchor.IsInputJump ? -20f : -15f;

					if (IsLocalPlayer(player))
					{ // spawns some projectiles when jumping out of lava
						for (int i = 0; i < Main.rand.Next(3) + 3; i++)
						{
							int projectileType = ModContent.ProjectileType<WardenSalamortarProj>();
							Vector2 velocity = new Vector2(Main.rand.NextFloat(-2.66f, 2.66f), -15f + Main.rand.NextFloat(-2.5f, 2.5f));
							ShapeshifterNewProjectile(shapeshifter, projectile.position + new Vector2(projectile.width * 0.5f - 4, 3f), velocity, projectileType, Item.damage * 0.2f, Item.crit, Item.knockBack, player.whoAmI, ai1: Main.rand.NextFloat(-6f, 6f));
						}
					}
				}
			}

			// Normal movement
			if ((anchor.IsInputLeft || anchor.IsInputRight))
			{ // Player is inputting a movement key
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, shapeshifter, -3.5f, speedMult, 0.4f);
					projectile.direction = -1;
					LateralMovement = true;

					projectile.spriteDirection = projectile.direction;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, shapeshifter, 3.5f, speedMult, 0.4f);
					projectile.direction = 1;
					LateralMovement = true;

					projectile.spriteDirection = projectile.direction;
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

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (Math.Abs(projectile.ai[0]) >= 90 ? 7 : 4))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override Color GetColorGlow(Color lightColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			return player.GetImmuneAlphaPure(Color.White, 0f) * (0.5f + (float)(Math.Sin(anchor.Timespent * 0.04851f) * 0.1f) + (float)(Math.Sin(anchor.Timespent * 0.123f) * 0.05f) + (float)(Math.Sin(anchor.Timespent * 0.31461f) * 0.02f) + (float)(Math.Sin(anchor.Timespent * 0.07124f) * 0.03f));
		}
	}
}