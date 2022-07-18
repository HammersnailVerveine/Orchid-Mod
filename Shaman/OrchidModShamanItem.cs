using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using OrchidMod.Common.Attributes;
using OrchidMod.Common;
using Terraria.GameContent.Creative;

namespace OrchidMod.Shaman
{
	public enum ShamanCatalystType : int
	{
		IDLE = 0,
		AIM = 1,
		ROTATE = 2
	}

	[ClassTag(ClassTags.Shaman)]
	public abstract class OrchidModShamanItem : OrchidModItem
	{
		public int empowermentType = 0;
		public int energy = 1;
		public Color? catalystEffectColor = null; // TODO: ...
		public ShamanCatalystType catalystType = ShamanCatalystType.IDLE;

		// ...

		protected override bool CloneNewInstances => true;

		public sealed override void AltSetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			this.SafeSetStaticDefaults();
		}

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ShamanDamageClass>();

			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Thrust;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeapon = true;

			this.SafeSetDefaults();

			orchidItem.shamanWeaponElement = this.empowermentType;
		}

		public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			Vector2 mousePosition = Main.MouseWorld;
			Vector2? catalystCenter = shaman.ShamanCatalystPosition;

			//if (catalystCenter != null && Collision.CanHit(position, 0, 0, position + (catalystCenter.Value - position), 0, 0))
			if (catalystCenter != null)
				position = catalystCenter.Value;

			Vector2 newMove = mousePosition - position;
			newMove.Normalize();
			newMove *= velocity.Length();
			velocity = newMove;
			//velocity.X = newMove.X;
			//velocity.Y = newMove.Y;
			int exhaustion = (int)(energy * shaman.shamanExhaustionRate);
			exhaustion = exhaustion < 2 ? 2 : exhaustion;

			switch (empowermentType)
			{
				case 1:
					shaman.shamanPollFire = shaman.shamanPollFire < 0 ? 0 : shaman.shamanPollFire;
					shaman.shamanPollFire += exhaustion;
					shaman.shamanPollFireMax = shaman.shamanFireBondLoading == 100 ? shaman.shamanPollFireMax : false;
					break;
				case 2:
					shaman.shamanPollWater = shaman.shamanPollWater < 0 ? 0 : shaman.shamanPollWater;
					shaman.shamanPollWater += exhaustion;
					shaman.shamanPollWaterMax = shaman.shamanWaterBondLoading == 100 ? shaman.shamanPollWaterMax : false;
					break;
				case 3:
					shaman.shamanPollAir = shaman.shamanPollAir < 0 ? 0 : shaman.shamanPollAir;
					shaman.shamanPollAir += exhaustion;
					shaman.shamanPollAirMax = shaman.shamanAirBondLoading == 100 ? shaman.shamanPollAirMax : false;
					break;
				case 4:
					shaman.shamanPollEarth = shaman.shamanPollEarth < 0 ? 0 : shaman.shamanPollEarth;
					shaman.shamanPollEarth += exhaustion;
					shaman.shamanPollEarthMax = shaman.shamanEarthBondLoading == 100 ? shaman.shamanPollEarthMax : false;
					break;
				case 5:
					shaman.shamanPollSpirit = shaman.shamanPollSpirit < 0 ? 0 : shaman.shamanPollSpirit;
					shaman.shamanPollSpirit += exhaustion;
					shaman.shamanPollSpiritMax = shaman.shamanSpiritBondLoading == 100 ? shaman.shamanPollSpiritMax : false;
					break;
				default:
					break;
			}

			if (this.SafeShoot(player, source, position, velocity, type, damage, knockback))
			{
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			}

			return false;
		}

		public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation = (player.MountedCenter + new Vector2(player.direction * 12, player.gravDir * 24)).Floor();
			player.itemRotation = -player.direction * player.gravDir * MathHelper.PiOver4;
		}

		public sealed override void HoldItem(Player player)
		{
			var shaman = player.GetModPlayer<OrchidShaman>();
			var catalystType = ModContent.ProjectileType<CatalystAnchor>();

			if (player.ownedProjectileCounts[catalystType] == 0)
			{
				var index = Projectile.NewProjectile(null, player.Center, Vector2.Zero, catalystType, 0, 0f, player.whoAmI); // change source to something that's not null ?
				shaman.shamanCatalystIndex = index;

				var proj = Main.projectile[index];
				if (!(proj.ModProjectile is CatalystAnchor catalyst))
				{
					proj.Kill();
					shaman.shamanCatalystIndex = -1;
				}
				else catalyst.OnChangeSelectedItem(player);
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == catalystType);
				if (proj != null && proj.ModProjectile is CatalystAnchor catalyst)
				{
					if (catalyst.SelectedItem != player.selectedItem)
					{
						catalyst.OnChangeSelectedItem(player);
					}
				}
				else shaman.shamanCatalystIndex = -1;
			}

			this.SafeHoldItem();
		}

		public sealed override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			switch (empowermentType)
			{
				case 1:
					damage *= modPlayer.shamanFireBondLoading < 100 ? 1f : 0.5f;
					break;
				case 2:
					damage *= modPlayer.shamanWaterBondLoading < 100 ? 1f : 0.5f;
					break;
				case 3:
					damage *= modPlayer.shamanAirBondLoading < 100 ? 1f : 0.5f;
					break;
				case 4:
					damage *= modPlayer.shamanEarthBondLoading < 100 ? 1f : 0.5f;
					break;
				case 5:
					damage *= modPlayer.shamanSpiritBondLoading < 100 ? 1f : 0.5f;
					break;
				default:
					break;
			}

			this.SafeModifyWeaponDamage(player, ref damage);
		}

		/* [CRIT]
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= player.GetModPlayer<OrchidShaman>().shamanCrit) crit = true;
			else crit = false;
		}
		*/

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " shamanic damage";
			}

			if (empowermentType > 0)
			{
				Color[] colors = new Color[5]
				{
					new Color(194, 38, 31),
					new Color(0, 119, 190),
					new Color(75, 139, 59),
					new Color(255, 255, 102),
					new Color(138, 43, 226)
				};

				string[] strType = new string[5] { "Fire", "Water", "Air", "Earth", "Spirit" };

				int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
				if (index != -1) tooltips.Insert(index + 1, new TooltipLine(Mod, "BondType", $"Bond type: [c/{Terraria.ID.Colors.AlphaDarken(colors[empowermentType - 1]).Hex3()}:{strType[empowermentType - 1]}]"));
			}
		}

		// ...

		public virtual void SafeSetStaticDefaults() { }
		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem() { }
		public virtual void SafeModifyWeaponDamage(Player player, ref StatModifier damage) { }
		public virtual bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;

		public virtual void ExtraAICatalyst(Projectile projectile, bool after) { }
		public virtual void PostAICatalyst(Projectile projectile) { }
		public virtual void PostDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreAICatalyst(Projectile projectile) { return true; }
		public virtual bool PreDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }

		public virtual string CatalystTexture => "OrchidMod/Shaman/CatalystTextures/" + this.Name + "_Catalyst";

		// ...

		public int NewShamanProjectile(Player player, EntitySource_ItemUse source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0.0f, float ai1 = 0.0f)
		{
			int newProjectileIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0, ai1);
			Projectile newProjectile = Main.projectile[newProjectileIndex];
			OrchidModProjectile.setShamanBond(newProjectile, this.empowermentType);
			return newProjectileIndex;
		}
	}
}
