using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class BulbScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 65;
			item.width = 50;
			item.height = 50;
			item.useTime = 36;
			item.useAnimation = 36;
			item.knockBack = 3.15f;
			item.rare = 7;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item42;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("BulbScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bulb Scepter");
			Tooltip.SetDefault("Shoots spiky seeds, gaining in height on each bounce."
							+ "\nThe number of seeds cast depends on the number of active shamanic bonds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			int numberProjectiles = 1 + Main.rand.Next(2) + nbBonds;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
				this.newShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
