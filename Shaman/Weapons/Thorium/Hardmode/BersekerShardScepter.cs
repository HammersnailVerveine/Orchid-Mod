using OrchidMod.Common.Interfaces;
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
			Item.damage = 93;
			Item.width = 62;
			Item.height = 62;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 7, 20, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("BersekerShardScepterProj").Type;
			this.empowermentType = 1;
			this.energy = 8;
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
				ModRecipe recipe = new ModRecipe(Mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "BersekerShard", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

