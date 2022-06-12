using Terraria;
using Terraria.ID;

namespace OrchidMod.Dancer.Weapons
{
	public class GoldFan : OrchidModDancerItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.width = 42;
			Item.height = 42;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item101;
			Item.knockBack = 3f;
			Item.damage = 8;
			Item.crit = 4;
			Item.rare = ItemRarityID.White;
			Item.autoReuse = true;
			Item.useAnimation = 30;
			Item.useTime = 30;
			this.dashTimer = 10;
			this.poiseCost = 0;
			this.dashVelocity = 30f;
			this.vertical = true;
			this.horizontal = true;
			this.dancerItemType = OrchidModDancerItemType.PHASE;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Fan");
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
