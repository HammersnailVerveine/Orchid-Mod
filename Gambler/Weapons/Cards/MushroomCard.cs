using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class MushroomCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 24;
			Item.knockBack = 2f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 10f;

			this.cardRequirement = 2;
			this.cardSets = GamblerCardSets.Biome;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Glowing Mushroom");
			Tooltip.SetDefault("Summons a mushroom bush above your head"
							+ "\nDrag and release a mushroom to launch it"
							+ "\nEmpowers once after bouncing");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Projectiles.MushroomCardBase>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI && proj.ai[1] != 1f)
				{
					found = true;
					break;
				}
			}
			if (!found) {
				DummyProjectile(Projectile.NewProjectile(source, position, Vector2.Zero, projType, damage, knockback, player.whoAmI), dummy);
			}
		}
	}
}
