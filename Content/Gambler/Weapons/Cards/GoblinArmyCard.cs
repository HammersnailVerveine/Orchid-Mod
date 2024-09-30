using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class GoblinArmyCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 55;
			Item.knockBack = 10f;
			Item.useAnimation = 60;
			Item.useTime = 60;
			Item.shootSpeed = 0.75f;

			this.cardRequirement = 4;
			cardSets.Add(GamblerCardSet.Elemental);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Erratic Magic");
			/* Tooltip.SetDefault("Throws a portal, firing shadow bolts at an increasing rate"
							+ "\nThe portal will disappear upon releasing left click or getting too far from it"); */
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Content.Gambler.Projectiles.GoblinArmyCardProj>();
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
				for (int i = 0; i < 2; i++)
				{
					int newProjInt = DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
					Projectile newProj = Main.projectile[newProjInt];
					newProj.ai[1] = i + 1;
					newProj.netUpdate = true;
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
