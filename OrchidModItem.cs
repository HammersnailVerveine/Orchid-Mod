using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod
{
	public abstract class OrchidModItem : ModItem
    {
		public bool glowmask; // Does this item have a glowmask?

		public sealed override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (glowmask)
			{
				Texture2D texture = mod.GetTexture($"Glowmasks/{item.modItem.GetType().Name}_Glowmask");
				Vector2 offset = new Vector2(0, 2); // Unnecessary in 1.4

				spriteBatch.Draw(texture, item.position - Main.screenPosition + item.Size * 0.5f + offset, null, Color.White, rotation, item.Size * 0.5f, scale, SpriteEffects.None, 0f);
			}

			OrchidPostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI); // Allows you to safely draw on item
		}

		public virtual void OrchidPostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) { }
	}
}