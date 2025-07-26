using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenSnail : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public bool ShellStartEffect = false;
		public bool WasSlug = false;
		public bool BonusJump = false;

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.NPCHit9;
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			Item.knockBack = 10f;
			Item.damage = 25;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			ShapeshiftTypeUI = ShapeshifterShapeshiftTypeUI.List;
			GroundedWildshape = true;
			AutoReuseRight = true;
		}

		public override void ShapeshiftGetUIInfo(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter, ref int uiCount, ref int uiCountMax)
		{
			uiCount = 0;
			uiCountMax = 3;
			if (anchor.ai[1] > 0)
			{ // shell
				uiCount = (int)anchor.ai[1];
			}
			else
			{ // slug
				int i = 0;
				while (i < anchor.ai[2])
				{
					i += 300;
					if (i < anchor.ai[2])
					{
						uiCount++;
					}
				}

				if (uiCount > uiCountMax)
				{
					uiCount = uiCountMax;
				}
			}
		}

		public override void ShapeshiftAnchorOnShapeshiftFast(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.ai[1] = 1;
			anchor.LeftCLickCooldown = 20;
			anchor.RightCLickCooldown = 10;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;
			ShellStartEffect = false;
			WasSlug = false;
			BonusJump = false;
			projectile.ai[0] = 1;
			anchor.ai[1] = -1;
			anchor.LeftCLickCooldown = 60;
			anchor.RightCLickCooldown = 60;

			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && (anchor.ai[1] < 0 && anchor.ai[2] > 300);

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			ShapeshiftOnRightClick(projectile, anchor, player, shapeshifter);
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter) && (anchor.ai[1] > 0 || anchor.ai[2] > 300);

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (anchor.ai[1] == -1)
			{
				int i = 0;
				while (i < anchor.ai[2])
				{
					i += 300;
					if (i < anchor.ai[2])
					{
						anchor.ai[1] ++;
					}
				}

				anchor.ai[1]++;

				if (anchor.ai[1] > 3)
				{
					anchor.ai[1] = 3;
				}
			}
			else
			{
				int projectileType = ModContent.ProjectileType<WardenSnailProj>();
				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
				ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI, ai1: anchor.ai[1]);
				anchor.ai[1] = -1;
			}

			anchor.RightCLickCooldown = Item.useTime;
			anchor.NeedNetUpdate = true;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds the last player direction for the shell roll animation
			// ai[1] holds the movement speed bonus for rolling in the same direction
			// ai[2] holds the last movement direction while rolling
			// anchor.ai[0] == 1 if the snail is rolling on the previous frame
			// anchor.ai[1] holds the shell durability (3 -> 0). Is -1 if the player is a slug.
			// anchor.ai[2] holds the time spent as a slug

			// MISC EFFECTS

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

			if (anchor.ai[1] == 0 || anchor.ai[1] > 3)
			{
				SoundEngine.PlaySound(anchor.ai[1] == 0 ? SoundID.Item89 : Item.UseSound, projectile.Center);

				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
				}

				for (int i = 0; i < 3; i++)
				{
					Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
					gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
					gore.velocity += projectile.velocity * Main.rand.NextFloat(0.5f, 1f);
				}

				anchor.ai[1]--;
			}

			if (anchor.IsLeftClick && anchor.ai[1] > 0 && ((anchor.LeftCLickCooldown <= 0) || anchor.ai[0] == 1f))
			{ // shell roll input
				anchor.ai[0] = 1f;

				if (!ShellStartEffect)
				{
					ShellStartEffect = true;
					anchor.LeftCLickCooldown = 30;
					anchor.RightCLickCooldown = 10;
					SoundStyle soundStyle = SoundID.NPCHit9;
					soundStyle.Volume *= 0.5f;
					SoundEngine.PlaySound(soundStyle, projectile.Center);

					for (int i = 0; i < 3; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke);
						dust.scale *= Main.rand.NextFloat(1f, 1.5f);
						dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
					}

					for (int i = 0; i < 2; i++)
					{
						Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
						gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
						gore.velocity += projectile.velocity * Main.rand.NextFloat(0.5f, 1f);
					}
				}
			}
			else
			{
				anchor.ai[0] = 0f;
				projectile.ai[1] = 0.2f;
				ShellStartEffect = false;
			}

			if (anchor.ai[1] == -1)
			{ // is slug
				speedMult *= 1.5f;
				shapeshifter.ShapeshifterJumpSpeed *= 1.2f;
				anchor.ai[2]++;
				WasSlug = true;

				if (grounded)
				{
					BonusJump = true;
				}
			}
			else
			{
				if (WasSlug)
				{ // shell pickup sound & effects
					SoundEngine.PlaySound(SoundID.Grab, projectile.Center);
					WasSlug = false;

					for (int i = 0; i < 5; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke);
						dust.scale *= Main.rand.NextFloat(1f, 1.5f);
						dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
					}

					for (int i = 0; i < 2; i++)
					{
						Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
						gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
					}
				}

				foreach (Projectile proj in Main.projectile)
				{
					if (proj.type == ModContent.ProjectileType<WardenSnailProj>() && proj.active && proj.owner == player.whoAmI)
					{
						proj.Kill();
					}
				}

				BonusJump = false;
				anchor.ai[2] = 0;
			}


			if (anchor.ai[0] == 1)
			{
				if (projectile.ai[1] > 0.5f || projectile.velocity.Y > 5f)
				{
					projectile.knockBack = Item.knockBack * Math.Max(projectile.ai[1], Math.Abs(projectile.velocity.Y * 0.1f));
					projectile.damage = shapeshifter.GetShapeshifterDamage(Item.damage * Math.Max(projectile.ai[1], Math.Abs(projectile.velocity.Y * 0.1f)));
					projectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
					projectile.friendly = true;
				}
				else
				{
					projectile.friendly = false;
				}

				projectile.ai[1] += 0.022f * speedMult;

				if (projectile.ai[1] > 2f)
				{
					projectile.ai[1] = 2f;
				}

				if (!LateralMovement)
				{
					projectile.ai[1] = 0.2f;
				}

				if (projectile.velocity.X > 0)
				{
					if (projectile.ai[2] <= 0f)
					{
						projectile.ai[2] = 1f;
						projectile.ai[1] = 0.2f;
					}
				}

				if (projectile.velocity.X < 0)
				{
					if (projectile.ai[2] >= 0f)
					{
						projectile.ai[2] = -1f;
						projectile.ai[1] = 0.2f;
					}
				}

				speedMult *= projectile.ai[1];
			}
			else
			{
				projectile.friendly = false;
			}

			// ANIMATION
			if (anchor.ai[0] == 1f)
			{
				anchor.Frame = 11;
				if (grounded && projectile.velocity.Y >= 0f)
				{
					projectile.rotation += projectile.velocity.X / 15f;
				}
				else
				{
					projectile.rotation += projectile.velocity.X / 30f;
				}
			}
			else if (grounded && projectile.velocity.Y >= 0f)
			{
				projectile.rotation = 0f;

				if (LateralMovement)
				{ // Player is moving left or right, cycle through frames
					if (anchor.Timespent % 5 == 0)
					{
						anchor.Frame++;
					}

					if (anchor.Frame < (anchor.ai[1] == -1 ? 5 : 1) || anchor.Frame > (anchor.ai[1] == -1 ? 8 : 4))
					{
						anchor.Frame = anchor.ai[1] == -1 ? 5 : 1;
					}
				}
				else
				{ // idle frame
					anchor.Frame = anchor.ai[1] == -1 ? 5 : 0;
				}
			}
			else
			{ // Falling frame
				projectile.rotation = 0f;
				anchor.Frame = anchor.ai[1] == -1 ? 10 : 9;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (anchor.IsInputJump)
			{ // Jump while no charge ready
				if (!grounded && BonusJump && anchor.JumpWithControlRelease(player))
				{
					BonusJump = false;
					TryJump(ref intendedVelocity, 8f, player, shapeshifter, anchor);
					SoundEngine.PlaySound(SoundID.DoubleJump, projectile.Center);

					for (int i = 0; i < 5; i++)
					{
						Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
					}

					for (int i = 0; i < 3; i++)
					{
						Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
						gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
						gore.velocity.Y += Main.rand.NextFloat(0.25f, 0.5f);
					}
				}
				else
				{
					anchor.JumpWithControlRelease(player);
					TryJump(ref intendedVelocity, 8f, player, shapeshifter, anchor, true);
				}
			}

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				float acceleration = speedMult;
				if (!grounded || anchor.ai[0] == 1f) acceleration *= anchor.ai[0] == 1f ? 0.25f : 0.5f;

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, shapeshifter, -2f, speedMult, 0.3f, acceleration);
					projectile.direction = anchor.ai[0] == 1f ? (int)projectile.ai[0] : -1;
					projectile.spriteDirection = projectile.direction;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, shapeshifter, 2f, speedMult, 0.3f, acceleration);
					projectile.direction = anchor.ai[0] == 1f ? (int)projectile.ai[0] : 1;
					projectile.spriteDirection = projectile.direction;
					LateralMovement = true;
				}
				else
				{ // Both keys pressed = no movement
					LateralMovement = false;
					TrySlowDown(ref intendedVelocity, anchor.ai[0] == 1f ? 0.95f : 0.7f, player, shapeshifter, projectile);
				}
			}
			else
			{ // no movement input
				LateralMovement = false;
				TrySlowDown(ref intendedVelocity, anchor.ai[0] == 1f ? 0.95f : 0.7f, player, shapeshifter, projectile);
			}
			
			if (anchor.ai[0] == 0f)
			{
				projectile.ai[0] = projectile.direction;
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[1] > 0.5f ? 5 : 4))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override bool ShapeshiftFreeDodge(Player.HurtInfo info, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (anchor.ai[0] == 1)
			{
				int immunityTime = (int)(40 * projectile.ai[1]);
				if (immunityTime < 40) immunityTime = 40;
				shapeshifter.modPlayer.SetDodgeImmuneTime(immunityTime);
				SoundEngine.PlaySound(SoundID.Item50, projectile.Center);
				anchor.ai[1]--;
				anchor.NeedNetUpdate = true;

				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
				}

				for (int i = 0; i < 3; i++)
				{
					Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
					gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
					gore.velocity += projectile.velocity * Main.rand.NextFloat(0.5f, 1f);
				}
				return true;
			}

			return false;
		}
	}
}