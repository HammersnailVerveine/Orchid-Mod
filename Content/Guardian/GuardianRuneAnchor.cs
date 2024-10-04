using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
	public class GuardianRuneAnchor : OrchidModGuardianProjectile
	{
		public bool Ding = false;
		public bool NeedNetUpdate = false;
		public int SelectedItem { get; set; } = -1;
		public Item RuneItem => Main.player[Projectile.owner].inventory[SelectedItem];

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
			overPlayers.Add(index); // Display the rune over the player if it is being held
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
			guardian.GuardianRuneCharge = 0;
			if (owner.inventory[owner.selectedItem].ModItem is OrchidModGuardianRune) SelectedItem = owner.selectedItem;
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if ((SelectedItem < 0 || RuneItem.ModItem is not OrchidModGuardianRune || !owner.active || owner.dead || owner.HeldItem.ModItem is not OrchidModGuardianRune) && IsLocalOwner)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				if (IsLocalOwner)
				{
					if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
					else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
					if (NeedNetUpdate)
					{
						NeedNetUpdate = false;
						Projectile.netUpdate = true;
					}
				}
				else if (Projectile.ai[0] == 0f)
				{ // Adresses a visual issue
					guardian.GuardianRuneCharge = 0;
				}

				Projectile.timeLeft = 5;

				if (RuneItem.ModItem is OrchidModGuardianRune guardianItem)
				{
					if (Projectile.ai[0] == 1f)
					{ // Being charged by the player
						if (guardian.GuardianRuneCharge <= 180f)
						{
							guardian.GuardianRuneCharge += 10f / guardianItem.Item.useTime * owner.GetAttackSpeed(DamageClass.Melee); // Very slow charge time compared to other items
							if (guardian.GuardianRuneCharge > 180f)
							{
								guardian.GuardianRuneCharge = 180f;
								Projectile.localAI[0] += 0.015f; // used for full charge glow
								if (Projectile.localAI[0] > 1f) Projectile.localAI[0] = 1f;
							}
							else Projectile.localAI[0] = 0f;
						}

						if (guardian.GuardianRuneCharge >= 180f && !Ding && IsLocalOwner)
						{
							Ding = true;
							SoundEngine.PlaySound(SoundID.MaxMana, owner.Center);
						}

						if (!owner.controlUseItem && IsLocalOwner)
						{
							bool fullyCharged = guardian.GuardianRuneCharge >= 180f;
							if (guardian.GuardianSlam >= guardianItem.RuneCost)
							{
								SoundEngine.PlaySound(guardianItem.Item.UseSound, owner.Center);
								guardian.GuardianSlam -= guardianItem.RuneCost;

								foreach (Projectile projectile in Main.projectile)
								{
									if (projectile.ModProjectile is GuardianRuneProjectile && projectile.owner == owner.whoAmI)
									{
										projectile.Kill();
									}
								}
								int crit = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + RuneItem.crit);
								int runeAmount = guardianItem.GetAmount(guardian);
								if (fullyCharged)
								{
									runeAmount++;
									CombatText.NewText(owner.Hitbox, new Color(175, 255, 175), "Charged", false);
								}

								guardianItem.Activate(owner, guardian, RuneItem.shoot, guardian.GetGuardianDamage(RuneItem.damage), RuneItem.knockBack, crit, (int)(guardianItem.RuneDuration * guardian.GuardianRuneTimer), guardianItem.RuneDistance, runeAmount);
							}
							else
							{
								CombatText.NewText(owner.Hitbox, new Color(125, 205, 125), "Cannot use", false, true);
							}

							guardian.GuardianRuneCharge = 0;
							Projectile.ai[0] = 0f;
							Projectile.netUpdate = true;
						}

						owner.itemAnimation = 1;
						owner.heldProj = Projectile.whoAmI;
						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(14f * owner.direction, -(2 + guardian.GuardianRuneCharge * 0.05f));
						Projectile.rotation = owner.direction * 0.4f - (owner.direction * 0.0025f) * guardian.GuardianRuneCharge;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(0.6f + guardian.GuardianRuneCharge * 0.0025f) * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -(1f + guardian.GuardianRuneCharge * 0.0025f) * owner.direction);
					}
					else
					{ // Idle - rune is held further and lower
						Ding = false;

						Projectile.Center = owner.MountedCenter.Floor() + new Vector2(16f * owner.direction, 4);
						Projectile.rotation = owner.direction * 0.5f;

						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -0.8f * owner.direction);
					}

					Projectile.spriteDirection = owner.direction;
					guardianItem.ExtraAIRune(Projectile);
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanCutTiles() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (RuneItem.ModItem is not OrchidModGuardianRune guardianItem) return false;
			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawRune(spriteBatch, Projectile, player, ref color))
			{
				var texture = TextureAssets.Item[RuneItem.type].Value;

				var drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				if (player.GetModPlayer<OrchidGuardian>().GuardianRuneCharge >= 180f) // max charge
				{
					spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
					spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });
					spriteBatch.Draw(texture, drawPosition, null, Color.White * Projectile.localAI[0], Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.9f, effect, 0f);
					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				spriteBatch.Draw(texture, drawPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.8f, effect, 0f);

			}
			guardianItem.PostDrawRune(spriteBatch, Projectile, player, color);

			return false;
		}
	}
}