using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Alchemist.Bag
{
	public partial class PotionBag : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (dye.IsAir) return;

			var sbInfo = new SpriteBatchInfo(spriteBatch);
			var shader = GameShaders.Armor.GetShaderFromItemId(dye.type);
			var texture = ModContent.Request<Texture2D>(Texture + "_Dye");
			var rect = new Rectangle(0, 0, texture.Width(), texture.Height());

			spriteBatch.End();
			sbInfo.Begin(spriteBatch, SpriteSortMode.Immediate, BlendState.AlphaBlend, null);

			shader.Apply(Item, new DrawData(texture.Value, rect, lightColor));
			spriteBatch.Draw(texture.Value, Item.Center - Main.screenPosition, rect, lightColor, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			sbInfo.Begin(spriteBatch);
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (dye.IsAir) return;

			var sbInfo = new SpriteBatchInfo(spriteBatch);
			var shader = GameShaders.Armor.GetShaderFromItemId(dye.type);
			var texture = ModContent.Request<Texture2D>(Texture + "_Dye");

			spriteBatch.End();
			sbInfo.Begin(spriteBatch, SpriteSortMode.Immediate, BlendState.AlphaBlend, null);

			shader.Apply(Item, new DrawData(texture.Value, frame, drawColor));
			spriteBatch.Draw(texture.Value, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			sbInfo.Begin(spriteBatch);
		}

		// ...

		private static void PostDrawItemSlot(On.Terraria.UI.ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig,
		SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
		{
			orig(spriteBatch, inv, context, slot, position, lightColor);

			var item = inv[slot];

			switch (context)
			{
				case ItemSlot.Context.InventoryItem:
				case ItemSlot.Context.HotbarItem:
					if (item.ModItem is PotionBag bag && !bag.IsActive)
					{
						var scale = 1f;
						var rect = TextureAssets.Item[item.type].Frame(1, 1, 0, 0, 0, 0);

						if (rect.Width > 32 || rect.Height > 32)
						{
							scale = ((rect.Width <= rect.Height) ? (32f / rect.Height) : (32f / rect.Width));
						}

						scale *= Main.inventoryScale;
						position += TextureAssets.InventoryBack.Size() * Main.inventoryScale / 2f;
						position -= TextureAssets.Cd.Value.Size() * Main.inventoryScale / 2f;

						spriteBatch.Draw(TextureAssets.Cd.Value, position, null, Color.White * 0.65f, 0f, default, scale, SpriteEffects.None, 0f);
					}
					break;
				default:
					break;
			}
		}
	}
}