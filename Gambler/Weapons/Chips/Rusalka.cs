using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class Rusalka : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 60;
			item.useTime = 20;
			item.knockBack = 4f;
			item.damage = 37;
			item.crit = 4;
			item.rare = 2;
			item.shootSpeed = 15f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.RusalkaProj>();
			item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 75;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed)
		{
			Vector2 velocity = new Vector2(0f, speed).RotatedBy(MathHelper.ToRadians(modPlayer.gamblerChipSpin));
			Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, item.shoot, damage, knockBack, player.whoAmI);
			
			int projType = ProjectileType<Gambler.Projectiles.Chips.RusalkaProjAlt>();
			Projectile.NewProjectile(position.X, position.Y, velocity.X * 1.05f, velocity.Y * 1.05f, projType, damage, knockBack, player.whoAmI);
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
