using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class TrueSanctify : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 62;
			item.width = 30;
			item.height = 30;
			item.useTime = 22;
			item.useAnimation = 22;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 9f;
			item.shoot = mod.ProjectileType("TrueSanctifyProj");
			this.empowermentType = 5;
			this.energy = 9;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("True Sanctify");
			Tooltip.SetDefault("Casts pure light projectiles to purge your foes"
							  + "\nHitting enemies will gradually grant you hallowed orbs"
							  + "\nWhen reaching 7 orbs, they will break free and home into your enemies"
							  + "\nHaving 3 or more active shamanic bonds will release homing projectiles");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 2)
			{
				for (int i = 0; i < 2; i++)
				{
					Vector2 projectileVelocity = (new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(i == 0 ? -20 : 20)));
					this.NewShamanProjectile(position.X, position.Y, projectileVelocity.X, projectileVelocity.Y, mod.ProjectileType("TrueSanctifyProjAlt"), (int)(item.damage * 0.75), knockBack, item.owner);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<Sanctify>(), 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BrokenHeroFragment") : ItemType<BrokenHeroScepter>(), (thoriumMod != null) ? 2 : 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
