using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SkyCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 21;
			Item.knockBack = 2f;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.shootSpeed = 8f;

			this.cardRequirement = 3;
			this.cardSets = GamblerCardSets.Biome;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Sky");
			Tooltip.SetDefault("Summons a cloud bush above your head"
							+ "\nDrag and release a fruit to launch it"
							+ "\nEvery third shot will release a star"
							+ "\nAim at the star to create more projectiles");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Gambler.Projectiles.SkyCardBase>();
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
