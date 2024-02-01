using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class IceMimicScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 63;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.UseSound = SoundID.Item28;
			Item.autoReuse = false;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<IceMimicScepterProj>();
			this.Element = ShamanElement.WATER;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Cycle");
			/* Tooltip.SetDefault("Releases a glacial spike, repeatedly impaling the closest enemy"
							  + "\nHaving 3 or more active shamanic bonds increases the spike attack rate"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
					proj.active = false;
			}

			Vector2 newVelocity = new Vector2(0f, velocity.Length() * -1f);
			float projAI = modPlayer.CountShamanicBonds() > 2 ? 3f : 0f;
			this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback, ai1: projAI);
			return false;
		}
	}
}
