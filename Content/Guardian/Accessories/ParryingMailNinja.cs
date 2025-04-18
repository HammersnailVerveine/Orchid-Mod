using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class ParryingMailNinja : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianParryDuration += 0.25f;
			modPlayer.GuardianSharpRebuttalParry = true;
			player.GetModPlayer<OrchidPlayer>().OrchidDodgeChance *= 0.88f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
			player.longInvince = true;
			player.dashType = 1;
			player.spikedBoots = 2;
		}
	}
}