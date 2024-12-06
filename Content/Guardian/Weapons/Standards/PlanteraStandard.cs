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
			Item.useTime = 36;
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

		public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			standardStats.lifeMax += 50;
			if (reinforced && affectedPlayer.statLife <= affectedPlayer.statLifeMax2 * 0.25f && isLocalPlayer)
			{
				standardStats.lifeRegen += 6;
				if (Main.rand.NextBool(20))
				{
					Dust.NewDustDirect(affectedPlayer.position, affectedPlayer.width, affectedPlayer.height, DustID.PlanteraBulb).noGravity = true;
				}

				if (Main.rand.NextBool(30))
				{
					Dust.NewDustDirect(affectedPlayer.position, affectedPlayer.width, affectedPlayer.height, DustID.HealingPlus);
				}
			}
			return true;
		}
	}
}
