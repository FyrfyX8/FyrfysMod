using GlowmaskHelper.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class FyrfyX8Chestplate : ModItem
    {

        private static Asset<Texture2D> glowTexture;
        private static Asset<Texture2D> glowEquipTexture;

        public override void Load()
        {
            glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
            glowEquipTexture = ModContent.Request<Texture2D>(Texture + "_Body_Glow");
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            //GlowmaskLoader.AssignGlowmaskTexture_Equip_Arms(Item.glowMask,EquipLoader.GetEquipSlot(Mod,Name,EquipType.Body));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
        {
            drawInfo.DrawDataCache.Add(drawData);
            drawInfo.DrawDataCache.Add(drawData with { color = Color.White, texture = glowEquipTexture.Value });
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(glowTexture.Value, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 position = Item.Bottom - Main.screenPosition - new Vector2(0, (glowTexture.Size() / 2).Y);
            spriteBatch.Draw(glowTexture.Value, position, null, Color.White, rotation, glowTexture.Size() / 2f, scale, SpriteEffects.None, 0f);
        }
    }
}
