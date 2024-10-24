using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.Guardian.Weapons.Misc;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class GuardianHorizonLanceAnchor : OrchidModGuardianProjectile
	{
		public int TimeSpent = 0;
		public bool Ding = false;
		public bool Blast = false;
		public bool NeedNetUpdate = false;
		public int SelectedItem { get; set; } = -1;
		public Item HorizonLanceItem => Main.player[Projectile.owner].inventory[SelectedItem];
		public bool Worn => Projectile.ai[1] > 0f; // Standard buff remaining duration

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
			var owner = Main.player[Projectile.owner];
			if (owner.HeldItem.ModItem is HorizonLance) overPlayers.Add(index); // Display the flag over the player if it is being held
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
			guardian.GuardianStandardCharge = 0;
			if (owner.inventory[owner.selectedItem].ModItem is HorizonLance) SelectedItem = owner.selectedItem;
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if (SelectedItem < 0 || HorizonLanceItem == null || HorizonLanceItem.ModItem is not HorizonLance || !owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				bool heldStandard = owner.HeldItem.ModItem is HorizonLance;

				if (IsLocalOwner)
				{
					if (NeedNetUpdate)
					{
						NeedNetUpdate = false;
						Projectile.netUpdate = true;
					}

					if (heldStandard)
					{
						if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
						else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
					}
					else if (!Worn)
					{
						Projectile.Kill();
						return;
					}
				}
				else
				{
					if (!heldStandard)
					{
						Projectile.ai[0] = 0f;
					}

					if (Projectile.ai[0] == 0f)
					{ // Adresses a visual issue
						guardian.GuardianStandardCharge = 0;
					}
				}

				TimeSpent++;
				Projectile.timeLeft = 5;

				if (Worn)
				{ // Handles buffs given to nearby players, npcs, etc
					Projectile.ai[1]--; 
					guardian.GuardianStandardStats.lifeRegen += 6;
					guardian.GuardianCurrentStandardAnchor = Projectile;
				}

				if (HorizonLanceItem.ModItem is HorizonLance guardianItem)
				{ 
					Projectile.localAI[0] = 0f; // used for block UI display
					if (Projectile.ai[0] < 0f)
					{ // Stabbing
						Vector2 puchDir = (Projectile.ai[2] + MathHelper.PiOver2).ToRotationVector2();
						if (puchDir.X > 0 && owner.direction != 1) owner.ChangeDir(1);
						else if (puchDir.X < 0 && owner.direction != -1) owner.ChangeDir(-1);

						float addedDistance = 36f;
						if (Projectile.ai[0] < -40)
						{
							addedDistance = 36f * (40f / -Projectile.ai[0]);
						}

						if (Projectile.ai[0] > -30)
						{
							addedDistance += 0.5f * (10 + Projectile.ai[0]);

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

								int damage = guardian.GetGuardianDamage(HorizonLanceItem.damage);
								Projectile newProjectile = Projectile.NewProjectileDirect(HorizonLanceItem.GetSource_FromAI(), Projectile.Center + owner.velocity * 1.5f, Projectile.ai[2].ToRotationVector2(), projectileType, damage, HorizonLanceItem.knockBack, owner.whoAmI);
								newProjectile.CritChance = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + HorizonLanceItem.crit);
								newProjectile.netUpdate = true;
							}
						}

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(4 * owner.direction, 2f) + Vector2.UnitY.RotatedBy(Projectile.ai[2]) * addedDistance;
						Projectile.rotation = Projectile.ai[2] - MathHelper.PiOver4 * 5f;
						float addedRotation = 0f;
						if (Projectile.ai[2] + MathHelper.PiOver2 > 0f)
						{
							addedRotation = (Projectile.ai[2] + owner.direction * MathHelper.PiOver2) * 0.65f;
						}
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -owner.direction + addedRotation);

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
						if (Projectile.ai[0] <= 1f || owner.immune)
						{
							Projectile.ai[0] = 0f;

							Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
							for (int i = 0; i < 3; i++)
							{
								Dust dust = Dust.NewDustDirect(pos, 20, 20, DustID.Smoke);
								dust.scale *= 0.75f;
								dust.velocity *= 0.25f;
							}
						}

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(8f * owner.direction, -15);
						Projectile.rotation = MathHelper.PiOver4 * 0.15f * owner.direction - MathHelper.PiOver4;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(0.6f + guardian.GuardianStandardCharge * 0.0025f) * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -(1f + guardian.GuardianStandardCharge * 0.0025f) * owner.direction);
					}
					else if (Projectile.ai[0] == 1f)
					{ // Being charged by the player
						Projectile.Center = owner.MountedCenter.Floor() + new Vector2((28f - guardian.GuardianStandardCharge * 0.03f) * owner.direction, 2f + guardian.GuardianStandardCharge * 0.045f);
						Projectile.rotation = MathHelper.PiOver4 * (1.75f + guardian.GuardianStandardCharge * 0.0015f) * owner.direction - MathHelper.PiOver4;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -(0.6f - guardian.GuardianStandardCharge * 0.0025f) * owner.direction);

						if (guardian.GuardianStandardCharge < 180f)
						{
							guardian.GuardianStandardCharge += 30f / guardianItem.Item.useTime * owner.GetAttackSpeed(DamageClass.Melee);
							if (guardian.GuardianStandardCharge > 180f) guardian.GuardianStandardCharge = 180f;
						}

						if (guardian.GuardianStandardCharge >= 180f && !Ding && IsLocalOwner)
						{
							Ding = true;
							if (ModContent.GetInstance<OrchidClientConfig>().AltGuardianChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, owner.Center);
							else SoundEngine.PlaySound(SoundID.MaxMana, owner.Center);
						}

						if ((!owner.controlUseItem || !heldStandard) && IsLocalOwner)
						{
							if (guardian.GuardianStandardCharge >= 180f)
							{ // Full charge
								SoundEngine.PlaySound(guardianItem.Item.UseSound, owner.Center);

								Projectile.ai[1] = guardianItem.StandardDuration * guardian.GuardianStandardTimer;
								guardian.AddGuard(3);

								foreach (Projectile proj in Main.projectile)
								{
									if (proj.type == ModContent.ProjectileType<GuardianStandardAnchor>() && proj.active && proj.owner == Projectile.owner)
									{
										proj.ai[1] = 30f;
										proj.netUpdate = true;
										break;
									}
								}

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
							guardian.GuardianStandardCharge = 0;
							Projectile.netUpdate = true;
						}
					}
					else if (Worn && !heldStandard)
					{ // Display on player back
						Projectile.Center = owner.MountedCenter.Floor();
					}
					else
					{ // Idle - Lance is held lower
						Ding = false;

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(22f * owner.direction, 10f);
						Projectile.rotation = MathHelper.PiOver4 * 2.2f * owner.direction - MathHelper.PiOver4;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -0.15f * owner.direction);
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

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (SelectedItem < 0 || SelectedItem > 58) return false;
			if (HorizonLanceItem.ModItem is not HorizonLance guardianItem) return false;
			if (!ModContent.HasAsset(guardianItem.LanceTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);
			float colorMult2 = 1f;
			if (Projectile.ai[1] < 30f && player.HeldItem.ModItem is not HorizonLance) colorMult2 *= Projectile.ai[1] / 30f;

			var texture = ModContent.Request<Texture2D>(guardianItem.LanceTexture).Value;

			SpriteEffects effect = SpriteEffects.None;
			Vector2 posproj = Projectile.Center;
			float drawRotation = Projectile.rotation;
			if (player.gravDir == -1)
			{
				drawRotation = -drawRotation + MathHelper.Pi;
				posproj.Y = (player.Bottom.Floor() + player.position.Floor()).Y - posproj.Y + (posproj.Y - player.Center.Floor().Y) * 2f;
				effect = SpriteEffects.FlipHorizontally;
			}


			var drawPosition = Vector2.Transform(posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);

			if (Worn && player.HeldItem.ModItem is not HorizonLance)
			{
				drawRotation = MathHelper.PiOver4 * 0.5f * -player.direction - MathHelper.PiOver4;
				if (player.gravDir == -1)
				{
					drawPosition.Y += 24f;
					drawRotation -= MathHelper.PiOver4 * player.direction;
					if (player.direction == -1)
					{
						drawRotation += MathHelper.Pi;
					}
				}

				var textureGlow = ModContent.Request<Texture2D>(guardianItem.LanceTextureGlow).Value;
				drawPosition += new Vector2(-8f * player.direction, -12);
				spriteBatch.Draw(texture, drawPosition, null, color * colorMult2, drawRotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);

				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float colorMult = (float)Math.Sin(TimeSpent * 0.075f) * 0.1f + 0.9f;
				spriteBatch.Draw(textureGlow, drawPosition, null, Color.White * colorMult * colorMult2, drawRotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
			else
			{
				spriteBatch.Draw(texture, drawPosition, null, color * colorMult2, drawRotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}


			return false;
		}
	}
}