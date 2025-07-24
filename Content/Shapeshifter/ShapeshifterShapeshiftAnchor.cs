using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Accessories;
using OrchidMod.Utilities;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	public class ShapeshifterShapeshiftAnchor : OrchidModShapeshifterProjectile
	{
		public bool NeedNetUpdate = false;
		public bool NeedKill = false;
		public int SelectedItem { get; set; } = -1;
		public Item ShapeshifterItem => Main.player[Projectile.owner].inventory[SelectedItem];

		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public List<int> OldFrame;
		public int Frame = 0;
		public int Timespent = 0;
		public int BlinkEffect = 20; // used to make the wildshape blink when Blink() is called
		public Texture2D TextureShapeshift;
		public Texture2D TextureShapeshiftHair;
		public Texture2D TextureShapeshiftHairGray;
		public Texture2D TextureShapeshiftGlow;
		public Texture2D TextureShapeshiftIcon;
		public Texture2D TextureShapeshiftIconBorder;
		public float[] ai;

		public bool CanLeftClick => LeftCLickCooldown <= 0f;
		public bool CanRightClick => RightCLickCooldown <= 0f;

		public bool IsLeftClick;
		public bool IsLeftClickRelease;
		public bool IsRightClick;
		public bool IsRightClickRelease;
		public bool IsInputLeft;
		public bool IsInputRight;
		public bool IsInputDown;
		public bool IsInputUp;
		public bool IsInputJump;
		public bool IsInputJumpRelease;

		public float LeftCLickCooldown
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public float RightCLickCooldown
		{
			get => Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public bool JumpWithControlRelease(Player player)
		{
			if (IsInputJumpRelease && player.controlJump)
			{
				IsInputJumpRelease = false;
				return true;
			}
			return false;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
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
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			OldFrame = new List<int>();
			ai = [0f, 0f, 0f, 0f, 0f];
			NeedKill = false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
			writer.Write(IsLeftClick);
			writer.Write(IsLeftClickRelease);
			writer.Write(IsRightClick);
			writer.Write(IsRightClickRelease);
			writer.Write(IsInputLeft);
			writer.Write(IsInputRight);
			writer.Write(IsInputUp);
			writer.Write(IsInputDown);
			writer.Write(IsInputJump);
			writer.Write(IsInputJumpRelease);
			writer.Write(ai[0]);
			writer.Write(ai[1]);
			writer.Write(ai[2]);
			writer.Write(ai[3]);
			writer.Write(ai[4]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			int selectedItem = reader.ReadInt32();
			IsLeftClick = reader.ReadBoolean();
			IsLeftClickRelease = reader.ReadBoolean();
			IsRightClick = reader.ReadBoolean();
			IsRightClickRelease = reader.ReadBoolean();
			IsInputLeft = reader.ReadBoolean();
			IsInputRight = reader.ReadBoolean();
			IsInputUp = reader.ReadBoolean();
			IsInputDown = reader.ReadBoolean();
			IsInputJump = reader.ReadBoolean();
			IsInputJumpRelease = reader.ReadBoolean();
			ai[0] = reader.ReadSingle();
			ai[1] = reader.ReadSingle();
			ai[2] = reader.ReadSingle();
			ai[3] = reader.ReadSingle();
			ai[4] = reader.ReadSingle();

			if (SelectedItem == -1)
			{
				SelectedItem = selectedItem;
				Player owner = Owner;
				if (owner.inventory[owner.selectedItem].ModItem is OrchidModShapeshifterShapeshift shapeshiftItem)
				{
					OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
					shapeshifter.Shapeshift = shapeshiftItem;
					shapeshifter.ShapeshiftAnchor = this;
					Projectile.width = shapeshiftItem.ShapeshiftWidth;
					Projectile.height = shapeshiftItem.ShapeshiftHeight;
					Projectile.position -= new Vector2(Projectile.width - 2, Projectile.height - 2) * 0.5f;
					SoundEngine.PlaySound(shapeshiftItem.Item.UseSound, owner.Center);
					Projectile.ai[0] = 0f;
					Projectile.ai[1] = 0f;
					Projectile.ai[2] = 0f;
					ai[0] = 0f;
					ai[1] = 0f;
					ai[2] = 0f;
					ai[3] = 0f;
					ai[4] = 0f;
					BlinkEffect = 20;
					Frame = 0;
					Timespent = 0;
					NeedKill = false;

					IsLeftClickRelease = false;
					IsRightClickRelease = false;
					IsInputJumpRelease = false;

					TextureShapeshift = ModContent.Request<Texture2D>(shapeshiftItem.TextureShapeshift, AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftHair = ModContent.Request<Texture2D>(shapeshiftItem.TextureShapeshift + "_Hair", AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftIcon = ModContent.Request<Texture2D>(shapeshiftItem.TextureIcon, AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftIconBorder = ModContent.Request<Texture2D>(shapeshiftItem.TextureIcon + "_Border", AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftGlow = null;
					TextureShapeshiftHairGray = null;

					if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.TextureShapeshift + "_Glow", out Asset<Texture2D> assetglow, AssetRequestMode.ImmediateLoad))
					{
						TextureShapeshiftGlow = assetglow.Value;
					}

					if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.TextureShapeshift + "_Hair_Gray", out Asset<Texture2D> assethair, AssetRequestMode.ImmediateLoad))
					{
						TextureShapeshiftHairGray = assethair.Value;
					}

					shapeshiftItem.ShapeshiftAnchorOnShapeshift(Projectile, this, owner, shapeshifter);
					shapeshifter.OnShapeshift(Projectile, this, owner, shapeshifter);

					if (shapeshifter.ShapeshifterFastShapeshiftTimer >= 300)
					{
						shapeshiftItem.ShapeshiftAnchorOnShapeshiftFast(Projectile, this, owner, shapeshifter);
						shapeshifter.OnShapeshiftFast(Projectile, this, owner, shapeshifter);
					}

					shapeshifter.ShapeshifterFastShapeshiftTimer = 0;
				}
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}

		public void OnChangeSelectedItem(Player owner)
		{
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
			if (owner.inventory[owner.selectedItem].ModItem is OrchidModShapeshifterShapeshift shapeshiftItem)
			{
				SelectedItem = owner.selectedItem;
				shapeshifter.Shapeshift = shapeshiftItem;
				shapeshifter.ShapeshiftAnchor = this;
				Projectile.width = shapeshiftItem.ShapeshiftWidth;
				Projectile.height = shapeshiftItem.ShapeshiftHeight;
				Projectile.position -= new Vector2(Projectile.width - 2, Projectile.height - 2) * 0.5f;
				SoundEngine.PlaySound(shapeshiftItem.Item.UseSound, owner.Center);
				LeftCLickCooldown = 30;
				RightCLickCooldown = 30;
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				Projectile.ai[2] = 0f;
				ai[0] = 0f;
				ai[1] = 0f;
				ai[2] = 0f;
				ai[3] = 0f;
				ai[4] = 0f;
				BlinkEffect = 20;
				Frame = 0;
				Timespent = 0;
				NeedKill = false;

				IsLeftClickRelease = false;
				IsRightClickRelease = false;
				IsInputJumpRelease = false;

				TextureShapeshift = ModContent.Request<Texture2D>(shapeshiftItem.TextureShapeshift, AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftHair = ModContent.Request<Texture2D>(shapeshiftItem.TextureShapeshift + "_Hair", AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftIcon = ModContent.Request<Texture2D>(shapeshiftItem.TextureIcon, AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftIconBorder = ModContent.Request<Texture2D>(shapeshiftItem.TextureIcon + "_Border", AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftGlow = null;
				TextureShapeshiftHairGray = null;

				if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.TextureShapeshift + "_Glow", out Asset<Texture2D> assetglow, AssetRequestMode.ImmediateLoad))
				{
					TextureShapeshiftGlow = assetglow.Value;
				}

				if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.TextureShapeshift + "_Hair_Gray", out Asset<Texture2D> assethair, AssetRequestMode.ImmediateLoad))
				{
					TextureShapeshiftHairGray = assethair.Value;
				}

				if (IsLocalOwner)
				{
					if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.TextureUI, out Asset<Texture2D> assetUI, AssetRequestMode.ImmediateLoad))
					{
						ShapeshifterUIState.TextureUI = assetUI.Value;
					}

					if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.TextureUIBack, out Asset<Texture2D> assetUIBack, AssetRequestMode.ImmediateLoad))
					{
						ShapeshifterUIState.TextureUIBack = assetUIBack.Value;
					}
				}

				shapeshiftItem.ShapeshiftAnchorOnShapeshift(Projectile, this, owner, shapeshifter);
				shapeshifter.OnShapeshift(Projectile, this, owner, shapeshifter);

				if (shapeshifter.ShapeshifterFastShapeshiftTimer >= 300)
				{
					LeftCLickCooldown = 5;
					RightCLickCooldown = 5;
					shapeshiftItem.ShapeshiftAnchorOnShapeshiftFast(Projectile, this, owner, shapeshifter);
					shapeshifter.OnShapeshiftFast(Projectile, this, owner, shapeshifter);
				}

				shapeshifter.ShapeshifterFastShapeshiftTimer = 0;
			}
			Projectile.netUpdate = true;
		}

		public void ExtraAI(Player player, OrchidShapeshifter shapeshifter)
		{
			Timespent++;

			LeftCLickCooldown -= shapeshifter.Shapeshift.MeleeSpeedLeft ? shapeshifter.GetShapeshifterMeleeSpeed() : 1;
			RightCLickCooldown -= shapeshifter.Shapeshift.MeleeSpeedRight ? shapeshifter.GetShapeshifterMeleeSpeed() : 1;

			if (Projectile.position.X > Main.maxTilesX * 16 - (672 + Projectile.width))
			{ // Prevents the player from leaving the map through the right
				Projectile.position.X = Main.maxTilesX * 16 - (672 + Projectile.width);
				if (Projectile.velocity.X > 0)
				{
					Projectile.velocity.X = 0;
				}
			}
			else if (Projectile.position.X < 657)
			{ // Prevents the player from leaving the map through the left
				Projectile.position.X = 657;
				if (Projectile.velocity.X < 0)
				{
					Projectile.velocity.X = 0;
				}
			}

			if (Projectile.position.Y > Main.maxTilesY * 16 - (672 + Projectile.height))
			{ // Prevents the player from leaving the map through the bottom
				Projectile.position.X = Main.maxTilesY * 16 - (672 + Projectile.height);
				if (Projectile.velocity.Y > 0)
				{
					Projectile.velocity.Y = 0;
				}
			}
			else if (Projectile.position.Y < 657)
			{ // Prevents the player from leaving the map through the top
				Projectile.position.Y = 657;
				if (Projectile.velocity.Y < 0)
				{
					Projectile.velocity.Y = 0;
				}
			}

			if (IsLocalOwner)
			{
				if (player.gravDir != 1)
				{ // Kill the projectile in reverse gravity
					Projectile.Kill();
				}
			}
		}

		public void Blink(bool playSound)
		{
			if (playSound)
			{
				SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
			}
			
			BlinkEffect = 0;
		}

		public void Teleport(Vector2 position)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
			if (shapeshifter.IsShapeshifted)
			{
				shapeshifter.Shapeshift.ShapeshiftTeleport(position, Projectile, this, player, shapeshifter);
			}
		}

		public void CheckInputs(Player player)
		{
			if (IsLeftClick != Main.mouseLeft)
			{
				IsLeftClick = Main.mouseLeft && !player.mouseInterface;
				NeedNetUpdate = true;
			}

			if (IsLeftClickRelease != Main.mouseLeftRelease)
			{
				IsLeftClickRelease = Main.mouseLeftRelease;
				NeedNetUpdate = true;
			}

			if (IsRightClick != Main.mouseRight)
			{
				IsRightClick = Main.mouseRight && !player.mouseInterface;
				NeedNetUpdate = true;
			}

			if (IsRightClickRelease != Main.mouseRightRelease)
			{
				IsRightClickRelease = Main.mouseRightRelease;
				NeedNetUpdate = true;
			}

			if (IsInputLeft != player.controlLeft)
			{
				IsInputLeft = player.controlLeft;
				NeedNetUpdate = true;
			}

			if (IsInputRight != player.controlRight)
			{
				IsInputRight = player.controlRight;
				NeedNetUpdate = true;
			}

			if (IsInputUp != player.controlUp)
			{
				IsInputUp = player.controlUp;
				NeedNetUpdate = true;
			}

			if (IsInputDown != player.controlDown)
			{
				IsInputDown = player.controlDown;
				NeedNetUpdate = true;
			}

			if (IsInputJump != player.controlJump)
			{
				IsInputJump = player.controlJump;
				NeedNetUpdate = true;
			}

			if (!IsInputJumpRelease && !player.controlJump)
			{
				IsInputJumpRelease = true;
			}
		}

		public override void AI()
		{
			Player owner = Owner;
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();

			if ((SelectedItem < 0 || !owner.active || owner.dead || owner.HeldItem.ModItem is not OrchidModShapeshifterShapeshift) && IsLocalOwner)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				if (NeedNetUpdate)
				{
					NeedNetUpdate = false;
					Projectile.netUpdate = true;
				}

				Projectile.timeLeft = 5;

				if (BlinkEffect < 20)
				{
					BlinkEffect++;
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			var owner = Owner;
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
			shapeshifter.Shapeshift = null;
			shapeshifter.ShapeshiftAnchor = null;
			owner.width = Player.defaultWidth;
			owner.height = Player.defaultHeight;
			owner.position = Projectile.position + new Vector2(Projectile.width * 0.5f - owner.width * 0.5f, Projectile.height - owner.height);

			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}

			if (ShapeshifterItem.ModItem is OrchidModShapeshifterShapeshift shapeshifterItem)
			{
				shapeshifterItem.OnKillAnchor(Projectile, this, owner, shapeshifter);
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (ShapeshifterItem.ModItem is OrchidModShapeshifterShapeshift shapeshifterItem)
			{
				shapeshifterItem.ShapeshiftOnHitNPC(target, hit, damageDone, Projectile, this, player, shapeshifter);
			}
		}

		public override bool? CanCutTiles() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (SelectedItem < 0) return false;
			if (ShapeshifterItem.ModItem is not OrchidModShapeshifterShapeshift shapeshifterItem || TextureShapeshift == null) return false;
			var player = Main.player[Projectile.owner];

			if (shapeshifterItem.ShapeshiftShouldDraw(spriteBatch, Projectile, player, ref lightColor))
			{
				OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
				bool drawAsAdditive = false;
				Texture2D hairTexture = (TextureShapeshiftHairGray != null && shapeshifter.ShapeshifterHairpin) ? TextureShapeshiftHairGray : TextureShapeshiftHair;
				Color color = shapeshifterItem.GetColor(ref drawAsAdditive, shapeshifter.ShapeshifterHairpin ? lightColor.MultiplyRGBA(player.hairColor) : lightColor, Projectile, this, player, shapeshifter, true);
				Color colorLight = shapeshifterItem.GetColor(ref drawAsAdditive, lightColor, Projectile, this, player, shapeshifter);
				Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix).Floor();
				Rectangle drawRectangle = TextureShapeshift.Bounds;
				drawRectangle.Height = drawRectangle.Width;
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				drawRectangle.Y = drawRectangle.Height * Frame;
				shapeshifterItem.ShapeshiftPreDraw(spriteBatch, Projectile, this, drawPosition, drawRectangle, effect, player, color);

				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				if (BlinkEffect < 20)
				{ // blink animation
					float scalemult = (float)Math.Sin(BlinkEffect * 0.157f) * 0.2f + 1f;
					spriteBatch.Draw(TextureShapeshift, drawPosition, drawRectangle, colorLight * 1.5f, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale * scalemult, effect, 0f);
					spriteBatch.Draw(hairTexture, drawPosition, drawRectangle, color * 1.5f, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale * scalemult, effect, 0f);
				}

				for (int i = 0; i < OldPosition.Count; i++)
				{
					drawRectangle.Y = drawRectangle.Height * OldFrame[i];
					Vector2 drawPosition2 = Vector2.Transform(OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
					spriteBatch.Draw(TextureShapeshift, drawPosition2, drawRectangle, colorLight * 0.075f * (i + 1), OldRotation[i], drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
					spriteBatch.Draw(hairTexture, drawPosition2, drawRectangle, color * 0.075f * (i + 1), OldRotation[i], drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
				}

				if (!drawAsAdditive)
				{
					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				/*
				if (player.GetModPlayer<OrchidShapeshifter>().ShapeshifterShampoo)
				{
					Main.instance.PrepareDrawnEntityDrawing(Projectile, player.cMinion, null);
				}
				*/

				drawRectangle.Y = drawRectangle.Height * Frame;
				if (shapeshifter.ShapeshifterHairpin)
				{ // draws hair before the shampoo shaders if the player uses the hairpin
					for (int i = 0; i < player.armor.Length; i++)
					{
						if (player.armor[i].type == ModContent.ItemType<ShapeshifterHairpin>())
						{
							if (i > 9) i -= 10;
							if (player.dye[i].type != ItemID.None)
							{
								Main.instance.PrepareDrawnEntityDrawing(Projectile, GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type), null);
							}
							break;
						}
					}

					Main.EntitySpriteDraw(hairTexture, drawPosition, drawRectangle, color, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
					Main.instance.PrepareDrawnEntityDrawing(Projectile, 0, null);
				}

				for (int i = 0; i < player.armor.Length; i++)
				{
					if (player.armor[i].type == ModContent.ItemType<ShapeshifterShampoo>())
					{
						if (i > 9) i -= 10;
						if (player.dye[i].type != ItemID.None)
						{
							Main.instance.PrepareDrawnEntityDrawing(Projectile, GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type), null);
						}
						break;
					}
				}

				if (!shapeshifter.ShapeshifterHairpin)
				{ // draws hair before the shaders if the player uses the hairpin
					Main.EntitySpriteDraw(hairTexture, drawPosition, drawRectangle, color, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
				}

				Main.EntitySpriteDraw(TextureShapeshift, drawPosition, drawRectangle, colorLight, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);

				if (TextureShapeshiftGlow != null)
				{
					color = shapeshifterItem.GetColorGlow(ref drawAsAdditive, lightColor, Projectile, this, player, player.GetModPlayer<OrchidShapeshifter>());
					Main.EntitySpriteDraw(TextureShapeshiftGlow, drawPosition, drawRectangle, color, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
				}

				if (drawAsAdditive)
				{
					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				//spriteBatch.End();
				//spriteBatch.Begin(spriteBatchSnapshot);
				shapeshifterItem.ShapeshiftPostDraw(spriteBatch, Projectile, this, drawPosition, drawRectangle, effect, player, color);
			}

			return false;
		}
	}
}