using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GuardianStandardAnchor : OrchidModProjectile
	{
		public int SelectedItem { get; set; } = -1;
		public Item StandardItem => Main.player[Projectile.owner].inventory[this.SelectedItem];

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
			Projectile.localNPCHitCooldown = 20;
		}

		public void OnChangeSelectedItem(Player owner)
		{
			SelectedItem = owner.selectedItem;
			Projectile.ai[0] = 0f;
			Projectile.ai[1] = 0f;
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();

			if (!owner.active || owner.dead || SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModGuardianStandard) || StandardItem == null || StandardItem.ModItem is not OrchidModGuardianStandard guardianItem)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				Projectile.timeLeft = 5;
			}

			guardianItem.ExtraAIStandard(Projectile);
		}
		public void spawnDusts()
		{
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(pos, 20, 20, DustID.Smoke)].velocity *= 0.25f;
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
			if (StandardItem.ModItem is not OrchidModGuardianStandard guardianItem) return false;
			if (!ModContent.HasAsset(guardianItem.ShaftTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawStandard(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(guardianItem.ShaftTexture).Value;
				var drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				Projectile.width = texture.Height;
				Projectile.height = texture.Height;
				spriteBatch.Draw(texture, drawPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawStandard(spriteBatch, Projectile, player, color);

			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
		}
	}
}