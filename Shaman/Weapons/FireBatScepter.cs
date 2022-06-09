using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class FireBatScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 25;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 3.25f;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shootSpeed = 16f;
			Item.shoot = Mod.Find<ModProjectile>("FireBatScepterProj").Type;
			this.empowermentType = 3;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
			this.energy = 6;
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
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			Item.useTime = 35 - 3 * nbBonds;
			Item.useAnimation = 35 - 3 * nbBonds;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			int numberProjectiles = 1 + Main.rand.Next(2);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, nbBonds < 3 ? type : Mod.Find<ModProjectile>("FireBatScepterProjHoming").Type, damage, knockBack, player.whoAmI);
			}

			return false;
		}
	}
}

