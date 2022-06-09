using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class HallowedCone : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 38;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 1.15f;
			Item.rare = 4;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item101;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("CrystalScepterProj").Type;
			this.empowermentType = 5;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Resonator");
			Tooltip.SetDefault("Shoots dividing crystalline beams"
							  + "\nHitting with the rupture point deals increased damage"
							  + "\nThe number of projectiles caused by the division scales with the number of active shamanic bonds");
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Pink.ToVector3() * 0.55f * Main.essScale);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(null, "RitualScepter", 1);
			recipe.AddIngredient(ItemID.CrystalShard, 20);
			recipe.AddIngredient(ItemID.SoulofLight, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
