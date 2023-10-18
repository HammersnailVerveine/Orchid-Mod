using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class BulbScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 65;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item42;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<BulbScepterProj>();
			this.Element = 4;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Bulb Scepter");
			/* Tooltip.SetDefault("Shoots spiky seeds, gaining in height on each bounce."
							+ "\nThe number of seeds cast depends on the number of active shamanic bonds"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			int rand = 1 + Main.rand.Next(2) + nbBonds;
			for (int i = 0; i < rand; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(12));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}
	}
}
