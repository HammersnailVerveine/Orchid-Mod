using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModSystems;
using OrchidMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Misc
{
	public class MushroomThread : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phosphorescent Thread");
			/* Tooltip.SetDefault("Result of the hidden reaction between blinkroot and glowing mushroom extracts"
							+ "\n'Quite an unexpected outcome'"); */
			//+  "\n[c/FF0000:PLEASE MAKE SURE THE HIDDEN REACTION KEYBIND IS SET]");
			Item.ResearchUnlockCount = 25;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(Item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress, rotation, scale);
		}
	}
}
