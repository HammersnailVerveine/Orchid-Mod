using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
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
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item91;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			//Item.shoot = ModContent.ProjectileType<MartianBeamerProj>();
			this.Element = ShamanElement.FIRE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Martian Beamer");
			/* Tooltip.SetDefault("Shoots martian homing lasers"
							  + "\nWeapon speed increases with the number of active shamanic bonds"); */

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				thoriumMod.Call("AddMartianItemID", Item.type);
			}
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.CountShamanicBonds();

			Item.useTime = 18 - (2 * nbBonds);
			Item.useAnimation = 18 - (2 * nbBonds);
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = -1; i < 2; i += 2)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20 * i));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}
	}
}