using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class TerrariumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 94;
			Item.width = 56;
			Item.height = 56;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 3.5f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(0, 13, 50, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			//Item.shoot = ModContent.ProjectileType<TerrariumScepterProj>();
			this.Element = ShamanElement.AIR;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Terrarium Scepter");
			/* Tooltip.SetDefault("Fires bolts of chromatic energy"
							+ "\nHitting enemies will gradually grant you terrarium orbs"
							+ "\nWhen reaching 7 orbs, they will break free and home into your enemies"); */
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			var tt = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.Mod == "Terraria");
			if (tt != null) tt.OverrideColor = Main.DiscoColor;
		}

		/*
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.LunarCraftingStation);
				recipe.AddIngredient(thoriumMod, "TerrariumCore", 9);
				recipe.Register();
			}
		}
		*/
	}
}

