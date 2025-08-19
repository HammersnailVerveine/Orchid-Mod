using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorGoblin : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 0, 48, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCHit6;
			Item.useTime = 45;
			Item.shootSpeed = 100f;
			Item.knockBack = 10f;
			Item.damage = 39;
			ShapeshiftWidth = 28;
			ShapeshiftHeight = 30;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			GroundedWildshape = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Shadowflame);
				dust.scale *= Main.rand.NextFloat(2f, 2.5f);
				dust.velocity *= Main.rand.NextFloat(0.25f, 0.5f);
				dust.noGravity = true;
				dust.noLightEmittence = true;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Shadowflame);
				dust.scale *= Main.rand.NextFloat(2f, 2.5f);
				dust.velocity *= Main.rand.NextFloat(0.25f, 0.5f);
				dust.noGravity = true;
				dust.noLightEmittence = true;
			}

			for (int i = 0; i < 8; i++)
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
				position += TileCollideShapeshifter(position, offSet, 2, 2, true, true, (int)player.gravDir);

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

			int projectileType = ModContent.ProjectileType<PredatorGoblinProj>();
			ShapeshifterNewProjectile(shapeshifter, position, offSet * 0.001f, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);

			anchor.LeftCLickCooldown = Item.useTime;
			projectile.ai[0] = 16;
			projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.ai[0] = 0;
			anchor.NeedNetUpdate = true;
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsRightClick && anchor.CanRightClick && projectile.ai[2] >= 100;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			float closestDistance = 80f;
			NPC closestTarget = null;

			if (shapeshifter.modPlayer.LastHitNPC != null)
			{
				if (OrchidModProjectile.IsValidTarget(shapeshifter.modPlayer.LastHitNPC) && shapeshifter.modPlayer.LastHitNPC.Center.Distance(projectile.Center) < 320f)
				{
					closestTarget = shapeshifter.modPlayer.LastHitNPC;
				}
			}

			foreach (NPC npc in Main.npc)
			{
				bool closestBoss = false;
				if (closestTarget != null)
				{
					if (closestTarget.boss)
					{
						closestBoss = true;
					}
				}

				if (OrchidModProjectile.IsValidTarget(npc) && Collision.CanHitLine(projectile.position + new Vector2(projectile.width * 0.5f, 0f), 2, 2, npc.position, npc.width, npc.height))
				{
					float distance = Main.MouseWorld.Distance(npc.Center);
					Point point = new Point((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y);
					if ((distance < closestDistance || (closestTarget == null && npc.Hitbox.Contains(point)) || (distance < 80f && npc.boss)) && (!closestBoss || npc.boss) && npc.Center.Distance(projectile.Center) < 320f)
					{
						closestTarget = npc;
						closestDistance = distance;
					}
				}
			}

			if (closestTarget != null)
			{
				int projectileType = ModContent.ProjectileType<PredatorGoblinProjAlt>();
				ShapeshifterNewProjectile(shapeshifter, projectile.Center, Vector2.Zero, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI, ai0: closestTarget.whoAmI);
				anchor.RightCLickCooldown = 60;

				projectile.ai[2] -= 100f;
				anchor.ai[0] = 0;
				anchor.NeedNetUpdate = true;
			}
		}

		public override void ShapeshiftBuffs(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (anchor.ai[0] >= 180)
			{
				player.aggro -= 1000;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds the attack animation
			// ai[1] holds the attack direction
			// ai[2] holds the right click charges (100, 200)
			// anchor.ai[0] holds the stealth timer (stealthed if > 180)
			// anchor.ai[1] holds removes deceleration if >0 (after a dash)
			// anchor.ai[2] holds bonus jumps

			projectile.ai[0]--;

			// MISC EFFECTS
			bool grounded = IsGrounded(projectile, player, 8f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

			if (anchor.ai[1] > 0)
			{
				anchor.ai[1]--;
				shapeshifter.ShapeshifterMoveSpeedDecelerate *= 0f;
			}

			if (grounded)
			{
				anchor.ai[2] = 1;
			}

			if (LateralMovement)
			{
				if (anchor.ai[0] >= 180 && projectile.ai[2] < 100)
				{
					projectile.ai[2] = 100;
					SoundStyle soundStyle = SoundID.Item103;
					soundStyle.Pitch -= 0.5f;
					SoundEngine.PlaySound(soundStyle, player.Center);

					for (int i = 0; i < 10; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Shadowflame);
						dust.scale *= Main.rand.NextFloat(1f, 1.5f);
						dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
					}
				}

				anchor.ai[0] = 0f;
			}

			if (projectile.ai[2] >= 100)
			{
				if (Main.rand.NextBool(10))
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Shadowflame);
					dust.scale *= Main.rand.NextFloat(0.5f, 1f);
					dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
					dust.noLightEmittence = true;
				}

				if (Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Shadowflame);
					dust.scale *= Main.rand.NextFloat(0.5f, 1f);
					dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
					dust.noGravity = true;
				}

				if (Main.rand.NextBool(6))
				{
					Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 0), projectile.width + 8, 8, DustID.Shadowflame);
					dust.scale *= Main.rand.NextFloat(0.5f, 1f);
					dust.velocity.Y = Main.rand.NextFloat(-1f, -0.5f);
					dust.velocity.X *= 0.2f;
					dust.noLightEmittence = true;
				}
			}

			// ANIMATION

			if (projectile.ai[0] > 0)
			{
				if (projectile.ai[0] > 6)
				{
					anchor.Frame = 9;
				}
				else
				{
					anchor.Frame = 10;
				}
			}
			else if (grounded && projectile.velocity.Y >= 0)
			{
				if (LateralMovement)
				{ // Player is moving left or right, cycle through frames
					if (anchor.Timespent % 4 == 0)
					{
						anchor.Frame++;
						if (anchor.Frame >= 9)
						{
							anchor.Frame = 1;
						}
					}
				}
				else
				{ // idle frame (& stealth progres)
					anchor.Frame = 0;
					anchor.ai[0]++;
				}
			}
			else
			{ // Falling frame
				anchor.Frame = 9;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (anchor.IsInputJump)
			{
				if (!grounded && anchor.ai[2] > 0 && anchor.JumpWithControlRelease(player))
				{ // double jump
					anchor.ai[2]--;
					anchor.ai[1] = 10;

					TryJump(ref intendedVelocity, 5.5f, player, shapeshifter, anchor, speedEfficiency: 0f);
					intendedVelocity.X = 10f * projectile.spriteDirection * speedMult * shapeshifter.ShapeshifterMoveSpeedMiscOverride;
					shapeshifter.ShapeshifterMoveSpeedDecelerate *= 0f;

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
				{ // grounded jump
					anchor.JumpWithControlRelease(player);
					TryJump(ref intendedVelocity, 9.3f, player, shapeshifter, anchor, true);
				}
			}

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				float acceleration = speedMult;
				if (!grounded) acceleration *= 0.5f;

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, shapeshifter, -4.25f, speedMult, 0.3f, acceleration);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, shapeshifter, 4.25f, speedMult, 0.3f, acceleration);
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

			if (projectile.ai[0] > 0)
			{ // override spire direction during attack animation
				projectile.direction = (int)projectile.ai[1];
				projectile.spriteDirection = (int)projectile.ai[1];
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[1] > 5 ? 7 : 5))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override Color GetColor(Color inputColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter, bool isHairColor = false)
		{
			Color color = base.GetColor(inputColor, projectile, anchor, player, shapeshifter, isHairColor);
			if (anchor.ai[0] > 180)
			{
				float colormult = 1f - (anchor.ai[0] - 180f) * 0.02f;
				colormult = colormult > 1f ? 1f : colormult < 0.2f ? 0.2f : colormult;

				color *= colormult;
			}

			return color;
		}

		public override void ShapeshiftPreDraw(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor)
		{
			if (projectile.ai[2] >= 100f)
			{
				//spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				//spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float colormult = 1f - (anchor.ai[0] - 180f) * 0.02f;
				colormult = colormult > 1f ? 1f : colormult < 0.2f ? 0.2f : colormult;
				Color color = new Color(153, 118, 255).MultiplyRGBA(lightColor * 2f);
				if (color.R < 64) color.R = 61;
				if (color.G < 64) color.G = 20;
				if (color.B < 64) color.B = 128;

				float scalemult = (float)Math.Sin(anchor.Timespent * 0.1f) * 0.03f + 1.125f;
				spriteBatch.Draw(anchor.TextureShapeshift, drawPosition, drawRectangle, color * 0.6f * colormult, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);
				spriteBatch.Draw(anchor.TextureShapeshiftHairGray, drawPosition, drawRectangle, color * 0.6f * colormult, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);

				//spriteBatch.End();
				//spriteBatch.Begin(spriteBatchSnapshot);
			}
		}
	}
}