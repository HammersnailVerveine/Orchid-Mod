using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class GamblingChip : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 60;
			item.useTime = 30;
			item.knockBack = 5f;
			item.damage = 25;
			item.crit = 4;
			item.rare = 1;
			item.shootSpeed = 10f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.GamblingChipProj>();
			item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambling Chip");
			Tooltip.SetDefault("Throws gambling chips at your foes");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed) {
			Vector2 velocity = new Vector2(0f, speed).RotatedBy(MathHelper.ToRadians(modPlayer.gamblerChipSpin));
			Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, item.shoot, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
