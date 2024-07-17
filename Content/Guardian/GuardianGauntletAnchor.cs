using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GuardianGauntletAnchor : OrchidModProjectile
	{
		public int TimeSpent = 0;

		public int SelectedItem { get; set; } = -1;
		public Item GauntletItem => Main.player[Projectile.owner].inventory[this.SelectedItem];
		public bool OffHandGauntlet => Projectile.ai[1] == 1;
		public bool Blocking => Projectile.ai[0] == 1;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (!OffHandGauntlet) overPlayers.Add(index);
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

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(SelectedItem);
		public override void ReceiveExtraAI(BinaryReader reader) => SelectedItem = reader.ReadInt32();

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
			var death = false;

			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}

			var item = GauntletItem;
			if (item == null || !(item.ModItem is OrchidModGuardianGauntlet guardianItem))
			{
				Projectile.Kill();
				return;
			}

			if (SelectedItem < 0 || !(owner.HeldItem.ModItem is OrchidModGuardianGauntlet))
			{
				Projectile.netUpdate = true;
				death = true;
			}

			if (!death)
			{
				//Projectile.velocity *= float.Epsilon;

				if (Main.MouseWorld.X > owner.Center.X && owner.direction != 1) owner.ChangeDir(1);
				else if (Main.MouseWorld.X < owner.Center.X && owner.direction != -1) owner.ChangeDir(-1);

				Projectile.timeLeft = 5;
				Projectile.Center = owner.Center.Floor() + new Vector2(-6 * owner.direction, 6);
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

				//owner.itemAnimation = 1;
				Projectile.timeLeft = 600;
				Projectile.spriteDirection = owner.direction;
				//owner.heldProj = Projectile.whoAmI;

				if (OffHandGauntlet)
				{
					float rotation = (Projectile.Center - new Vector2(6 * owner.direction, -6) - owner.Center.Floor()).ToRotation();
					owner.handoff = -1;
					owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.ThreeQuarters, rotation - MathHelper.PiOver2);
				}
				else
				{
					float rotation = (Projectile.Center + new Vector2(6 * owner.direction, 6) - owner.Center.Floor()).ToRotation();
					owner.handon = -1;
					owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.ThreeQuarters, rotation - MathHelper.PiOver2);
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

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!(this.GauntletItem.ModItem is OrchidModGuardianGauntlet guardianItem)) return false;
			if (!ModContent.HasAsset(guardianItem.GauntletTexture)) return false;

			var player = Main.player[Projectile.owner];
			var color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), Color.White);

			if (guardianItem.PreDrawGauntlet(spriteBatch, Projectile, player, ref color))
			{
				var texture = ModContent.Request<Texture2D>(guardianItem.GauntletTexture).Value;
				var drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);

				var effect = SpriteEffects.None;
				if (Projectile.spriteDirection != 1)
				{
					if (player.velocity.X != 0) effect = SpriteEffects.FlipVertically;
					else effect = SpriteEffects.FlipHorizontally;
				}

				spriteBatch.Draw(texture, drawPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0f);
			}
			guardianItem.PostDrawGauntlet(spriteBatch, Projectile, player, color);

			return false;
		}
	}
}