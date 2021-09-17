using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class WyvernMoray : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 32;
			item.width = 52;
			item.height = 52;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 3.8f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = ModContent.ProjectileType<Projectiles.WyvernMorayProj>();
			this.empowermentType = 3;
			this.energy = 13;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Moray");
			Tooltip.SetDefault("Spits lingering cloud energy"
							  + "\nThe weapon itself can critically strike, releasing a more powerful projectile"
							  + "\nThe more shamanic bonds you have, the higher the chances of critical strike");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			var proj = Main.projectile[this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, damage * 2, knockBack, player.whoAmI)];
			if (proj.modProjectile is Projectiles.WyvernMorayProj modProj)
			{
				modProj.Improved = (Main.rand.Next(101) < 4 + nbBonds * 4);
				proj.netUpdate = true;
			}

			return false;
		}
	}
}
