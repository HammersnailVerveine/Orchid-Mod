using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace OrchidMod.Shaman.Weapons
{
	public class AvalancheScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 19;
			item.width = 36;
			item.height = 36;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 3f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item28;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("IceSpearScepterProj");
			this.empowermentType = 2;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Avalanche");
			Tooltip.SetDefault("Hitting will spawn and empower a giant icicle above your head"
							  + "\nAfter enough hits, the icicle will be launched, dealing massive damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			this.NewShamanProjectile(position.X + Main.rand.Next(50) - 35, position.Y - Main.rand.Next(14) + 7, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
