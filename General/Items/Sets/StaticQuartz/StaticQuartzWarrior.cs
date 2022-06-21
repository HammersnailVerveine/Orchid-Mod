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
			Item.damage = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 31;
			Item.useTime = 31;
			Item.shootSpeed = 3.7f;
			Item.knockBack = 6.5f;
			Item.width = 40;
			Item.height = 40;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<General.Items.Sets.StaticQuartz.Projectiles.StaticQuartzWarriorProj>();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			flat += Math.Abs(player.velocity.X + player.velocity.Y) > 2.5f ? 3 : 0;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 12);
			recipe.Register();
			recipe.Register();
		}
	}
}
