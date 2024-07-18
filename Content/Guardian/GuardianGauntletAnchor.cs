using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
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
	public class GuardianGauntletAnchor : OrchidModProjectile
	{
		public int TimeSpent = 0;
		public int LockedOwnerDir = 0;
		public bool OffHandGauntlet = false;
		public bool Ding = false;

		public int SelectedItem { get; set; } = -1;
		public Item GauntletItem => Main.player[Projectile.owner].inventory[this.SelectedItem];
		public bool Blocking => Projectile.ai[0] > 0 && !Charging;
		public bool Slamming => Projectile.ai[0] < 0;
		public bool Charging => Projectile.ai[2] > 0;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (!OffHandGauntlet || Blocking) overPlayers.Add(index);
		}

		// ...

		public override void AltSetDefaults()
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
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
			writer.Write(OffHandGauntlet);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
			OffHandGauntlet = reader.ReadBoolean();
		}

		public void OnChangeSelectedItem(Player owner)
		{
			SelectedItem = owner.selectedItem;
			Projectile.netUpdate = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			var owner = Main.player[Projectile.owner];
			var item = GauntletItem;
			if (item == null || !(item.ModItem is OrchidModGuardianGauntlet guardianItem))
			{
				Projectile.Kill();
				return;
			}
		}

		public override void AI()
		{
			TimeSpent++;
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if (!owner.active || owner.dead || SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModGuardianGauntlet) || GauntletItem == null || GauntletItem.ModItem is not OrchidModGuardianGauntlet guardianItem)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				Projectile.timeLeft = 5;
				if (OffHandGauntlet) // Offhand is always loaded first; no need to do that twice
				{
					if (Main.projectile[guardianItem.GetAnchors(owner)[1]].ai[0] >= 0)
					{ // Lock the player direction while slamming
						if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
						else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);
						LockedOwnerDir = owner.direction;
					}
					else owner.direction = LockedOwnerDir;
				}

				if (Blocking)
				{
					guardian.GuardianGauntletParry = true;
					guardian.GuardianGauntletParry2 = true;

					Projectile.Center = owner.Center.Floor() + new Vector2(4 * owner.direction, 0);
					if (OffHandGauntlet) Projectile.position.X += 6 * owner.direction;
					Projectile.rotation = 0f;

					Projectile.ai[0]--;
					if (Projectile.ai[0] <= 0f || owner.immune)
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
					Projectile.Center = owner.Center.Floor() + new Vector2(4 * owner.direction, 0) + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * addedDistance;

					if (Projectile.localAI[1] == slamTime)
					{ // Slam just started, make projectile
						Ding = false; // Also reset ding song for full charge
						if (guardianItem.OnPunch(owner, guardian, Projectile, Projectile.ai[0] == -2f))
						{
							int projectileType = ModContent.ProjectileType<GauntletPunchProjectile>();
							float strikeVelocity = guardianItem.strikeVelocity * (Projectile.ai[0] == -1f ? 0.75f : 1f) * guardianItem.Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetSlamDistance();
							Projectile punchProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedBy((Main.MouseWorld - owner.Center).ToRotation() - MathHelper.PiOver2) * strikeVelocity, projectileType, 1, 1f, owner.whoAmI, Projectile.ai[0] == -1f ? 0f : 1f, OffHandGauntlet ? 1f : 0f);
							if (punchProj.ModProjectile is GauntletPunchProjectile punch)
							{
								punch.SelectedItem = SelectedItem;
								punchProj.damage = (int)owner.GetDamage<GuardianDamageClass>().ApplyTo(guardianItem.Item.damage);
								punchProj.CritChance = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + guardianItem.Item.crit);
								punchProj.knockBack = guardianItem.Item.knockBack;
								punchProj.position += punchProj.velocity * 0.5f;
								punchProj.rotation = punchProj.velocity.ToRotation();
								punchProj.velocity += owner.velocity * 1.5f;

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
					}
				}
				else
				{
					if (Charging)
					{
						guardian.GuardianGauntletCharge += 30f / GauntletItem.useTime * (owner.GetAttackSpeed(DamageClass.Melee) * 2f - 1f);
						if (guardian.GuardianGauntletCharge > 180f)
						{
							if (!Ding)
							{
								SoundEngine.PlaySound(SoundID.MaxMana, owner.Center);
								Ding = true;
							}
							guardian.GuardianGauntletCharge = 180f;
						}
						else if (guardian.GuardianGauntletCharge > 8f) guardian.SlamCostUI = 1;

						if (!owner.controlUseItem && owner.whoAmI == Main.myPlayer)
						{
							if (guardian.GuardianGauntletCharge < 180f && guardian.GuardianSlam > 0)
							{ // Consume a slam to fully charge if the player has one
								guardian.GuardianSlam--;
								guardian.GuardianGauntletCharge = 180f;
							}

							if (guardian.GuardianGauntletCharge >= 180f) Projectile.ai[0] = -2f;
							else Projectile.ai[0] = -1f;

							guardian.GuardianGauntletCharge = 0;
							Projectile.ai[1] = Vector2.Normalize(Main.MouseWorld - owner.Center).ToRotation() - MathHelper.PiOver2;
							Projectile.ai[2] = 0f;
							Projectile.netUpdate = true;
							//owner.itemAnimation = (int)Projectile.ai[0];
						}
						else
						{
							Projectile.Center = owner.Center.Floor() + new Vector2(-(4 + guardian.GuardianGauntletCharge * 0.033f) * owner.direction, 4);
							Projectile.rotation = MathHelper.PiOver2;
						}
					}
					else
					{
						Projectile.Center = owner.Center.Floor() + new Vector2((-6 + guardian.GuardianGauntletCharge * 0.01f) * owner.direction, 6);
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

				if (OffHandGauntlet)
				{
					//owner.handoff = -1;
					/*
					float rotation = (Projectile.Center + new Vector2(4 * owner.direction, Slamming ? 2 : 6) - owner.Center.Floor()).ToRotation();
					CompositeArmStretchAmount compositeArmStretchAmount = CompositeArmStretchAmount.Quarter; // Tweak the arm based on punch direction if necessary
					if (Projectile.localAI[1] > 0.55f && (Projectile.ai[1] > -2.25f || Projectile.ai[1] < -4f)) compositeArmStretchAmount = CompositeArmStretchAmount.ThreeQuarters;
					owner.SetCompositeArmBack(true, compositeArmStretchAmount, rotation - MathHelper.PiOver2);
					*/
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, owner.direction == 1 ? MathHelper.PiOver2 : -MathHelper.Pi);
				}
				else
				{
					//owner.handon = -1;
					float rotation = (Projectile.Center + new Vector2(6 * owner.direction, Slamming ? 2 : Charging ? 8 : 6) - owner.Center.Floor()).ToRotation();
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

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(GauntletItem.ModItem is OrchidModGuardianGauntlet guardianItem)) return false;
			if (!ModContent.HasAsset(guardianItem.GauntletTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawGauntlet(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(guardianItem.GauntletTexture).Value;
				var drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				float rotation = Projectile.rotation;

				var effect = SpriteEffects.None;
				if (player.direction != 1)
				{
					if (player.velocity.X != 0 && !Blocking || (player.GetModPlayer<OrchidGuardian>().GuardianGauntletCharge > 0 && Projectile.ai[2] != 0) || Slamming) effect = SpriteEffects.FlipVertically;
					else effect = SpriteEffects.FlipHorizontally;
				}

				spriteBatch.Draw(texture, drawPosition, null, color, rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawGauntlet(spriteBatch, Projectile, player, color);

			return false;
		}
	}
}