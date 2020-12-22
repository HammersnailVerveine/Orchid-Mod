using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
    public class AbyssalChitinScepter : OrchidModShamanItem
    {
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
			item.rare = 4;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item43;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("AbyssalChitinScepterProj");
			this.empowermentType = 2;
			this.empowermentLevel = 3;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Naga Fizzler");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Spits out a burst of bubbles, growing stronger with time"
							+ "\nOnly one set of bubbles can be active at once"
							+ "\nYour number of active shamanic bonds increases the damage increase rate"
							+"\n'Used to be called the fizzling wand of fizzly fizzies'");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            for (int l = 0; l < Main.projectile.Length; l++)
            {  
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			for (int i = 0; i < 5; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) // Useful because of damage range
		{
			int index = -1;
			for (int m = 0; m < tooltips.Count; m++)
            {
                if (tooltips[m].Name.Equals("Damage")) { index = m; break;}
            }
            string oldTooltip = tooltips[index].text;
            string[] split = oldTooltip.Split(' ');
			int dmg2 = 0;
			Int32.TryParse(split[0], out dmg2);
			dmg2 *= 15;
            tooltips.RemoveAt(index);
            tooltips.Insert(index, new TooltipLine(mod, "Damage", split[0] + " - " + dmg2 + " shamanic damage"));
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));
				if (index != -1)
				{
					tooltips.Insert(index + 1, new TooltipLine(mod, "ShamanTag", "-Shaman Class-") // 00C0FF
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
			}
			
			if (this.empowermentType > 0) {
				string emp = "";
				Color col = new Color(0, 0, 0);
				switch (this.empowermentType) {
					case 1:
						emp = "Fire";
						col = new Color(194, 38, 31);
						break;
					case 2:
						emp = "Water";
						col = new Color(0, 119, 190);
						break;
					case 3:
						emp = "Air";
						col = new Color(75, 139, 59);
						break;
					case 4:
						emp = "Earth";
						col = new Color(255, 255, 102);
						break;
					case 5:
						emp = "Spirit";
						col = new Color(138, 43, 226);
						break;
					default:
						break;
				}
				
				index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					tooltips.Insert(index, new TooltipLine(mod, "BondType", emp + " bond")
					{
						overrideColor = col
					});
				}
			}
			
			if (this.empowermentLevel > 0) {
				string lev = "";
				switch (this.empowermentLevel) {
					case 1:
						lev = "I";
						break;
					case 2:
						lev = "II";
						break;
					case 3:
						lev = "III";
						break;
					case 4:
						lev = "IV";
						break;
					case 5:
						lev = "V";
						break;
					default:
						break;
				}
				
				index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					tooltips.Insert(index, new TooltipLine(mod, "BondLevel", "Shamanic bond level " + lev)
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
			}
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(null, "AbyssalChitin", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
