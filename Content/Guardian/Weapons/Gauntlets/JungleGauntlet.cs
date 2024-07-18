using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class JungleGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 34;
			Item.knockBack = 5f;
			Item.damage = 87;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 25;
			strikeVelocity = 17f;
			parryDuration = 40;
		}

		public override Color GetColor(bool offHand)
		{
			if (offHand) return new Color(184, 118, 124);
			return new Color(143, 215, 29);
		}

		public override void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info)
		{
			player.AddBuff(ModContent.BuffType<GuardianJungleGauntletBuff>(), 120);
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
		{
			if (Main.rand.NextBool(5) || charged) target.AddBuff(BuffID.Poisoned, 240 + Main.rand.Next(120));
		}
	}
}
