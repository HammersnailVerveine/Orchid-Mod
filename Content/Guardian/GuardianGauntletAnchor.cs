using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.General.Prefixes;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace OrchidMod.Content.Guardian
{
	public class GuardianGauntletAnchor : OrchidModGuardianParryAnchor
	{
		public int LockedOwnerDir = 0;
		public bool OffHandGauntlet = false;
		public bool Ding = false;
		public bool NeedNetUpdate = false;
		public float GauntletDashAngle = 0f;
		public int GauntletDashTimer = 0;

		public int SelectedItem { get; set; } = -1;
		public Item GauntletItem => Main.player[Projectile.owner].inventory[SelectedItem];
		public bool Blocking => Projectile.ai[0] > 0 && !Charging;
		public bool Slamming => Projectile.ai[0] < 0;
		public bool Charging => Projectile.ai[2] > 0;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (!OffHandGauntlet || Blocking) overPlayers.Add(index);
		}

		// ...

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.netImportant = true;
			GauntletDashAngle = 0f;
			GauntletDashTimer = 0;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
			writer.Write(OffHandGauntlet);
			writer.Write(GauntletDashAngle);
			writer.Write(GauntletDashTimer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
			OffHandGauntlet = reader.ReadBoolean();
			GauntletDashAngle = reader.ReadSingle();
			GauntletDashTimer = reader.ReadInt32();
		}

		public void OnChangeSelectedItem(Player owner)
		{
			SelectedItem = owner.selectedItem;
			Projectile.ai[0] = 0f;
			Projectile.ai[1] = 0f;
			Projectile.ai[2] = 0f;
			Projectile.netUpdate = true;
			GauntletDashAngle = 0f;
			GauntletDashTimer = 0;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if (!owner.active || owner.dead || SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModGuardianGauntlet) || GauntletItem == null || GauntletItem.ModItem is not OrchidModGuardianGauntlet guardianItem)
			{
				if (IsLocalOwner) Projectile.Kill();
				return;
			}
			else
			{
				if (NeedNetUpdate)
				{
					NeedNetUpdate = false;
					Projectile.netUpdate = true;
				}

				Projectile.timeLeft = 5;
				if (OffHandGauntlet && IsLocalOwner) // Offhand is always loaded first; no need to do that twice
				{
					if (Main.projectile[guardianItem.GetAnchors(owner)[1]].ai[0] >= 0)
					{ // Lock the player direction while slamming
						if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
						else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
						LockedOwnerDir = owner.direction;
					}
					else owner.direction = LockedOwnerDir;
				}

				if (GauntletDashTimer > 0)
				{ // handles the player dash (after a parry)
					GauntletDashTimer--;
					Vector2 intendedVelocity = Vector2.UnitY.RotatedBy(GauntletDashAngle) * -guardianItem.ParryDashSpeed;
					owner.velocity = intendedVelocity;

					if (Main.rand.NextBool())
					{
						Dust dust = Dust.NewDustDirect(owner.position, owner.width, owner.height, DustID.Smoke);
						dust.noGravity = true;
					}
				}

				if (Blocking)
				{
					guardian.GuardianGauntletParry = true;
					guardian.GuardianGauntletParry2 = true;

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(4 * owner.direction, 0);
					if (OffHandGauntlet) Projectile.position.X += 6 * owner.direction;
					Projectile.rotation = 0f;

					Projectile.ai[0]--;
					if (owner.immune)
					{
						if (owner.eocHit != -1)
						{
							guardian.DoParryItemParry(Main.npc[owner.eocHit]); // this resets both gauntlets' parry state
						}
						else
						{
							Projectile.ai[0] = 0f;
							guardian.GuardianGuardRecharging += Projectile.ai[0] / (guardianItem.ParryDuration * guardianItem.Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration);
							Rectangle rect = owner.Hitbox;
							rect.Y -= 64;
							CombatText.NewText(guardian.Player.Hitbox, Color.LightGray, Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.Interrupted"), false, true);
							if (OffHandGauntlet)
							{
								//Main.NewText("Starting sweep from offhand gauntlet projectile[" + Projectile.whoAmI + "] for mainhand gauntlet");
								for (int i = Projectile.whoAmI + 1; i < Main.maxProjectiles; i++)
								{
									if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].ModProjectile is GuardianGauntletAnchor offhand)
									{
										//Main.NewText("Found at projectile[" + i + "]!");
										if (offhand.Blocking)
										{
											Main.projectile[i].ai[0] = 0f;
											//Main.NewText("Disabling mainhand gauntlet parry");
											break;
										}
										//Main.NewText("Mainhand gauntlet not parrying, exiting");
										break;
									}
								}
								//Main.NewText("Sweep done");
							}
							//else Main.NewText("Sweep initiated from mainhand gauntlet (projectile[" + Projectile.whoAmI + "]), ignoring");
						}
					}
					else if (Projectile.ai[0] <= 0f)
					{
						spawnDusts();
						Projectile.ai[0] = 0f;
					}
				}
				else if (Slamming)
				{
					float slamTime = Projectile.ai[0] == -1f ? 10f : 15f;
					if (Projectile.localAI[1] == 0f) // Register base slam length
					{
						Projectile.localAI[1] = slamTime;
					}

					float addedDistance = (float)Math.Sin(MathHelper.Pi / slamTime * Projectile.localAI[1]) * slamTime;
					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(4 * owner.direction, 0) + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * addedDistance;

					if (!IsLocalOwner)
					{ // Rotates the player in the direction of the punch for other clients
						Vector2 puchDir = (Projectile.ai[1] + MathHelper.PiOver2).ToRotationVector2();
						if (puchDir.X > 0 && owner.direction != 1) owner.ChangeDir(1);
						else if (puchDir.X < 0 && owner.direction != -1) owner.ChangeDir(-1);
					}
					else if (Projectile.localAI[1] == slamTime)
					{ // Slam just started, make projectile
						int damage = guardian.GetGuardianDamage(guardianItem.Item.damage);
						if (guardianItem.OnPunch(owner, guardian, Projectile, Projectile.ai[0] == -2f, ref damage))
						{
							int projectileType = ModContent.ProjectileType<GauntletPunchProjectile>();
							float strikeVelocity = guardianItem.StrikeVelocity * (Projectile.ai[0] == -1f ? 0.75f : 1f) * guardianItem.Item.GetGlobalItem<GuardianPrefixItem>().GetSlamDistance() * owner.GetTotalAttackSpeed(DamageClass.Melee);
							Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2) * strikeVelocity * 0.25f;
							Projectile punchProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, projectileType, 1, 1f, owner.whoAmI, Projectile.ai[0] == -1f ? 0f : 1f, OffHandGauntlet ? 1f : 0f);
							if (punchProj.ModProjectile is GauntletPunchProjectile punch)
							{
								punch.GauntletItem = GauntletItem.ModItem as OrchidModGuardianGauntlet;
								punchProj.damage = damage;
								punchProj.CritChance = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + guardianItem.Item.crit);
								punchProj.knockBack = guardianItem.Item.knockBack;
								//punchProj.position += punchProj.velocity * 0.5f;
								punchProj.velocity += owner.velocity * 0.375f;

								if (Projectile.ai[0] == -1f)
								{
									punchProj.damage = (int)(punchProj.damage / 4f);
									SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, owner.Center);
								}
								else SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundMiss, owner.Center);

								punchProj.netUpdate = true;
							}
							else punchProj.Kill();
						}
						Ding = false;
					}

					if (Projectile.ai[1] < 1f && Projectile.ai[1] > -1f)
					{ // Offset the gauntlet when aiming down
						int offset = 2;
						if (Projectile.ai[1] < 0.7f && Projectile.ai[1] > -0.7f) offset += 2;
						if (Projectile.ai[1] < 0.4f && Projectile.ai[1] > -0.4f) offset += 2;
						Projectile.position.Y += offset;
						Projectile.position.X -= offset * owner.direction;
					}

					Projectile.rotation = Projectile.ai[1];
					if (owner.direction == 1) Projectile.rotation += MathHelper.Pi;

					Projectile.localAI[1] *= 0.8f;
					if (Projectile.localAI[1] <= 0.04f)
					{
						Projectile.localAI[1] = 0f;
						Projectile.ai[0] = 0;
						Projectile.ai[1] = 0;
						if (owner.direction == -1) Projectile.rotation += MathHelper.Pi; // weird issue fix, gauntlets flips for 1 frame at the end of a punch when facing left
					}
				}
				else
				{
					if (Charging)
					{
						guardian.GuardianItemCharge += 30f / GauntletItem.useTime * (owner.GetTotalAttackSpeed(DamageClass.Melee) * 2f - 1f);
						if (guardian.GuardianItemCharge > 180f)
						{
							if (!Ding && IsLocalOwner)
							{
								if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, owner.Center);
								else SoundEngine.PlaySound(SoundID.MaxMana, owner.Center);
								Ding = true;
							}
							guardian.GuardianItemCharge = 180f;
						}
						else if (guardian.GuardianItemCharge > (70 * owner.GetTotalAttackSpeed(DamageClass.Melee) - owner.HeldItem.useTime) / 2.5f) guardian.SlamCostUI = 1;

						if ((ModContent.GetInstance<OrchidClientConfig>().GuardianSwapGauntletImputs ? !Main.mouseRight : !Main.mouseLeft) && owner.whoAmI == Main.myPlayer)
						{
							if (guardian.GuardianItemCharge < 180f && guardian.UseSlam(1, true))
							{ // Consume a slam to fully charge if the player has one
								guardian.UseSlam();
								guardian.GuardianItemCharge = 180f;
							}

							if (guardian.GuardianItemCharge >= 180f) Projectile.ai[0] = -2f;
							else Projectile.ai[0] = -1f;

							guardian.GuardianItemCharge = 0;

							if (IsLocalOwner)
							{
								Projectile.ai[1] = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2;
								Projectile.ai[2] = 0f;
								Projectile.netUpdate = true;
							}
						}
						else
						{
							Projectile.Center = owner.MountedCenter.Floor() + new Vector2(-(4 + guardian.GuardianItemCharge * 0.033f) * owner.direction, 4);
							Projectile.rotation = MathHelper.PiOver2;
						}
					}
					else
					{
						Projectile.Center = owner.MountedCenter.Floor() + new Vector2((-6 + guardian.GuardianItemCharge * 0.01f) * owner.direction, 6);
						if (OffHandGauntlet) Projectile.position.X += 8 * owner.direction;

						if (owner.velocity.X != 0)
						{
							Projectile.position.X -= 2 * owner.direction;
							Projectile.position.Y -= 2;
							Projectile.rotation = MathHelper.PiOver2 + MathHelper.PiOver4 * owner.direction * 0.5f;
						}
						else
						{
							Projectile.rotation = MathHelper.Pi - MathHelper.PiOver4 * owner.direction;
						}
					}
				}

				if (!OffHandGauntlet)
				{ // Composite arm stuff for the front arm (the back arm is disabled while holding gauntlets)
					float rotation = (Projectile.Center + new Vector2(6 * owner.direction, Slamming ? 2 : Charging ? 8 : 6) - owner.MountedCenter.Floor()).ToRotation();
					CompositeArmStretchAmount compositeArmStretchAmount = CompositeArmStretchAmount.ThreeQuarters; // Tweak the arm based on punch direction if necessary
					if (Charging) compositeArmStretchAmount = CompositeArmStretchAmount.Quarter;
					if (Projectile.localAI[1] > 0.55f && (Projectile.ai[1] > -2.25f || Projectile.ai[1] < -4f)) compositeArmStretchAmount = CompositeArmStretchAmount.Full;
					owner.SetCompositeArmFront(true, compositeArmStretchAmount, rotation - MathHelper.PiOver2);
				}
			}

			guardianItem.ExtraAIGauntlet(Projectile);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public void spawnDusts()
		{
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(pos, 20, 20, DustID.Smoke);
				dust.scale *= 0.75f;
				dust.velocity *= 0.25f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (SelectedItem < 0 || SelectedItem > 58) return false;
			if (!(GauntletItem.ModItem is OrchidModGuardianGauntlet guardianItem)) return false;
			if (!ModContent.HasAsset(guardianItem.GauntletTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawGauntlet(spriteBatch, Projectile, player, ref color))
			{
				if (guardianItem.hasArm)
				{
					var effectArm = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
					Texture2D textureArm = guardianItem.GetArmTexture(out Rectangle? drawRectangleArm);
					float armRotation = player.compositeFrontArm.rotation + MathHelper.PiOver2 * (player.direction == -1 ? 1.5f : 0.5f);
					if (Blocking) armRotation += MathHelper.PiOver4 * -0.5f * player.direction;
					Vector2 armPosition = Vector2.Transform(player.Center.Floor() + new Vector2(6 * -player.direction, -2.5f) - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
					Vector2 originArm = drawRectangleArm == null ? textureArm.Size() * 0.5f : drawRectangleArm.GetValueOrDefault().Size() * 0.5f;
					spriteBatch.Draw(textureArm, armPosition, drawRectangleArm, color, armRotation, originArm, Projectile.scale, effectArm, 0f);
				}

				if (guardianItem.hasShoulder)
				{
					var effectShoulder = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
					Texture2D textureShoulder = guardianItem.GetShoulderTexture(out Rectangle? drawRectangleShoulder);
					Vector2 shouldePosition = Vector2.Transform(player.Center.Floor() + new Vector2(6 * -player.direction, -3f) - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
					Vector2 originShoulder = drawRectangleShoulder == null ? textureShoulder.Size() * 0.5f : drawRectangleShoulder.GetValueOrDefault().Size() * 0.5f;
					spriteBatch.Draw(textureShoulder, shouldePosition, drawRectangleShoulder, color, 0f, originShoulder, Projectile.scale, effectShoulder, 0f);
				}

				Texture2D texture = guardianItem.GetGauntletTexture(OffHandGauntlet, out Rectangle? drawRectangle);

				var effect = SpriteEffects.None;
				if (player.direction != 1)
				{
					if (player.velocity.X != 0 && !Blocking || (player.GetModPlayer<OrchidGuardian>().GuardianItemCharge > 0 && Projectile.ai[2] != 0) || Slamming) effect = SpriteEffects.FlipVertically;
					else effect = SpriteEffects.FlipHorizontally;
				}

				float drawRotation = Projectile.rotation;
				Vector2 posproj = Projectile.Center;
				if (player.gravDir == -1)
				{
					drawRotation = -drawRotation;
					posproj.Y = (player.Bottom + player.position).Y - posproj.Y + (posproj.Y - player.Center.Y) * 2f;
					if (effect == SpriteEffects.FlipVertically)
					{
						effect = SpriteEffects.None;
					}
					else if (effect == SpriteEffects.FlipHorizontally)
					{
						effect = SpriteEffects.None;
						drawRotation += MathHelper.Pi;
					}
					else if (effect == SpriteEffects.None)
					{
						effect = SpriteEffects.FlipVertically;
					}
				}

				var drawPosition = Vector2.Transform(posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				float rotation = Projectile.rotation;
				Vector2 origin = drawRectangle == null ? texture.Size() * 0.5f : drawRectangle.GetValueOrDefault().Size() * 0.5f;
				spriteBatch.Draw(texture, drawPosition, drawRectangle, color, drawRotation, origin, Projectile.scale, effect, 0f);

			}
			guardianItem.PostDrawGauntlet(spriteBatch, Projectile, player, color);

			return false;
		}
	}
}