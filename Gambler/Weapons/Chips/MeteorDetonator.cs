using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class MeteorDetonator : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.width = 24;
			Item.height = 36;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 90;
			Item.useTime = 90;
			Item.knockBack = 1f;
			Item.damage = 30;
			Item.crit = 4;
			Item.rare = 1;
			Item.shootSpeed = 0f;
			Item.shoot = ProjectileType<Gambler.Projectiles.Chips.MeteorDetonatorProj>();
			Item.autoReuse = true;
			this.chipCost = 3;
			this.consumeChance = 100;
			this.pauseRotation = false;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			SoundEngine.PlaySound(2, (int)position.X, (int)position.Y, 14);
			return false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Detonator");
			Tooltip.SetDefault("Timed explosions will deal double damage in a larger radius");
		}
		
		public sealed override void SafeHoldItem(Player player, OrchidModPlayer modPlayer)
		{
			modPlayer.gamblerUIChipSpinDisplay = false;
		}

		public override void SafeModifyWeaponDamage(Player player, OrchidModPlayer modPlayer, ref float add, ref float mult, ref float flat)
		{
			float index = 720f / 8;
			bool timed = (modPlayer.gamblerChipSpin > index * 4 && modPlayer.gamblerChipSpin < index * 5);
			mult *= (timed ? 5 : 2.5f);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
