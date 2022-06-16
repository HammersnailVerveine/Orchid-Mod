using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Hell
{
	[AutoloadEquip(EquipType.Head)]
	public class HellShamanTiara : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = 3;
			Item.defense = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depths Weaver Tiara");
			Tooltip.SetDefault("6% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.06f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("HellShamanTunic").Type && legs.type == Mod.Find<ModItem>("HellShamanKilt").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Shamanic fire bonds and causes attacks to release spreading fire"
							+ "\n             Shamanic earth bonds causes you reflect damage to nearby enemies"
							+ "\n             Your shamanic bonds will last 3 seconds longer";
			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanHell = true;
		}

		public override bool DrawHead()
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
			drawAltHair = false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
