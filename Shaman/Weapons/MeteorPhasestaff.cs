using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class MeteorPhasestaff : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 4;
			Item.useAnimation = 32;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 48, 0);
			Item.autoReuse = true;
			Item.shootSpeed = 5.25f;
			Item.shoot = ModContent.ProjectileType<MeteorPhasestaffProj>();
			Item.UseSound = SoundID.Item15;
			this.empowermentType = 1;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			this.energy = 2;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Phasestaff");
			Tooltip.SetDefault("Channels a disintegrating beam of energy"
							  + "\nThe beam gets weaker the further it goes"
							  + "\nWeapon damage doubles if you have 3 or more active shamanic bonds");
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			if (modPlayer.GetNbShamanicBonds() > 3)
				damage *= 2f;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.MeteoriteBar, 20)
			.AddTile(TileID.Anvils)
			.Register();
	}
}