using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistItem : ModItem
	{
		public int potencyCost = 0;
		public AlchemistElement element = AlchemistElement.NULL;
		public int rightClickDust = -1;
		public int colorR = 255;
		public int colorG = 255;
		public int colorB = 255;
		
		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.crit = 4;
			item.useStyle = 1;
			item.UseSound = SoundID.Item106;
			item.consumable = false;
			item.noUseGraphic = true;
			item.useAnimation = 30;
			item.useTime = 30;
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.knockBack = 1f;
			
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.alchemistColorR = this.colorR;
			orchidItem.alchemistColorG = this.colorG;
			orchidItem.alchemistColorB = this.colorB;
			orchidItem.alchemistRightClickDust = this.rightClickDust;
			orchidItem.alchemistPotencyCost = this.potencyCost;
			orchidItem.alchemistElement = this.element;
			orchidItem.alchemistWeapon = true;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().alchemistCrit;
		}
		
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).shamanCrit)
                crit = true;
			else crit = false;
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			OrchidModPlayer orchidModPlayer = player.GetModPlayer<OrchidModPlayer>();
			bool[] elements = orchidModPlayer.alchemistElements;
			
			if (player == Main.LocalPlayer) {
				orchidModPlayer.alchemistPotencyDisplayTimer = 180;
				
				bool alreadyContains = false;
				if ((int)this.element > 0 && (int)this.element < 7) {
					alreadyContains = elements[(int)this.element - 1];
				}
				
				if (player.altFunctionUse == 2) {
					item.useAnimation = 10;
					item.useTime = 10;
					bool noPotency = orchidModPlayer.alchemistPotency < this.potencyCost + 1;

					if (alreadyContains || noPotency || orchidModPlayer.alchemistNbElements >= orchidModPlayer.alchemistNbElementsMax) {
						if (noPotency && !alreadyContains) {
							item.UseSound = SoundID.Item7;
							Main.PlaySound(19, (int)player.Center.X ,(int)player.Center.Y, 1);
						} else {
							if (Main.rand.Next(2) == 0) {
								item.UseSound = SoundID.Item112;
							} else {
								item.UseSound = SoundID.Item111;
							}
						}
					} else {
						int rand = Main.rand.Next(3);
						switch (rand) {
							case 1:
								item.UseSound = SoundID.Item86;
								break;
							case 2:
								item.UseSound = SoundID.Item87;
								break;
							default:
								item.UseSound = SoundID.Item85;
								break;
						}
						playerAddFlask(player, element, item.type, item.damage, potencyCost, rightClickDust, colorR, colorG, colorB);
						if (this.rightClickDust != -1) {
							for(int i=0; i<5; i++)
							{
								int dust = Dust.NewDust(player.Center, 10, 10, this.rightClickDust);
								Main.dust[dust].scale *= 1f;
							}	
						}
					}
					item.shoot = mod.ProjectileType("AlchemistRightClick");
				} else {
					if (!alreadyContains && orchidModPlayer.alchemistPotency > this.potencyCost && orchidModPlayer.alchemistNbElements < orchidModPlayer.alchemistNbElementsMax) {
						playerAddFlask(player, element, item.type, item.damage, potencyCost, rightClickDust, colorR, colorG, colorB);
					}
					
					if (orchidModPlayer.alchemistNbElements > 0) {
						item.shootSpeed = 10f * orchidModPlayer.alchemistVelocity;
						item.shoot = ProjectileType<Alchemist.Projectiles.AlchemistProj>();
						item.UseSound = SoundID.Item106;
						item.useAnimation = 30;
						item.useTime = 30;
					} else {
						Main.PlaySound(19, (int)player.Center.X ,(int)player.Center.Y, 1);
						return false;
					}
				}
			} else {
				return false;
			}
			return base.CanUseItem(player);
		}
		
		public override bool CloneNewInstances {
			get
			{
				return true;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " chemical " + damageWord;
			}
			
			switch (this.element) {
				case AlchemistElement.WATER:
					tooltips.Insert(1, new TooltipLine(mod, "ElementTag", "Water Element")
					{
						overrideColor = new Color(0, 119, 190)
					});
					break;
				case AlchemistElement.FIRE:
					tooltips.Insert(1, new TooltipLine(mod, "ElementTag", "Fire Element")
					{
						overrideColor = new Color(194, 38, 31)
					});
					break;
				case AlchemistElement.NATURE:
					tooltips.Insert(1, new TooltipLine(mod, "ElementTag", "Nature Element")
					{
						overrideColor = new Color(75, 139, 59)
					});
					break;
				case AlchemistElement.AIR:
					tooltips.Insert(1, new TooltipLine(mod, "ElementTag", "Air Element")
					{
						overrideColor = new Color(166, 231, 255)
					});
					break;
				case AlchemistElement.LIGHT:
					tooltips.Insert(1, new TooltipLine(mod, "ElementTag", "Light Element")
					{
						overrideColor = new Color(255, 255, 102)
					});
					break;
				case AlchemistElement.DARK:
					tooltips.Insert(1, new TooltipLine(mod, "ElementTag", "Dark Element")
					{
						overrideColor = new Color(138, 43, 226)
					});
					break;
				default:
					break;
			}
			
			tooltips.Insert(1, new TooltipLine(mod, "Mix", "Right click to mix")
			{
				overrideColor = new Color(155, 255, 155)
			});
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
			
			tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.mod == "Terraria");
			if (tt != null) tt.text = "Uses " + this.potencyCost + " potency";
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			//tooltips.Insert(tooltips.Count - 1, new TooltipLine(mod, "PotencyUse", "Uses " + this.potencyCost + " potency"));
		}
		
		public static void playerAddFlask(Player player, AlchemistElement element, int flaskType, int damage, int potencyCost, int rightClickDust, int colorR, int colorG, int colorB) {
			OrchidModPlayer orchidModPlayer = player.GetModPlayer<OrchidModPlayer>();
			bool[] elements = orchidModPlayer.alchemistElements;
			int[] flasks = orchidModPlayer.alchemistFlasks;
			int[] dusts = orchidModPlayer.alchemistDusts;
			
			int index = (int)element - 1;
			
			orchidModPlayer.alchemistPotency -= potencyCost;
			orchidModPlayer.alchemistPotencyWait = 300;
			
			dusts[index] = rightClickDust;
			elements[index] = true;
			flasks[index] = flaskType;
			orchidModPlayer.alchemistFlaskDamage += damage;
			orchidModPlayer.alchemistNbElements ++;
			
			int divider = orchidModPlayer.alchemistNbElements;
			orchidModPlayer.alchemistColorR = (int)(((orchidModPlayer.alchemistColorR * (divider - 1)) + colorR) / divider);
			orchidModPlayer.alchemistColorG = (int)(((orchidModPlayer.alchemistColorG * (divider - 1)) + colorG) / divider);
			orchidModPlayer.alchemistColorB = (int)(((orchidModPlayer.alchemistColorB * (divider - 1)) + colorB) / divider);
			
			orchidModPlayer.alchemistColorR = orchidModPlayer.alchemistColorR > 255 ? 255 : orchidModPlayer.alchemistColorR;
			orchidModPlayer.alchemistColorG = orchidModPlayer.alchemistColorG > 255 ? 255 : orchidModPlayer.alchemistColorG;
			orchidModPlayer.alchemistColorB = orchidModPlayer.alchemistColorB > 255 ? 255 : orchidModPlayer.alchemistColorB;
		}
	}
}
