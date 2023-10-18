using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Buffs;
using OrchidMod.Shaman.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class SporeCaller : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 6;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item43;
			Item.shootSpeed = 5f;
			Item.shoot = ModContent.ProjectileType<SporeCallerProj>();
			this.Element = 3;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 25;
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (Main.LocalPlayer.FindBuffIndex(ModContent.BuffType<SporeEmpowerment>()) > -1)
				damage += 2f;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Spore Caller");
			/* Tooltip.SetDefault("Spits out a stack of life-seeking spores, growing stronger with time"
							  + "\nOnly one stack of spores can be active at once"
							  + "\nThe number of spores depends on your number of active shamanic bonds"
							  + "\nIf the projectiles last for long enough before hitting an opponent, your next attack with this weapon will deal triple damage"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			player.ClearBuff(Mod.Find<ModBuff>("SporeEmpowerment").Type);

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			for (int i = 0; i < nbBonds + 2; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				newVelocity *=  scale;
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			var tooltip = tooltips.Find(i => i.Name.Equals("Damage") && i.Mod == "Terraria");
			if (tooltip != null)
			{
				string[] split = tooltip.Text.Split(' ');
				if (Int32.TryParse(split[0], out int dmg2))
				{
					dmg2 += 45;
					split[0] = split[0] + " - " + dmg2;
					tooltip.Text = String.Join(" ", split);
				}
			}
		}
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.JungleSpores, 8)
			.AddIngredient(ItemID.Stinger, 5)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
