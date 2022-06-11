using Microsoft.Xna.Framework;
using Terraria;
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
			Item.rare = 5;
			Item.value = Item.sellPrice(0, 4, 55, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			Item.shoot = Mod.Find<ModProjectile>("SanctifyProj").Type;
			this.empowermentType = 5;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Sanctify");
			Tooltip.SetDefault("Casts radiant projectiles to purge your foes"
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
					this.NewShamanProjectile(position.X, position.Y, projectileVelocity.X, projectileVelocity.Y, Mod.Find<ModProjectile>("SanctifyProjAlt").Type, (int)(Item.damage * 0.75), knockBack, Item.playerIndexTheItemIsReservedFor);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
