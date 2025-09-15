using Terraria;
using Terraria.ID;
using OrchidMod.Common.Attributes;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumThorsHammerWarhammer : OrchidModGuardianHammer
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.knockBack = 5.5f;
			Item.useTime = 18;
			Item.shootSpeed = 16f;
			Item.damage = 142;
			Range = 18;
			GuardStacks = 1;
			ReturnSpeed = 1.8f;
			SwingSpeed = 1.5f;
			HitCooldown = 20;
			Penetrate = true;
			BlockDuration = 255;
		}

		//public override bool CanRightClick() => true;

		/*
		public override void RightClick(Player player)
		{
			if (OrchidMod.ThoriumMod != null)
			{
				Item = new Item(OrchidMod.ThoriumMod.Find<ModItem>("MeleeThorHammer").Type);
			}
		}
		*/

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod, "ThoriumAnvil");
				recipe.AddIngredient(thoriumMod, "ThoriumBar", 20);
				recipe.AddIngredient(ItemID.HellstoneBar, 4);
				recipe.Register();

				recipe = CreateRecipe();
				recipe.AddRecipeGroup("ThorsHammers", 1);
				recipe.Register();

				recipe = CreateRecipe();
				recipe.AddIngredient(Item.type, 1);
				recipe.ReplaceResult(thoriumMod, "MeleeThorHammer", 1);
				recipe.Register();
			}
		}
	}
}
