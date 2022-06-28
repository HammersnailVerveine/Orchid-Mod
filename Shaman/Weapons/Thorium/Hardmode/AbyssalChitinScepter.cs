using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class AbyssalChitinScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.shootSpeed = 5f;
			Item.shoot = ModContent.ProjectileType<AbyssalChitinScepterProj>();
			this.empowermentType = 2;
			this.energy = 20;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Naga Fizzler");
			Tooltip.SetDefault("Spits out a burst of bubbles, growing stronger with time"
							+ "\nOnly one set of bubbles can be active at once"
							+ "\nYour number of active shamanic bonds increases the damage increase rate"
							+ "\n'Used to be called the fizzling wand of fizzly fizzies'");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			for (int i = 0; i < 5; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				newVelocity *= scale;
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
					dmg2 *= 15;
					split[0] = split[0] + " - " + dmg2;
					tooltip.Text = String.Join(" ", split);
				}
			}
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "AbyssalChitin", 9);
				recipe.Register();
			}
		}
	}
}
