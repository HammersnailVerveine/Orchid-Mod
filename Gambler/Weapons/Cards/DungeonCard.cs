using Microsoft.Xna.Framework;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class DungeonCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 20;
			Item.crit = 10;
			Item.knockBack = 1f;
			Item.useAnimation = 45;
			Item.useTime = 15;
			Item.reuseDelay = 65;
			Item.shootSpeed = 1f;
			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Spirit Tear");
			Tooltip.SetDefault("Fires a burst of spirit bolts"
							+ "\nHitting the same target with all projectiles will rip a part of their soul"
							+ "\nPicking it up will deal a large amount of damage");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.DungeonCardProj>();
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
				for (int k = 0; k < Main.npc.Length; k++)
				{
					if (Main.npc[k].active)
					{
						OrchidGlobalNPC modTarget = Main.npc[k].GetGlobalNPC<OrchidGlobalNPC>();
						modTarget.GamblerDungeonCardCount = 0;
					}
				}
			}

			int newProj = DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
			Main.projectile[newProj].ai[1] = Main.rand.Next(4);
			Main.projectile[newProj].netUpdate = true;
			SoundEngine.PlaySound(SoundID.Item8);
		}
	}
}
