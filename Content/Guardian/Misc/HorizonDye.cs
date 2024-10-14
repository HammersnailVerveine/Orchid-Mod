using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class HorizonDye : ModItem
	{
		public override void SetStaticDefaults()
		{
			if (!Main.dedServ)
			{
				GameShaders.Armor.BindShader(Item.type, new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/HorizonGlow"), "HorizonShaderPass"));
			}
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults()
		{
			int dye = Item.dye;
			Item.CloneDefaults(ItemID.SolarDye);
			Item.dye = dye;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.DyeVat);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient<HorizonFragment>(10);
			recipe.Register();
		}
	}
}
