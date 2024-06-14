using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianRune : OrchidModGuardianItem
	{
		public int RuneCost;
		public int RuneDuration;
		public float RuneDistance;
		public int RuneNumber;

		public int GetNumber(Player player) => RuneNumber + player.GetModPlayer<OrchidGuardian>().GuardianBonusRune;
		public int GetNumber(OrchidGuardian guardian) => RuneNumber + guardian.GuardianBonusRune;

		public virtual void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, GetNumber(guardian));
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
			RuneDuration = 3600;
			RuneDistance = 100f;
			RuneNumber = 1;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
		}

		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;
			guardian.SlamCostUI = RuneCost;
		}

		public override bool? UseItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianSlam -= RuneCost;
			foreach (Projectile projectile in guardian.RuneProjectiles)
				projectile.Kill();
			int crit = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
			Activate(player, guardian, Item.shoot, (int)player.GetDamage<GuardianDamageClass>().ApplyTo(Item.damage), Item.knockBack, crit, (int)(RuneDuration * guardian.GuardianRuneTimer), RuneDistance, RuneNumber);
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
				string damageWord = splitText.Last();
				tt.Text = damageValue + " opposing damage";
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback")); // "UseMana"
			tooltips.Insert(index + 1, new TooltipLine(Mod, "UseSlams", "Uses " + this.RuneCost + " shield slams"));
		}

		public Projectile NewRuneProjectile(Player player, OrchidGuardian guardian, int duration, int type, int damage, float knockback, int critChance, float distance = 0f, float angle = 0f, float ai2 = 0f)
		{
			Projectile projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI, distance, angle, ai2)];
			projectile.timeLeft = duration;
			projectile.CritChance = critChance;
			projectile.netUpdate = true;
			guardian.RuneProjectiles.Add(projectile);
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
