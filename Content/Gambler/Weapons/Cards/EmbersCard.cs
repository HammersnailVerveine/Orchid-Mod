using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class EmbersCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 7;
			Item.knockBack = 1f;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.shootSpeed = 5f;

			this.cardRequirement = 0;
			cardSets.Add(GamblerCardSet.Elemental);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Embers");
			// Tooltip.SetDefault("Releases homing embers");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(15)) / 5f;
			int projType = ProjectileType<Projectiles.EmbersCardProj>();
			DummyProjectile(Projectile.NewProjectile(source, position, vel, projType, damage, knockback, player.whoAmI), dummy);
			SoundEngine.PlaySound(SoundID.Item1);
		}
	}
}
