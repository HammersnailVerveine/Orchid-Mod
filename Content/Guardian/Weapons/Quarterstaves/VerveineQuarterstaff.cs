using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using System.Collections.Generic;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class VerveineQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 0, 85, 0);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 26;
			ParryDuration = 90;
			Item.knockBack = 8f;
			Item.damage = 63;
			CounterSpeed = 1.8f;
			CounterKnockback = 0.25f;
			CounterHits = 0;
			GuardStacks = 1;
			SlamStacks = 1;
		}

		public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
		{
			if (counterAttack)
			{
				//"fart"
				//      -Verveine
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<VerveineFart>(), (int)(Item.damage * 0.75f), Item.knockBack * 0.25f, projectile.owner);
				newProjectile.CritChance = guardian.GetGuardianCrit(Item.crit);
			}
		}
	}

	public class VerveineQuarterstaffTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.JungleGrass };
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(222, 124, 192), name);

			DustType = DustID.JunglePlants;
			HitSound = SoundID.Grass;
			RegisterItemDrop(ModContent.ItemType<VerveineQuarterstaff>());
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.06f;
			g = 0.03f;
			b = 0.075f;
		}

		public override bool IsTileDangerous(int i, int j, Player player) => true;
		public override bool CanDrop(int i, int j) => true;
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			return base.GetItemDrops(i, j);
		}
	}

	/*public class FartingPlant : ModItem
	{
		//debug item
		public override string Texture => $"Terraria/Images/Gore_435";
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<VerveineQuarterstaffTile>());
		}
	}*/
}
