using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			Item.damage = 0;
			Item.useStyle = 1;
			Item.shoot = Mod.Find<ModProjectile>("HauntedCandle").Type;
			Item.width = 16;
			Item.height = 30;
			Item.UseSound = SoundID.Item2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.buffType = Mod.Find<ModBuff>("HauntedCandle").Type;
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