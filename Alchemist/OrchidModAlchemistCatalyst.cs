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
	public abstract class OrchidModAlchemistCatalyst : ModItem
	{
		public int catalystType = 0; // 1 = melee swing, 2 = throw, 3 = gun
		
		public virtual void SafeSetDefaults() {}
		
		public virtual void CatalystInteractionEffect(Player player) {}
		
		public virtual void GeneralCatalystInteractionEffect(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			CatalystInteractionEffect(player);
			
			if (modPlayer.alchemistFlowerSet) {
				modPlayer.alchemistFlower ++;
				modPlayer.alchemistFlowerTimer = 600;
				if (modPlayer.alchemistFlower == 1) {
					Projectile.NewProjectile(player.Center.X, player.position.Y - 65, 0f, 0f, ProjectileType<Alchemist.Projectiles.Reactive.BloomingReactive>(), 0, 0, player.whoAmI, 0f, 0f);	
				}
				if (modPlayer.alchemistFlower >= 9) {
					modPlayer.alchemistFlower = 0;
					int dmg = (int)(25 * modPlayer.alchemistDamage);
					Projectile.NewProjectile(player.Center.X, player.position.Y - 65, 0f, 0f, ProjectileType<Alchemist.Projectiles.Reactive.BloomingReactiveAlt>(), dmg, 0, player.whoAmI, 0f, 0f);
				}
			}
		}

		public sealed override void SetDefaults() {
			item.useAnimation = 0;
			item.useTime = 0;
			item.shootSpeed = 0f;
			item.damage = 0;
			item.crit = 0;
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = this.catalystType != 1;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.consumable = false;
			item.noUseGraphic = this.catalystType == 2;
			item.useAnimation = item.useAnimation == 0 ? 20 : 0;
			item.useTime = item.useTime == 0 ? 20 : 0;
			item.autoReuse = false;
			item.shootSpeed = item.shootSpeed == 0f ? 10f : item.shootSpeed;
			item.knockBack = 0f;
			item.crit = item.crit == 0 ? 0 : item.crit;
			item.damage = item.damage == 0 ? 0 : item.damage;
			
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.alchemistCatalyst = true;
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().alchemistCrit;
		}
		
		public override bool? CanHitNPC(Player player, NPC target) {
			return false;
		}
		
		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (this.catalystType == 1) {
				for (int l = 0; l < Main.projectile.Length; l++)
				{  
					Projectile proj = Main.projectile[l];
					if (proj.active && hitbox.Intersects(proj.Hitbox))  {
						OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
						if (modProjectile.alchemistReactiveProjectile) {
							proj.Kill();
							GeneralCatalystInteractionEffect(player);
						}
					}
				}
				for (int l = 0; l < Main.npc.Length; l++) {  
					NPC target = Main.npc[l];
					if (hitbox.Intersects(target.Hitbox))  {
						target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Catalyzed>(), 60 * 10);
					}
				}
			}
		}
		
		public override bool CloneNewInstances {
			get
			{
				return true;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
			
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " chemical " + damageWord;
			}
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
		}
	}
}
