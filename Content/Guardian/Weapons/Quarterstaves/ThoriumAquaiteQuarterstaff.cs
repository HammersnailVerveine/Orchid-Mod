using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using OrchidMod.Common.Attributes;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumAquaiteQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 1, 75);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item71.WithPitchOffset(0.5f).WithVolumeScale(0.5f);
			Item.useTime = 18;
			ParryDuration = 45;
			Item.knockBack = 5f;
			Item.damage = 46;
			Item.shootSpeed = 18f;
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
			if (jabAttack)
			{
				if (guardian.GuardianGauntletCharge < 180)
				{
					CombatText.NewText(player.Hitbox, new Color(175, 255, 175), Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.Charged"), false);
					SoundEngine.PlaySound(SoundID.Item66, player.Center);
				}
			}
			else if (!counterAttack)
			{
				Projectile.perIDStaticNPCImmunity[ModContent.ProjectileType<ThoriumAquaiteQuarterstaffProjectile>()][target.whoAmI] = Main.GameUpdateCount + 20;
			}
		}

		public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack && !counterAttack)
			{
				SoundEngine.PlaySound(SoundID.Item66, player.Center);
				Vector2 vel = -Vector2.UnitX.RotatedBy((player.Center - Main.MouseWorld).ToRotation()) * Item.shootSpeed;
				Projectile.NewProjectileDirect(Item.GetSource_FromAI(), player.Center, vel, ModContent.ProjectileType<ThoriumAquaiteQuarterstaffProjectile>(), (int)(Item.damage * 2.5f), Item.knockBack * 2, projectile.owner);
			}
		}
	}
}
