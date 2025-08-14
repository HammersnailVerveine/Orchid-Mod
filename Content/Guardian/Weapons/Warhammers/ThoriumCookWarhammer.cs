using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using OrchidMod.Common.Attributes;
using System;


namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumCookWarhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 10f;
			Item.shootSpeed = 10f;
			Item.damage = 80;
			Item.useTime = 22;
			Range = 25;
			GuardStacks = 1;
			ReturnSpeed = 0.8f;
			SwingChargeGain = 1.5f;
		}

		public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
		{
			GuardianHammerAnchor anchor = projectile.ModProjectile as GuardianHammerAnchor;
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null && anchor.Strong)
			{
				Item.NewItem(player.GetSource_OnHit(target), target.Hitbox, thoriumMod.Find<ModItem>("MeatSlab").Type);
			}
		}
	}
}
