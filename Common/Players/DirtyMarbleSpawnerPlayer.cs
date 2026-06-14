using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using FyrfysMod.Content.Items;

namespace FyrfysMod.Common.Players
{
    internal class DirtyMarbleSpawnerPlayer : ModPlayer
    {
        private float preHorizontalPos;

        public override void Initialize()
        {
            preHorizontalPos = Player.position.X;
        }

        public override void PostUpdate()
        {
            if (Player.position.X != preHorizontalPos && Main.hardMode)
            {
                float playerLuck = Player.RollLuck(100) * 0.01f;
                preHorizontalPos = Player.position.X;

                if (Main.rand.NextBool((int)MathHelper.Lerp(100000,10000,playerLuck)) && Player.velocity.Y == 0f)
                {
                    var entitySource = Player.GetSource_DropAsItem();

                    WeightedRandom<int> randomDirtyMarble = new WeightedRandom<int>();

                    // base weights of DirtyMarbles
                    float baseSilver = 1f;
                    float baseColoured = 0.8f;
                    float baseGold = 0.05f;

                    randomDirtyMarble.Add(ModContent.ItemType<SilverDirtyMarble>(), MathHelper.Lerp(baseSilver, baseSilver * 0.1f, playerLuck));

                    randomDirtyMarble.Add(ModContent.ItemType<RedDirtyMarble>(), baseColoured + 0.1f);
                    randomDirtyMarble.Add(ModContent.ItemType<GreenDirtyMarble>(), baseColoured + 0.05f);
                    randomDirtyMarble.Add(ModContent.ItemType<BlueDirtyMarble>(), baseColoured);

                    // GoldDirtyMarbles are unobtanable untill rewards are done
                    // randomDirtyMarble.Add(ModContent.ItemType<GoldDirtyMarble>(), MathHelper.Lerp(baseGold, baseGold * 5f, playerLuck));
                
                    int dirtyMarble = randomDirtyMarble;

                    Player.QuickSpawnItem(entitySource, dirtyMarble);
                }
            }
        }
    }
}
