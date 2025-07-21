using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Shapeshifter.Armors.Ashwood
{
	[AutoloadEquip(EquipType.Legs)]
	public class ShapeshifterAshwoodLegs : OrchidModShapeshifterItem
	{
		public static Texture2D TextureGlow;

		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
			TextureGlow ??= ModContent.Request<Texture2D>(Item.ModItem.Texture + "_Legs_Glow", AssetRequestMode.ImmediateLoad).Value;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AshWood, 25);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class ShapeshifterAshwoodLegsGlowmask : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Leggings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[12].type == ItemType<ShapeshifterAshwoodLegs>() || (drawPlayer.armor[12].type == ItemID.None && drawPlayer.armor[2].type == ItemType<ShapeshifterAshwoodLegs>()))
			{
				OrchidPlayer modPlayer = drawPlayer.GetModPlayer<OrchidPlayer>();
				float colorFlicker = (0.4f + (float)(Math.Sin(modPlayer.Timer * 0.04851f) * 0.1f) + (float)(Math.Sin(modPlayer.Timer * 0.123f) * 0.05f) + (float)(Math.Sin(modPlayer.Timer * 0.31461f) * 0.02f) + (float)(Math.Sin(modPlayer.Timer * 0.07124f) * 0.03f));
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White * colorFlicker, drawInfo.shadow);

				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
				Vector2 legsOffset = drawInfo.legsOffset;
				DrawData drawData = new DrawData(ShapeshifterAshwoodLegs.TextureGlow, drawPos.Floor() + legsOffset, drawPlayer.legFrame, color, drawPlayer.legRotation, legsOffset, 1f, drawInfo.playerEffect, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}
