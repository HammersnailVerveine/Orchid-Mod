using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman
{
	public class CatalystSummon : OrchidModProjectile
	{
		public int SelectedItem { get; set; } = -1;
		public Item CatalystItem;
		public int TimeSpent = 0;

		public override void AltSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			TimeSpent++;
			var owner = Main.player[Projectile.owner];

			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}

			if (CatalystItem == null && SelectedItem != -1)
			{
				CatalystItem = new Item();
				CatalystItem.SetDefaults(SelectedItem, true);
			}
			
			if (CatalystItem.ModItem is OrchidModShamanItem shamanItem)
			{
				shamanItem.ExtraAICatalyst(Projectile, false);
				if (shamanItem.PreAICatalyst(Projectile))
				{
					switch (shamanItem.catalystType)
					{
						case ShamanCatalystType.IDLE:
							Projectile.rotation = Projectile.velocity.X * 0.035f;
							Projectile.rotation = Projectile.rotation > 0.35f ? 0.35f : Projectile.rotation;
							Projectile.rotation = Projectile.rotation < -0.35f ? -0.35f : Projectile.rotation;
							break;
						case ShamanCatalystType.ROTATE:
							Projectile.rotation += 0.05f;
							break;
					}
				}

				Vector2 targetPosition = owner.Center;

				switch (shamanItem.catalystMovement)
				{
					case ShamanSummonMovement.CUSTOM:
						break;
					case ShamanSummonMovement.TOWARDSTARGET:
						break;
					case ShamanSummonMovement.FLOATABOVE:
						targetPosition = owner.Center - new Vector2(0f, 100f).RotatedBy(MathHelper.ToRadians(-60f + ((int)shamanItem.Element * 20f)));
						break;
					default:
						break;
				}

				Vector2 target = owner.Center;
				Vector2 offSet = (targetPosition - owner.Center) / 10f;

				for (int i = 0; i < 10; i++)
				{
					offSet = Collision.TileCollision(target, offSet, 5, 5, true, true, (int)owner.gravDir);
					target += offSet;
				}

				Vector2 newMove = target - Projectile.Center;
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo > 1000f)
				{
					Projectile.position = target + new Vector2(Projectile.width / 2, Projectile.height / 2);
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
					Projectile.velocity *= 0f;
				}

				Projectile.timeLeft = 30;
				shamanItem.PostAICatalyst(Projectile);
				shamanItem.CatalystSummonAI(Projectile, TimeSpent);

				if (!owner.GetModPlayer<OrchidShaman>().IsShamanicBondReleased(shamanItem.Element))
				{
					shamanItem.CatalystSummonKill(Projectile, TimeSpent);
					Projectile.Kill();
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
		public override bool? CanDamage() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(CatalystItem.ModItem is OrchidModShamanItem shamanItem)) return false;
			if (!ModContent.HasAsset(shamanItem.CatalystTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (shamanItem.PreDrawCatalyst(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(shamanItem.CatalystTexture).Value;
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