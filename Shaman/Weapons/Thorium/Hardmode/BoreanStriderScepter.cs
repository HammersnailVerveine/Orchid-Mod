using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class BoreanStriderScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = 56;
			item.height = 56;
			item.useTime = 34;
			item.useAnimation = 34;
			item.knockBack = 2.75f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = ModContent.ProjectileType<Projectiles.Thorium.BoreanStriderScepterProj>();

			this.empowermentType = 2;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Taiga Truncheon");
			Tooltip.SetDefault("Fires out a damaging frost ball"
							+ "\nReleases icicles on impact, based on your number of active shamanic bonds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
			this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}

