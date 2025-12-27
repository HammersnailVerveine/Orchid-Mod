using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using OrchidMod.Common.Attributes;
using OrchidMod.Common;

namespace OrchidMod.Content.Shaman
{
	public abstract class ShamanWeaponAnchor : OrchidModProjectile
	{
		//base stats
		/// <summary> The interval between <c>Attack()</c> being called.</summary>
		public int AttackRate = 30;
		/// <summary> The maximum range the ShamanWeapon can target enemies in.</summary>
		public float TargetRange = 160;
		/// <summary> The rate at which the ShamanWeapon moves during <c>Movement()</c>.</summary>
		public float MoveSpeed = 4;

		//working variables
		/// <summary> How long this ShamanWeapon currently lasts for. Increases while holding its item.</summary>
		public float BondTime = 0;
		/// <summary> Whether this ShamanWeapon hasn't been activated yet. While true, snaps to the player's position and doesn't use most of its AI. Becomes false when <c>BondTime</c> reaches 60 for the first time, which also calls <c>Activate()</c></summary>
		public bool NotActivated = true;
		/// <summary> The position this ShamanWeapon is currently trying to attack. Defaults to <c>Vector2.Zero</c> if the ShamanWeapon has no target.</summary>
		/// <remarks> By default AI, this is set to <c>Main.NPC[TargetNPC].Center</c> in <c>Target()</c>. FireShamanWeapons prioritize setting this to the projectile owner's <c>OrchidShaman.FireTarget</c> if <c>OrchidShaman.HasFireTarget</c>. Override <c>Target()</c> to change this behavior.</remarks>
		public Vector2 CurrentTarget;
		/// <summary> Alias for <c>CurrentTarget != Vector2.Zero</c>.</summary>
		public bool HasTarget => CurrentTarget != Vector2.Zero;
		/// <summary> Array position in <c>Main.NPC</c> of the NPC that <c>CurrentTarget</c>'s position is based on. Defaults to -1 if the ShamanWeapon is not targeting an NPC.</summary>
		/// <remarks> Can be used to retrieve additional information about the NPC being targeted, such as for predictive aiming.
		/// By default AI, this is set to the projectile owner's <c>OrchidShaman.NearestTarget</c>, and will be ignored by Fire ShamanWeapons if <c>OrchidShaman.HasFireTarget</c>.</remarks>
		public int TargetNPC;
		/// <summary> The amount of frames before <c>Attack()</c> is called again. Decremented by 1 every frame that it's above 0.</summary>
		public int AttackCooldown;
		/// <summary> The amount of frames before <c>Target()</c> is called again. Decremented by 1 every frame that it's above 0.</summary>
		/// <remarks> Use for multi-stage, animated, or continuous attacks to avoid interrupting them midway through. Set <c>TargetCooldown</c> and <c>AttackAnimation</c> to the same value to resume targeting as soon as the animation completes.</remarks>
		public int TargetCooldown;

		/// <summary> Optional variable for storing an attack animation timer. Automatically decremented by 1 every frame that it's above 0.</summary>
		public int AttackAnimation;
		/// <summary> Optional variable for drawing the weapon's visual offset.</summary>
		public Vector2 VisualOffset = Vector2.Zero;
		/// <summary> Alias for getting or setting <c>VisualOffset</c> using an in-world position instead of an offset.</summary>
		public Vector2 VisualCenter {get => VisualOffset + Projectile.Center; set => VisualOffset = value - Projectile.Center;}
		/// <summary> The total offset of both <c>VisualOffset</c> and <c>Projectile.oldPosition</c> from <c>Projectile.position</c>.</summary>
		/// <remarks> This is an alias for <c>VisualOffset</c> and does not store its own variable. Before updating <c>VisualOffset</c> on a frame, this effectively returns the offset of anything drawn with <c>VisualOffset</c> on the previous frame. Useful for <c>Vector2.Lerp()</c> smoothing.</remarks>
		public Vector2 OldVisualOffset => VisualOffset - Projectile.position + Projectile.oldPosition;
		/// <summary> Like <c>VisualCenter</c> but with <c>OldVisualOffset</c> instead. </summary>
		public Vector2 OldVisualCenter => VisualOffset + Projectile.oldPosition + new Vector2(Projectile.width, Projectile.height) * 0.5f;
		/// <summary> Whether the default override for DrawBehind places it in the <c>BehindProjectiles</c> layer. Will otherwise place in the <c>OverPlayers</c> layer. Defaults to false.</summary>
		/// <remarks> Can be used for swinging or orbiting animations to give the impression of three dimensional movement.</remarks>
		public bool BehindPlayer = false;

