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
			Item.damage = 62;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.knockBack = 1.15f;
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = Mod.Find<ModProjectile>("TrueSanctifyProj").Type;
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

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) > 2)
			{
				for (int i = 0; i < 2; i++)
				{
					Vector2 projectileVelocity = (new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(i == 0 ? -20 : 20)));
					this.NewShamanProjectile(position.X, position.Y, projectileVelocity.X, projectileVelocity.Y, Mod.Find<ModProjectile>("TrueSanctifyProjAlt").Type, (int)(Item.damage * 0.75), knockBack, Item.playerIndexTheItemIsReservedFor);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(ItemType<Sanctify>(), 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BrokenHeroFragment").Type : ItemType<BrokenHeroScepter>(), (thoriumMod != null) ? 2 : 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
