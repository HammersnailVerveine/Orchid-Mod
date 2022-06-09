using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class MartianBeamer : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 70;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 1.15f;
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item91;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			Item.shoot = Mod.Find<ModProjectile>("MartianBeamerProj").Type;
			this.empowermentType = 1;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Martian Beamer");
			Tooltip.SetDefault("Shoots martian homing lasers"
							  + "\nWeapon speed increases with the number of active shamanic bonds");

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				thoriumMod.Call("AddMartianItemID", Item.type);
			}
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			Item.useTime = 18 - (2 * nbBonds);
			Item.useAnimation = 18 - (2 * nbBonds);
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < Main.rand.Next(1, 1); i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

				perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(-20));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}