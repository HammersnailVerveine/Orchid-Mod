﻿using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class SpectreQuarterstaff : OrchidModGuardianQuarterstaff
	{
		private int Timer = 0;
		private int TimerHit = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 60;
			Item.height = 66;
			Item.value = Item.sellPrice(0, 4, 29, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 35;
			ParryDuration = 100;
			Item.knockBack = 8f;
			Item.damage = 261;
			GuardStacks = 1;
			SlamStacks = 2;
			CounterHits = 5;
			CounterDamage = 0.6f;
		}

		public override void SafeHoldItem(Player player)
		{
			Timer++;
			TimerHit--;
			if (Timer > 60)
			{
				Timer = 0;
				SpawnProjectile(player.GetModPlayer<OrchidGuardian>());
			}
		}

		public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (TimerHit <= 0)
			{
				TimerHit = 5;
				SpawnProjectile(guardian);
			}
		}

		public override void OnParryQuarterstaff(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
		{
			int projectileType = ModContent.ProjectileType<SpectreQuarterstaffProj>();
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == projectileType && proj.owner == player.whoAmI) 
				{
					proj.ai[1] = Main.rand.Next(5, 35);
				}
			}
		}

		public void SpawnProjectile(OrchidGuardian guardian)
		{
			int damage = guardian.GetGuardianDamage(Item.damage * 0.66f);
			Vector2 position = guardian.Player.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(32f, 160f);
			int projectileType = ModContent.ProjectileType<SpectreQuarterstaffProj>();
			Projectile newProjectile = Projectile.NewProjectileDirect(guardian.Player.GetSource_ItemUse(Item), position, Vector2.Zero, projectileType, damage, 0f, guardian.Player.whoAmI);
			newProjectile.CritChance = guardian.GetGuardianCrit(Item.crit);
			newProjectile.netUpdate = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.SpectreBar, 18);
			recipe.Register();
		}
	}
}
