using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class IceFlakeCone : OrchidModShamanItem
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Flake");
			Tooltip.SetDefault("Shoots returning ice blades"
							  + "\nThe maximum number of projectiles launched depends on the number of active shamanic bonds");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 72;
			item.width = 42;
			item.height = 42;
			item.useTime = 10;
			item.useAnimation = 10;
			item.knockBack = 1.15f;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 4, 80, 0);
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 12f;
			item.shoot = ModContent.ProjectileType<Projectiles.IceFlakeConeProj>();

			this.empowermentType = 2;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 3;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			int buffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			int numberProjectiles = 1 + Main.rand.Next(2 + buffsCount);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}

			return false;
		}
	}
}
