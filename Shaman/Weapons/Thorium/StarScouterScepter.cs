using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class StarScouterScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

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
			Item.shoot = ModContent.ProjectileType<Projectiles.Thorium.StarScouterScepterProj>();

			this.empowermentType = 3;
			this.energy = 13;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Orbital Minefield");
			Tooltip.SetDefault("Launches an orbital mine, activating after a while"
							+ "\nIf you have 3 or more bonds, the explosion will release additional bombs");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
			this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}

