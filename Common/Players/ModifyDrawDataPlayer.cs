using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FyrfysMod.Common.Players
{
    internal class ModifyDrawDataPlayer : ModPlayer
    {
        public override void TransformDrawData(ref PlayerDrawSet drawInfo)
        {
            if (Player.tail == EquipLoader.GetEquipSlot(Mod, "FyrfyX8Tail", EquipType.Back)) // Check if player wears FyrfyX8Tail
            {
                // Search for Tail draw data and modify its position
                for (int i = 0; i < drawInfo.DrawDataCache.Count; i++)
                {
                    DrawData data = drawInfo.DrawDataCache[i];
                    if (data.texture == ModContent.Request<Texture2D>("FyrfysMod/Content/Items/Accessories/FyrfyX8Tail_Back").Value)
                    {
                        // Modify the position of the tail
                        Vector2 offset = new Vector2(-11, 0); // offset in Terraria pixels
                        data.position += new Vector2(offset.X * 2 * Player.direction, offset.Y * 2).RotatedBy(data.rotation);
                        drawInfo.DrawDataCache[i] = data;
                    }
                }
            }
        }
    }
}
