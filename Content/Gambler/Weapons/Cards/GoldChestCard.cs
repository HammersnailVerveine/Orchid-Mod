using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class GoldChestCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 16;
			Item.knockBack = 1f;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.shootSpeed = 10f;

			this.cardRequirement = 3;
			cardSets.Add(GamblerCardSet.Elemental);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Enchantment");
			// Tooltip.SetDefault("Releases damaging sparkles");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Content.Gambler.Projectiles.GoldChestCardProj>();
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(15)) * scale;
			int newProj = DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
			Main.projectile[newProj].ai[1] = Main.rand.Next(4);
			Main.projectile[newProj].netUpdate = true;
			SoundEngine.PlaySound(SoundID.Item1);
		}
	}
}
