using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class AbyssalChitinScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.magic = true;
			item.width = 42;
			item.height = 42;
			item.useTime = 60;
			item.useAnimation = 60;
			item.knockBack = 3.15f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item43;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("AbyssalChitinScepterProj");
			this.empowermentType = 2;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Naga Fizzler");
			Tooltip.SetDefault("Spits out a burst of bubbles, growing stronger with time"
							+ "\nOnly one set of bubbles can be active at once"
							+ "\nYour number of active shamanic bonds increases the damage increase rate"
							+ "\n'Used to be called the fizzling wand of fizzly fizzies'");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			for (int i = 0; i < 5; i++)
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
					dmg2 *= 15;
					split[0] = split[0] + " - " + dmg2;
					tooltip.text = String.Join(" ", split);
				}
			}
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "AbyssalChitin", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