		public sealed override void AltSetDefaults()
		{
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.DamageType = ModContent.GetInstance<ShamanDamageClass>();
			SafeSetDefaults();
		}
		/// <summary> Handles ShamanWeapon movement.</summary>
		/// <remarks> Override and call <c>base.Movement()</c> with different variables to modify movement target without changing behavior.</remarks>
		public virtual void Movement(Vector2 moveTarget)
		{
			float dist = Projectile.Center.Distance(moveTarget);
			if (dist < 8) Projectile.velocity = Vector2.Zero;
			else if (dist - 8 < MoveSpeed) Projectile.velocity = new Vector2(dist - 8, 0).RotatedBy((moveTarget - Projectile.Center).ToRotation());
			else Projectile.velocity = (moveTarget - Projectile.Center) * MoveSpeed / dist;
		}
		/// <summary> Handles ShamanWeapon targeting.</summary>
		public virtual void Target(Player player, OrchidShaman shaman)
		{
			if (shaman.NearestTarget != -1 && Main.npc[shaman.NearestTarget].Distance(Projectile.Center) < TargetRange)
			{
				CurrentTarget = Main.npc[shaman.NearestTarget].Center;
				TargetNPC = shaman.NearestTarget;
			}
			else
			{
				CurrentTarget = Vector2.Zero;
				TargetNPC = -1;
			}
		}
		/// <summary> Overridable method for generating ShamanWeapon attacks. Called when <c>AttackCooldown</c> is 0 or less and <c>HasTarget</c> is true, immediately after <c>AttackCooldown</c> is set to <c>AttackRate</c>.</summary>
		public virtual void Attack() {}
		/// <summary> Method to set variables at the start of an attack for a ShamanWeapon that uses <c>AttackAnimation</c>. Return <c>true</c> to call <c>Attack()</c> immediately. Returns <c>true</c> by default.</summary>
		/// <remarks> This method is called immediately before <c>Attack()</c> if <c>AttackAnimation</c> is not greater than 0. Set <c>AttackCooldown</c> to set a delay before the next time <c>Attack()</c> will be called, and set <c>TargetCooldown</c> to prevent changing targets mid-animation. If <c>AttackAnimation</c> is higher than <c>AttackCooldown</c> then <c>Attack()</c> will be called during the animation.</remarks>
		public virtual bool StartAttackAnimation() => true;
		/// <summary> Called at the end of AI each frame while the ShamanWeapon is activated.</summary>
		public virtual void ExtraAIActive() {}
		/// <summary> Called at the end of AI each frame before the ShamanWeapon is activated. On the last frame before becoming active, <c>Activate()</c> will be called instead of this.</summary>
		public virtual void ExtraAIInactive() {}
		/// <summary> Called when the ShamanWeapon becomes active, on the first frame its <c>BondTime</c> exceeds 120.</summary>
		public virtual void Activate() {}
		/// <summary> Call to return position for standard ShamanWeapon animations, bobbing up and down, staying a fixed distance from the player's center, and animation smoothing.</summary>
		public Vector2 GetFloatAnimation(Vector2 realPosition, Vector2 drawPosition, bool bobbing = true, bool playerSpacing = true, float? smoothingAmount = null)
		{
			Vector2 output = realPosition;
			if (playerSpacing || smoothingAmount is null)
			{
				Vector2 playerCenter = Main.player[Projectile.owner].Center;
				Vector2 offs = realPosition - playerCenter;
				Vector2 drawOffs = drawPosition - playerCenter;
				if (playerSpacing && offs.Length() < 32 && offs.Length() != 0)
				{
					output += new Vector2(32f - offs.Length() - (float)Math.Cos(offs.Length() * 0.049f) * 16f, 0).RotatedBy(offs.ToRotation());
					//output += offs * ((float)Math.Sin(offs.Length() * -0.049f) + 1f) * 20f / offs.Length();
				}
				smoothingAmount ??= Math.Clamp(drawOffs.Length() / MoveSpeed * 0.25f - 0.5f, 0.1f, 0.5f);
			}
			if (bobbing)
			{
				output += new Vector2(0, 6f * (float)Math.Sin(Main.timeForVisualEffects * 0.05f + Projectile.whoAmI));
			}
			return Vector2.Lerp(drawPosition, output, smoothingAmount.Value);
		}
		/// <summary> Call to position the player's hand and return visual offsets to appear in it properly.</summary>
		public Vector2 GetInactiveAnimation()
		{
			Player player = Main.player[Projectile.owner];
			player.bodyFrame.Y = player.bodyFrame.Height * 2;
			return new (0, player.gfxOffY);
		}
		
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.timeLeft = 2;
			if (NotActivated)
			{
				if (player.HeldItem.shoot != Projectile.type)
				{
					Projectile.Kill();
				}
				Projectile.Center = player.Center + Vector2.UnitX * 8 * player.direction;
				if (BondTime >= 120)
				{
					NotActivated = false;
					Activate();
				}
				else ExtraAIInactive();
			}
			else
			{
				BondTime--;
				if (BondTime <= 0)
				{
					Projectile.Kill();
					return;
				}
				if (AttackCooldown > 0) AttackCooldown--;
				if (AttackAnimation > 0) AttackAnimation--;
				if (TargetCooldown > 0) TargetCooldown--;
				OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
				Movement(player.Center);
				if (TargetCooldown <= 0)
					Target(player, shaman);
				if (AttackCooldown <= 0)
				{
					if (HasTarget && (AttackAnimation > 0 || StartAttackAnimation()))
					{
						AttackCooldown = AttackRate;
						Attack();
					}
				}
				ExtraAIActive();
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (NotActivated) VisualOffset = GetInactiveAnimation();
			else VisualCenter = GetFloatAnimation(Projectile.Center, OldVisualCenter);
			spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, VisualCenter - Main.screenPosition, null, lightColor, Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}
	}

