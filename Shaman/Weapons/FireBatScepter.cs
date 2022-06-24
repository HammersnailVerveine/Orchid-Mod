using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
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
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shootSpeed = 16f;
			Item.shoot = ModContent.ProjectileType<FireBatScepterProj>();
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
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			Item.useTime = 35 - 3 * nbBonds;
			Item.useAnimation = 35 - 3 * nbBonds;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			int numberProjectiles = 1 + Main.rand.Next(2);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(25));
				int newType = nbBonds < 3 ? type : ModContent.ProjectileType<FireBatScepterProjHoming>();
				this.NewShamanProjectile(player, source, position, newVelocity, newType, damage, knockback);
			}

			return false;
		}
	}
}

