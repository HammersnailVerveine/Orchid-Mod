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
		public int TimeSpent = 0;
		public bool Ding = false;
		public bool NeedNetUpdate = false;
		public bool hitTarget = false;
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
			Projectile.localNPCHitCooldown = 20;
			Projectile.netImportant = true;
			HitBox = new Rectangle(0, 0, 24, 24);
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
				int length = (int)Math.Sqrt(2 * (QuarterstaffTexture.Width * QuarterstaffTexture.Width));
				Projectile.width = length;
				Projectile.height = length;
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

				if (Projectile.ai[0] == 1f)
				{ // Being charged by the player
					if (guardian.GuardianGauntletCharge < 180f)
					{ // Increase guardian charge
						guardian.GuardianGauntletCharge += 30f / guardianItem.Item.useTime * owner.GetTotalAttackSpeed(DamageClass.Melee);
						if (guardian.GuardianGauntletCharge > 180f) guardian.GuardianGauntletCharge = 180f;
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
							{
								SoundEngine.PlaySound(guardianItem.Item.UseSound, owner.Center);
								guardian.AddGuard(guardianItem.GuardStacks);
								guardian.AddSlam(guardianItem.SlamStacks);

								// TODO INITIATE SLAM
							}

							guardian.GuardianGauntletCharge = 0;
							Projectile.ai[0] = 0f;
							Projectile.netUpdate = true;
						}
						else if (jabInput)
						{
							Projectile.damage = guardian.GetGuardianDamage(QuarterstaffItem.damage);
							Projectile.CritChance = guardian.GetGuardianCrit(QuarterstaffItem.crit);
							Projectile.knockBack = QuarterstaffItem.knockBack;

							Projectile.ai[0] = -40f;
							Projectile.ai[1] = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter).ToRotation() - MathHelper.PiOver2;
							Projectile.netUpdate = true;
							hitTarget = false;
						}
					}

					// Rotate the staff as charge progresses, readying an upward swipe attack

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(12f * owner.direction, guardian.GuardianGauntletCharge * 0.03f);
					Projectile.rotation = MathHelper.PiOver4 * (1f + guardian.GuardianGauntletCharge * 0.0025f) * owner.direction - MathHelper.PiOver4;

					owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -(0.6f - guardian.GuardianGauntletCharge * 0.0025f) * owner.direction);
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -(1f - guardian.GuardianGauntletCharge * 0.0025f) * owner.direction);
				}
				else if (Projectile.ai[0] < 0)
				{ // Jabbing
					if (Projectile.ai[0] == -40f)
					{ // First frame of the jab
						Projectile.friendly = true;
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
						guardianItem.OnAttack(owner, guardian, Projectile, true, false);
					}

					if (Projectile.ai[0] > -30)
					{
						Projectile.friendly = false;
					}

					//Projectile.Center = owner.MountedCenter.Floor() - Vector2.UnitY.RotatedBy(Projectile.ai[1]) * (float)Math.Sin(0.0392f * Projectile.ai[0]) * 32f;
					Projectile.Center = owner.MountedCenter.Floor() - Vector2.UnitY.RotatedBy(Projectile.ai[1]) * (float)Math.Sin(0.052f * Projectile.ai[0]) * 32f;
					if (Projectile.ai[0] >= -30)
					{
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + (float)Math.Sin(0.1046f * (30 + Projectile.ai[0])) * 0.4f * -owner.direction + MathHelper.Pi;
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * (38f - (float)Math.Sin(0.0523f * (30 + Projectile.ai[0])) * 24f);
						Projectile.position.Y -= (float)Math.Sin(0.0523f * (30 + Projectile.ai[0])) * 2f;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f + (float)Math.Sin(0.1046f * (30 + Projectile.ai[0])) * 0.3f * owner.direction);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f + (float)Math.Sin(0.1046f * (30 + Projectile.ai[0])) * 0.2f * owner.direction);
					}
					else
					{
						Projectile.Center = owner.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(Projectile.ai[1]) * 3.8f * (Projectile.ai[0] + 40);
						Projectile.rotation = Projectile.ai[1] - MathHelper.PiOver4 + MathHelper.Pi;
						owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * owner.direction + Projectile.ai[1] + 0.1f);
						owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, Projectile.ai[1] - 0.1f);
					}

					Projectile.ai[0] += 1f * owner.GetTotalAttackSpeed(DamageClass.Melee);

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
				else
				{ // Idle - guarterstaff is held further and lower
					Ding = false;

					Projectile.Center = owner.MountedCenter.Floor() + new Vector2(12f * owner.direction, 0f);
					Projectile.rotation = MathHelper.PiOver4 * owner.direction - MathHelper.PiOver4;

					owner.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -0.7f * owner.direction);
					owner.SetCompositeArmBack(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver2 * -1.2f * owner.direction);
				}

				// Hitbox management for jabs and swings
				Vector2 position = Projectile.Center - Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver4) * (Projectile.width - 12) * 0.5f;
				HitBox.X = (int)position.X - 12;
				HitBox.Y = (int)position.Y - 12;

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

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.Hitbox.Intersects(HitBox)) return base.CanHitNPC(target);
			return false;
		}

		public override bool? CanCutTiles() => false; // TODO : can cut while attacking

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (SelectedItem < 0 || SelectedItem > 58 || QuarterstaffTexture== null) return false;
			if (QuarterstaffItem.ModItem is not OrchidModGuardianQuarterstaff guardianItem) return false;

			var player = Main.player[Projectile.owner];

			if (guardianItem.PreDrawQuarterstaff(spriteBatch, Projectile, player, ref lightColor))
			{
				float drawRotation = Projectile.rotation;
				Vector2 posproj = Projectile.Center;

				if (player.gravDir == -1)
				{
					drawRotation = -drawRotation + MathHelper.PiOver2;
					posproj.Y = (player.Bottom.Floor() + player.position.Floor()).Y - posproj.Y + (posproj.Y - player.Center.Floor().Y) * 2f;
				}

				var drawPosition = Vector2.Transform(posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);

				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				spriteBatch.Draw(QuarterstaffTexture, drawPosition, null, lightColor, drawRotation, QuarterstaffTexture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawQuarterstaff(spriteBatch, Projectile, player, lightColor);

			return false;
		}
	}
}