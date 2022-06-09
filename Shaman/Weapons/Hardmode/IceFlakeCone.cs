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
			Item.damage = 72;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 4, 80, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<Projectiles.IceFlakeConeProj>();

			this.empowermentType = 2;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 3;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			int buffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
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
