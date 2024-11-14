﻿using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class ShardQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.value = Item.sellPrice(0, 2, 88, 0);
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 30;
			ParryDuration = 90;
			Item.knockBack = 8f;
			Item.damage = 172;
			GuardStacks = 2;
		}

		public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
		{
			if (counterAttack && IsLocalPlayer(player))
			{
				int damage = guardian.GetGuardianDamage(Item.damage * 0.5f);
				int projectileType = ModContent.ProjectileType<ShardQuarterstaffProjectile>();
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), player.Center, Vector2.Zero, projectileType, damage, 0f, projectile.owner, 0.8f, Main.rand.NextFloat(3.14f));
				newProjectile.CritChance = guardian.GetGuardianCrit(Item.crit);
			}
		}
	}
}