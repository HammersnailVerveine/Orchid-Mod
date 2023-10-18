using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class StarScouterScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 40;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = false;
			Item.shootSpeed = 7f;
			//Item.shoot = ModContent.ProjectileType<Projectiles.Thorium.StarScouterScepterProj>();
			this.Element = 3;
			this.energy = 13;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Orbital Minefield");
			/* Tooltip.SetDefault("Launches an orbital mine, activating after a while"
							+ "\nIf you have 3 or more bonds, the explosion will release additional bombs"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
			this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			return false;
		}
	}
}

