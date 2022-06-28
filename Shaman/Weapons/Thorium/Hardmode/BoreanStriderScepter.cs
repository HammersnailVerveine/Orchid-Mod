using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class BoreanStriderScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 35;
			Item.width = 56;
			Item.height = 56;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.knockBack = 2.75f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Thorium.BoreanStriderScepterProj>();
			this.empowermentType = 2;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Taiga Truncheon");
			Tooltip.SetDefault("Fires out a damaging frost ball"
							+ "\nReleases icicles on impact, based on your number of active shamanic bonds");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
			this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}
	}
}

