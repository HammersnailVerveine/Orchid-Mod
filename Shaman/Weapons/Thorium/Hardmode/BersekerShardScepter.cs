using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class BersekerShardScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 93;
			item.width = 62;
			item.height = 62;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 4.25f;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 7, 20, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("BersekerShardScepterProj");
			this.empowermentType = 1;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Berserker Wrath");
			Tooltip.SetDefault("Fires out a bolt pure rage"
							+ "\nThe less health you have, the more damage dealt"
							+ "\nIf you have 5 shamanic bonds, enemies hit by the staff return as aggressive spirits for a short time after dying");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			if (player.statLifeMax2 - player.statLife > (int)(player.statLifeMax2 * 0.75f)) add += 0.33f;
			if (player.statLifeMax2 - player.statLife > (int)(player.statLifeMax2 * 0.5f)) add += 0.33f;
			if (player.statLifeMax2 - player.statLife > (int)(player.statLifeMax2 * 0.25f)) add += 0.33f;

		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "BersekerShard", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

