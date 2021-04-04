using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Interfaces;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartzHealer : OrchidModItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SetDefaults()
		{
			item.damage = 7;
			item.magic = true;
			item.width = 44;
			item.height = 36;
			item.useTime = 22;
			item.useAnimation = 22;
			item.maxStack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.knockBack = 6.5f;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("StaticQuartzHealerPro");
			item.shootSpeed = 0.1f;
			item.crit = 0;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Scythe");
			Tooltip.SetDefault("Rapidly spins a static quartz scythe all around you"
							+  "\nDeals increased damage while moving");
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(ItemType<StaticQuartz>(), 12);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
				FieldInfo field1 = thoriumPlayer.GetType().GetField("flatRadiantDamage", BindingFlags.Public | BindingFlags.Instance);
				FieldInfo field2 = thoriumPlayer.GetType().GetField("radiantBoost", BindingFlags.Public | BindingFlags.Instance);
				
				if (field1 != null && field2 != null) {
					int healDamage = (int)field1.GetValue(thoriumPlayer);
					float healBoost = (float)field2.GetValue(thoriumPlayer);
					
					add = player.allDamage + (healBoost - 1);
					mult = player.allDamageMult;
					flat += Math.Abs(player.velocity.X + player.velocity.Y) > 2.5f ? healDamage + 3 : healDamage;
				}
			}
		}
		
		public override void GetWeaponCrit(Player player, ref int crit)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
				FieldInfo field = thoriumPlayer.GetType().GetField("radiantCrit", BindingFlags.Public | BindingFlags.Instance);
				if (field != null) {
					int healCrit = (int)field.GetValue(thoriumPlayer);
					crit = item.crit + healCrit;
				}
			}
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int k = 0; k < 2; k++)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("StaticQuartzHealerEffect"), damage, knockBack, player.whoAmI, k, 0f);
			}
			return true;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			Player player = Main.player[Main.myPlayer];
			
			if (thoriumMod != null)
			{
				ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
				FieldInfo field = thoriumPlayer.GetType().GetField("darkAura", BindingFlags.Public | BindingFlags.Instance);
				bool dark = (bool)field.GetValue(thoriumPlayer);
				
				if (field != null) {
					TooltipLine tooltip = tooltips.Find(tt => tt.mod.Equals("Terraria") && tt.Name.Equals("Damage"));
					if (tooltip != null)
					{
						tooltip.text = tooltip.text.Split(' ')[0] + " radiant damage";
					}

					int index = tooltips.FindIndex(tt => tt.mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
					if (index != -1)
					{
						tooltips.Insert(index + 1, new TooltipLine(mod, "HealerTag", "-Healer Class-")
						{
							overrideColor = !dark ? new Color(255, 255, 91) : new Color(178, 102, 255)
						});
					}
					
					index = tooltips.FindIndex(tt => tt.mod.Equals("Terraria") && tt.Name.Equals("Knockback"));
					if (index != -1)
					{
						tooltips.Insert(index + 1, new TooltipLine(mod, "ScytheSoulCharge", "Grants 1 soul essence on direct hit"));
					}
				} else {
					TooltipLine tooltip = tooltips.Find(tt => tt.mod.Equals("Terraria") && tt.Name.Equals("Damage"));
					if (tooltip != null)
					{
						tooltips.Insert(1, new TooltipLine(mod, "ReflectionFail", "Reflection Borked"){
							overrideColor = new Color(255, 0, 0)
						});
					}
				}
			}
		}
		
	}
}
