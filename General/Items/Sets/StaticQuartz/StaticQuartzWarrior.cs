using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartzWarrior : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Spear");
			Tooltip.SetDefault("Deals increased damage while moving");
		}

		public override void SetDefaults()
		{
			item.damage = 8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 31;
			item.useTime = 31;
			item.shootSpeed = 3.7f;
			item.knockBack = 6.5f;
			item.width = 40;
			item.height = 40;
			item.scale = 1f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.melee = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileType<General.Items.Sets.StaticQuartz.Projectiles.StaticQuartzWarriorProj>();
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			flat += Math.Abs(player.velocity.X + player.velocity.Y) > 2.5f ? 3 : 0;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 12);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
