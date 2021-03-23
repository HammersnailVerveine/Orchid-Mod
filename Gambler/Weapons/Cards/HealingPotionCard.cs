using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class HealingPotionCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 0;
			item.crit = 0;
			item.knockBack = 0f;
			item.useAnimation = 15;
			item.useTime = 15;
			item.shootSpeed = 1f;
			this.cardRequirement = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Lesser Healing");
		    Tooltip.SetDefault("Rapidly heals when used");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			if (!dummy) {
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				modPlayer.gamblerShuffleCooldown -= (int)(modPlayer.gamblerShuffleCooldownMax / 5);
				if (modPlayer.gamblerShuffleCooldown < 0) modPlayer.gamblerShuffleCooldown = 0;
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(4, true);
				player.statLife += 4;	
			}
		}
	}
}
