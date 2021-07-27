using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Misc
{
	public class RCRemote : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("RC Remote");
			Tooltip.SetDefault("'Highly advanced elven technology enabling control over new, recently developed non-reindeer aircraft'");
		}

		public override void SetDefaults() {
			item.CloneDefaults(ItemID.ZephyrFish);
			item.shoot = ModContent.ProjectileType<General.Projectiles.Pets.RCRemotePet>();
			item.buffType = ModContent.BuffType<General.Buffs.RCRemoteBuff>();
		}
		
		public override void UseStyle(Player player) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}