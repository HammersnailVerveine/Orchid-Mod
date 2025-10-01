using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class DesertStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 25;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			SlamStacks = 1;
			FlagOffset = 8;
			AuraRange = 18;
			StandardDuration = 1200;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
		}
		public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (isNPC && isOwner && isReinforced) || (isPlayer && !PlayerisOwner);

		public override Color GetColor()
		{
			return new Color(24, 204, 223);
		}

		public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			standardStats.moveSpeed += 0.15f;
			if (reinforced && isLocalPlayer)
			{
				guardian.GuardianStandardDesert = true;
			}
			return true;
		}

		public override void ExtraAIStandardWorn(GuardianStandardAnchor anchor, Projectile projectile, Player player, OrchidGuardian guardian)
		{
			if (projectile.ai[2] == 1 && Main.rand.NextBool(Math.Max(10, (int)(70 - projectile.ai[1] / 10))))
			{
				bool onBack = player.HeldItem.ModItem is not OrchidModGuardianStandard;
				Vector2 dustPos = projectile.Center + new Vector2(0, onBack ? -30 : -18).RotatedBy(onBack ? MathHelper.PiOver4 * 0.5f * -player.oldDirection : MathHelper.PiOver4 + projectile.rotation);
				Dust dust = Dust.NewDustPerfect(dustPos, DustID.Electric);
				dust.velocity = new Vector2(0, -0.5f) + dust.velocity * Main.rand.NextFloat();
				dust.scale *= 0.5f;
			}
		}

		public override bool DrawCustomFlag(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor, Vector2 drawPosition, float drawRotation)
		{
			float lightIntensity = Math.Min(0.2f, projectile.ai[1] / 3000f);
			if (projectile.ai[2] == 1) lightIntensity *= 2f;
			Lighting.AddLight(projectile.Center, new Vector3(0, lightIntensity * 0.5f, lightIntensity));
			return false;
		}

		public override bool NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{
			return true;
		}
	}
}
