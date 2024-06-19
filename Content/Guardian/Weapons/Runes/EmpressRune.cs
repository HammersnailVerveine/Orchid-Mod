using OrchidMod.Content.Guardian.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class EmpressRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 4, 85);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item176;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 2f;
			Item.damage = 254;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.EmpressRuneProj>();
			RuneCost = 4;
			RuneNumber = 2;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			for (int i  = 0; i < GetNumber(guardian) * 2; i ++)
			{
				int offset = (60 - 4 * i);
				if (offset < 20) offset = 20;
				NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, 100 + offset * i, 1, Main.rand.NextFloat(360));
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GuardianEmpressMaterial>(10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
