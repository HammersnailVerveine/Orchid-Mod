using Terraria;
using Terraria.ID;

namespace OrchidMod.Dancer.Weapons
{
	public class WoodenTekko : OrchidModDancerItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 3f;
			Item.damage = 8;
			Item.rare = ItemRarityID.White;
			Item.autoReuse = true;
			Item.useAnimation = 30;
			Item.useTime = 30;
			this.dashTimer = 30;
			this.poiseCost = 0;
			this.dashVelocity = 7f;
			this.vertical = false;
			this.horizontal = true;
			this.dancerItemType = OrchidModDancerItemType.IMPACT;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Tekko");
			Tooltip.SetDefault("Horizontally dashes at your foes"
							+ "\n[c/FF0000:Test Item]"
							+ "\nThe Dancer class is a proof of concept"
							+ "\nDo not expect it to be released soon, if at all");
		}

		// public override void AddRecipes()
		// {
		// var recipe = new ModRecipe(mod);
		// recipe.AddTile(TileID.WorkBenches);		
		// recipe.AddIngredient(ItemID.Wood, 8);
		// recipe.Register();
		// recipe.AddRecipe();
		// }
	}
}
