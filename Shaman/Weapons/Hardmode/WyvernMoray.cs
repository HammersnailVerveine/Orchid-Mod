using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class WyvernMoray : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 28;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 3.8f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<Projectiles.WyvernMorayProj>();
			this.Element = 3;
			this.energy = 13;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Wyvern Moray");
			/* Tooltip.SetDefault("Spits lingering cloud energy"
							  + "\nThe weapon itself can critically strike, releasing a more powerful projectile"
							  + "\nThe more shamanic bonds you have, the higher the chances of critical strike"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			/*
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			var proj = Main.projectile[this.NewShamanProjectile(player, source, position, velocity, type, damage * 2, knockback)];
			if (proj.ModProjectile is Projectiles.WyvernMorayProj modProj)
			{
				modProj.Improved = (Main.rand.Next(101) < 4 + nbBonds * 4);
				proj.netUpdate = true;
			}
			*/

			return false;
		}
	}
}
