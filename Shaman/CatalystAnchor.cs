using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman
{
	public class CatalystAnchor : OrchidModProjectile
	{
		public int SelectedItem { get; set; } = -1;
		public Item CatalystItem => Main.player[Projectile.owner].inventory[this.SelectedItem];

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
			Projectile.netUpdate = true;
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

			var item = this.CatalystItem;
			if (item == null || !(item.ModItem is OrchidModShamanItem shamanItem))
			{
				Projectile.Kill();
				return;
			}

			if (this.SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModShamanItem))
			{
				Projectile.netUpdate = true;
				death = true;
			}

			shamanItem.ExtraAICatalyst(Projectile, false);

			if (!death)
			{			
				if (shamanItem.PreAICatalyst(Projectile))
				{
					switch (shamanItem.catalystType)
					{
						case ShamanCatalystType.IDLE:
							Projectile.rotation = Projectile.velocity.X * 0.035f;
							Projectile.rotation = Projectile.rotation > 0.35f ? 0.35f : Projectile.rotation;
							Projectile.rotation = Projectile.rotation < -0.35f ? -0.35f : Projectile.rotation;
							break;
						case ShamanCatalystType.AIM:
							// Vector2 aimVector = mousePosition - projectile.Center;
							// projectile.rotation = aimVector.ToRotation();
							// projectile.direction = projectile.spriteDirection;
							break;
						case ShamanCatalystType.ROTATE:
							Projectile.rotation += 0.05f;
							break;
					}
				}
				shamanItem.PostAICatalyst(Projectile);

				// ...

				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 mousePosition = Main.MouseWorld;
					Vector2 playerCenter = owner.Center; // mountedCenter

					if (owner.itemAnimation <= 1) // Not 0
					{
						Projectile.ai[0] = (mousePosition - Projectile.Center).ToRotation();
					}

					int mouseDir = mousePosition.X < playerCenter.X ? -1 : 1;
					int mouseUnderValid = mousePosition.Y > playerCenter.Y + 30 && Collision.CanHit(playerCenter, 0, 0, playerCenter + (mousePosition - playerCenter), 0, 0) ? 2 : 0;
					bool tooFar = mousePosition.X < playerCenter.X - 500 || mousePosition.X > playerCenter.X + 500;

					if (mousePosition.X < playerCenter.X + 50 && mousePosition.X > playerCenter.X - 50)
					{
						Projectile.netUpdate = Projectile.ai[1] != (0f + mouseUnderValid * 2); ;
						Projectile.ai[1] = (0f + mouseUnderValid * 2);
					}
					else if ((mousePosition.Y < playerCenter.Y - 50 || mouseUnderValid != 0) && !tooFar)
					{
						Projectile.netUpdate = Projectile.ai[1] != (1f + mouseUnderValid) * mouseDir;
						Projectile.ai[1] = (1f + mouseUnderValid) * mouseDir;
					}
					else
					{
						Projectile.netUpdate = Projectile.ai[1] != 2f * mouseDir;
						Projectile.ai[1] = 2f * mouseDir;
					}
				}

				Vector2 angleVector = new Vector2(0f, -60f).RotatedBy(MathHelper.ToRadians(45 * Projectile.ai[1]));
				angleVector.X *= 0.8f;
				angleVector.Y += 10;
				Vector2 aimedLocation = owner.position + angleVector;

				Vector2 newMove = aimedLocation - Projectile.position;
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo > 1000f)
				{
					Projectile.position = aimedLocation;
					Projectile.netUpdate = true;
				}
				else if (distanceTo > 0.01f)
				{
					newMove.Normalize();
					float vel = ((distanceTo * 0.075f) + (owner.velocity.Length() / 2)) * (owner.HasBuff(ModContent.BuffType<Shaman.Buffs.ShamanicEmpowerment>()) ? 1.5f : 1f);
					vel = vel > 50f ? 50f : vel;
					newMove *= vel;
					Projectile.velocity = newMove;
				}
				else
				{
					if (Projectile.velocity.Length() > 0f)
					{
						Projectile.velocity *= 0f;
					}
				}

				Projectile.timeLeft = 30;
			}
			else
			{
				Projectile.velocity *= 0.95f;
			}

			shamanItem.ExtraAICatalyst(Projectile, true);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanCutTiles() => false;
		public override bool? CanDamage()/* Suggestion: Return null instead of false */ => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(this.CatalystItem.ModItem is OrchidModShamanItem shamanItem)) return false;
			if (!ModContent.HasAsset(shamanItem.CatalystTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (shamanItem.PreDrawCatalyst(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.GetTexture(shamanItem.CatalystTexture);
				var position = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				spriteBatch.Draw(texture, position, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			shamanItem.PostDrawCatalyst(spriteBatch, Projectile, player, color);

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