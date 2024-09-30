using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.GemTiaras
{
	[AutoloadEquip(EquipType.Head)]
	public class TopazTiara : OrchidModShamanEquipable
	{

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 12;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shaman, ShamanElement element, Projectile catalyst)
		{
			if (element == ShamanElement.EARTH) catalyst.damage = (int)(catalyst.damage * 1.2f);
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.ShamanBondDuration += 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (body.type == ItemID.CopperChainmail && legs.type == ItemID.CopperGreaves)
				|| (body.type == ItemID.TinChainmail && legs.type == ItemID.TinGreaves)
				|| (body.type == ItemID.IronChainmail && legs.type == ItemID.IronGreaves)
				|| (body.type == ItemID.LeadChainmail && legs.type == ItemID.LeadGreaves)
				|| (body.type == ItemID.SilverChainmail && legs.type == ItemID.SilverGreaves)
				|| (body.type == ItemID.TungstenChainmail && legs.type == ItemID.TungstenGreaves)
				|| (body.type == ItemID.GoldChainmail && legs.type == ItemID.GoldGreaves)
				|| (body.type == ItemID.PlatinumChainmail && legs.type == ItemID.PlatinumGreaves);
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "2 defense";
			player.statDefense += 2;
		}
		/*
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Topaz, 1);
			recipe.AddIngredient(null, "EmptyTiara", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		*/
	}
}
