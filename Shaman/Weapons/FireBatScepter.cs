using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons
{
	public class FireBatScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 23;
			item.width = 30;
			item.height = 30;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 3.25f;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 54, 0);
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shootSpeed = 16f;
			item.shoot = mod.ProjectileType("FireBatScepterProj");
			this.empowermentType = 3;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Bat Scepter");
			Tooltip.SetDefault("Shoots fiery bats at your foes"
							  + "\nThe weapon speed increases slightly with the number of active shamanic bonds"
							  + "\nIf you have 3 or more active shamanic bonds, the bats will home towards enemies");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			item.useTime = 35 - 3 * nbBonds;
			item.useAnimation = 35 - 3 * nbBonds;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			int numberProjectiles = 1 + Main.rand.Next(2);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, nbBonds < 3 ? type : mod.ProjectileType("FireBatScepterProjHoming"), damage, knockBack, player.whoAmI);
			}

			return false;
		}
	}
}

