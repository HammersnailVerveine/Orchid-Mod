using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace OrchidMod.Content.Shaman.Weapons
{
	public class AvalancheScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 19;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 3f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item28;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			////Item.shoot = ModContent.ProjectileType<IceSpearScepterProj>();
			this.Element = ShamanElement.WATER;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Avalanche");
			/* Tooltip.SetDefault("Hitting will spawn and empower a giant icicle above your head"
							  + "\nAfter enough hits, the icicle will be launched, dealing massive damage"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newPosition = new Vector2(position.X + Main.rand.Next(50) - 35, position.Y - Main.rand.Next(14) + 7);
			this.NewShamanProjectile(player, source, newPosition, velocity, type, damage, knockback);
			return false;
		}
	}
}