	public abstract class FireShamanWeaponAnchor : ShamanWeaponAnchor
	{
		public override void Target(Player player, OrchidShaman shaman)
		{
			if (shaman.HasFireTarget)
			{
				TargetNPC = -1;
				if (shaman.FireTarget.Distance(Projectile.Center) < TargetRange)
					CurrentTarget = shaman.FireTarget;
				else CurrentTarget = Vector2.Zero;
			}
			else if (shaman.NearestTarget != -1 && Main.npc[shaman.NearestTarget].Distance(Projectile.Center) < TargetRange)
			{
				CurrentTarget = Main.npc[shaman.NearestTarget].Center;
				TargetNPC = shaman.NearestTarget;
			}
			else
			{
				CurrentTarget = Vector2.Zero;
				TargetNPC = -1;
			}
		}
	}

	public abstract class EarthShamanWeaponAnchor : ShamanWeaponAnchor
	{
		public override void Movement(Vector2 moveTarget)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			base.Movement(shaman.HasEarthTarget ? shaman.EarthTarget : player.Center);
		}
	}

	[ClassTag(ClassTags.Shaman)]
	public abstract class ShamanWeaponItem : ModItem
	{
		public float displaySpeed = 0;
		public bool FireElement = false;
		public bool EarthElement = false;

		public virtual void SafeSetDefaults() { }

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.DamageType = ModContent.GetInstance<ShamanDamageClass>();
			SafeSetDefaults();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			if (FireElement)
			{
				if (Main.MouseWorld.Distance(player.Center) > 64)
				{
					shaman.FireTarget = Main.MouseWorld;
				}
				else
				{
					shaman.FireTarget = Vector2.Zero;
				}
			}
			else if (EarthElement)
			{
				if (Main.MouseWorld.Distance(player.Center) > 64)
				{
					shaman.EarthTarget = Main.MouseWorld;
				}
				else
				{
					shaman.EarthTarget = Vector2.Zero;
				}
			}
			return false;
		}

		public override void HoldItem(Player player)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			int arrayPos = shaman.ShamanWeapons.FindIndex(x => x.type == Item.shoot);
			if (arrayPos != -1 && shaman.ShamanWeapons[arrayPos].ModProjectile is ShamanWeaponAnchor anchor) //exists
			{
				anchor.BondTime = Math.Min(anchor.BondTime + 2, 1800);
			}
			else //doesn't exist, create it
			{
				if (player == Main.LocalPlayer)
				{
					Projectile projectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.Zero, Item.shoot, player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
					shaman.ShamanWeapons.Add(projectile);
				}
			}
		}
	}
}