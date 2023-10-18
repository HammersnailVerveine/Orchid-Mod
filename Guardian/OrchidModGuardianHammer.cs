using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Guardian;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Guardian
{
	public abstract class OrchidModGuardianHammer : OrchidModGuardianItem
	{
		public int range;
		public int slamStacks;
		public int blockStacks;
		public bool penetrate;
		public bool tileCollide;
		public virtual void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, float knockback, bool crit, bool Weak) { }
		public virtual void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, float knockback, bool crit, bool Weak) { }
		public virtual bool ThrowAI(Player player, OrchidGuardian guardian, bool Weak) => true;

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 10f;
			Item.shootSpeed = 10f;
			range = 0;
			penetrate = false;
			tileCollide = true;
			slamStacks = 0;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
		}

		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.guardianDisplayUI = 300;
			guardian.holdingHammer = true;
		}

		public override bool? UseItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			int projType = ProjectileType<Guardian.HammerThrow>();
			Vector2 dir = Main.MouseWorld - player.Center;
			dir.Normalize();
			Projectile projectile;
			if (guardian.guardianThrowCharge >= 180) {
				dir *= Item.shootSpeed;
				projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center + dir, dir, projType, Item.damage, Item.knockBack, player.whoAmI)];
			} 
			else
			{
				dir *= Item.shootSpeed * (0.3f * (guardian.ThrowLevel() + 2) / 3);
				projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center + dir, dir, projType, (int)Item.damage / 3, Item.knockBack / 3f, player.whoAmI)];
				projectile.ai[0] = 1f;
			}
			projectile.CritChance = Item.crit + (int)player.GetCritChance<GuardianDamageClass>();
			projectile.rotation = dir.ToRotation();
			projectile.direction = projectile.spriteDirection;
			projectile.netUpdate = true;
			guardian.guardianThrowCharge = 0;
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{
			int projType = ProjectileType<Guardian.HammerThrow>();
			if (player.ownedProjectileCounts[projType] > 0 || player.GetModPlayer<OrchidGuardian>().ThrowLevel() == 0)
				return false;
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " opposing " + damageWord;
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			if (blockStacks > 0) {
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Grants " + this.blockStacks + " shield blocks")
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

			if (slamStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldSlams", "Grants " + this.blockStacks + " shield slams")
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}
		}
	}
}
