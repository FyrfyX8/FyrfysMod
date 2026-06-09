using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Content.Items.Accessories.FeyaButterflyNectar
{
    internal class FeyaButterflyPlayer : ModPlayer
    {
        public bool hasFeyaButterflyNectar;

        public override void ResetEffects()
        {
            hasFeyaButterflyNectar = false;
        }

        public override void PostUpdate()
        {
            if (!hasFeyaButterflyNectar || Player.dead || !Player.active)
                return;

            int projType = ModContent.ProjectileType<FeyaButterflyProjectile>();
            int desired = 8; // number of butterflies

            // check in wich accessory slot Feya's Butterfly Nectar is equiped and increase butterfly count based on that
            for (int i = 0; i < Player.armor.Length; i++)
            {
                if (Player.armor[i] is Item slot && !slot.IsAir && slot.type == ModContent.ItemType<FeyaButterflyNectar>())
                {
                    if ( 3 <= i && i <= 9 || 13 <= i && i <= 19)
                    {
                        desired = 1 + (i > 10 ? i - 10 : i) ;
                    }
                }
            }

            int existing = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == Player.whoAmI && p.type == projType)
                    existing++;
            }

            for (int i = existing; i < desired; i++)
            {
                float phase = (float)(i * (Math.PI * 2.0 / desired));
                int idx = Projectile.NewProjectile(Player.GetSource_Accessory(null), Player.Center, Vector2.Zero, projType, 0, 0f, Player.whoAmI, phase, 0f);
                if (idx >= 0 && Main.projectile[idx].active)
                {
                    Main.projectile[idx].netUpdate = true;
                    Main.projectile[idx].ai[2] = i;
                    Main.projectile[idx].scale = Main.rand.NextFloat(0.8f, 1f);
                }
            }
        }
    }
    public class FeyaButterflyProjectile : ModProjectile
    {
        public override string Texture => "FyrfysMod/Content/Items/Accessories/FeyaButterflyNectar/FeyaButterflies";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // reset timer if player has Feya's Butterfly Nectar eqquiped
            if (player.GetModPlayer<FeyaButterflyPlayer>().hasFeyaButterflyNectar)
                Projectile.timeLeft = 10;

            // Initialize target pick state: ai[0]=angle, ai[1]=radius, localAI[1]=frames until next pick
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f; // init flag
                Projectile.ai[0] = Main.rand.NextFloat(0f, (float)(Math.PI * 2)); // angle
                Projectile.ai[1] = Main.rand.NextFloat(22f, 48f); // radius
                Projectile.localAI[1] = Main.rand.Next(30, 90); // frames until next target
            }

            // setting the position around wich the butterflies should fly
            Vector2 headPos = player.Center + new Vector2(0f, -player.height * 0.5f - 12f);

            float angle = Projectile.ai[0];
            float radius = Projectile.ai[1];

            Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
            Vector2 target = headPos + offset + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-8f, 4f));
            Vector2 desiredPos = target - Projectile.Size * 0.5f;
            Vector2 toTarget = desiredPos - Projectile.position;

            // scaling to let butterflies catch up to player
            float distance = toTarget.Length();
            float distMultiplier = MathHelper.Clamp(distance / 28f, 1f, 5f);

            // select new target if current one is reached
            if (distance < 10f)
            {
                Projectile.ai[0] = Main.rand.NextFloat(0f, (float)(Math.PI * 2));
                Projectile.ai[1] = Main.rand.NextFloat(18f, 48f);
                Projectile.localAI[1] = Main.rand.Next(30, 90);


                Projectile.velocity *= 0.25f;


                offset = new Vector2((float)Math.Cos(Projectile.ai[0]), (float)Math.Sin(Projectile.ai[0])) * Projectile.ai[1];
                target = headPos + offset + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-8f, 4f));
                desiredPos = target - Projectile.Size * 0.5f;
                toTarget = desiredPos - Projectile.position;
                distance = toTarget.Length();
            }

            // base movement toward target
            Vector2 newVelocity = toTarget * 0.05f * distMultiplier;
            float maxSpeed = Main.rand.NextFloat(0.2f,1f) * distMultiplier;
            
            // increse speed if player is moving too fast
            if (distance >= 100f)
            {
                maxSpeed = 2f * distMultiplier;
            }

            // teleport to player when teleporting
            if (distance >= 1000f)
            {
                Projectile.position = player.position;
            }
            if (newVelocity.Length() > maxSpeed)
                newVelocity = Vector2.Normalize(newVelocity) * maxSpeed;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, newVelocity, 0.10f);

            // rotation based on velocity
            Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.X * 0.06f, 0.18f);

            // animation (wing flapping)
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            // rotation based on velocity to look natural
            Projectile.rotation = Projectile.velocity.X * 0.08f;

            // subtle light
            Lighting.AddLight(Projectile.Center, 0.18f, 0.08f, 0.22f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            int frameHeight = tex.Height / Main.projFrames[Projectile.type] / 8;
            Rectangle source = new Rectangle(0, frameHeight * Projectile.frame + frameHeight * Main.projFrames[Projectile.type] * (int)Projectile.ai[2], tex.Width, frameHeight);
            Vector2 origin = source.Size() * 0.5f;

            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, source, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0f);
            return false;
        }
    }

    public abstract class  FeyaButterfly
    {
        
    }
    public class FeyaButterflyNectar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 28;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 5);
            Item.accessory = true;
            Item.vanity = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Enable the visual butterflies only when this accessory is shown (not hidden)
            player.GetModPlayer<FeyaButterflyPlayer>().hasFeyaButterflyNectar = !hideVisual;

        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<FeyaButterflyPlayer>().hasFeyaButterflyNectar = true;
        }
    }
}