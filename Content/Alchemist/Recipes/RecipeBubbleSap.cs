﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeBubbleSap : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.name = "Sap Bubble";
			this.description = "Creates a catalytic sap bubble";
			this.debuffDuration = 20;
			this.sound = SoundID.Item85;
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CloudInAVial>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.LivingSapVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<Alchemist.Weapons.Nature.LivingSapVial>();
			int dmg = modPlayer.GetSecondaryDamage(itemType, 25, false);
			int spawnProj = ProjectileType<Content.Alchemist.Projectiles.Reactive.LivingSapBubble>();
			Vector2 vel = new Vector2(0f, -5f);
			Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, spawnProj, dmg, 0f, player.whoAmI);
		}
	}
}