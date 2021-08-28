using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class CorruptCone : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 30;
			item.width = 60;
			item.height = 60;
			item.useTime = 10;
			item.useAnimation = 10;
			item.knockBack = 4.15f;
			item.rare = 4;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.UseSound = SoundID.Item13;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("CorruptConeProj");
			this.empowermentType = 1;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupt Scepter");
			Tooltip.SetDefault("Sprays your enemies with piercing corrupt flames"
							  + "\nThe first enemy hit will fill a corrupt tank above you"
							  + "\nYour next hit after the tank is full will release a shower of corrupt flames in the direction you're moving");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RitualScepter", 1);
			recipe.AddIngredient(ItemID.CursedFlame, 20);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
