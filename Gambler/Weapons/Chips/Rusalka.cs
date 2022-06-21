using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class Rusalka : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 60;
			Item.useTime = 20;
			Item.knockBack = 4f;
			Item.damage = 37;
			Item.crit = 4;
			Item.rare = ItemRarityID.Green;
			Item.shootSpeed = 15f;
			Item.shoot = ProjectileType<Gambler.Projectiles.Chips.RusalkaProj>();
			Item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 75;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed)
		{
			velocity = new Vector2(0f, speed).RotatedBy(MathHelper.ToRadians(modPlayer.gamblerChipSpin));
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockBack, player.whoAmI);
			
			int projType = ProjectileType<Gambler.Projectiles.Chips.RusalkaProjAlt>();
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.05f, velocity.Y * 1.05f, projType, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusalka");
			Tooltip.SetDefault("Sends delayed projectiles, dealing 50% more damage"
							+ "\n25% chance not to consume chip");
		}
	}
}
