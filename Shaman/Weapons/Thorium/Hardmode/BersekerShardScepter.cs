using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class BersekerShardScepter : OrchidModShamanItem
	{
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
			Item.shoot = ModContent.ProjectileType<BersekerShardScepterProj>();
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

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.statLifeMax2 - player.statLife > (int)(player.statLifeMax2 * 0.75f)) damage += 0.33f;
			if (player.statLifeMax2 - player.statLife > (int)(player.statLifeMax2 * 0.5f)) damage += 0.33f;
			if (player.statLifeMax2 - player.statLife > (int)(player.statLifeMax2 * 0.25f)) damage += 0.33f;

		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "BersekerShard", 9);
				recipe.Register();
			}
		}
	}
}

