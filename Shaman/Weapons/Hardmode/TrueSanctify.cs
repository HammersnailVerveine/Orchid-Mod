using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Misc;
using OrchidMod.Shaman.Projectiles.OreOrbs.Large;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class TrueSanctify : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 62;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<TrueSanctifyProj>();
			this.empowermentType = 5;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("True Sanctify");
			Tooltip.SetDefault("Casts pure light projectiles to purge your foes"
							  + "\nHitting enemies will gradually grant you hallowed orbs"
							  + "\nWhen reaching 7 orbs, they will break free and home into your enemies"
							  + "\nHaving 3 or more active shamanic bonds will release homing projectiles");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			if (modPlayer.GetNbShamanicBonds() > 2)
			{
				int typeAlt = ModContent.ProjectileType<TrueSanctifyProjAlt>();
				int newDamage = (int)(Item.damage * 0.75);
				for (int i = -1; i < 3; i += 2)
				{
					Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(20 * i));
					this.NewShamanProjectile(player, source, position, newVelocity, typeAlt, newDamage, knockback);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Sanctify>(), 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BrokenHeroFragment").Type : ItemType<BrokenHeroScepter>(), (thoriumMod != null) ? 2 : 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
