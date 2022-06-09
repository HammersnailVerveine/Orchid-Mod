using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 10;
			Item.crit = 4;
			Item.knockBack = 0.5f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 3f;
			Item.UseSound = SoundID.Item1;
			Item.channel = true;

			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			type = ModContent.ProjectileType<Gambler.Projectiles.SapCardProj>();

			SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 1);

			if (player.ownedProjectileCounts[type] == 0 && player.channel) OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI), dummy);
		}
	}
}
