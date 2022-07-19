using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Misc
{
	public class RCRemote : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ZephyrFish);
			Item.shoot = ModContent.ProjectileType<General.Projectiles.Pets.RCRemotePet>();
			Item.buffType = ModContent.BuffType<General.Buffs.RCRemoteBuff>();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RC Remote");
			Tooltip.SetDefault("'Highly advanced elven technology enabling control over new, recently developed non-reindeer aircraft'");
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
}