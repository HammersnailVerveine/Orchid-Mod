using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles.OreOrbs.Large;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class Sanctify : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 45;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 4, 55, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			Item.shoot = ModContent.ProjectileType<SanctifyProj>();
			this.Element = 5;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Sanctify");
			/* Tooltip.SetDefault("Hitting enemies will gradually grant you hallowed orbs"
							  + "\nUpon reaching 7 orbs, they will break free and home into your enemies"
							  + "\nHaving 3 or more active shamanic bonds will release bonus homing projectiles"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			if (modPlayer.GetNbShamanicBonds() > 2)
			{
				int typeAlt = ModContent.ProjectileType<SanctifyProjAlt>();
				int newDamage = (int)(Item.damage * 0.75);
				for (int i = -1; i < 3; i+= 2)
				{
					Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(20 * i));
					this.NewShamanProjectile(player, source, position, newVelocity, typeAlt, newDamage, knockback);
				}
			}

			return true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
