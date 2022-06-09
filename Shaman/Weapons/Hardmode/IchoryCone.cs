using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class IchoryCone : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 30;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 4.15f;
			Item.rare = 4;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item13;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = Mod.Find<ModProjectile>("IchoryConeProj").Type;
			this.empowermentType = 1;
			this.energy = 4;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Scepter");
			Tooltip.SetDefault("Sprays your enemies with piercing ichor bursts"
							  + "\nThe first enemy hit will fill an ichor cyst above you"
							  + "\nYour next hit after the cyst is full will release a shower of ichor in the direction you're moving");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(null, "RitualScepter", 1);
			recipe.AddIngredient(ItemID.Ichor, 20);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
