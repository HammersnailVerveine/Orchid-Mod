using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Catalysts
{
	public class DungeonCatalyst : OrchidModAlchemistCatalyst
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ripple");
			Tooltip.SetDefault("Used to interact with alchemist catalytic elements"
							+ "\nUpon successful catalysis, releases a burst of homing water bolts"
							+ "\nHit an enemy to apply catalyzed"
							+ "\nCatalyzed replaces most alchemical debuffs");
		}
		
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.rare = 2;
			item.damage = 21;
			item.crit = 4;
			item.value = Item.sellPrice(0, 0, 75, 0);
			this.catalystType = 1;
		}
		
		public override void CatalystInteractionEffect(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dmg = (int)(item.damage * modPlayer.alchemistDamage);
			int rand = Main.rand.Next(3);
			for (int i = 0 ; i < 4 + rand ; i ++) {
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y,  ProjectileType<Alchemist.Projectiles.Reactive.ReactiveSpawn.DungeonCatalystProj>(), dmg, 0f, player.whoAmI);
			}
		}
	}
}
