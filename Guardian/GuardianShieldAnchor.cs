using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Guardian
{
	public class GuardianShieldAnchor : OrchidModProjectile
	{
		public int SelectedItem { get; set; } = -1;
		public Item ShieldItem => Main.player[projectile.owner].inventory[this.SelectedItem];

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

		public void OnChangeSelectedItem(Player owner)
		{
			this.SelectedItem = owner.selectedItem;
			projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Main.player[projectile.owner];
			var death = false;

			if (!owner.active || owner.dead)
			{
				projectile.Kill();
				return;
			}

			var item = this.ShieldItem;
			if (item == null || !(item.modItem is OrchidModGuardianShield guardianItem))
			{
				projectile.Kill();
				return;
			}

			if (this.SelectedItem < 0 || !(owner.HeldItem.modItem is OrchidModGuardianShield))
			{
				projectile.netUpdate = true;
				death = true;
			}
			
			if (!death)
			{
				Vector2 aimedLocation = Main.MouseWorld - owner.Center;
				aimedLocation.Normalize();
				aimedLocation *= guardianItem.distance * - 10f;
				
				projectile.rotation = aimedLocation.ToRotation();
				projectile.direction = projectile.spriteDirection;
				
				aimedLocation = owner.Center - aimedLocation - new Vector2(projectile.width / 2f, projectile.height / 2f); 
				projectile.position = aimedLocation;
					
				projectile.timeLeft = 30;
			}
			
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.hostile && projectile.Hitbox.Intersects(proj.Hitbox))
				{
					guardianItem.Block(owner, projectile, proj);
				}
			}
			
			guardianItem.ExtraAIShield(projectile, true);
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
			if (!(this.ShieldItem.modItem is OrchidModGuardianShield guardianItem)) return false;
			if (!ModContent.TextureExists(guardianItem.ShieldTexture)) return false;

			var player = Main.player[projectile.owner];
			var color = Lighting.GetColor((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawShield(spriteBatch, projectile, player, ref color))
			{
				var texture = ModContent.GetTexture(guardianItem.ShieldTexture);
				var position = projectile.Center - Main.screenPosition + Vector2.UnitY * projectile.gfxOffY;
				var effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				
				projectile.width = texture.Width;
				projectile.height = texture.Height;

				spriteBatch.Draw(texture, position, null, color, projectile.rotation, texture.Size() * 0.5f, projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawShield(spriteBatch, projectile, player, color);

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