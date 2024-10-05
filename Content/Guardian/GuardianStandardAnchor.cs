using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
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
	public class GuardianStandardAnchor : OrchidModGuardianProjectile
	{
		public static Texture2D TextureAura;
		public int TimeSpent = 0;
		public bool Ding = false;
		public bool NeedNetUpdate = false;
		public bool Worn => Projectile.ai[1] > 0f; // Standard buff remaining duration
		public bool Reinforced => Projectile.ai[2] == 1f && ((BuffItem == StandardItem) || (Main.player[Projectile.owner].HeldItem.ModItem is not OrchidModGuardianStandard && Worn)); // Has the item been used twice (stronger effects)
		public Item BuffItem = null;
		public int SelectedItem { get; set; } = -1;
		public Item StandardItem => Main.player[Projectile.owner].inventory[SelectedItem];

		public float Alpha
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public override void Load()
		{
			TextureAura ??= ModContent.Request<Texture2D>(Texture + "_Aura", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
			writer.Write(BuffItem != null);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
			if (reader.ReadBoolean()) BuffItem = StandardItem;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			var owner = Main.player[Projectile.owner];
			if (owner.HeldItem.ModItem is OrchidModGuardianStandard) overPlayers.Add(index); // Display the flag over the player if it is being held
		}

		public override void AltSetDefaults()
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
			if (!Worn || owner.inventory[owner.selectedItem].ModItem is OrchidModGuardianStandard) SelectedItem = owner.selectedItem;
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if ((!Worn && (SelectedItem < 0 || StandardItem == null || StandardItem.ModItem is not OrchidModGuardianStandard)) || !owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				bool heldStandard = owner.HeldItem.ModItem is OrchidModGuardianStandard;
				if (!heldStandard)
				{
					if (IsLocalOwner)
					{
						if (!Worn)
						{
							Projectile.Kill();
							return;
						}
					}
					else
					{
						Projectile.ai[0] = 0f;
					}
				}

				if (!IsLocalOwner && Projectile.ai[0] == 0f)
				{ // Adresses a visual issue
					guardian.GuardianStandardCharge = 0;
				}

				if ((heldStandard || owner.HeldItem.ModItem is OrchidModGuardianStandard) && IsLocalOwner)
				{
					if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
					else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
					if (NeedNetUpdate)
					{
						NeedNetUpdate = false;
						Projectile.netUpdate = true;
					}
				}

				TimeSpent++;
				Projectile.timeLeft = 5;

				if (owner.velocity.X > 0) // This part allows moving the flag based on player movement
				{
					Projectile.localAI[1] += Math.Abs(Projectile.localAI[1] - 1) * 0.1f;
				}
				else if (owner.velocity.X < 0)
				{
					Projectile.localAI[1] -= Math.Abs(-1 - Projectile.localAI[1]) * 0.1f;
				}
				else Projectile.localAI[1] *= 0.95f;

				if (Worn)
				{ // Handles buffs given to nearby players, npcs, etc
					if (BuffItem == null || BuffItem.ModItem is not OrchidModGuardianStandard buffItem)
					{
						Main.NewText("Error with a Standard effect, please tell me how that happened!", Color.Red);
						Projectile.Kill();
						return;
					}
					else
					{
						bool AnyNear = false; // Used for Alpha stuff

						if (buffItem.AffectNearbyPlayers)
						{ // Called on every client to affect nearby players
							foreach (Player player in Main.player)
							{
								if (player.active && !player.dead && player.Center.Distance(owner.Center) < (buffItem.AuraRange + player.width * 0.5f))
								{
									buffItem.NearbyPlayerEffect(player.GetModPlayer<OrchidGuardian>().GuardianStandardStats, player, guardian, player == owner, Projectile.ai[2] == 1f);
									AnyNear = buffItem.DrawAura(true, player.whoAmI == owner.whoAmI, false, IsLocalOwner, Projectile.ai[2] == 1f);
								}
							}
						}

						if (buffItem.AffectNearbyNPCs)
						{ // Called on every client to affect nearby NPCs
							foreach (NPC npc in Main.npc)
							{
								if (npc.active && !npc.friendly && !npc.CountsAsACritter && npc.Center.Distance(owner.Center) < (buffItem.AuraRange + npc.width * 0.5f))
								{
									buffItem.NearbyNPCEffect(owner, guardian, npc, IsLocalOwner, Projectile.ai[2] == 1f);
									AnyNear = buffItem.DrawAura(false, false, true, IsLocalOwner, Projectile.ai[2] == 1f);
								}
							}
						}

						if (AnyNear && Projectile.ai[1] > 50f)
						{ // Used for the Alpha of the effect radius aura
							Alpha += 0.01f;
							if (Alpha > 0.5f) Alpha = 0.5f;
						}
						else
						{
							Alpha -= 0.01f;
							if (Alpha < 0f) Alpha = 0f;
						}

						guardian.StandardAnchor = Projectile;
						Projectile.ai[1]--;
						if (Projectile.ai[1] <= 0)
						{
							Projectile.ai[1] = 0;
							Projectile.ai[2] = 0;
							BuffItem = null;
							if (!heldStandard && IsLocalOwner) Projectile.Kill();
						}
					}
				}

				if (StandardItem.ModItem is OrchidModGuardianStandard guardianItem)
				{
					if (Projectile.ai[0] == 1f)
					{ // Being charged by the player
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
							{
								SoundEngine.PlaySound(guardianItem.Item.UseSound, owner.Center);

								if (BuffItem == StandardItem && !Reinforced)
								{
									Projectile.ai[2] = 1f;
									CombatText.NewText(owner.Hitbox, new Color(175, 255, 175), "Reinforced");
								}
								else
								{
									if (BuffItem != StandardItem) Projectile.ai[2] = 0f;
									if (!Reinforced) CombatText.NewText(owner.Hitbox, new Color(175, 255, 175), "Charged", false);
								}

								BuffItem = StandardItem;
								Projectile.ai[1] = guardianItem.StandardDuration * guardian.GuardianStandardTimer;

								guardian.AddGuard(guardianItem.GuardStacks);
								guardian.AddSlam(guardianItem.SlamStacks);

								int projectileType = ModContent.ProjectileType<StandardAuraProjectile>();
								Projectile auraProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), owner.Center, Vector2.Zero, projectileType, 0, 0f, owner.whoAmI);
							}

							guardian.GuardianStandardCharge = 0;
							Projectile.ai[0] = 0f;
							Projectile.netUpdate = true;
						}

						owner.itemAnimation = 1;
						owner.heldProj = Projectile.whoAmI;

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(8f * owner.direction, -(12 + guardian.GuardianStandardCharge * 0.03f));
						Projectile.rotation = MathHelper.PiOver4 * (0.25f - guardian.GuardianStandardCharge * 0.0025f) * owner.direction - MathHelper.PiOver4;

						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(0.6f + guardian.GuardianStandardCharge * 0.0025f) * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -(1f + guardian.GuardianStandardCharge * 0.0025f) * owner.direction);
					}
					else if (Worn && !heldStandard)
					{ // Display on player back
						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(-8f * owner.direction, -12);
						Projectile.rotation = MathHelper.PiOver4 * -0.3f * owner.direction - MathHelper.PiOver4;
					}
					else
					{ // Idle - flag is held further and lower
						Ding = false;

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(16f * owner.direction, -8);
						Projectile.rotation = MathHelper.PiOver4 * 0.5f * owner.direction - MathHelper.PiOver4;

						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -0.8f * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -1.2f * owner.direction);
					}
					guardianItem.ExtraAIStandard(Projectile);
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
			if (StandardItem.ModItem is not OrchidModGuardianStandard guardianItem) return false;
			if (!ModContent.HasAsset(guardianItem.ShaftTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);
			if (Projectile.ai[1] < 30f && player.HeldItem.ModItem is not OrchidModGuardianStandard) color *= Projectile.ai[1] / 30f;

			if (player.HeldItem.ModItem is not OrchidModGuardianStandard && Worn) guardianItem = (OrchidModGuardianStandard)BuffItem.ModItem;

			if (guardianItem.PreDrawStandard(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(guardianItem.ShaftTexture).Value;

				var drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				spriteBatch.Draw(texture, drawPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);

				float windSpeed = 0.25f * Main.windSpeedCurrent;
				float flagRotation = MathHelper.PiOver4 * Projectile.localAI[1] * 0.5f + (float)Math.Sin(TimeSpent * (Math.Abs(windSpeed) > 0.05f ? windSpeed : 0.05f)) * 0.1f;
				Vector2 flagOffset = Vector2.UnitX.RotatedBy(Projectile.rotation) * -1.5f * Projectile.localAI[1];

				Texture2D textureEnd = ModContent.Request<Texture2D>(guardianItem.FlagEndTexture).Value;
				Texture2D textureQuarter = ModContent.Request<Texture2D>(guardianItem.FlagTwoQuarterTexture).Value;
				Texture2D textureTwoQuarter = ModContent.Request<Texture2D>(guardianItem.FlagQuarterTexture).Value;
				Texture2D textureUp = ModContent.Request<Texture2D>(guardianItem.FlagUpTexture).Value;

				if (Alpha > 0f || Reinforced)
				{
					spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
					spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

					if (Alpha > 0f && BuffItem != null)
					{ // Aura around the player showing range
						OrchidModGuardianStandard standard = (BuffItem.ModItem as OrchidModGuardianStandard);
						float lightalpha = (color.R + color.G + color.B) / 765f;
						if (lightalpha < 0.5f) lightalpha = 0.5f;
						float alphamult = (float)(Math.Sin(TimeSpent * 0.075f) * 0.075f + 1f) * Alpha * lightalpha;
						Vector2 drawPositionAura = Vector2.Transform(player.Center.Floor() - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
						spriteBatch.Draw(TextureAura, drawPositionAura, null, standard.GetColor() * alphamult, 0f, TextureAura.Size() * 0.5f, 0.00625f * standard.AuraRange, SpriteEffects.None, 0f); ;
					}

					if (Reinforced)
					{ // Flag glow effect when reinforced
						Color glowColor = Color.White;
						if (Projectile.ai[1] < 30f && player.HeldItem.ModItem is not OrchidModGuardianStandard) glowColor *= Projectile.ai[1] / 30f;
						spriteBatch.Draw(textureEnd, drawPosition + flagOffset * 1.3f, null, glowColor, Projectile.rotation + flagRotation * 2.4f, texture.Size() * 0.5f, Projectile.scale * 1.1f, effect, 0f);
						spriteBatch.Draw(textureQuarter, drawPosition + flagOffset * 1.2f, null, glowColor, Projectile.rotation + flagRotation * 1.4f, texture.Size() * 0.5f, Projectile.scale * 1.1f, effect, 0f);
						spriteBatch.Draw(textureTwoQuarter, drawPosition + flagOffset * 1.1f, null, glowColor, Projectile.rotation + flagRotation * 1f, texture.Size() * 0.5f, Projectile.scale * 1.1f, effect, 0f);
						spriteBatch.Draw(textureUp, drawPosition + flagOffset, null, glowColor, Projectile.rotation + flagRotation * 0.5f, texture.Size() * 0.5f, Projectile.scale * 1.1f, effect, 0f);
					}
					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				spriteBatch.Draw(textureEnd, drawPosition + flagOffset * 1.3f, null, color, Projectile.rotation + flagRotation * 2.4f, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
				spriteBatch.Draw(textureQuarter, drawPosition + flagOffset * 1.2f, null, color, Projectile.rotation + flagRotation * 1.4f, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
				spriteBatch.Draw(textureTwoQuarter, drawPosition + flagOffset * 1.1f, null, color, Projectile.rotation + flagRotation * 1f, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
				spriteBatch.Draw(textureUp, drawPosition + flagOffset, null, color, Projectile.rotation + flagRotation * 0.5f, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawStandard(spriteBatch, Projectile, player, color);

			return false;
		}
	}
}