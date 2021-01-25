using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;
 
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
			this.empowermentLevel = 2;
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			if (Main.LocalPlayer.FindBuffIndex(mod.BuffType("SporeEmpowerment")) > -1)
				add += 2f;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Spore Caller");
		  Tooltip.SetDefault("Spits out a stack of life-seeking spores, growing stronger with time"
							+ "\nOnly one stack of spores can be active at once"
							+ "\nThe number of spores depends on your number of active shamanic bonds"
							+ "\nIf the projectiles last for long enough before hitting an opponent, your next attack with this weapon will deal increased damage");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			for (int i = 0; i < nbBonds + 2; i++)
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
			dmg2 += 45;
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
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.JungleSpores, 8);
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
