using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class TerrariumScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 94;
			item.width = 56;
			item.height = 56;
			item.useTime = 10;
			item.useAnimation = 10;
			item.knockBack = 3.5f;
			item.rare = 10;
			item.value = Item.sellPrice(0, 13, 50, 0);
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("TerrariumScepterProj");
			this.empowermentType = 3;
			this.empowermentLevel = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarium Scepter");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Fires bolts of chromatic energy"
							+"\nHitting enemies will gradually grant you terrarium orbs"
							+"\nWhen reaching 7 orbs, they will break free and home into your enemies");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f; 
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			return true;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " shamanic " + damageWord;
			}
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));
				if (index != -1)
				{
					tooltips.Insert(index + 1, new TooltipLine(mod, "ShamanTag", "-Shaman Class-") // 00C0FF
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
			}
			
			tt = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
			if (tt != null) tt.overrideColor = Main.DiscoColor;
			
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
				
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
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
				
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
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
				recipe.AddTile(412); // Ancient Manipulator
				recipe.AddIngredient(null, "TerrariumCore", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

