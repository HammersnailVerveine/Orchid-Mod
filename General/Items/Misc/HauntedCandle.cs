using Terraria;
using Terraria.ID;

namespace OrchidMod.General.Items.Misc
{
	public class HauntedCandle : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Candle");
			Tooltip.SetDefault("Summons a strange candle to lighten up your way");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.useStyle = 1;
			item.shoot = mod.ProjectileType("HauntedCandle");
			item.width = 16;
			item.height = 30;
			item.UseSound = SoundID.Item2;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 1;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.buffType = mod.BuffType("HauntedCandle");
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}