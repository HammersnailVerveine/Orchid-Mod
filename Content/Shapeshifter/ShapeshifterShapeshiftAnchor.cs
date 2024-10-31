using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Accessories;
using OrchidMod.Utilities;
using ReLogic.Content;
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
		public int SelectedItem { get; set; } = -1;
		public Item ShapeshifterItem => Main.player[Projectile.owner].inventory[SelectedItem];

		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public List<int> OldFrame;
		public int Frame = 0;
		public int Timespent = 0;
		public Texture2D TextureShapeshift;
		public Texture2D TextureShapeshiftGlow;
		public Texture2D TextureShapeshiftIcon;
		public Texture2D TextureShapeshiftIconBorder;

		public bool CanLeftClick => LeftCLickCooldown <= 0f;
		public bool CanRightClick => RightCLickCooldown <= 0f;

		public bool IsLeftClick;
		public bool IsRightClick;
		public bool IsInputLeft;
		public bool IsInputRight;
		public bool IsInputDown;
		public bool IsInputUp;
		public bool IsInputJump;
		public bool ControlJumpRelease;

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
			if (ControlJumpRelease && player.controlJump)
			{
				ControlJumpRelease = false;
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
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
			writer.Write(IsLeftClick);
			writer.Write(IsRightClick);
			writer.Write(IsInputLeft);
			writer.Write(IsInputRight);
			writer.Write(IsInputUp);
			writer.Write(IsInputDown);
			writer.Write(IsInputJump);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			int selectedItem = reader.ReadInt32();
			IsLeftClick = reader.ReadBoolean();
			IsRightClick = reader.ReadBoolean();
			IsInputLeft = reader.ReadBoolean();
			IsInputRight = reader.ReadBoolean();
			IsInputUp = reader.ReadBoolean();
			IsInputDown = reader.ReadBoolean();
			IsInputJump = reader.ReadBoolean();

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
					shapeshiftItem.ShapeshiftAnchorOnShapeshift(Projectile, this, owner, shapeshifter);
					SoundEngine.PlaySound(shapeshiftItem.Item.UseSound, owner.Center);
					Projectile.ai[0] = 0f;
					Projectile.ai[1] = 0f;
					Projectile.ai[2] = 0f;

					TextureShapeshift = ModContent.Request<Texture2D>(shapeshiftItem.ShapeshiftTexture, AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftIcon = ModContent.Request<Texture2D>(shapeshiftItem.IconTexture, AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftIconBorder = ModContent.Request<Texture2D>(shapeshiftItem.IconTexture + "_Border", AssetRequestMode.ImmediateLoad).Value;
					TextureShapeshiftGlow = null;
					if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.ShapeshiftTexture + "_Glow", out Asset<Texture2D> asset, AssetRequestMode.ImmediateLoad))
					{
						TextureShapeshiftGlow = asset.Value;
					}
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
				shapeshiftItem.ShapeshiftAnchorOnShapeshift(Projectile, this, owner, shapeshifter);
				SoundEngine.PlaySound(shapeshiftItem.Item.UseSound, owner.Center);
				LeftCLickCooldown = shapeshiftItem.Item.useTime;
				RightCLickCooldown = shapeshiftItem.Item.useTime;
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				Projectile.ai[2] = 0f;

				TextureShapeshift = ModContent.Request<Texture2D>(shapeshiftItem.ShapeshiftTexture, AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftIcon = ModContent.Request<Texture2D>(shapeshiftItem.IconTexture, AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftIconBorder = ModContent.Request<Texture2D>(shapeshiftItem.IconTexture + "_Border", AssetRequestMode.ImmediateLoad).Value;
				TextureShapeshiftGlow = null;
				if (ModContent.RequestIfExists<Texture2D>(shapeshiftItem.ShapeshiftTexture + "_Glow", out Asset<Texture2D> asset, AssetRequestMode.ImmediateLoad))
				{
					TextureShapeshiftGlow = asset.Value;
				}
			}
			Projectile.netUpdate = true;
		}

		public void ExtraAI(Player player, OrchidShapeshifter shapeshifter)
		{
			Timespent++;

			LeftCLickCooldown -= shapeshifter.Shapeshift.MeleeSpeedLeft ? player.GetAttackSpeed(DamageClass.Melee) + shapeshifter.ShapeshifterMeleeSpeedBonus : 1;
			RightCLickCooldown -= shapeshifter.Shapeshift.MeleeSpeedRight ? player.GetAttackSpeed(DamageClass.Melee) + shapeshifter.ShapeshifterMeleeSpeedBonus : 1;

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

		public void CheckInputs(Player player)
		{

			if (IsLeftClick != Main.mouseLeft)
			{
				IsLeftClick = Main.mouseLeft;
				NeedNetUpdate = true;
			}

			if (IsRightClick != Main.mouseRight)
			{
				IsRightClick = Main.mouseRight;
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

			if (!ControlJumpRelease && !player.controlJump)
			{
				ControlJumpRelease = true;
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

		public override bool? CanCutTiles() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (SelectedItem == ItemID.None) return false;
			if (ShapeshifterItem.ModItem is not OrchidModShapeshifterShapeshift shapeshifterItem || TextureShapeshift == null) return false;
			var player = Main.player[Projectile.owner];

			if (shapeshifterItem.ShouldDrawShapeshift(spriteBatch, Projectile, player, ref lightColor))
			{
				bool drawAsAdditive = false;
				Color color = shapeshifterItem.GetColor(ref drawAsAdditive, lightColor, Projectile, this, player, player.GetModPlayer<OrchidShapeshifter>());
				Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				Rectangle drawRectangle = TextureShapeshift.Bounds;
				drawRectangle.Height = drawRectangle.Width;
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				drawRectangle.Y = drawRectangle.Height * Frame;
				shapeshifterItem.PreDrawShapeshift(spriteBatch, Projectile, this, drawPosition, drawRectangle, effect, player, color);

				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				for (int i = 0; i < OldPosition.Count; i++)
				{
					drawRectangle.Y = drawRectangle.Height * OldFrame[i];
					Vector2 drawPosition2 = Vector2.Transform(OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
					spriteBatch.Draw(TextureShapeshift, drawPosition2, drawRectangle, color * 0.075f * (i + 1), OldRotation[i], drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
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

				//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				//GameShaders.Misc["OrchidMod:HorizonGlow"].Apply();

				drawRectangle.Y = drawRectangle.Height * Frame;
				Main.EntitySpriteDraw(TextureShapeshift, drawPosition, drawRectangle, color, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);

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
				shapeshifterItem.PostDrawShapeshift(spriteBatch, Projectile, this, drawPosition, drawRectangle, effect, player, color);
			}

			return false;
		}
	}
}