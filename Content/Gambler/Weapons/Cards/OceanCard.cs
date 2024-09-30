using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class OceanCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 20;
			Item.knockBack = 5f;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.shootSpeed = 5f;

			this.cardRequirement = 1;
			cardSets.Add(GamblerCardSet.Biome);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Ocean");
			/* Tooltip.SetDefault("Summons a palm bush above your head"
							+ "\nDrag and release a coconut to launch it"
							+ "\nA harsh landing will make the coconut explode"); */
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Content.Gambler.Projectiles.OceanCardBase>();
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
			if (!found)
			{
				DummyProjectile(Projectile.NewProjectile(source, position, Vector2.Zero, projType, damage, knockback, player.whoAmI), dummy);
			}
		}
	}
}
