using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class MeteorPhasestaff : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 7;
			item.width = 30;
			item.height = 30;
			item.useTime = 4;
			item.useAnimation = 32;
			item.knockBack = 0f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 48, 0);
			item.autoReuse = true;
			item.shootSpeed = 5.25f;
			item.shoot = mod.ProjectileType("MeteorPhasestaffProj");
			item.UseSound = SoundID.Item15;
			this.empowermentType = 1;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Phasestaff");
			Tooltip.SetDefault("Channels a disintegrating beam of energy"
							  + "\nThe beam gets weaker the further it goes"
							  + "\nWeapon damage doubles if you have 3 or more active shamanic bonds");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 3)
			{
				flat += 7f;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.MeteoriteBar, 20);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}