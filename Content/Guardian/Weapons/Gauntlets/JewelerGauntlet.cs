using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.Alchemist;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public enum JewelerGauntletGem : byte
	{
		NONE = 0,
		AMETHYST = 1,
		TOPAZ = 2,
		SAPPHIRE = 3,
		EMERALD = 4,
		RUBY = 5,
		DIAMOND = 6,
		AMBER = 7,
		AQUAMARINE = 8,
		OPAL = 9
	}

	public class JewelerGauntlet : OrchidModGuardianGauntlet
	{
		public JewelerGauntletGem GemType;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.knockBack = 3f;
			Item.damage = 53;
			Item.value = Item.sellPrice(0, 1, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 35;
			strikeVelocity = 15f;
			parryDuration = 60;
			GemType = 0;
		}

		public override Color GetColor(bool offHand)
		{
			switch(GemType)
			{
				default:
					return new Color(180, 180, 180);
				case JewelerGauntletGem.AMETHYST:
					return new Color(219, 97, 255);
				case JewelerGauntletGem.TOPAZ:
					return new Color(255, 226, 133);
				case JewelerGauntletGem.SAPPHIRE:
					return new Color(23, 147, 234);
				case JewelerGauntletGem.EMERALD:
					return new Color(81, 207, 160);
				case JewelerGauntletGem.RUBY:
					return new Color(243, 114, 113);
				case JewelerGauntletGem.DIAMOND:
					return new Color(218, 185, 210);
				case JewelerGauntletGem.AMBER:
					return new Color(233, 168, 48);
				case JewelerGauntletGem.AQUAMARINE:
					return new Color(109, 255, 216);
				case JewelerGauntletGem.OPAL:
					return new Color(255, 146, 163);
			}
		}

		public override Texture2D GetGauntletTexture(bool OffHandGauntlet, out Rectangle? drawRectangle)
		{
			Texture2D texture = ModContent.Request<Texture2D>(GauntletTexture).Value;
			Rectangle rectangle = texture.Bounds;
			rectangle.Height = rectangle.Height / 10;
			rectangle.Y += (byte)GemType * rectangle.Height;
			drawRectangle = rectangle;
			return texture;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			frame.Height /= 10;
			frame.Y += frame.Height * (byte)GemType;
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, frame.Size() * 0.5f, scale * 10f, SpriteEffects.None, 0f);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			Rectangle drawRectangle = texture.Bounds;
			drawRectangle.Height /= 10;
			drawRectangle.Y += drawRectangle.Height * (byte)GemType;
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, drawRectangle, lightColor, rotation, drawRectangle.Size() * 0.5f, scale, SpriteEffects.None, 0f);
			return false;
		}

		public override bool CanRightClick() => true;

		public override bool ConsumeItem(Player player) => false;

		public override void RightClick(Player player)
		{
			if (GemType == JewelerGauntletGem.AMETHYST) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Amethyst);
			if (GemType == JewelerGauntletGem.TOPAZ) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Topaz);
			if (GemType == JewelerGauntletGem.SAPPHIRE) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Sapphire);
			if (GemType == JewelerGauntletGem.EMERALD) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Emerald);
			if (GemType == JewelerGauntletGem.RUBY) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Ruby);
			if (GemType == JewelerGauntletGem.DIAMOND) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Diamond);
			if (GemType == JewelerGauntletGem.AMBER) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), ItemID.Amber);

			if (OrchidMod.ThoriumMod != null)
			{
				if (GemType == JewelerGauntletGem.AQUAMARINE) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), OrchidMod.ThoriumMod.Find<ModItem>("Aquamarine").Type);
				if (GemType == JewelerGauntletGem.OPAL) player.QuickSpawnItem(Item.GetSource_Misc("Jeweler Gauntlet Socket"), OrchidMod.ThoriumMod.Find<ModItem>("Opal").Type);
			}

			GemType = JewelerGauntletGem.NONE;

			if (Main.mouseItem.type == ItemID.Amethyst) GemType = JewelerGauntletGem.AMETHYST;
			if (Main.mouseItem.type == ItemID.Topaz) GemType = JewelerGauntletGem.TOPAZ;
			if (Main.mouseItem.type == ItemID.Sapphire) GemType = JewelerGauntletGem.SAPPHIRE;
			if (Main.mouseItem.type == ItemID.Emerald) GemType = JewelerGauntletGem.EMERALD;
			if (Main.mouseItem.type == ItemID.Ruby) GemType = JewelerGauntletGem.RUBY;
			if (Main.mouseItem.type == ItemID.Diamond) GemType = JewelerGauntletGem.DIAMOND;
			if (Main.mouseItem.type == ItemID.Amber) GemType = JewelerGauntletGem.AMBER;

			if (OrchidMod.ThoriumMod != null)
			{
				if (Main.mouseItem.type == OrchidMod.ThoriumMod.Find<ModItem>("Aquamarine").Type) GemType = JewelerGauntletGem.AQUAMARINE;
				if (Main.mouseItem.type == OrchidMod.ThoriumMod.Find<ModItem>("Opal").Type) GemType = JewelerGauntletGem.OPAL;
			}

			if (GemType != JewelerGauntletGem.NONE)
			{ // Type changed, remove a gem from the player cursor
				if (Main.mouseItem.stack > 1) Main.mouseItem.stack--;
				else Main.mouseItem.TurnToAir();
			}
			return;
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("Gem", (byte)GemType);
		}

		public override void LoadData(TagCompound tag)
		{
			GemType = (JewelerGauntletGem)tag.GetByte("Gem");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((byte)GemType);
		}

		public override void NetReceive(BinaryReader reader)
		{
			GemType = (JewelerGauntletGem)reader.ReadByte();
		}
	}
}
