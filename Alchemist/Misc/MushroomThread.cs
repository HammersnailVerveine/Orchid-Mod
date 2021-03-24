using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	public class MushroomThread : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phosphorescent Thread");
			Tooltip.SetDefault("Result of the hidden reaction between blinkroot and glowing mushroom extracts"
							+  "\n'Quite an unexpected outcome'");
							//+  "\n[c/FF0000:PLEASE MAKE SURE THE HIDDEN REACTION KEYBIND IS SET]");
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/MushroomThread_Glowmask"), new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress, rotation, scale);
		}
	}
}
