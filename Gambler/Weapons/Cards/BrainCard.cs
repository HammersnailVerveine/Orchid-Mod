using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class BrainCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 40;
			Item.knockBack = 1f;
			Item.useAnimation = 30;
			Item.useTime = 30;

			this.cardRequirement = 5;
			this.cardSets = GamblerCardSets.Boss;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : The Hivemind");
			/* Tooltip.SetDefault("Summons 3 brains around you, one of them following your cursor"
							+ "\nOnly one of them is real, and deals contact damage"
							+ "\nHitting randomly changes the true brain, and increases damage a lot"
							+ "\nBrains cannot deal damage if they are too close to you"); */
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Projectiles.BrainCardProj>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					found = true;
					break;
				}
			}
			if (!found)
			{
				for (int i = 0; i < 3; i++)
				{
					int newProj = DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockBack, player.whoAmI), dummy);
					Main.projectile[newProj].ai[1] = (float)(i);
					Main.projectile[newProj].ai[0] = (float)(i == 0 ? 300 : 0);
					Main.projectile[newProj].friendly = i == 0;
					Main.projectile[newProj].netUpdate = true;
				}
				SoundEngine.PlaySound(SoundID.Item1);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Item7);
			}
		}
	}
}
