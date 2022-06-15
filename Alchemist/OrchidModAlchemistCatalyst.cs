using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistCatalyst : OrchidModItem
	{
		public int catalystType = 0; // 1 = melee swing, 2 = throw, 3 = gun

		public virtual void SafeSetDefaults() { }

		public virtual void CatalystInteractionEffect(Player player) { }

		public virtual void GeneralCatalystInteractionEffect(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			CatalystInteractionEffect(player);

			if (modPlayer.alchemistFlowerSet)
			{
				modPlayer.alchemistFlower++;
				modPlayer.alchemistFlowerTimer = 600;
				if (modPlayer.alchemistFlower == 1)
				{
					Projectile.NewProjectile(null, player.Center.X, player.position.Y - 65, 0f, 0f, ProjectileType<Alchemist.Projectiles.Reactive.BloomingReactive>(), 0, 0, player.whoAmI, 0f, 0f);
				}
				if (modPlayer.alchemistFlower >= 9)
				{
					modPlayer.alchemistFlower = 0;
					int dmg = (int)(25 * modPlayer.alchemistDamage);
					Projectile.NewProjectile(null, player.Center.X, player.position.Y - 65, 0f, 0f, ProjectileType<Alchemist.Projectiles.Reactive.BloomingReactiveAlt>(), dmg, 0, player.whoAmI, 0f, 0f);
				}
			}
		}

		public sealed override void SetDefaults()
		{
			Item.useAnimation = 0;
			Item.useTime = 0;
			Item.shootSpeed = 0f;
			Item.damage = 0;
			Item.crit = 0;
			SafeSetDefaults();
			Item.DamageType = DamageClass.Generic;
			Item.noMelee = this.catalystType != 1;
			Item.useStyle = 1;
			Item.UseSound = SoundID.Item1;
			Item.consumable = false;
			Item.noUseGraphic = this.catalystType == 2;
			Item.useAnimation = Item.useAnimation == 0 ? 20 : 0;
			Item.useTime = Item.useTime == 0 ? 20 : 0;
			Item.autoReuse = false;
			Item.shootSpeed = Item.shootSpeed == 0f ? 10f : Item.shootSpeed;
			Item.knockBack = 0f;
			Item.crit = Item.crit == 0 ? 0 : Item.crit;
			Item.damage = Item.damage == 0 ? 0 : Item.damage;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.alchemistCatalyst = true;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().alchemistCrit;
		}

		public override bool? CanHitNPC(Player player, NPC target)
		{
			return false;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (this.catalystType == 1)
			{
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && hitbox.Intersects(proj.Hitbox))
					{
						OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
						if (modProjectile.alchemistReactiveProjectile)
						{
							modProjectile.alchemistCatalyticTriggerDelegate(player, proj, modProjectile);
							GeneralCatalystInteractionEffect(player);
						}
					}
				}
				for (int l = 0; l < Main.npc.Length; l++)
				{
					NPC target = Main.npc[l];
					if (hitbox.Intersects(target.Hitbox))
					{
						target.AddBuff(BuffType<Alchemist.Debuffs.Catalyzed>(), 60 * 10);
					}
				}
			}
		}

		protected override bool CloneNewInstances => true;

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Alchemist Class-")
				{
					OverrideColor = new Color(155, 255, 55)
				});
			}

			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " chemical " + damageWord;
			}

			tt = tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);

			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
		}
	}
}
