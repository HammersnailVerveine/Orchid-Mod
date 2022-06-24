using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class HealingPotionCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 0;
			Item.crit = 0;
			Item.knockBack = 0f;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.shootSpeed = 1f;
			this.cardRequirement = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Lesser Healing");
			Tooltip.SetDefault("Rapidly heals when used");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool dummy = false)
		{
			if (!dummy)
			{
				OrchidModPlayerGambler modPlayer = player.GetModPlayer<OrchidModPlayerGambler>();
				modPlayer.gamblerShuffleCooldown -= (int)(modPlayer.gamblerShuffleCooldownMax / 5);
				if (modPlayer.gamblerShuffleCooldown < 0) modPlayer.gamblerShuffleCooldown = 0;
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(4, true);
				player.statLife += 4;
			}
		}
	}
}
