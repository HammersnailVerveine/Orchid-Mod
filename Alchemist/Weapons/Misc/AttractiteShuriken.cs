using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Misc
{
	public class AttractiteShuriken : OrchidModAlchemistMisc
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Attractite Shuriken");
			Tooltip.SetDefault("Inflicts attractite to hit enemies");
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.crit = 4;
			Item.useStyle = 1;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.shootSpeed = 9f;
			Item.knockBack = 0f;
			Item.width = 22;
			Item.height = 22;
			Item.scale = 1f;
			Item.rare = 0;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.maxStack = 5;
			Item.UseSound = SoundID.Item1;
			Item.consumable = true;
			Item.shoot = ProjectileType<Alchemist.Projectiles.Misc.AttractiteShurikenProj>();
		}

		public override bool OnPickup(Player player)
		{
			return !this.alreadyInInventory(player, true);
		}
	}
}
