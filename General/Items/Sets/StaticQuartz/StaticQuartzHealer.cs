using Microsoft.Xna.Framework;
using OrchidMod.General.Items.Sets.StaticQuartz.Projectiles;
using OrchidMod.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartzHealer : OrchidModItem, ICrossmodItem
	{
		Mod thoriumMod = OrchidMod.ThoriumMod;

		public string CrossmodName => "Thorium Mod";

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.magic = true;
			Item.width = 44;
			Item.height = 36;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.knockBack = 6.5f;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<StaticQuartzHealerPro>();
			Item.shootSpeed = 0.1f;
			Item.crit = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Scythe");
			Tooltip.SetDefault("Rapidly spins a static quartz scythe all around you"
							+ "\nDeals increased damage while moving");
		}

		public override void AddRecipes()
		{
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(ItemType<StaticQuartz>(), 12);
				recipe.Register();
				recipe.Register();
			}
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (thoriumMod != null)
			{
				object result = thoriumMod.Call("GetHealerDamageMods", player);

				if (result is ValueTuple<float, float, int> tuple)
				{
					float radiantBoost = tuple.Item1;
					float radiantBoostMult = tuple.Item2;
					int flatRadiantDamage = tuple.Item3;

					add = player.allDamage + (radiantBoost - 1);
					mult = player.allDamageMult + (radiantBoostMult - 1);
					flat += Math.Abs(player.velocity.X + player.velocity.Y) > 2.5f ? flatRadiantDamage + 3 : flatRadiantDamage;
				}
			}
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			if (thoriumMod != null)
			{
				object result = thoriumMod.Call("GetHealerCrit", player);
				if (result is int healerCrit)
				{
					crit = Item.crit + healerCrit;
				}
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int k = 0; k < 2; k++)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("StaticQuartzHealerEffect").Type, damage, knockBack, player.whoAmI, k, 0f);
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.player[Main.myPlayer];

			//TODO thorium
			if (thoriumMod != null)
			{
				ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
				FieldInfo field = thoriumPlayer.GetType().GetField("darkAura", BindingFlags.Public | BindingFlags.Instance);
				bool dark = (bool)field.GetValue(thoriumPlayer);

				if (field != null)
				{
					TooltipLine tooltip = tooltips.Find(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Damage"));
					if (tooltip != null)
					{
						tooltip.Text = tooltip.Text.Split(' ')[0] + " radiant damage";
					}

					int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
					if (index != -1)
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "HealerTag", "-Healer Class-")
						{
							OverrideColor = !dark ? new Color(255, 255, 91) : new Color(178, 102, 255)
						});
					}

					index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Knockback"));
					if (index != -1)
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ScytheSoulCharge", "Grants 1 soul essence on direct hit"));
					}
				}
				else
				{
					TooltipLine tooltip = tooltips.Find(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Damage"));
					if (tooltip != null)
					{
						tooltips.Insert(1, new TooltipLine(Mod, "ReflectionFail", "Reflection Borked")
						{
							OverrideColor = new Color(255, 0, 0)
						});
					}
				}
			}
		}

	}
}
