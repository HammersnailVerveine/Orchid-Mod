using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Shaman
{
	public enum ShamanCatalystType : int
	{
		IDLE = 0,
		AIM = 1,
		ROTATE = 2
	}

	public abstract class OrchidModShamanItem : OrchidModItem
	{
		public int empowermentType = 0;
		public int energy = 1;
		public ShamanCatalystType catalystType = ShamanCatalystType.IDLE;

		// ...

		public override bool CloneNewInstances => true;

		public sealed override void SetStaticDefaults()
		{
			Item.staff[item.type] = true;

			this.SafeSetStaticDefaults();
		}

		public sealed override void SetDefaults()
		{
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.crit = 4;
			item.useStyle = ItemUseStyleID.Stabbing;

			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeapon = true;

			this.SafeSetDefaults();

			if (this.energy == -1)
			{
				this.energy = (int)(item.useTime / 2);
			}
		}

		public sealed override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			Vector2 mousePosition = Main.MouseWorld;
			Vector2 catalystCenter = modPlayer.shamanCatalystPosition;

			if (Collision.CanHit(position, 0, 0, position + (catalystCenter - position), 0, 0))
			{
				position = catalystCenter;
			}

			Vector2 newMove = mousePosition - position;
			newMove.Normalize();
			newMove *= new Vector2(speedX, speedY).Length();
			speedX = newMove.X;
			speedY = newMove.Y;

			switch (empowermentType)
			{
				case 1:
					modPlayer.shamanPollFire = modPlayer.shamanPollFire < 0 ? 0 : modPlayer.shamanPollFire;
					modPlayer.shamanPollFire += energy;
					modPlayer.shamanPollFireMax = false;
					break;
				case 2:
					modPlayer.shamanPollWater = modPlayer.shamanPollWater < 0 ? 0 : modPlayer.shamanPollWater;
					modPlayer.shamanPollWater += energy;
					modPlayer.shamanPollWaterMax = false;
					break;
				case 3:
					modPlayer.shamanPollAir = modPlayer.shamanPollAir < 0 ? 0 : modPlayer.shamanPollAir;
					modPlayer.shamanPollAir += energy;
					modPlayer.shamanPollAirMax = false;
					break;
				case 4:
					modPlayer.shamanPollEarth = modPlayer.shamanPollEarth < 0 ? 0 : modPlayer.shamanPollEarth;
					modPlayer.shamanPollEarth += energy;
					modPlayer.shamanPollEarthMax = false;
					break;
				case 5:
					modPlayer.shamanPollSpirit = modPlayer.shamanPollSpirit < 0 ? 0 : modPlayer.shamanPollSpirit;
					modPlayer.shamanPollSpirit += energy;
					modPlayer.shamanPollSpiritMax = false;
					break;
				default:
					break;
			}

			if (this.SafeShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack))
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}

			return false;
		}

		public sealed override void UseStyle(Player player)
		{
			player.itemLocation = (player.MountedCenter + new Vector2(player.direction * 12, player.gravDir * 24)).Floor();
			player.itemRotation = -player.direction * player.gravDir * MathHelper.PiOver4;
		}

		public sealed override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (modPlayer.shamanCatalyst < 1)
			{
				int projType = ProjectileType<CatalystAnchor>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projType, 0, 0f, player.whoAmI);
			}

			if (modPlayer.shamanSelectedItem != item.type)
			{
				modPlayer.shamanSelectedItem = item.type;
				string textureLocation = "OrchidMod/Shaman/CatalystTextures/" + this.Name + "_Catalyst";

				if (TextureExists(textureLocation))
				{
					modPlayer.shamanCatalystTexture = GetTexture(textureLocation);
					modPlayer.shamanCatalystType = catalystType;
				}
			}

			modPlayer.shamanCatalyst = 3;

			this.SafeHoldItem();
		}

		public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;

			switch (empowermentType)
			{
				case 1:
					mult *= modPlayer.shamanFireBondLoading < 100 ? 1f : 0.5f;
					break;
				case 2:
					mult *= modPlayer.shamanWaterBondLoading < 100 ? 1f : 0.5f;
					break;
				case 3:
					mult *= modPlayer.shamanAirBondLoading < 100 ? 1f : 0.5f;
					break;
				case 4:
					mult *= modPlayer.shamanEarthBondLoading < 100 ? 1f : 0.5f;
					break;
				case 5:
					mult *= modPlayer.shamanSpiritBondLoading < 100 ? 1f : 0.5f;
					break;
				default:
					break;
			}

			this.SafeModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().shamanCrit;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= player.GetOrchidPlayer().shamanCrit) crit = true;
			else crit = false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " shamanic damage";
			}

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));
				if (index != -1)
				{
					tooltips.Insert(index + 1, new TooltipLine(mod, "ShamanTag", "-Shaman Class-") // 00C0FF
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
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

				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
				if (index != -1) tooltips.Insert(index + 1, new TooltipLine(mod, "BondType", $"Bond type: [c/{Terraria.ID.Colors.AlphaDarken(colors[empowermentType - 1]).Hex3()}:{strType[empowermentType - 1]}]"));
			}
		}

		// ...

		public virtual void SafeSetStaticDefaults() { }
		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem() { }
		public virtual void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) { }
		public virtual bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) => true;

		// ...

		public int NewShamanProjectile(float posX, float posY, float speedX, float speedY, int type, int damage, float knockBack, int playerID, float ai0 = 0.0f, float ai1 = 0.0f)
		{
			int newProjectileIndex = Projectile.NewProjectile(posX, posY, speedX, speedY, type, damage, knockBack, playerID, ai0, ai1);
			Projectile newProjectile = Main.projectile[newProjectileIndex];

			OrchidModProjectile.setShamanBond(newProjectile, this.empowermentType);
			return newProjectileIndex;
		}
	}
}
