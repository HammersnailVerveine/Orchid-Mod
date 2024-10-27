using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using OrchidMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorIceFox : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.NPCDeath4;
			Item.useTime = 40;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 34;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			MeleeSpeedLeft = true;
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

			int projectileType = ModContent.ProjectileType<PredatorIceFoxProjAlt>();
			foreach (Projectile proj in Main.projectile)
			{ // Despawns dash indicators when shapeshifting from fox to fox
				if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType)
				{
					proj.Kill();
				}
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
			int projectileType = ModContent.ProjectileType<PredatorIceFoxProj>();
			float ai1 = 0f;
			float ai0 = 0f;
			int count = 0;
			bool found2 = false;
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType && proj.ai[1] >= 0f)
				{
					count++;
					ai0 = proj.ai[0];
					if (proj.ai[1] == 0f && ai1 == 0f) ai1 = 1f;
					if (proj.ai[1] == 1f && !found2) ai1 = 2f;
					if (proj.ai[1] == 2f)
					{
						found2 = true;
						if (ai1 == 2f)
						{
							ai1 = 0f;
						}
					}
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

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => Main.mouseRight && (Main.mouseRightRelease || AutoReuseRight) && anchor.CanRightClick;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.RightCLickCooldown = 180;
			anchor.NeedNetUpdate = true;
			projectile.ai[1] = 90;
			SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, projectile.Center);

			for (int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1.4f, 2f));
				dust.noGravity = true;
				dust.velocity += projectile.velocity;
			}
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && projectile.ai[0] >= 300;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1.4f, 2f));
				dust.noGravity = true;
			}

			projectile.ai[0] -= 300;
			Vector2 position = projectile.position;
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center) * 8f * GetSpeedMult(player, shapeshifter);

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
			anchor.LeftCLickCooldown = Item.useTime;
			projectile.ai[2] = 30;
			SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, projectile.Center);

			// Spawn 3 foxfire flames after the dash and makes the old ones remain in place
			int projectileType = ModContent.ProjectileType<PredatorIceFoxProj>();

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType)
				{
					proj.ai[1] = -2f;
				}
			}

			for (int j = 0; j < 3; j++)
			{
				int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, Vector2.Zero, projectileType, damage, 0f, player.whoAmI, 0f, j);
				newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			}

			// Kill one of the dash indicators following the player
			int projectileType2 = ModContent.ProjectileType<PredatorIceFoxProjAlt>();
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

			float speedMult = GetSpeedMult(player, shapeshifter);
			bool grounded = IsGrounded(projectile, player);

			if ((int)projectile.ai[0] < 601)
			{ // Increases the dash timer
				projectile.ai[0]++;

				if ((int)projectile.ai[0] % 300 == 0)
				{ // Spawns a projectile following the player when the dash is ready
					int projectileType = ModContent.ProjectileType<PredatorIceFoxProjAlt>();
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

					for (int i = 0; i < 20; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1.4f, 2f));
						dust.noGravity = true;
						dust.velocity += projectile.velocity;
					}
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
			if (intendedVelocity.Y < 9.5f)
			{ // gravity
				intendedVelocity.Y += 0.2f;
				if (intendedVelocity.Y > 9.5f)
				{
					intendedVelocity.Y = 9.5f;
				}
			}

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				float acceleration = speedMult;
				if (!grounded) acceleration *= 0.5f;

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					intendedVelocity.X -= 0.2f * acceleration;
					if (intendedVelocity.X < -5f * speedMult) intendedVelocity.X = -5f * speedMult;
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
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