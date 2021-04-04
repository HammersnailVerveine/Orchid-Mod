using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Misc
{
	public class AttractiteShuriken : OrchidModAlchemistMisc
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Attractite Shuriken");
			Tooltip.SetDefault("Inflicts attractite to hit enemies");
		}

		public override void SafeSetDefaults() {
			item.damage = 5;
			item.crit = 4;
			item.useStyle = 1;
			item.useAnimation = 15;
			item.useTime = 15;
			item.shootSpeed = 9f;
			item.knockBack = 0f;
			item.width = 22;
			item.height = 22;
			item.scale = 1f;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.noMelee = true;
			item.noUseGraphic = true;
			item.maxStack = 5;
			item.UseSound = SoundID.Item1;
			item.consumable = true;
			item.shoot = ProjectileType<Alchemist.Projectiles.Misc.AttractiteShurikenProj>();
		}

		public override bool OnPickup(Player player) {
			return !this.alreadyInInventory(player);
		}
	}
}
