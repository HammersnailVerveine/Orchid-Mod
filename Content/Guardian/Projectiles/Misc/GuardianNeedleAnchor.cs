using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.Guardian.Weapons.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;
using Terraria.Localization;
using OrchidMod.Content.General.Prefixes;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class GuardianNeedleAnchor : OrchidModGuardianAnchor
	{
		public int TimeSpent = 0;
		public bool Ding = false;
		public bool Blast = false;
		public bool NeedNetUpdate = false;
		public int SelectedItem { get; set; } = -1;
		public Item GuardianItem => Main.player[Projectile.owner].inventory[SelectedItem];
		Vector2 visualSway = Vector2.Zero;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
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
			Projectile.localNPCHitCooldown = 20;
			Projectile.netImportant = true;
		}

		public void OnChangeSelectedItem(Player owner)
		{
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
			Projectile.ai[0] = 0f;
			guardian.GuardianItemCharge = 0;
			if (owner.inventory[owner.selectedItem].ModItem is GuardianNeedle) SelectedItem = owner.selectedItem;
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if (SelectedItem < 0 || GuardianItem == null || GuardianItem.ModItem is not GuardianNeedle || !owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				bool heldItem = owner.HeldItem.ModItem is GuardianNeedle;

				if (IsLocalOwner)
				{
					if (NeedNetUpdate)
					{
						NeedNetUpdate = false;
						Projectile.netUpdate = true;
					}

					if (heldItem)
					{
						if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
						else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
					}
					else
					{
						Projectile.Kill();
						return;
					}
				}
				else
				{
					if (!heldItem)
					{
						Projectile.ai[0] = 0f;
					}

					if (Projectile.ai[0] == 0f)
					{ // Adresses a visual issue
						guardian.GuardianItemCharge = 0;
					}
				}

				TimeSpent++;
				Projectile.timeLeft = 5;

				if (GuardianItem.ModItem is GuardianNeedle guardianItem)
				{ 
					Projectile.localAI[0] = 0f; // used for block UI display
					if (Projectile.ai[0] < 0f)
					{ // Stabbing
						Vector2 puchDir = (Projectile.ai[2] + MathHelper.PiOver2).ToRotationVector2();
						if (puchDir.X > 0 && owner.direction != 1) owner.ChangeDir(1);
						else if (puchDir.X < 0 && owner.direction != -1) owner.ChangeDir(-1);

						float addedDistance = 28f;
						//lunge forward
						if (Projectile.ai[0] < -30)
						{
							addedDistance = 28f * (40f / -Projectile.ai[0]);
						}
						//fire, recoil back
						if (Projectile.ai[0] > -30)
						{
							addedDistance += 0.4f * Projectile.ai[0];

							if (!Blast)
							{
								Blast = true;
								SoundEngine.PlaySound(SoundID.Item105, owner.Center);
								int projectileType = ModContent.ProjectileType<GuardianHorizonLanceProj>();

								if (IsLocalOwner)
								{
									foreach (Projectile projectile in Main.projectile)
									{
										if (projectile.type == projectileType && projectile.active && projectile.owner == owner.whoAmI)
										{
											projectile.ai[1] = 1f;
										}
									}
								}

								int damage = guardian.GetGuardianDamage(GuardianItem.damage);
								Projectile newProjectile = Projectile.NewProjectileDirect(GuardianItem.GetSource_FromAI(), Projectile.Center + owner.velocity * 1.5f, Projectile.ai[2].ToRotationVector2(), projectileType, damage, GuardianItem.knockBack, owner.whoAmI);
								newProjectile.CritChance = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + GuardianItem.crit);
								newProjectile.netUpdate = true;
							}
						}

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(4f * owner.direction, 2f) + Vector2.UnitY.RotatedBy(Projectile.ai[2]) * addedDistance;
						Projectile.rotation = Projectile.ai[2] - MathHelper.PiOver4 * 5f;
						float addedRotation = 0f;
						if (Projectile.ai[2] + MathHelper.PiOver2 > 0f)
						{
							addedRotation = (Projectile.ai[2] + owner.direction * MathHelper.PiOver2) * 0.5f;
						}
						addedRotation += (addedDistance - 28) * (puchDir.Y > 0 ? 0.06f : 0.12f) * owner.direction * puchDir.Y;
						owner.SetCompositeArmFront(true, addedDistance > 24 ? CompositeArmStretchAmount.Full : addedDistance > 18 ? CompositeArmStretchAmount.ThreeQuarters : CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * 2.2f * -owner.direction + addedRotation);

						Projectile.ai[0]++;

						if (Projectile.ai[0] >= 0f)
						{
							Projectile.ai[0] = 0f;
							Projectile.ai[2] = 0f;
						}
					}
					else if (Projectile.ai[0] > 1f)
					{ // Blocking
						Projectile.localAI[0] = 90f; // used for block UI display
						guardian.GuardianGauntletParry = true;
						guardian.GuardianGauntletParry2 = true;

						Projectile.ai[0]--;
						if (owner.immune)
						{
							if (owner.eocHit != -1)
							{
								guardian.DoParryItemParry(Main.npc[owner.eocHit]);
							}
							else
							{
								guardian.GuardianGuardRecharging += Projectile.ai[2] / (guardianItem.ParryDuration * guardianItem.Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianParryDuration);
								Rectangle rect = owner.Hitbox;
								rect.Y -= 64;
								CombatText.NewText(guardian.Player.Hitbox, Color.LightGray, "Interrupted", false, true);
							}
							Projectile.ai[0] = 0f;

							Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
							for (int i = 0; i < 3; i++)
							{
								Dust dust = Dust.NewDustDirect(pos, 20, 20, DustID.Smoke);
								dust.scale *= 0.75f;
								dust.velocity *= 0.25f;
							}
						}
						else if (Projectile.ai[0] <= 0f)
						{
							Projectile.ai[0] = 0f;
						}

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(8f * owner.direction, -15);
						Projectile.rotation = MathHelper.PiOver4 * 0.15f * owner.direction - MathHelper.PiOver4;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(0.6f + guardian.GuardianItemCharge * 0.0025f) * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -(1f + guardian.GuardianItemCharge * 0.0025f) * owner.direction);
					}
					else if (Projectile.ai[0] == 1f)
					{ // Being charged by the player
						Projectile.Center = owner.MountedCenter.Floor() + new Vector2((26f - guardian.GuardianItemCharge * 0.03f) * owner.direction, guardian.GuardianItemCharge * 0.045f);
						Projectile.rotation = MathHelper.PiOver4 * (1.75f + guardian.GuardianItemCharge * 0.0015f) * owner.direction - MathHelper.PiOver4;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.ThreeQuarters, MathHelper.PiOver2 * -(0.6f - guardian.GuardianItemCharge * 0.0025f) * owner.direction);

						if (guardian.GuardianItemCharge < 180f)
						{
							guardian.GuardianItemCharge += 30f / guardianItem.Item.useTime * owner.GetTotalAttackSpeed(DamageClass.Melee);
							if (guardian.GuardianItemCharge > 180f) guardian.GuardianItemCharge = 180f;
						}

						if (guardian.GuardianItemCharge >= 180f && !Ding && IsLocalOwner)
						{
							Ding = true;
							if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, owner.Center);
							else SoundEngine.PlaySound(SoundID.MaxMana, owner.Center);
						}

						if ((!owner.controlUseItem || !heldItem) && IsLocalOwner)
						{
							if (guardian.GuardianItemCharge >= 180f)
							{ // Full charge
								SoundEngine.PlaySound(guardianItem.Item.UseSound, owner.Center);
								guardian.AddGuard(3);

								// Stab starts
								Projectile.ai[2] = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2;
								Projectile.ai[0] = -50f;
								owner.itemTime = 51;
								owner.itemAnimation = 51;
							}
							else
							{ // Not enough charge = Reset to idle
								Projectile.ai[0] = 0f;
							}

							Blast = false;
							guardian.GuardianItemCharge = 0;
							Projectile.netUpdate = true;
						}
					}
					else
					{ // Idle - Lance is held lower
						Ding = false;
						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(18f * owner.direction, 14f);
						Projectile.rotation = MathHelper.PiOver4 * 2.2f * owner.direction - MathHelper.PiOver4;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -0.05f * owner.direction);
					}
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanCutTiles() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (SelectedItem < 0 || SelectedItem > 58 || GuardianItem.ModItem is not GuardianNeedle guardianItem) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);
			float colorMult2 = 1f;

			SpriteEffects effect = SpriteEffects.None;
			Vector2 posproj = Projectile.Center;
			float drawRotation = Projectile.rotation;
			if (player.gravDir == -1)
			{
				drawRotation = -drawRotation + MathHelper.Pi;
				posproj.Y = (player.Bottom.Floor() + player.position.Floor()).Y - posproj.Y + (posproj.Y - player.Center.Floor().Y) * 2f;
				effect = SpriteEffects.FlipHorizontally;
			}

			var texture = ModContent.Request<Texture2D>(Texture).Value;
			var drawPosition = Vector2.Transform(posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);

			spriteBatch.Draw(texture, drawPosition, null, color * colorMult2, drawRotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);


			return false;
		}
	}
}
