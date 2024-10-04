using Microsoft.Xna.Framework;
using OrchidMod.Common.Global.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianRune : OrchidModGuardianItem
	{
		public int RuneCost;
		public int RuneDuration;
		public float RuneDistance;
		public int RuneNumber;

		public int GetNumber(Player player) => RuneNumber + player.GetModPlayer<OrchidGuardian>().GuardianBonusRune;
		public int GetAmount(OrchidGuardian guardian, int factor = 1) => RuneNumber + guardian.GuardianBonusRune * factor;

		public virtual void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, GetAmount(guardian));
		}

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 00f;
			RuneCost = 1;
			RuneDuration = 1800;
			RuneDistance = 100f;
			RuneNumber = 1;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();
		}

		public override bool WeaponPrefix() => true;

		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;
			guardian.SlamCostUI = RuneCost;
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				var guardian = player.GetModPlayer<OrchidGuardian>();
				guardian.GuardianSlam -= RuneCost;
				foreach (Projectile projectile in Main.projectile)
				{
					if (projectile.ModProjectile is GuardianRuneProjectile && projectile.owner == player.whoAmI)
					{
						projectile.Kill();
					}
				}
				int crit = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
				Activate(player, guardian, Item.shoot, guardian.GetGuardianDamage(Item.damage), Item.knockBack, crit, (int)(RuneDuration * guardian.GuardianRuneTimer), RuneDistance, RuneNumber);
			}
			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<OrchidGuardian>().GuardianSlam < RuneCost)
				return false;
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int tooltipSeconds = Math.DivRem((int)(RuneDuration * Main.LocalPlayer.GetModPlayer<OrchidGuardian>().GuardianRuneTimer), 60, out int tooltipTicks);
			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "RuneDuration", tooltipSeconds + " seconds duration"));

			index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "UseSlams", "Uses " + this.RuneCost + " shield slams"));
		}

		public Projectile NewRuneProjectile(Player player, OrchidGuardian guardian, int duration, int type, int damage, float knockback, int critChance, float distance = 0f, float angle = 0f, float ai2 = 0f)
		{
			Projectile projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI, distance, angle, ai2)];
			projectile.timeLeft = duration;
			projectile.CritChance = critChance;
			projectile.netUpdate = true;
			return projectile;
		}

		public List<Projectile> NewRuneProjectiles(Player player, OrchidGuardian guardian, int duration, int type, int damage, float knockback, int critChance, float distance, int number, float angle = 0f, float ai2 = 0f)
		{
			List<Projectile> projectiles = new List<Projectile>();

			for (int i = 0; i < number; i++)
				projectiles.Add(NewRuneProjectile(player, guardian, duration, type, damage, knockback, critChance, distance, angle + (360 / number) * i, ai2));
			return projectiles;
		}
	}
}
