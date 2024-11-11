using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace OrchidMod.Content.Guardian
{
	public class GuardianQuarterstaffAnchor : OrchidModGuardianProjectile
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public int TimeSpent = 0;
		public bool Ding = false;
		public bool NeedNetUpdate = false;
		public bool hitTarget = false;
		public bool DamageReset = false;
		public Rectangle HitBox;

		public int SelectedItem { get; set; } = -1;
		public Item QuarterstaffItem => Main.player[Projectile.owner].inventory[SelectedItem];
		public Texture2D QuarterstaffTexture;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.netImportant = true;

			HitBox = new Rectangle(0, 0, 40, 40);
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overPlayers.Add(index);

		public void OnChangeSelectedItem(Player owner)
		{
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
			Projectile.ai[0] = 0f;
			guardian.GuardianGauntletCharge = 0;
			SelectedItem = owner.selectedItem;
			Projectile.netUpdate = true;

			if (QuarterstaffItem.ModItem is OrchidModGuardianQuarterstaff guardianItem)
			{
				QuarterstaffTexture = ModContent.Request<Texture2D>(guardianItem.QuarterstaffTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ResetSize();
			}
			else if (IsLocalOwner)
			{
				Projectile.Kill();
			}
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if (SelectedItem < 0 || QuarterstaffItem == null || QuarterstaffItem.ModItem is not OrchidModGuardianQuarterstaff guardianItem || owner.HeldItem.ModItem is not OrchidModGuardianQuarterstaff || !owner.active || owner.dead)
			{ // Kills the projectile if anything is wrong
				Projectile.Kill();
				return;
			}
			else
			{
				if (IsLocalOwner)
				{ // Player rotation & Item netupdate
					if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
					else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
					if (NeedNetUpdate)
					{
						NeedNetUpdate = false;
						Projectile.netUpdate = true;
					}
				}
				else
				{
					if (Projectile.ai[0] == 0f)
					{ // Adresses a visual issue
						guardian.GuardianGauntletCharge = 0;
					}
				}

				TimeSpent++;
				Projectile.timeLeft = 5;

				if (Projectile.ai[2] > 0f)
				{ // Blocking
					guardian.GuardianGauntletParry = true;
					guardian.GuardianGauntletParry2 = true;

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(10f * owner.direction, -2f);
					Projectile.rotation = MathHelper.PiOver4 * 0.55f * owner.direction - MathHelper.PiOver4;

					owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -0.9f * owner.direction);
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -1.4f * owner.direction);

					if (OldPosition.Count > 0)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					Projectile.ai[2] --;

					if (Projectile.ai[2] <= 0 || owner.immune)
					{
						Projectile.ai[2] = 0f;
					}
				}
				else if (Projectile.ai[2] < 0f)
				{ // Counterattacking
					if (Projectile.ai[2] == -40f)
					{ // First frame of the counterattack
						Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage);
						Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
						Projectile.knockBack = QuarterstaffItem.knockBack;
						Projectile.friendly = true;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
						guardianItem.OnAttack(owner, guardian, Projectile, false, true);
						DamageReset = true;
						Projectile.scale *= 1.2f;
						Projectile.width = (int)(Projectile.width * 1.2f);
						Projectile.height = (int)(Projectile.height * 1.2f);
						hitTarget = false;
					}

					if (Projectile.ai[2] >= - 13.3f && !DamageReset)
					{ // Reset damage twice while spinning
						DamageReset = true;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
					}
					else if (Projectile.ai[2] < -13.3f && Projectile.ai[2] >= -26.6f && DamageReset)
					{
						DamageReset = false;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
					}

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(-4 * owner.direction, 0f);
					Projectile.rotation = MathHelper.PiOver4 * 0.55f * owner.direction - MathHelper.PiOver4 + (Projectile.ai[2] + 60f) * 0.209f;

					owner.SetCompositeArmFront(true, CompositeArmStretchAmount.ThreeQuarters, MathHelper.PiOver4 * owner.direction + (Projectile.ai[2] + 60f) * 0.209f);
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -1.4f * owner.direction);

					OldPosition.Add(Projectile.Center);
					OldRotation.Add(Projectile.rotation);

					if (OldPosition.Count > 10)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					Projectile.ai[2] += owner.GetTotalAttackSpeed(DamageClass.Melee);

					if (Projectile.ai[2] >= 0)
					{
						Projectile.ai[0] = 0f;
						Projectile.ai[1] = 0f;
						Projectile.ai[2] = 0f;
						ResetSize();
						Projectile.friendly = false;
					}
				}
				else if (Projectile.ai[0] == 1f)
				{ // Being charged by the player
					if (guardian.GuardianGauntletCharge < 180f)
					{ // Increase guardian charge
						guardian.GuardianGauntletCharge += 30f / guardianItem.Item.useTime * owner.GetTotalAttackSpeed(DamageClass.Melee);
						if (guardian.GuardianGauntletCharge > 180f) guardian.GuardianGauntletCharge = 180f;
					}

					// Rotate the staff as charge progresses, readying an upward swipe attack

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(12f * owner.direction, guardian.GuardianGauntletCharge * 0.03f);
					Projectile.rotation = MathHelper.PiOver4 * (1f + guardian.GuardianGauntletCharge * 0.0025f) * owner.direction - MathHelper.PiOver4;

					owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(0.6f - guardian.GuardianGauntletCharge * 0.0025f) * owner.direction);
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(1f - guardian.GuardianGauntletCharge * 0.0025f) * owner.direction);

					if (OldPosition.Count > 0)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					if (IsLocalOwner)
					{
						if (guardian.GuardianGauntletCharge >= 180f && !Ding)
						{ // Sound cue when fully charged
							Ding = true;
							if (ModContent.GetInstance<OrchidClientConfig>().AltGuardianChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, owner.Center);
							else SoundEngine.PlaySound(SoundID.MaxMana, owner.Center);
						}

						bool jabInput = Main.mouseRight;
						bool chargeInput = Main.mouseLeft;

						if (ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs)
						{
							jabInput = Main.mouseLeft;
							chargeInput = Main.mouseRight;
						}

						if (!chargeInput)
						{
							if (guardian.GuardianGauntletCharge >= 180f)
							{ // swing
								guardian.AddGuard(guardianItem.GuardStacks);
								guardian.AddSlam(guardianItem.SlamStacks);
								Projectile.ai[0] = 41f;
							}
							else
							{ // Null ful charge, jab instead
								Projectile.ai[0] = -40f;
							}

							Projectile.ai[1] = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2;
							guardian.GuardianGauntletCharge = 0;
							Projectile.netUpdate = true;
						}
						else if (jabInput)
						{
							Projectile.ai[0] = -40f;
							Projectile.ai[1] = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2;
							Projectile.netUpdate = true;
						}
					}
				}
				else if (Projectile.ai[0] < 0)
				{ // Jabbing
					if (Projectile.ai[0] == -40f)
					{ // First frame of the jab
						Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage * 0.5f);
						Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
						Projectile.knockBack = QuarterstaffItem.knockBack;
						Projectile.friendly = true;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
						guardianItem.OnAttack(owner, guardian, Projectile, true, false);
						hitTarget = false;
					}

					if (Projectile.ai[1] > -3.14f && Projectile.ai[1] < 0f)
					{ // Facing Right
						if (owner.direction != 1)
						{
							owner.ChangeDir(1);
						}
					}
					else
					{
						if (owner.direction != -1)
						{
							owner.ChangeDir(-1);
						}
					}

					if (Projectile.ai[0] >= -30)
					{ // Returning
						Projectile.friendly = false;
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (float)Math.Sin(0.1046f * (30 + Projectile.ai[0])) * 0.4f * -owner.direction + MathHelper.Pi;
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * (38f - (float)Math.Sin(0.0523f * (30 + Projectile.ai[0])) * 24f);
						Projectile.position.Y -= (float)Math.Sin(0.0523f * (30 + Projectile.ai[0])) * 2f;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f + (float)Math.Sin(0.1046f * (30 + Projectile.ai[0])) * 0.3f * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Sin(0.1046f * (30 + Projectile.ai[0])) * 0.2f * owner.direction);
					}
					else
					{ // Jabbing
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * 3.8f * (Projectile.ai[0] + 40);
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + MathHelper.Pi;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.None, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, Projectile.ai[1] - 0.1f);
					}

					// Trail
					OldPosition.Add(Projectile.Center);
					OldRotation.Add(Projectile.rotation);

					if (OldPosition.Count > 8)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					// Animation progress
					Projectile.ai[0] += guardianItem.JabSpeed * owner.GetTotalAttackSpeed(DamageClass.Melee);

					if (Projectile.ai[0] >= 0)
					{
						if (guardian.GuardianGauntletCharge > 0)
						{
							Projectile.ai[0] = 1f;
						}
						else
						{
							Projectile.ai[0] = 0f;
						}

						Projectile.ai[1] = 0f;
						Projectile.friendly = false;
					}
				}
				else if (Projectile.ai[0] > 1f)
				{ // Swinging (charged attack)
					if (Projectile.ai[0] == 41f)
					{ // First frame of the swing
						Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage * 1.5f);
						Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
						Projectile.knockBack = QuarterstaffItem.knockBack * 1.5f;
						Projectile.friendly = true;
						DamageReset = false;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(QuarterstaffItem.UseSound, Projectile.Center);
						guardianItem.OnAttack(owner, guardian, Projectile, false, false);
						hitTarget = false;
					}

					if (Projectile.ai[0] < 20 && !DamageReset)
					{
						DamageReset = true;
						Projectile.ResetLocalNPCHitImmunity();
					}

					if (Projectile.ai[1] > -3.14f && Projectile.ai[1] < 0f)
					{ // Facing Right
						if (owner.direction != 1)
						{
							owner.ChangeDir(1);
						}
					}
					else
					{
						if (owner.direction != -1)
						{
							owner.ChangeDir(-1);
						}
					}

					if (Projectile.ai[0] > 15)
					{ // Swinging
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 - (float)Math.Cos(0.209f * (Projectile.ai[0] - 10)) * 1.75f * -owner.direction + MathHelper.Pi;
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] - (float)Math.Cos(0.209f * (Projectile.ai[0] - 10)) * 1.6f * -owner.direction) * 24f;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f + (float)Math.Cos(0.209f * (Projectile.ai[0] - 10)) * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.209f * (Projectile.ai[0] - 10)) * 0.2f * owner.direction);
					}
					else
					{ // Returning
						Projectile.friendly = false;
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (-0.5f * 1.1f - (float)Math.Sin(0.12f * -Projectile.ai[0] + 3) + 0.8f) * -owner.direction + MathHelper.Pi;
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] + (-0.5f - (float)Math.Sin(0.12f * -Projectile.ai[0] + 3) + 0.8f) * -owner.direction) * (Projectile.ai[0] * 0.75f + 9f);
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f + (float)Math.Cos(0.145f * (Projectile.ai[0] - 10)) * owner.direction + (20 - Projectile.ai[0]) * 0.04f * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.145f * (Projectile.ai[0] - 5)) * 0.2f * owner.direction);
					}
					/*
					else
					{ // Returning
						Projectile.friendly = false;
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (float)Math.Cos(0.145f * (Projectile.ai[0] - 5)) * 1.1f * -owner.direction + MathHelper.Pi;
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] + (float)Math.Cos(0.145f * (Projectile.ai[0] - 5)) * -owner.direction) * (Projectile.ai[0] * 0.75f + 9f);
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f + (float)Math.Cos(0.145f * (Projectile.ai[0] - 5)) * owner.direction + (20 - Projectile.ai[0]) * 0.02f * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.145f * (Projectile.ai[0] - 5)) * 0.2f * owner.direction);
					}
					*/

					// Trail
					OldPosition.Add(Projectile.Center);
					OldRotation.Add(Projectile.rotation);

					if (OldPosition.Count > 10)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					// Animation progress
					Projectile.ai[0] -= guardianItem.SwingSpeed * owner.GetTotalAttackSpeed(DamageClass.Melee);

					if (Projectile.ai[0] <= 1f)
					{
						Projectile.ai[0] = 0f;
						Projectile.ai[1] = 0f;
						Projectile.friendly = false;
					}
				}
				else
				{ // Idle - guarterstaff is held further and lower
					Ding = false;

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(12f * owner.direction, 0f);
					Projectile.rotation = MathHelper.PiOver4 * owner.direction - MathHelper.PiOver4;

					owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -0.7f * owner.direction);
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -1.2f * owner.direction);

					if (OldPosition.Count > 0)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}
				}

				// Hitbox management for jabs and swings
				Vector2 position = Projectile.Center - Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver4) * (Projectile.width - 20) * 0.5f;
				HitBox.X = (int)position.X - 20;
				HitBox.Y = (int)position.Y - 20;

				Projectile.velocity = Vector2.UnitX * 0.001f * owner.direction; // So enemies are KBd in the right direction

				/*
				for (int i = 0; i < 30; i ++)
				{
					Dust.NewDustDirect(HitBox.TopLeft(), HitBox.Width, HitBox.Height, 6).noGravity = true;
				}
				*/

				// Extra AI (can be overriden in item code)
				guardianItem.ExtraAIQuarterstaff(Projectile);
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (QuarterstaffItem.ModItem is OrchidModGuardianQuarterstaff guardianItem)
			{
				if (Projectile.ai[0] > 1f)
				{ // Swing
					if (!hitTarget)
					{
						hitTarget = true;
						guardianItem.OnHitFirst(player, guardian, target, Projectile, hit, false, false);
					}
					guardianItem.OnHit(player, guardian, target, Projectile, hit, false, false);
				}
				else if (Projectile.ai[0] < 0f)
				{ // Jab
					if (!hitTarget)
					{
						hitTarget = true;
						guardianItem.OnHitFirst(player, guardian, target, Projectile, hit, true, false);
						if (guardian.GuardianGauntletCharge > 0f)
						{
							guardian.GuardianGauntletCharge += 60f * player.GetTotalAttackSpeed(DamageClass.Melee);
							if (guardian.GuardianGauntletCharge > 180f)
							{
								guardian.GuardianGauntletCharge = 180f;
							}
						}
					}
					guardianItem.OnHit(player, guardian, target, Projectile, hit, true, false);
				}
				else
				{ // Counterattack
					if (!hitTarget)
					{
						hitTarget = true;
						guardianItem.OnHitFirst(player, guardian, target, Projectile, hit, false, true);
					}
					guardianItem.OnHit(player, guardian, target, Projectile, hit, false, true);
				}
			}
		}

		public void ResetSize()
		{
			int length = (int)Math.Sqrt(2 * (QuarterstaffTexture.Width * QuarterstaffTexture.Width));
			Projectile.width = length;
			Projectile.height = length;
			Projectile.scale = 1f;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{ // hitting wiith the end of the staff or spinning
			if (target.Hitbox.Intersects(HitBox) || Projectile.ai[2] < 0f) return base.CanHitNPC(target);
			return false;
		}

		public override bool? CanCutTiles() => false; // TODO : can cut while attacking

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (SelectedItem < 0 || SelectedItem > 58 || QuarterstaffTexture == null) return false;
			if (QuarterstaffItem.ModItem is not OrchidModGuardianQuarterstaff guardianItem) return false;

			var player = Main.player[Projectile.owner];

			if (guardianItem.PreDrawQuarterstaff(spriteBatch, Projectile, player, ref lightColor))
			{
				if (Projectile.ai[0] > 1f || Projectile.ai[0] < 0f || Projectile.ai[2] < 0f)
				{ // attacking = draw trail
					spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
					spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

					for (int i = 0; i < OldPosition.Count; i++)
					{
						Vector2 drawPositionTrail = OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
						spriteBatch.Draw(QuarterstaffTexture, drawPositionTrail, null, lightColor * 0.065f * (i + 1), OldRotation[i], QuarterstaffTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
					}

					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
				spriteBatch.Draw(QuarterstaffTexture, drawPosition, null, lightColor, Projectile.rotation, QuarterstaffTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}
			guardianItem.PostDrawQuarterstaff(spriteBatch, Projectile, player, lightColor);

			return false;
		}
	}
}