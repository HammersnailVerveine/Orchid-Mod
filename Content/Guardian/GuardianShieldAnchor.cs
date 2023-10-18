using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GuardianShieldAnchor : OrchidModProjectile
	{
		public int SelectedItem { get; set; } = -1;
		public Item ShieldItem => Main.player[Projectile.owner].inventory[this.SelectedItem];
		
		public float bashDistanceRef = 0f;
		public bool shieldEffectReady = true;
		public Vector2 aimedLocation = Vector2.Zero;
		public Vector2 oldOwnerPos = Vector2.Zero;
		
		public Vector2 hitbox = Vector2.Zero;
		public Vector2 hitboxOrigin = Vector2.Zero;

		public Vector2 networkedPosition = Vector2.Zero;

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
		}

		public void OnChangeSelectedItem(Player owner)
		{
			this.SelectedItem = owner.selectedItem;
			Projectile.ai[0] = 0f;
			Projectile.ai[1] = 0f;
			Projectile.netUpdate = true;
		}

		public override void OnSpawn(IEntitySource source)
		{
			networkedPosition = Main.player[Projectile.owner].Center;
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

			var item = this.ShieldItem;
			if (item == null || !(item.ModItem is OrchidModGuardianShield guardianItem))
			{
				Projectile.Kill();
				return;
			}

			if (this.SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModGuardianShield))
			{
				Projectile.netUpdate = true;
				death = true;
			}
			
			if (!death)
			{
				float addedDistance = 0f;
				if (Projectile.ai[1] > 0f) { // Shield bash
				
					if (bashDistanceRef == 0f) {
						bashDistanceRef = Projectile.ai[1] / 2;
						Projectile.damage = (int)owner.GetDamage<GuardianDamageClass>().ApplyTo(guardianItem.Item.damage);
						Projectile.CritChance = (int)owner.GetCritChance<GuardianDamageClass>() +guardianItem.Item.crit;
						Projectile.knockBack = guardianItem.Item.knockBack;
						Projectile.friendly = true;
						guardianItem.Slam(owner, Projectile);
					}
					
					for (int k = 0; k < Main.maxNPCs ; k++)
					{
						NPC target = Main.npc[k];
						if (target.active && !target.dontTakeDamage && !target.friendly && Projectile.Hitbox.Intersects(target.Hitbox))
						{
							guardianItem.SlamHit(owner, Projectile, target);
							if (this.shieldEffectReady) {
								guardianItem.SlamHitFirst(owner, Projectile, target);
								this.shieldEffectReady = false;
							}
						}
					}
					
					if (Projectile.ai[1] > bashDistanceRef) {
						addedDistance = bashDistanceRef * 2 - Projectile.ai[1];
					} else {
						addedDistance = Projectile.ai[1];
					}
					
					Projectile.ai[1] -= (bashDistanceRef * 2) / guardianItem.Item.useTime;
					Projectile.ai[1] = Projectile.ai[1] > 0f ? Projectile.ai[1] : 0f;
				} else {
					bashDistanceRef = 0f;
					Projectile.friendly = false;
				}
				
				
			
				if (Projectile.ai[0] > 0f) {
					aimedLocation += owner.Center - oldOwnerPos;
					Point p1 = new Point((int)this.hitboxOrigin.X, (int)this.hitboxOrigin.Y);
					Point p2 = new Point((int)(this.hitboxOrigin.X + this.hitbox.X), (int)(this.hitboxOrigin.Y + this.hitbox.Y));

					OrchidGuardian modPlayer = owner.GetModPlayer<OrchidGuardian>();
					modPlayer.guardianSlamRecharge = (int)(OrchidGuardian.guardianRechargeTime * modPlayer.guardianRecharge);
					
					for (int l = 0; l < Main.projectile.Length; l++)
					{
						Projectile proj = Main.projectile[l];
						if (proj.active && proj.hostile && this.LineIntersectsRect(p1, p2, proj.Hitbox))
						{
							guardianItem.Block(owner, Projectile, proj);
							if (this.shieldEffectReady) {
								guardianItem.Protect(owner, Projectile);
								this.shieldEffectReady = false;
							}
						}
					}
					
					for (int k = 0; k < Main.maxNPCs ; k++)
					{
						NPC target = Main.npc[k];
						if (target.active && !target.dontTakeDamage && !target.friendly && this.LineIntersectsRect(p2, p1, target.Hitbox))
						{
							//int blockDuration = projectile.ai[0] > 60f ? (int)projectile.ai[0] : 60;
							modPlayer.guardianBlockedEnemies.Add(new BlockedEnemy(target, (int)Projectile.ai[0] + 60));
							
							if (target.knockBackResist > 0f) 
							{								
								Vector2 push = Projectile.Center - owner.Center;
								push.Normalize();
								push += owner.Center - oldOwnerPos;
								target.velocity = push;
							}

							guardianItem.Push(owner, Projectile, target);
							if (this.shieldEffectReady)
							{
								modPlayer.AddSlam(1);
								guardianItem.Protect(owner, Projectile);
								this.shieldEffectReady = false;
							}
						}
					}
					
					Projectile.ai[0] -= 1f;
					Projectile.ai[0] = Projectile.ai[0] > 0f ? Projectile.ai[0] : 0f;
					
					if (Projectile.ai[0] == 0f) {
						this.spawnDusts();
					}
				} 
				else
				{
					if (Main.myPlayer == Projectile.owner)
					{
						aimedLocation = Main.MouseWorld - owner.Center;
						aimedLocation.Normalize();
						Projectile.velocity = aimedLocation;
						aimedLocation *= (guardianItem.distance + addedDistance) * -1f;

						Projectile.rotation = aimedLocation.ToRotation();
						Projectile.direction = Projectile.spriteDirection;

						aimedLocation = owner.Center - aimedLocation - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

						if (networkedPosition.Distance(aimedLocation) > 5f && Projectile.ai[1] <= 0f)
						{
							networkedPosition = aimedLocation;
							Projectile.netUpdate = true;
						}
					}
					else
					{
						aimedLocation = Projectile.position;
					}
				}

				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.position = aimedLocation;
				}
				else
				{
					Vector2 dir = owner.Center - Projectile.Center;
					Projectile.rotation = dir.ToRotation();
					Projectile.direction = Projectile.spriteDirection;
					if (addedDistance > 0f)
					{
						dir.Normalize();
						Projectile.position = networkedPosition + owner.Center - oldOwnerPos + dir * -addedDistance;
					}
					else
					{
						Projectile.position += owner.Center - oldOwnerPos;
						networkedPosition = Projectile.position;
					}
				}

				Projectile.timeLeft = 5;
				Projectile.velocity *= float.Epsilon;

				this.UpdateHitbox();
				//this.SeeHitbox();
			}
			
			this.oldOwnerPos = owner.Center;
			guardianItem.ExtraAIShield(Projectile, true);
		}
		
		// https://stackoverflow.com/questions/5514366/how-to-know-if-a-line-intersects-a-rectangle
		// Laziness kicking in, might redo with Cohen-Sutherland algorithm
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

			if( d == 0 )
			{
				return false;
			}

			float r = q / d;

			q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
			float s = q / d;

			if( r < 0 || r > 1 || s < 0 || s > 1 )
			{
				return false;
			}

			return true;
		}
		// end of Stackoverflow code :P
		
		public void UpdateHitbox() {
			this.hitboxOrigin = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY;
			this.hitboxOrigin -= new Vector2(0f, Projectile.height / 2f).RotatedBy(Projectile.rotation);
			hitboxOrigin -= new Vector2(4f, 4f);
			
			this.hitbox = new Vector2(0f, Projectile.height).RotatedBy(Projectile.rotation);
		}
		
		public void SeeHitbox() {
			Vector2 vector = this.hitbox;
			vector.Normalize();
			for (int i = 0; i < this.hitbox.Length() ; i ++) {
				Vector2 pos = this.hitboxOrigin + vector * i;
				Dust dust = Main.dust[Dust.NewDust(pos, 0, 0, 6)];
				dust.velocity *= 0f;
				dust.noGravity = true;
			}
		}
		
		public void spawnDusts() {						
			int dustType = 31;
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			for (int i = 0 ; i < 5 ; i ++) {
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

		public override bool? CanCutTiles() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(this.ShieldItem.ModItem is OrchidModGuardianShield guardianItem)) return false;
			if (!ModContent.HasAsset(guardianItem.ShieldTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawShield(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(guardianItem.ShieldTexture).Value;
				var position = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				
				Projectile.width = texture.Width;
				Projectile.height = texture.Height;
				float colorMult = (Projectile.ai[1] + Projectile.ai[0] > 0 ? 1f : (0.4f + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().timer120 - 60) / 120f)));
				spriteBatch.Draw(texture, position, null, color * colorMult, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
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