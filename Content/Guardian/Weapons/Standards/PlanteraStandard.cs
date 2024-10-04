using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class PlanteraStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 30;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			SlamStacks = 2;
			GuardStacks = 1;
			FlagOffset = 4;
			AuraRange = 8;
			StandardDuration = 2400;
			AffectNearbyPlayers = true;
		}

		public override Color GetColor()
		{
			return new Color(225, 128, 206);
		}

		public override void NearbyPlayerEffect(Player player, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			player.statLifeMax2 += 50;
			if (reinforced && player.statLife <= player.statLifeMax2 * 0.25f && isLocalPlayer)
			{
				guardian.GuardianPlanteraStandardHeal = true;

				if (Main.rand.NextBool(20))
				{
					Dust.NewDustDirect(player.position, player.width, player.height, DustID.PlanteraBulb).noGravity = true;
				}
			}
		}

		public override void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{
		}
	}
}
