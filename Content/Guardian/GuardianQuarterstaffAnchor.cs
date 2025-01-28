using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.Shaman.Buffs.Debuffs;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace OrchidMod.Content.Guardian
{
	public class GuardianQuarterstaffAnchor : OrchidModGuardianAnchor
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public int TimeSpent = 0;
		public bool Ding = false;
		public bool NeedNetUpdate = false;
		public int DamageReset = 0;
		public Rectangle[] HitBox;

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

			HitBox = [new Rectangle(0, 0, 30, 30), new Rectangle(0, 0, 30, 30), new Rectangle(0, 0, 30, 30)];
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
					Projectile.friendly = false;
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

					Projectile.ai[2]--;
					if (owner.immune)
					{
						if (owner.eocHit != -1)
						{
							guardian.DoParryItemParry(Main.npc[owner.eocHit]);
						}
						else
						{
							Projectile.ai[2] = 0f;
							guardian.GuardianGuardRecharging += Projectile.ai[2] / guardianItem.ParryDuration;
							Rectangle rect = owner.Hitbox;
							rect.Y -= 64;
							CombatText.NewText(guardian.Player.Hitbox, Color.LightGray, "Interrupted", false, true);
						}
					}
					else if (Projectile.ai[2] <= 0f)
					{
						Projectile.ai[2] = 0f;
					}

					if (Projectile.scale > 1f)
					{
						ResetSize();
					}
				}
				else if (Projectile.ai[2] < 0f)
				{ // Counterattacking
					if (Projectile.ai[2] == -40f)
					{ // First frame of the counterattack
						Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage * guardianItem.CounterDamage);
						Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
						Projectile.knockBack = QuarterstaffItem.knockBack * guardianItem.CounterKnockback;
						Projectile.friendly = true;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
						ResetHitStatus(true);
						DamageReset = 1;
						Projectile.scale *= 1.2f;
						Projectile.width = (int)(Projectile.width * 1.2f);
						Projectile.height = (int)(Projectile.height * 1.2f);
					}

					if (Projectile.ai[2] >= (-40 / guardianItem.CounterHits) * (guardianItem.CounterHits - DamageReset))
					{ // Reset damage twice while spinning
						DamageReset ++;
						Projectile.ResetLocalNPCHitImmunity();

						if ((int)(guardianItem.CounterHits * 0.5f) == DamageReset)
						{
							SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
						}
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

					if (Projectile.ai[2] == -40f)
					{ // First frame of the counterattack
						guardianItem.OnAttack(owner, guardian, Projectile, false, true);
					}

					Projectile.ai[2] += guardianItem.CounterSpeed * owner.GetTotalAttackSpeed(DamageClass.Melee);

					if (Projectile.ai[2] >= 0)
					{
						Projectile.ai[0] = 0f;
						Projectile.ai[1] = 0f;
						Projectile.ai[2] = 0f;
						ResetSize();
						Projectile.friendly = false;

						if (IsLocalOwner)
						{
							bool blockInput = Main.mouseRight;

							if (ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs)
							{
								blockInput = Main.mouseRight;
							}

							if (blockInput && guardian.UseGuard(1, true))
							{
								owner.immuneTime = 0;
								owner.immune = false;
								guardian.modPlayer.PlayerImmunity = 0;
								guardian.GuardianGauntletCharge = 0f;
								guardian.UseGuard(1);
								Projectile.ai[2] = guardianItem.ParryDuration;
								Projectile.netUpdate = true;
								SoundEngine.PlaySound(SoundID.Item37, owner.Center);
								guardian.GuardianGauntletParry = true;
								guardian.GuardianGauntletParry2 = true;
							}
						}
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

					if (Projectile.scale > 1f)
					{
						ResetSize();
					}
				}
				else if (Projectile.ai[0] < 0)
				{ // Jabbing
					if (Projectile.ai[0] == -40f)
					{ // First frame of the jab
						Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage * guardianItem.JabDamage);
						Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
						Projectile.knockBack = QuarterstaffItem.knockBack * guardianItem.JabKnockback;
						Projectile.friendly = true;
						DamageReset = 0;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
						ResetHitStatus(false);
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

					if (guardianItem.PreJabAI(owner, guardian, Projectile)) DoAnimStyle(guardianItem.JabStyle, -Projectile.ai[0], SoundID.DD2_MonkStaffSwing);
					/*if (Projectile.ai[0] >= -30)
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
					}*/

					// Trail
					OldPosition.Add(Projectile.Center);
					OldRotation.Add(Projectile.rotation);

					if (OldPosition.Count > 8)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					if (Projectile.ai[0] == -40f)
					{ // First frame of the jab
						guardianItem.OnAttack(owner, guardian, Projectile, true, false);
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

						if (Main.mouseLeft && Main.mouseRight)
						{ // Perfect jab loop while holding the attack
							Projectile.ai[0] = -40f;
							Projectile.ai[1] = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2;
							Projectile.netUpdate = true;

							if (guardian.GuardianGauntletCharge <= 0)
							{ // Fix an occurence where jabs loop and the player doesn't have any charge
								guardian.GuardianGauntletCharge = 1f;
							}
						}
					}
				}
				else if (Projectile.ai[0] > 1f)
				{ // Swinging (charged attack)
					if (Projectile.ai[0] == 41f)
					{ // First frame of the swing
						Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage * guardianItem.SwingDamage);
						Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
						Projectile.knockBack = QuarterstaffItem.knockBack * guardianItem.SwingKnockback;
						Projectile.friendly = true;
						DamageReset = 0;
						Projectile.ResetLocalNPCHitImmunity();
						SoundEngine.PlaySound(QuarterstaffItem.UseSound, Projectile.Center);
						ResetHitStatus(true);
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

					if (guardianItem.PreSwingAI(owner, guardian, Projectile)) DoAnimStyle(guardianItem.SwingStyle, Projectile.ai[0] - 1, QuarterstaffItem.UseSound.Value);
					/*if (guardianItem.SingleSwing)
					{
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (float)Math.Cos(0.102f * (Projectile.ai[0] - 10)) * 1.9f * -owner.direction + MathHelper.Pi;
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] + (float)Math.Cos(0.102f * (Projectile.ai[0] - 10)) * 1.8f * -owner.direction) * 24f;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f - (float)Math.Cos(0.102f * (Projectile.ai[0] - 10)) * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.102f * (Projectile.ai[0] - 10)) * 0.2f * owner.direction);
					}
					else
					{
						if (Projectile.ai[0] < 26 && DamageReset == 0)
						{
							DamageReset++;
							SoundEngine.PlaySound(QuarterstaffItem.UseSound, Projectile.Center);
							Projectile.ResetLocalNPCHitImmunity();
							Projectile.ai[0] -= 3f;
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
					}*/

					// Trail
					OldPosition.Add(Projectile.Center);
					OldRotation.Add(Projectile.rotation);

					if (OldPosition.Count > 10)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}

					if (Projectile.ai[0] == 41f)
					{ // First frame of the swing
						guardianItem.OnAttack(owner, guardian, Projectile, false, false);
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
				Vector2 position = Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver4) * (Projectile.width - 15);
				HitBox[0].X = (int)(Projectile.Center.X - position.X * 0.5f) - 15;
				HitBox[0].Y = (int)(Projectile.Center.Y - position.Y * 0.5f) - 15;
				HitBox[1].X = (int)(Projectile.Center.X - position.X * 0.2f) - 15;
				HitBox[1].Y = (int)(Projectile.Center.Y - position.Y * 0.2f) - 15;
				HitBox[2].X = (int)(Projectile.Center.X - position.X * -0.1f) - 15;
				HitBox[2].Y = (int)(Projectile.Center.Y - position.Y * -0.1f) - 15;

				Projectile.velocity = Vector2.UnitX * 0.001f * owner.direction; // So enemies are KBd in the right direction

				// Hitbox display for testing
				/*
				if (Projectile.friendly)
				{
					for (int i = 0; i < 30; i++)
					{
						Dust.NewDustDirect(HitBox[0].TopLeft(), HitBox[0].Width, HitBox[0].Height, DustID.RedTorch).noGravity = true;
					}

					for (int i = 0; i < 30; i++)
					{
						Dust.NewDustDirect(HitBox[1].TopLeft(), HitBox[1].Width, HitBox[1].Height, DustID.GreenTorch).noGravity = true;
					}

					for (int i = 0; i < 30; i++)
					{
						Dust.NewDustDirect(HitBox[2].TopLeft(), HitBox[2].Width, HitBox[2].Height, DustID.BlueTorch).noGravity = true;
					}
				}
				*/

				// Extra AI (can be overriden in item code)
				guardianItem.ExtraAIQuarterstaff(owner, guardian, Projectile);
			}
		}

		/// <summary>Executes the given animation style. <c>ai</c> should begin at 40 and decrement to 0 for the animation to play in correct order.</summary>
		/// <remarks>Animation style IDs<br/>
		/// 0: Jab (default jab)<br/>
		/// 1: Double swing (default swing)<br/>
		/// 2: Single swing<br/>
		/// </remarks>
		public void DoAnimStyle(int style, float ai, SoundStyle sound)
		{
			Player player = Main.player[Projectile.owner];
			switch(style)
			{
				case 0:
					if (ai <= 30)
					{ // Returning
						Projectile.friendly = false;
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (float)Math.Sin(0.1046f * (30 - ai)) * 0.4f * -player.direction + MathHelper.Pi;
						Projectile.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * (38f - (float)Math.Sin(0.0523f * (30 - ai)) * 24f);
						Projectile.position.Y -= (float)Math.Sin(0.0523f * (30 - ai)) * 2f;
						player.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * player.direction + Projectile.ai[1] + 0.1f + (float)Math.Sin(0.1046f * (30 - ai)) * 0.3f * player.direction);
						player.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Sin(0.1046f * (30 - ai)) * 0.2f * player.direction);
					}
					else
					{ // Jabbing
						Projectile.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * 3.8f * (40 - ai);
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + MathHelper.Pi;
						player.SetCompositeArmFront(true, CompositeArmStretchAmount.None, MathHelper.PiOver4 * player.direction + Projectile.ai[1] + 0.1f);
						player.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, Projectile.ai[1] - 0.1f);
					}
				break;
				case 1:
					if (ai < 25 && DamageReset == 0)
					{
						DamageReset++;
						SoundEngine.PlaySound(sound, Projectile.Center);
						Projectile.ResetLocalNPCHitImmunity();
						Projectile.ai[0] -= 3f;
					}
					if (ai > 14)
					{ // Swinging
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 - (float)Math.Cos(0.209f * (ai - 9)) * 1.75f * -player.direction + MathHelper.Pi;
						Projectile.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] - (float)Math.Cos(0.209f * (ai - 9)) * 1.6f * -player.direction) * 24f;
						player.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver4 * player.direction + Projectile.ai[1] + 0.1f + (float)Math.Cos(0.209f * (ai - 9)) * player.direction);
						player.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.209f * (ai - 9)) * 0.2f * player.direction);
					}
					else
					{ // Returning
						Projectile.friendly = false;
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (-0.5f * 1.1f - (float)Math.Sin(0.12f * -ai + 2.88f) + 0.8f) * -player.direction + MathHelper.Pi;
						Projectile.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] + (-0.5f - (float)Math.Sin(0.12f * -ai + 2.88f) + 0.8f) * -player.direction) * (ai * 0.75f + 9.75f);
						player.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * player.direction + Projectile.ai[1] + 0.1f + (float)Math.Cos(0.145f * (ai - 9)) * player.direction + (19 - ai) * 0.04f * player.direction);
						player.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.145f * (ai - 4)) * 0.2f * player.direction);
					}
				break;
				case 2:
					Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (float)Math.Cos(0.102f * (ai - 9)) * 1.9f * -player.direction + MathHelper.Pi;
					Projectile.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1] + (float)Math.Cos(0.102f * (ai - 9)) * 1.8f * -player.direction) * 24f;
					player.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver4 * player.direction + Projectile.ai[1] + 0.1f - (float)Math.Cos(0.102f * (ai - 9)) * player.direction);
					player.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Cos(0.102f * (ai- 9)) * 0.2f * player.direction);
				break;
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (QuarterstaffItem.ModItem is OrchidModGuardianQuarterstaff guardianItem)
			{
				if (Projectile.ai[0] > 1f)
				{ // Swing
					if (FirstHit)
					{
						guardian.AddGuard(guardianItem.GuardStacks);
						guardian.AddSlam(guardianItem.SlamStacks);
						guardianItem.OnHitFirst(player, guardian, target, Projectile, hit, false, false);
					}
					guardianItem.OnHit(player, guardian, target, Projectile, hit, false, false);
				}
				else if (Projectile.ai[0] < 0f)
				{ // Jab
					if (FirstHit)
					{
						guardianItem.OnHitFirst(player, guardian, target, Projectile, hit, true, false);
						if (guardian.GuardianGauntletCharge > 0f)
						{
							guardian.GuardianGauntletCharge += 60f * guardianItem.JabChargeGain * player.GetTotalAttackSpeed(DamageClass.Melee);
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
					if (FirstHit)
					{
						guardianItem.OnHitFirst(player, guardian, target, Projectile, hit, false, true);
					}
					guardianItem.OnHit(player, guardian, target, Projectile, hit, false, true);
				}
			}
		}

		public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (QuarterstaffItem.ModItem is OrchidModGuardianQuarterstaff guardianItem)
			{
				Player player = Owner;
				OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
				guardianItem.QuarterstaffModifyHitNPC(player, guardian, target, Projectile, ref modifiers, Projectile.ai[0] < 0f, Projectile.ai[2] < 0f, FirstHit);
			}
		}

		public void ResetSize()
		{
			int length = (int)Math.Sqrt(2 * (QuarterstaffTexture.Width * QuarterstaffItem.scale * QuarterstaffTexture.Width * QuarterstaffItem.scale));
			Projectile.width = length + 4;
			Projectile.height = length + 4;
			Projectile.scale = QuarterstaffItem.scale;
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
			if (target.Hitbox.Intersects(HitBox[0]) || target.Hitbox.Intersects(HitBox[1]) || target.Hitbox.Intersects(HitBox[2]) || Projectile.ai[2] < 0f) return base.CanHitNPC(target);
			return false;
		}

		public override bool? CanCutTiles() => false; // TODO : can cut while attacking

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (SelectedItem < 0 || SelectedItem > 58 || QuarterstaffTexture == null) return false;
			if (QuarterstaffItem.ModItem is not OrchidModGuardianQuarterstaff guardianItem) return false;

			var player = Main.player[Projectile.owner];
			SpriteEffects effect = SpriteEffects.None;
			float rotationoffset = 0f;

			if (player.direction == -1)
			{
				effect = SpriteEffects.FlipVertically;
				rotationoffset = -MathHelper.PiOver2;
			}

			if (guardianItem.PreDrawQuarterstaff(spriteBatch, Projectile, player, ref lightColor))
			{
				if (Projectile.ai[0] > 1f || Projectile.ai[0] < 0f || Projectile.ai[2] < 0f)
				{ // attacking = draw trail
					spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
					spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

					for (int i = 0; i < OldPosition.Count; i++)
					{
						Vector2 drawPositionTrail = OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
						spriteBatch.Draw(QuarterstaffTexture, drawPositionTrail, null, lightColor * 0.05f * (i + 1), OldRotation[i] + rotationoffset, QuarterstaffTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
					}

					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
				spriteBatch.Draw(QuarterstaffTexture, drawPosition, null, lightColor, Projectile.rotation + rotationoffset, QuarterstaffTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawQuarterstaff(spriteBatch, Projectile, player, lightColor);

			return false;
		}
	}
}