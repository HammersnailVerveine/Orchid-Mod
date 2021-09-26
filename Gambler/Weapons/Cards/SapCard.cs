using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SapCard : OrchidModGamblerItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Unstable Sap");
			Tooltip.SetDefault("Releases a slow-moving sap bubble, following the cursor"
							+ "\nUpon releasing the mouse click, the bubble will explode"
							+ "\nThe longer the bubble exists, the more explosion damage");
		}

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Blue;
			item.damage = 10;
			item.crit = 4;
			item.knockBack = 0.5f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 3f;
			item.UseSound = SoundID.Item1;
			item.channel = true;

			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			type = ModContent.ProjectileType<Gambler.Projectiles.SapCardProj>();

			Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 1);

			if (player.ownedProjectileCounts[type] == 0 && player.channel) OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI), dummy);
		}
	}
}
