using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
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
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item101;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			//Item.shoot = ModContent.ProjectileType<CrystalScepterProj>();
			this.Element = ShamanElement.SPIRIT;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Prismatic Resonator");
			/* Tooltip.SetDefault("Shoots dividing crystalline beams"
							  + "\nHitting with the rupture point deals increased damage"
							  + "\nThe number of projectiles caused by the division scales with the number of active shamanic bonds"); */
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Pink.ToVector3() * 0.55f * Main.essScale);
		}

		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ModContent.ItemType<Misc.RitualScepter>(), 1)
			.AddIngredient(ItemID.CrystalShard, 20)
			.AddIngredient(ItemID.SoulofLight, 15)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		*/
	}
}
