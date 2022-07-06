using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SnowCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 24;
			Item.knockBack = 1f;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Snow");
			Tooltip.SetDefault("Summons a snow bush above your head"
							+ "\nDrag and release a fruit to launch it");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Projectiles.SnowCardBase>();
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
