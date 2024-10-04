using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GuardianShieldAnchor : OrchidModGuardianProjectile
	{
		public int SelectedItem { get; set; } = -1;
		public Item ShieldItem => Main.player[Projectile.owner].inventory[this.SelectedItem];

		public bool shieldEffectReady = true;
		public bool NeedNetUpdate = false;

		public byte isSlamming = 0;
		public Vector2 aimedLocation = Vector2.Zero;
		public Vector2 oldOwnerPos = Vector2.Zero;

		public Vector2 hitbox = Vector2.Zero;
		public Vector2 hitboxOrigin = Vector2.Zero;

		public float networkedRotation => Projectile.ai[2];

		// ...

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
			Projectile.localNPCHitCooldown = 120;
		}

		public void OnChangeSelectedItem(Player owner)
		{
			SelectedItem = owner.selectedItem;
			Projectile.ai[0] = 0f;
			Projectile.ai[1] = 0f;
			Projectile.netUpdate = true;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			var owner = Main.player[Projectile.owner];
			var item = ShieldItem;
			if (item == null || !(item.ModItem is OrchidModGuardianShield guardianItem))
			{
				Projectile.Kill();
				return;
			}

			guardianItem.SlamHit(owner, Projectile, target);
			if (shieldEffectReady)
			{
				guardianItem.SlamHitFirst(owner, Projectile, target);
				shieldEffectReady = false;
			}
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			var death = false;

			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}

			var item = ShieldItem;
			if (item == null || !(item.ModItem is OrchidModGuardianShield guardianItem))
			{
				Projectile.Kill();
				return;
			}

			if (SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModGuardianShield))
			{
				Projectile.netUpdate = true;
				death = true;
			}

			if (!death)
			{
				if (NeedNetUpdate)
				{
					NeedNetUpdate = false;
					Projectile.netUpdate = true;
				}

				float addedDistance = 0f;
				if (Projectile.ai[1] > 0f)
				{ // Shield bash

					if (isSlamming == 0)
					{
						isSlamming = 1;
						Projectile.damage = (int)owner.GetDamage<GuardianDamageClass>().ApplyTo(guardianItem.Item.damage);
						Projectile.CritChance = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + guardianItem.Item.crit);
						Projectile.knockBack = guardianItem.Item.knockBack;
						Projectile.ResetLocalNPCHitImmunity();
						Projectile.friendly = true;
					}

					float slamDistance = (int)(guardianItem.slamDistance * guardianItem.Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetSlamDistance() * owner.GetAttackSpeed(DamageClass.Melee));
					addedDistance = (float)Math.Sin(MathHelper.Pi / guardianItem.Item.useTime * Projectile.ai[1]) * slamDistance;
					Projectile.ai[1] -= slamDistance / guardianItem.Item.useTime;

					if (Projectile.ai[1] <= 0f)
					{
						Projectile.ai[1] = 0f;
						isSlamming = 0;
						Projectile.friendly = false;
					}
				}

				if (Projectile.ai[0] > 0f)
				{
					aimedLocation += owner.Center.Floor() - oldOwnerPos.Floor();
					Point p1 = new Point((int)hitboxOrigin.X, (int)hitboxOrigin.Y);
					Point p2 = new Point((int)(hitboxOrigin.X + hitbox.X), (int)(hitboxOrigin.Y + hitbox.Y));

					OrchidGuardian modPlayer = owner.GetModPlayer<OrchidGuardian>();
					modPlayer.GuardianSlamRecharge = (int)(OrchidGuardian.GuardianRechargeTime * modPlayer.GuardianRecharge);

					for (int l = 0; l < Main.projectile.Length; l++)
					{
						Projectile proj = Main.projectile[l];
						if (proj.active && proj.hostile && proj.damage > 0)
						{
							if (LineIntersectsRect(p1, p2, proj.Hitbox) || proj.Hitbox.Intersects(Projectile.Hitbox))
							{
								guardianItem.Block(owner, Projectile, proj);
								modPlayer.OnBlockProjectile(Projectile, proj);
								if (shieldEffectReady)
								{
									modPlayer.OnBlockProjectileFirst(Projectile, proj);
									guardianItem.Protect(owner, Projectile);
									shieldEffectReady = false;
									SoundEngine.PlaySound(SoundID.Item37, owner.Center);
								}
								proj.Kill();
								SoundEngine.PlaySound(SoundID.Dig, owner.Center);
							}
						}
					}

					for (int k = 0; k < Main.maxNPCs; k++)
					{
						NPC target = Main.npc[k];
						if (target.active && !target.dontTakeDamage && !target.friendly && this.LineIntersectsRect(p2, p1, target.Hitbox))
						{
							bool contained = false;
							foreach(BlockedEnemy blockedEnemy in modPlayer.GuardianBlockedEnemies)
							{
								if (blockedEnemy.npc == target)
								{ // Enemy already blocked, reset the timer
									blockedEnemy.time = (int)Projectile.ai[0] + 60;
									contained = true;
									break;
								}
							}

							if (!contained)
							{ // First time blocking an enemy
								modPlayer.GuardianBlockedEnemies.Add(new BlockedEnemy(target, (int)Projectile.ai[0] + 60));
								SoundEngine.PlaySound(SoundID.Dig, owner.Center);
							}

							if (target.knockBackResist > 0f)
							{ // Push enemy if possible
								Vector2 push = Projectile.Center - owner.Center;
								push.Normalize();
								push += owner.Center - oldOwnerPos;
								target.velocity = push;
							}

							guardianItem.Push(owner, Projectile, target);
							modPlayer.OnBlockNPC(Projectile, target);
							if (shieldEffectReady)
							{ // First parry stuff
								modPlayer.OnBlockNPCFirst(Projectile, target);
								guardianItem.Protect(owner, Projectile);
								shieldEffectReady = false;
								SoundEngine.PlaySound(SoundID.Item37, owner.Center);
							}
						}
					}

					Projectile.ai[0] --;
					if (Projectile.ai[0] <= 0f)
					{
						spawnDusts();
						Projectile.ai[0] = 0f;
					}
				}
				else
				{
					if (Main.myPlayer == Projectile.owner)
					{
						aimedLocation = Main.MouseWorld - owner.Center.Floor();
						aimedLocation.Normalize();
						Projectile.velocity = aimedLocation * float.Epsilon;
						aimedLocation *= (guardianItem.distance + addedDistance) * -1f;

						Projectile.rotation = aimedLocation.ToRotation();
						Projectile.direction = Projectile.spriteDirection;

						aimedLocation = owner.Center.Floor() - aimedLocation - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

						if (Math.Abs(networkedRotation - Projectile.rotation) > 0.025f)
						{
							Projectile.ai[2] = Projectile.rotation; // networked rotation
							Projectile.netUpdate = true;
						}
					}
				}

				if (IsLocalOwner)
				{
					Projectile.position = aimedLocation;
				}
				else
				{
					Projectile.Center = owner.Center.Floor() - networkedRotation.ToRotationVector2() * (guardianItem.distance + addedDistance);
					Projectile.rotation = networkedRotation;
				}

				Projectile.timeLeft = 5;

				if (isSlamming == 1) // Slam() is called here so the projectile has the time to reposition properly before effects such as projectile spawns are called
				{
					isSlamming = 2;
					guardianItem.Slam(owner, Projectile);
				} 

				UpdateHitbox();
				//this.SeeHitbox();
			}

			oldOwnerPos = owner.Center;
			guardianItem.ExtraAIShield(Projectile);
		}

		// https://stackoverflow.com/questions/5514366/how-to-know-if-a-line-intersects-a-rectangle
		public bool LineIntersectsRect(Point p1, Point p2, Rectangle r)
		{
			return LineIntersectsLine(p1, p2, new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y)) ||
				   LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height)) ||
				   LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y + r.Height), new Point(r.X, r.Y + r.Height)) ||
				   LineIntersectsLine(p1, p2, new Point(r.X, r.Y + r.Height), new Point(r.X, r.Y)) ||
				   (r.Contains(p1) && r.Contains(p2));
		}

		private bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
		{
			float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
			float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

			if (d == 0)
			{
				return false;
			}

			float r = q / d;

			q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
			float s = q / d;

			if (r < 0 || r > 1 || s < 0 || s > 1)
			{
				return false;
			}

			return true;
		}
		// end of Stackoverflow code

		public void UpdateHitbox()
		{
			this.hitboxOrigin = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY;
			this.hitboxOrigin -= new Vector2(0f, Projectile.height / 2f).RotatedBy(Projectile.rotation);
			hitboxOrigin -= new Vector2(4f, 4f);

			this.hitbox = new Vector2(0f, Projectile.height).RotatedBy(Projectile.rotation);
		}

		public void SeeHitbox()
		{
			Vector2 vector = this.hitbox;
			vector.Normalize();
			for (int i = 0; i < this.hitbox.Length(); i++)
			{
				Vector2 pos = this.hitboxOrigin + vector * i;
				Dust dust = Main.dust[Dust.NewDust(pos, 0, 0, 6)];
				dust.velocity *= 0f;
				dust.noGravity = true;
			}
		}

		public void spawnDusts()
		{
			int dustType = 31;
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(pos, 20, 20, dustType)].velocity *= 0.25f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanCutTiles() => Projectile.ai[1] > 0f;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(ShieldItem.ModItem is OrchidModGuardianShield guardianItem)) return false;
			if (!ModContent.HasAsset(guardianItem.ShieldTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawShield(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(guardianItem.ShieldTexture).Value;
				var drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				//Projectile.width = texture.Width;
				Projectile.width = texture.Height;
				Projectile.height = texture.Height;
				float colorMult = (Projectile.ai[1] + Projectile.ai[0] > 0 ? 1f : (0.4f + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().Timer120 - 60) / 120f)));
				spriteBatch.Draw(texture, drawPosition, null, color * colorMult, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawShield(spriteBatch, Projectile, player, color);

			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(this.SelectedItem);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			this.SelectedItem = reader.ReadInt32();
		}
	}
}