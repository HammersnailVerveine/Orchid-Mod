using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using OrchidMod.Content.Guardian.Misc;
using System.Collections.Generic;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
    public class GuardianHorizonLanceCounter : OrchidModGuardianProjectile
    {
        const int maxTime = 30;
		public List<int> HitNPCs;

		public override void SafeSetDefaults()
        {
            Projectile.timeLeft = maxTime;
			HitNPCs = new List<int>();
		}

        ref float TargetRotation => ref Projectile.ai[0];
        ref float RotationSpeed => ref Projectile.ai[1];
        float sine;
        float sineIn;
        float sineOut;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft == maxTime)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
                NPC closestTarget = null;
                float distanceClosest = 256;
                foreach (NPC npc in Main.npc)
                {
                    float distance = Projectile.Center.Distance(npc.Center);
                    if (IsValidTarget(npc) && distance < distanceClosest)
                    {
                        closestTarget = npc;
                        distanceClosest = distance;
                    }
                }
                if (closestTarget != null)
                {
                    TargetRotation = (closestTarget.Center - Projectile.Center + (closestTarget.velocity - player.velocity) * maxTime * 0.5f).ToRotation();
                    RotationSpeed = MathHelper.WrapAngle(TargetRotation - Projectile.rotation) / (maxTime * 0.5f);
                    if (Math.Abs(RotationSpeed) > 0.1f)
                    {
                        RotationSpeed = 0.1f * (RotationSpeed / Math.Abs(RotationSpeed));
                        Projectile.rotation = TargetRotation - RotationSpeed * (maxTime * 0.5f);
                    }
                }
                else
                {
                    RotationSpeed = Main.rand.NextFloat(0.04f) - 0.02f;
                    Projectile.rotation -= RotationSpeed * maxTime * 0.3f;
                }
            }
            Projectile.Center = player.Center;
            sine = (float)Math.Sin(Projectile.timeLeft * MathHelper.Pi / maxTime);
            sineIn = (float)Math.Cos(Projectile.timeLeft * MathHelper.Pi / (maxTime * 2));
            sineOut = (float)Math.Sin(Projectile.timeLeft * MathHelper.Pi / (maxTime * 2));
            Projectile.rotation += RotationSpeed * sineIn * sine * 1.75f;
            Vector2 tip = Projectile.Center + new Vector2(512 * (float)Math.Pow(sineIn, 3) * sine, 0).RotatedBy(Projectile.rotation);
            if ((tip - Projectile.Center).X < 0) Projectile.direction = -1;
			if (IsLocalOwner)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (IsValidTarget(npc) && Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), Projectile.Center, tip) && !HitNPCs.Contains(npc.whoAmI))
                    {
                        player.ApplyDamageToNPC(npc, Projectile.damage, 0f, Projectile.direction, Main.rand.Next(100) < Projectile.CritChance, ModContent.GetInstance<GuardianDamageClass>());
						HitNPCs.Add(npc.whoAmI);
					}
				}
            }
        }

        public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
        {
            Main.instance.PrepareDrawnEntityDrawing(Projectile, GameShaders.Armor.GetShaderIdFromItemId(ModContent.ItemType<HorizonDye>()), null);
            float angle = Projectile.rotation + MathHelper.PiOver2;
            Vector2 spritePosition = Projectile.Center - Main.screenPosition;
            Vector2 flareOffset = new Vector2(sineOut * 10, 0).RotatedBy(angle);
            float flareTilt = sine - sineIn * 0.5f;
            DrawData sideFlare = new DrawData(ModContent.Request<Texture2D>(Texture).Value, spritePosition + flareOffset, new Rectangle(36, 188, 30, 68), new Color(255, 255, 255, 0), angle + flareTilt, new Vector2(6, 64), new Vector2(sine, 2 * sine * sineIn), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(sideFlare);
            sideFlare.position = spritePosition - flareOffset;
            sideFlare.effect = SpriteEffects.FlipHorizontally;
            sideFlare.origin.X = 24;
            sideFlare.rotation = angle - flareTilt;
            Main.EntitySpriteDraw(sideFlare);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture).Value, spritePosition, new Rectangle(0, 0, 34, 256), new Color(255, 255, 255, 0), angle, new Vector2(17, 252), new Vector2(sine, 2 * (float)Math.Pow(sineIn, 3) * sine), SpriteEffects.None, 0);
            return false;
        }
    }
}