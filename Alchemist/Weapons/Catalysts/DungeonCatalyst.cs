using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace OrchidMod.Alchemist.Weapons.Catalysts
{
	public class DungeonCatalyst : OrchidModAlchemistCatalyst
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripple");
			Tooltip.SetDefault("Used to interact with alchemist catalytic elements"
							+ "\nUpon successful catalysis, releases a burst of homing water bolts"
							+ "\nHit an enemy to apply catalyzed"
							+ "\nCatalyzed replaces most alchemical debuffs");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.damage = 21;
			Item.crit = 4;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			this.catalystType = 1;
		}

		public override void CatalystInteractionEffect(Player player)
		{
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			int dmg = Item.damage;
			int rand = Main.rand.Next(3);
			for (int i = 0; i < 4 + rand; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				SpawnProjectile(player.GetSource_ItemUse(Item), player.Center, vel, ProjectileType<Projectiles.Reactive.ReactiveSpawn.DungeonCatalystProj>(), dmg, 0f, player.whoAmI);
			}
		}
	}
}
