using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Guardian.Weapons.Runes
{
	public class LivingRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 0, 15);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 5f;
			Item.damage = 15;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.LivingRuneProj>();
			this.cost = 2;
			this.duration = 3600;
			this.distance = 120f;
			this.number = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Rune");
			Tooltip.SetDefault("Summons sap bubbles around you");
		}
	}
}
