using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenSlime : OrchidModShapeshifterShapeshift
	{
		public int jumpCooldown = 0;
		public float JumpCharge = 0f;
		public float SpikeCharge = 0f;
		public bool ChargeCue = false;
		public bool ResetNPCs = false;
		public List<int> HitNPCs;

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item154;
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			Item.knockBack = 10f;
			Item.damage = 43;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 28;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			GroundedWildshape = true;
			AutoReuseLeft = false;
			HitNPCs = new List<int>();
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			jumpCooldown = 0;
			JumpCharge = 0f;
			SpikeCharge = 0f;
			ChargeCue = false;
			ResetNPCs = false;
			projectile.ai[0] = 2.5f;

			for (int i = 0; i < 10; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 7; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && !IsGrounded(projectile, player, 64f) && projectile.ai[1] == 0f;

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{ // slam start
			ResetNPCs = false;
			HitNPCs.Clear();
			projectile.ai[0] = 0f;
			projectile.ai[1] = 1f;
			projectile.velocity.Y = 15f;
			projectile.velocity.X = 0f;

			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
				dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
				dust.velocity *= 0.25f;
				dust.velocity.Y -= 0.65f;
			}

			SoundStyle sound = SoundID.DD2_WyvernDiveDown;
			sound.Pitch += Main.rand.NextFloat(0.2f, 0.3f);
			SoundEngine.PlaySound(sound, projectile.Center);
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsRightClick && Main.mouseRightRelease && anchor.CanLeftClick && !IsGrounded(projectile, player, 64f) && projectile.ai[1] == 0f;
		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => ShapeshiftOnLeftClick(projectile, anchor, player, shapeshifter);

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] stores the lateral movement speed cap (increased after a jump, defaults to 2.5f)
			// ai[1] stores the left click slam impact charge (increases by falling) and the recovery animation when landing (<0)

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);
			jumpCooldown--;

			if (projectile.ai[0] >= 2.6f)
			{ // lateral speed goes back to 2.5f after being modified
				projectile.ai[0] -= 0.1f;
			}
			else if (projectile.ai[0] <= 2.4f)
			{
				projectile.ai[0] += 0.1f;
			}
			else
			{
				projectile.ai[0] = 2.5f;
			}

			if (projectile.ai[1] >= 1f)
			{ // Slam charges up within the first 0.25 sec of free fall
				projectile.ai[1] += 0.07f;
				if (projectile.ai[1] >= 2f)
				{
					projectile.ai[1] = 2f;
					projectile.damage = shapeshifter.GetShapeshifterDamage(Item.damage * 5f);

					if (!ResetNPCs)
					{
						ResetNPCs = true;
						HitNPCs.Clear();
					}
				}
				else
				{
					projectile.damage = shapeshifter.GetShapeshifterDamage(Item.damage);
				}

				Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(6, 0), projectile.width - 12, 0, DustID.Smoke);
				dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
				dust.velocity *= 0.25f;

				projectile.friendly = true;
				projectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			}
			else if (projectile.ai[1] < 0f)
			{ // recovery after touching the ground
				projectile.ai[1]++;
				if (projectile.ai[1] >= 0f)
				{
					projectile.ai[1] = 0f;
				}

				projectile.friendly = false;
			}
			else
			{ // redundant, just in case
				projectile.friendly = false;
			}

			player.noFallDmg = true;

			// MISC EFFECTS & ANIMATION

			if (projectile.ai[1] < 0)
			{ // recovering after slam
				if (projectile.ai[1] < -10)
				{
					anchor.Frame = 2;
				}
				else
				{
					anchor.Frame = 1;
				}
			}
			else if (JumpCharge > 0f)
			{ // charging
				if (JumpCharge > 20f)
				{
					anchor.Frame = 2;
				}
				else
				{
					anchor.Frame = 1;
				}
			}
			else if (SpikeCharge > 0f)
			{ // charging (2)
				if (SpikeCharge > 20f)
				{
					anchor.Frame = 2;
				}
				else
				{
					anchor.Frame = 1;
				}
			}
			else if (projectile.velocity.Y < 0f)
			{ // ascending
				if (anchor.Timespent < 0)
				{
					anchor.Frame = 6;
				}
				else
				{
					anchor.Frame = 5;
				}
			}
			else if (grounded)
			{
				anchor.Frame = 0;
			}
			else
			{ // descending
				if (projectile.velocity.Y < 4f)
				{
					anchor.Frame = 3;
				}
				else
				{
					anchor.Frame = 4;
				}
			}

			// ATTACK & MOVEMENT

			if(grounded && projectile.ai[1] >= 1f)
			{ // landing after left click slam

				// fetches the tiles the player lands on and uses them for dust
				int dustType1 = -1;
				int dustType2 = -1;

				Point point = new Point((int)((int)projectile.position.X / 16f), (int)(((int)projectile.position.Y + projectile.height) / 16f));
				Tile tile = Framing.GetTileSafely(point);
				if (tile.HasTile)
				{
					dustType1 = Main.dust[WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile)].type;
				}
				else
				{ // sometimes it selects one tile too high, so pick the one below
					point.Y++;

					tile = Framing.GetTileSafely(point);
					if (tile.HasTile)
					{
						dustType1 = Main.dust[WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile)].type;
					}
				}

				point.X++;
				tile = Framing.GetTileSafely(point);
				if (tile.HasTile)
				{
					dustType2 = Main.dust[WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile)].type;
				}

				// a lot of dust for the slam ...

				int dustAmount = projectile.ai[1] >= 2f ? 5 : 2;
				float velocityMult = projectile.ai[1] >= 2f ? 1f : 0.6f;

				if (dustType1 != -1)
				{
					for (int i = 0; i < dustAmount * 2; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, dustType1);
						dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
						dust.velocity *= 0.5f * velocityMult;
						dust.velocity.Y -= 0.85f;
					}

					for (int i = 0; i < dustAmount; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, dustType1);
						dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
						dust.velocity *= 1.25f * velocityMult;
						dust.velocity.Y -= 1.25f;
					}
				}

				if (dustType2 != -1)
				{
					for (int i = 0; i < dustAmount * 2; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, dustType2);
						dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
						dust.velocity *= 1.5f * velocityMult;
						dust.velocity.Y -= 1.5f;
					}

					for (int i = 0; i < dustAmount; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, dustType2);
						dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
						dust.velocity *= 0.75f * velocityMult;
						dust.velocity.Y -= 1f;
					}
				}

				for (int i = 0; i < dustAmount * 4; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(0.6f, 1f);
					dust.velocity *= 0.5f * velocityMult;
					dust.velocity.Y -= 0.85f;
				}

				for (int i = 0; i < dustAmount * 2; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
					dust.velocity *= velocityMult;
					dust.velocity.Y -= 1.2f;
				}

				if (projectile.ai[1] >= 2f)
				{
					SoundEngine.PlaySound(SoundID.Item167, projectile.Center);

					for (int i = -4; i <= 4; i += 2)
					{
						int rand = Main.rand.Next(3);
						switch (rand) {
							case 0:
								rand = GoreID.Smoke1;
								break;
							case 1:
								rand = GoreID.Smoke2;
								break;
							default:
								rand = GoreID.Smoke3;
								break;
						}

						Gore.NewGoreDirect(player.GetSource_ItemUse(Item), projectile.Center, Vector2.UnitX * i * Main.rand.NextFloat(0.4f, 0.7f), rand).scale *= Main.rand.NextFloat(0.7f, 1f);
					}
				}
				else
				{
					SoundStyle sound = SoundID.Item167;
					sound.Volume *= 0.7f;
					sound.Pitch += Main.rand.NextFloat(0.3f, 0.4f);
					SoundEngine.PlaySound(sound, projectile.Center);
				}

				// spawns the projectile & resets values

				if (IsLocalPlayer(player))
				{
					int projectileType = ModContent.ProjectileType<WardenSlimeProj>();
					int damage = shapeshifter.GetShapeshifterDamage(Item.damage * (projectile.ai[1] >= 2f ? 5f : 1f));
					Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, Vector2.Zero, projectileType, damage, 0f, player.whoAmI, projectile.ai[1]);
					newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
					newProjectile.netUpdate = true;
				}

				shapeshifter.modPlayer.PlayerImmunity = 30;
				player.immuneTime = 30;
				player.immune = true;

				projectile.ai[1] = -20f;
			}

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, projectile.ai[1] >= 1f ? 15f : 10f);

			// Attack Charges

			if (anchor.IsRightClick && grounded && (SpikeCharge == 0f || intendedVelocity.Y >= 0f) && projectile.ai[1] >= 0f && JumpCharge == 0f)
			{ // Charges the spike while left clicking & grounded. This also prevents normal movement
				if (SpikeCharge == 0f)
				{
					SoundEngine.PlaySound(SoundID.Item65, projectile.Center);
					ChargeCue = false;
				}

				SpikeCharge += shapeshifter.GetShapeshifterMeleeSpeed();

				if (SpikeCharge >= 60 && !ChargeCue)
				{
					ChargeCue = true;
					anchor.Blink(true);
				}
			}
			else
			{
				if (SpikeCharge >= 60)
				{ // Fire spike
					if (IsLocalPlayer(player))
					{
						int projectileType = ModContent.ProjectileType<WardenSlimeProjAlt>();
						Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
						int damage = shapeshifter.GetShapeshifterDamage(Item.damage * 3);
						Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center + new Vector2(0f, 2f), velocity, projectileType, damage, Item.knockBack, player.whoAmI);
						newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
						newProjectile.netUpdate = true;
					}

					SoundEngine.PlaySound(SoundID.Item17, projectile.Center);
				}

				SpikeCharge = 0;
			}

			if (anchor.IsLeftClick && grounded && (JumpCharge > 0f || intendedVelocity.Y >= 0f) && projectile.ai[1] >= 0f && SpikeCharge == 0f)
			{ // Charges the jump while left clicking & grounded. This also prevents normal movement
				if (JumpCharge == 0f)
				{
					SoundEngine.PlaySound(SoundID.Item65, projectile.Center);
					ChargeCue = false;
				}

				JumpCharge += shapeshifter.GetShapeshifterMeleeSpeed();

				if (JumpCharge >= 60 && !ChargeCue)
				{
					ChargeCue = true;
					anchor.Blink(true);
				}
			}
			else
			{
				if (JumpCharge >= 60)
				{ // Full charge jump
					anchor.Timespent = -15;
					intendedVelocity.Y = -15f;
					jumpCooldown = 10;
					projectile.ai[0] = 5.5f;

					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, -projectile.ai[0], speedMult, 5.5f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, projectile.ai[0], speedMult, 5.5f);
						projectile.direction = 1;
						projectile.spriteDirection = 1;
					}

					for (int i = 0; i < 15; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
						dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
						dust.velocity *= 0.25f;
						dust.velocity.Y -= 0.65f;
					}

					SoundStyle sound = SoundID.Item154;
					sound.Pitch += Main.rand.NextFloat(0.2f, 0.3f);
					sound.Volume *= 1.2f;
					SoundEngine.PlaySound(sound, projectile.Center);
				}
				else if (JumpCharge > 0)
				{ // Jump not fully charged, do a normal one
					anchor.Timespent = -12;
					intendedVelocity.Y = -10f;

					for (int i = 0; i < 8; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
						dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
						dust.velocity *= 0.25f;
						dust.velocity.Y -= 0.75f;
					}

					SoundEngine.PlaySound(SoundID.Item154, projectile.Center);
				}

				JumpCharge = 0;
			}

			// Normal movement
			if ((anchor.IsInputLeft || anchor.IsInputRight || player.controlJump) && JumpCharge <= 0f && SpikeCharge <= 0f && projectile.ai[1] >= 0f)
			{ // Player is inputting a movement key
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, -projectile.ai[0], speedMult, 0.1f);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, projectile.ai[0], speedMult, 0.1f);
					projectile.direction = 1;
					projectile.spriteDirection = 1;
				}
				else
				{ // Both keys pressed = no movement
					intendedVelocity.X *= 0.7f;
				}

				if (grounded && intendedVelocity.Y > 0f && jumpCooldown <= 0)
				{
					jumpCooldown = 10;

					if (player.controlJump)
					{
						anchor.Timespent = -12;
						intendedVelocity.Y = -10f;

						for (int i = 0; i < 8; i++)
						{
							Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
							dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
							dust.velocity *= 0.25f;
							dust.velocity.Y -= 0.75f;
						}

						SoundEngine.PlaySound(SoundID.Item154, projectile.Center);
					}
					else
					{
						anchor.Timespent = -10;
						intendedVelocity.Y = -6f;

						for (int i = 0; i < 5; i++)
						{
							Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
							dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
							dust.velocity *= 0.25f;
							dust.velocity.Y -= 0.5f;
						}

						SoundStyle sound = SoundID.Item154;
						sound.Pitch -= Main.rand.NextFloat(0.2f, 0.3f);
						sound.Volume *= 0.5f;
						SoundEngine.PlaySound(sound, projectile.Center);
					}
				}
			}
			else
			{ // no movement input
				intendedVelocity.X *= 0.7f;
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[1] >= 1f ? 6 : projectile.ai[0] > 2.5f ? 6 : 4))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}
		public override Color GetColorGlow(ref bool drawPlayerAsAdditive, Color lightColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => player.GetImmuneAlphaPure(lightColor * 2.5f, 0f);

		public override void ShapeshiftOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			HitNPCs.Add(target.whoAmI);
			TrySpawnHealingGoo(projectile, player);
		}

		public override bool ShapeshiftFreeDodge(Player.HurtInfo info, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (projectile.ai[1] != 0f) return true;
			return base.ShapeshiftFreeDodge(info, projectile, anchor, player, shapeshifter);
		}

		public override void ShapeshiftOnHitByAnything(Player.HurtInfo hurtInfo, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			TrySpawnHealingGoo(projectile, player);

			if (projectile.ai[1] != 0f || JumpCharge > 0 || SpikeCharge > 0 || projectile.ai[0] > 2.5f)
			{ // kb immunity while charging & big jump
				player.noKnockback = true;
			}
		}

		public void TrySpawnHealingGoo(Projectile projectile, Player player)
		{
			int chance = 10;
			int projectileType = ModContent.ProjectileType<WardenSlimeProjPassive>();

			foreach(Projectile proj in Main.projectile)
			{ // each existing projectile makes it less likely for a new one to spawn
				if (proj.type == projectileType && proj.owner == player.whoAmI && proj.active)
				{
					chance--;
				}
			}

			if (chance > 0)
			{
				if (Main.rand.NextBool(12 -  chance))
				{
					int damage = 10; // healing
					Vector2 velocity = new Vector2(Main.rand.NextFloat(3f, 7f) * (Main.rand.NextBool() ? 1f : -1f), Main.rand.NextFloat(-5f, -7f));
					Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, damage, 0f, player.whoAmI);
					newProjectile.netUpdate = true;
				}
			}
		}
	}
}