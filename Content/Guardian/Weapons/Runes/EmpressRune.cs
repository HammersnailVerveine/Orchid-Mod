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
			Item.value = Item.sellPrice(0, 3, 25, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item176;
			Item.useTime = 25;
			Item.knockBack = 2f;
			Item.damage = 254;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.EmpressRuneProj>();
			RuneCost = 4;
			RuneNumber = 6;
			RuneDuration = 45 * 60;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int amount)
		{
			for (int i  = 0; i < amount; i ++)
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
