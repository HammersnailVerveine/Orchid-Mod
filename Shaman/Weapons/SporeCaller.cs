using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class SporeCaller : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 42;
			item.height = 42;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 3.15f;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 54, 0);
			item.UseSound = SoundID.Item43;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("SporeCallerProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			if (Main.LocalPlayer.FindBuffIndex(mod.BuffType("SporeEmpowerment")) > -1)
				add += 2f;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Caller");
			Tooltip.SetDefault("Spits out a stack of life-seeking spores, growing stronger with time"
							  + "\nOnly one stack of spores can be active at once"
							  + "\nThe number of spores depends on your number of active shamanic bonds"
							  + "\nIf the projectiles last for long enough before hitting an opponent, your next attack with this weapon will deal increased damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			player.ClearBuff(mod.BuffType("SporeEmpowerment"));

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			for (int i = 0; i < nbBonds + 2; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			var tooltip = tooltips.Find(i => i.Name.Equals("Damage") && i.mod == "Terraria");
			if (tooltip != null)
			{
				string[] split = tooltip.text.Split(' ');
				if (Int32.TryParse(split[0], out int dmg2))
				{
					dmg2 += 45;
					split[0] = split[0] + " - " + dmg2;
					tooltip.text = String.Join(" ", split);
				}
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.JungleSpores, 8);
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
