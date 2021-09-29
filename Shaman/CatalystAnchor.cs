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
		public Item CatalystItem => Main.player[projectile.owner].inventory[this.SelectedItem];

		// ...

		public override void AltSetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.alpha = 255;
		}

		public override void OnSpawn()
		{
			var owner = Main.player[projectile.owner];

			this.SelectedItem = owner.selectedItem;
		}

		public void OnChangeSelectedItem()
		{
			// ...
		}

		public override void AI()
		{
			var owner = Main.player[projectile.owner];
			var shaman = owner.GetOrchidPlayer();
			var death = false;

			if (!owner.active || owner.dead)
			{
				projectile.Kill();
				return;
			}

			var item = this.CatalystItem;
			if (item == null || !(item.modItem is OrchidModShamanItem shamanItem))
			{
				return;
			}

			if (this.SelectedItem < 0 || !(owner.HeldItem.modItem is OrchidModShamanItem))
			{
				projectile.netUpdate = true;
				death = true;
			}

			shamanItem.ExtraAICatalyst(projectile, false);

			if (!death)
			{			
				if (shamanItem.PreAICatalyst(projectile))
				{
					switch (shamanItem.catalystType)
					{
						case ShamanCatalystType.IDLE:
							projectile.rotation = projectile.velocity.X * 0.035f;
							projectile.rotation = projectile.rotation > 0.35f ? 0.35f : projectile.rotation;
							projectile.rotation = projectile.rotation < -0.35f ? -0.35f : projectile.rotation;
							break;
						case ShamanCatalystType.AIM:
							// Vector2 aimVector = mousePosition - projectile.Center;
							// projectile.rotation = aimVector.ToRotation();
							// projectile.direction = projectile.spriteDirection;
							break;
						case ShamanCatalystType.ROTATE:
							projectile.rotation += 0.05f;
							break;
					}
				}
				shamanItem.PostAICatalyst(projectile);

				// ...

				if (Main.myPlayer == projectile.owner)
				{
					Vector2 mousePosition = Main.MouseWorld;
					Vector2 playerCenter = owner.Center; // mountedCenter

					if (owner.itemAnimation <= 1) // Not 0
					{
						projectile.ai[0] = (mousePosition - projectile.Center).ToRotation();
					}

					int mouseDir = mousePosition.X < playerCenter.X ? -1 : 1;
					int mouseUnderValid = mousePosition.Y > playerCenter.Y + 30 && Collision.CanHit(playerCenter, 0, 0, playerCenter + (mousePosition - playerCenter), 0, 0) ? 2 : 0;
					bool tooFar = mousePosition.X < playerCenter.X - 500 || mousePosition.X > playerCenter.X + 500;

					if (mousePosition.X < playerCenter.X + 50 && mousePosition.X > playerCenter.X - 50)
					{
						projectile.netUpdate = projectile.ai[1] != (0f + mouseUnderValid * 2); ;
						projectile.ai[1] = (0f + mouseUnderValid * 2);
					}
					else if ((mousePosition.Y < playerCenter.Y - 50 || mouseUnderValid != 0) && !tooFar)
					{
						projectile.netUpdate = projectile.ai[1] != (1f + mouseUnderValid) * mouseDir;
						projectile.ai[1] = (1f + mouseUnderValid) * mouseDir;
					}
					else
					{
						projectile.netUpdate = projectile.ai[1] != 2f * mouseDir;
						projectile.ai[1] = 2f * mouseDir;
					}
				}

				Vector2 angleVector = new Vector2(0f, -60f).RotatedBy(MathHelper.ToRadians(45 * projectile.ai[1]));
				angleVector.X *= 0.8f;
				angleVector.Y += 10;
				Vector2 aimedLocation = owner.position + angleVector;

				Vector2 newMove = aimedLocation - projectile.position;
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo > 1000f)
				{
					projectile.position = aimedLocation;
					projectile.netUpdate = true;
				}
				else if (distanceTo > 0.01f)
				{
					newMove.Normalize();
					float vel = ((distanceTo * 0.075f) + (owner.velocity.Length() / 2)) * (owner.HasBuff(ModContent.BuffType<Shaman.Buffs.ShamanicEmpowerment>()) ? 1.5f : 1f);
					vel = vel > 50f ? 50f : vel;
					newMove *= vel;
					projectile.velocity = newMove;
				}
				else
				{
					if (projectile.velocity.Length() > 0f)
					{
						projectile.velocity *= 0f;
					}
				}

				projectile.timeLeft = 30;
			}
			else
			{
				projectile.velocity *= 0.95f;
			}

			shamanItem.ExtraAICatalyst(projectile, true);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanCutTiles() => false;
		public override bool CanDamage() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(this.CatalystItem.modItem is OrchidModShamanItem shamanItem)) return false;
			if (!ModContent.TextureExists(shamanItem.CatalystTexture)) return false;

			var player = Main.player[projectile.owner];
			var color = Lighting.GetColor((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), Color.White);

			if (shamanItem.PreDrawCatalyst(spriteBatch, projectile, player, ref color))
			{
				var texture = ModContent.GetTexture(shamanItem.CatalystTexture);
				var position = projectile.Center - Main.screenPosition + Vector2.UnitY * projectile.gfxOffY;
				var effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				spriteBatch.Draw(texture, position, null, color, projectile.rotation, texture.Size() * 0.5f, projectile.scale, effect, 0f);
			}
			shamanItem.PostDrawCatalyst(spriteBatch, projectile, player, color);

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