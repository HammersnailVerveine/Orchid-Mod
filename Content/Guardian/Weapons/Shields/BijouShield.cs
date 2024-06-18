﻿using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class BijouShield : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.width = 28;
			Item.height = 34;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item101;
			Item.knockBack = 6f;
			Item.damage = 80;
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 50;
			distance = 40f;
			slamDistance = 90f;
			blockDuration = 200;
		}

		public override void Slam(Player player, Projectile shield)
		{
			Projectile anchor = GetAnchor(player).Projectile;
			int type = ModContent.ProjectileType<BijouShieldProj>();
			Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * 5f;
			Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center, dir, type, (int)(shield.damage * 1.5f), Item.knockBack, player.whoAmI);
		}
	}
}
