using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class IceFlakeCone : OrchidModShamanItem
	{
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
			Item.shoot = ModContent.ProjectileType<IceFlakeConeProj>();

			Element = 2;
			catalystType = ShamanCatalystType.ROTATE;
			energy = 3;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Flake");
			/* Tooltip.SetDefault("Shoots returning ice blades\n" +
							   "The maximum number of projectiles launched depends on the number of active shamanic bonds"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			int buffsCount = modPlayer.GetNbShamanicBonds();
			int numberProjectiles = 1 + Main.rand.Next(2 + buffsCount);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}

			return false;
		}
	}
}
