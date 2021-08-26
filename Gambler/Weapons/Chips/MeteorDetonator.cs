using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class MeteorDetonator : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.width = 24;
			item.height = 36;
			item.useStyle = 4;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 60;
			item.useTime = 60;
			item.knockBack = 1f;
			item.damage = 30;
			item.crit = 4;
			item.rare = 1;
			item.shootSpeed = 0f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.MeteorDetonatorProj>();
			item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
			return false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Detonator");
			Tooltip.SetDefault("Uses up to 5 chips to create a powerful explosion around you");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			this.chipCost = modPlayer.gamblerChips > 0 ? modPlayer.gamblerChips > 5 ? 5 : modPlayer.gamblerChips : 1;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbChips = modPlayer.gamblerChips > 5 ? 5 : modPlayer.gamblerChips;
			mult *= modPlayer.gamblerDamage * nbChips;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
