using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class GoblinStick : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 54;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 60;
			Item.knockBack = 3.15f;
			Item.rare = 5;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item103;
			Item.autoReuse = false;
			Item.shootSpeed = 3f;
			Item.shoot = Mod.Find<ModProjectile>("GoblinStickProj").Type;
			this.empowermentType = 3;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Stick");
			Tooltip.SetDefault("Channels a volley of shadowflame balls"
							  + "\nThe number of projectiles shot during the channeling depends on the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			switch (nbBonds)
			{
				case 0:
					Item.useTime = 60;
					break;
				case 1:
					Item.useTime = 30;
					break;
				case 2:
					Item.useTime = 20;
					break;
				case 3:
					Item.useTime = 15;
					break;
				case 4:
					Item.useTime = 12;
					break;
				case 5:
					Item.useTime = 10;
					break;
			}
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
