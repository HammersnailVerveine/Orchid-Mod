using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using OrchidMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageFox : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCDeath4;
			Item.useTime = 40;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 34;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
		}


		public override Color GetColor(ref bool drawPlayerAsAdditive, Color lightColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (projectile.ai[1] > 0)
			{ // Right click phaseshift is active
				drawPlayerAsAdditive = true;
				return new Color(54, 150, 248) * 0.75f;
			}
			return base.GetColor(ref drawPlayerAsAdditive, lightColor, projectile, anchor, player, shapeshifter);
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			shapeshifter.ShapeshifterSageFoxSpeed = 180;

			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;

			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			shapeshifter.ShapeshifterSageFoxSpeed = 180;

			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageFoxProj>();
			float ai1 = 0f;
			float ai0 = 0f;
			int count = 0;
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType && proj.ai[1] >= 0f)
				{
					count++;
					ai0 = proj.ai[0];
					if (proj.ai[1] == 0f) ai1 = 1f;
					if (proj.ai[1] == 1f && ai1 == 1f) ai1 = 2f;
				}
			}

			if (count < 3)
			{ // Spawn a flame if less than 3 currently exist
				int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, Vector2.Zero, projectileType, damage, 0f, player.whoAmI, ai0, ai1);
				newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);

				projectile.ai[2] = 30;
				anchor.LeftCLickCooldown = Item.useTime;
				anchor.NeedNetUpdate = true;
				SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, projectile.Center);


				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
		}

		public override bool CanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => Main.mouseRight && (Main.mouseRightRelease || AutoReuseRight) && anchor.CanRightClick;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.RightCLickCooldown = 180;
			anchor.NeedNetUpdate = true;
			projectile.ai[1] = 90;
			SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, projectile.Center);

			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override bool CanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && projectile.ai[0] >= 300;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1.4f, 2f));
				dust.noGravity = true;
			}

			projectile.ai[0] -= 300;
			Vector2 position = projectile.position;
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center) * 8f;

			for (int i = 0; i < 32; i++)
			{
				position += Collision.TileCollision(position, offSet, projectile.width, projectile.height, true, false, (int)player.gravDir);
				Dust dust = Dust.NewDustDirect(position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1.4f, 2f));
				dust.noGravity = true;
			}

			projectile.position = position;
			projectile.velocity = offSet;
			projectile.velocity.Y *= 0.5f;
			anchor.NeedNetUpdate = true;

			// Spawn 3 foxfire flames after the dash
			int projectileType = ModContent.ProjectileType<SageFoxProj>();

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType)
				{
					proj.ai[1] = -1;
				}
			}

			for (int j = 0; j < 3; j++)
			{
				int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, Vector2.Zero, projectileType, damage, 0f, player.whoAmI, 0f, j);
				newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);

				projectile.ai[2] = 30;
				anchor.LeftCLickCooldown = Item.useTime;
				anchor.NeedNetUpdate = true;
			}

			SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, projectile.Center);

			// Kill one of the dash indicators following the player
			int projectileType2 = ModContent.ProjectileType<SageFoxProjAlt>();
			Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType2).Kill();

			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1.4f, 2f));
				dust.noGravity = true;
			}

			SoundEngine.PlaySound(SoundID.Item28, projectile.Center);
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS

			bool grounded = IsGrounded(projectile, player);

			if ((int)projectile.ai[0] < 601)
			{ // Increases the dash timer
				projectile.ai[0]++;

				if ((int)projectile.ai[0] % 300 == 0)
				{ // Spawns a projectile following the player when the dash is ready
					int projectileType = ModContent.ProjectileType<SageFoxProjAlt>();
					Vector2 offset = Vector2.UnitY.RotatedByRandom(3.14f) * 64f;
					Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center + offset, Vector2.Zero, projectileType, 0, 0f, player.whoAmI, offset.X * 0.375f, offset.Y * 0.375f);
					SoundEngine.PlaySound(SoundID.Item30, projectile.Center);
				}
			}

			if (projectile.ai[1] > 0)
			{ // Right click shift lighting and damage
				projectile.damage = shapeshifter.GetShapeshifterDamage(Item.damage * 1.5f);
				projectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
				projectile.friendly = true;

				projectile.ai[1]--;
				Color color = Color.Aqua * ((float)Math.Sin(projectile.ai[1] * 0.1046f) * 0.2f + 0.2f);
				Lighting.AddLight(projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

				if (Main.rand.NextBool(2))
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
					dust.noGravity = true;
					dust.noLight = true;
				}

				if (projectile.ai[1] <= 0)
				{
					SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, projectile.Center);
				}
			}
			else
			{
				projectile.damage = 0;
				projectile.CritChance = 0;
				projectile.friendly = false;
			}

			if (projectile.ai[2] > 0)
			{ // Left click lighting
				anchor.Projectile.ai[2]--;
				Color color = Color.Aqua * (float)Math.Sin(projectile.ai[2] * 0.1046f) * 0.2f;
				Lighting.AddLight(projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			}

			// ANIMATION

			if (grounded)
			{
				if (LateralMovement)
				{ // Player is moving left or right, cycle through frames
					if (anchor.Timespent % 4 == 0 && anchor.Timespent > 0)
					{
						anchor.Frame++;
						if (anchor.Frame == 8)
						{
							anchor.Frame = 1;
						}
					}
				}
				else
				{ // idle frame
					anchor.Timespent = 0;
					anchor.Frame = 0;
				}
			}
			else
			{ // Falling frame
				anchor.Timespent = 0;
				anchor.Frame = 5;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;

			intendedVelocity.Y += 0.2f;

			// Normal movement
			if (player.controlLeft || player.controlRight)
			{ // Player is inputting a movement key
				float speedMult = player.moveSpeed;
				float acceleration = speedMult;
				if (!grounded) acceleration *= 0.5f;

				if (player.controlLeft && !player.controlRight)
				{ // Left movement
					intendedVelocity.X -= 0.2f * acceleration;
					if (intendedVelocity.X < -5f * speedMult) intendedVelocity.X = -5f * speedMult;
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (player.controlRight && !player.controlLeft)
				{ // Right movement
					intendedVelocity.X += 0.2f * acceleration;
					if (intendedVelocity.X > 5f * speedMult) intendedVelocity.X = 5f * speedMult;
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

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[1] > 0 ? 8 : 5))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override void PreDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor)
		{
			if (projectile.ai[2] > 0)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float scalemult = (float)Math.Sin(projectile.ai[2] * 0.1046f) * 0.125f + 1f;
				spriteBatch.Draw(anchor.TextureShapeshift, drawPosition, drawRectangle, new Color(54, 150, 248) * 0.75f, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
		}

		public override bool ShouldDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor)
		{
			return base.ShouldDrawShapeshift(spriteBatch, projectile, player, ref lightColor);
		}

		public override bool ShapeshiftFreeDodge(Player.HurtInfo info, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (projectile.ai[1] > 0)
			{
				return true;
			}

			return false;
		}
	}
}