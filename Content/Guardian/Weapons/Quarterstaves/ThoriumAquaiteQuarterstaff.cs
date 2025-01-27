using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class ThoriumAquaiteQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 1, 75);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 18;
			ParryDuration = 45;
			Item.knockBack = 5f;
			Item.damage = 46;
			JabStyle = 1;
			JabDamage = 0.75f;
			JabChargeGain = 4;
			SwingStyle = 0;
			SwingSpeed = 1.5f;
			CounterSpeed = 1.5f;
			GuardStacks = 1;
			SlamStacks = 1;
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (jabAttack && guardian.GuardianGauntletCharge < 180)
			{
				CombatText.NewText(player.Hitbox, new Color(175, 255, 175), "Charged", false);
				SoundEngine.PlaySound(SoundID.Item66, player.Center);
			}
		}
	}
}
