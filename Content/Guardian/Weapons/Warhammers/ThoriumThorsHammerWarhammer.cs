using Terraria;
using Terraria.ID;
using OrchidMod.Common.Attributes;
using Terraria.ModLoader;

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
			Item.damage = 76;
			Range = 18;
			GuardStacks = 1;
			ReturnSpeed = 1.8f;
			Penetrate = true;
		}

		public override bool CanRightClick() => true;

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
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.HellstoneBar, 20);
			recipe.Register();
		}
	}
}
