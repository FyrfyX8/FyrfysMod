using FyrfysMod.Common;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Content.Items;

[ReinitializeDuringResizeArrays]
public static class DirtyMarbleSet
{
    public const string DirtyMarbleSetKey = "DirtyMarble";

    public const string ColoredDirtyMarbleSetKey = "ColoredDirtyMarble";

    public static bool[] DirtyMarble = ItemID.Sets.Factory.CreateNamedSet(DirtyMarbleSetKey)
        .Description("Items in this set make the Gravitraxer NPC Spawn and can ge returned to him")
        .RegisterBoolSet(false, 
        ModContent.ItemType<SilverDirtyMarble>(),
        ModContent.ItemType<RedDirtyMarble>(),
        ModContent.ItemType<GreenDirtyMarble>(),
        ModContent.ItemType<BlueDirtyMarble>(),
        ModContent.ItemType<GoldDirtyMarble>()
        );

    public static bool[] ColoredDirtyMarble = ItemID.Sets.Factory.CreateNamedSet(ColoredDirtyMarbleSetKey)
        .Description("Items in this set give the colored rewards when returned to the Gravitraxer NPC")
        .RegisterBoolSet(false,
        ModContent.ItemType<RedDirtyMarble>(),
        ModContent.ItemType<GreenDirtyMarble>(),
        ModContent.ItemType<BlueDirtyMarble>()
        );
}
public abstract class DirtyMarble : ModItem
{
    public override string Texture => "FyrfysMod/Content/Items/DirtyMarble";

    public virtual Color TintColor => Color.White;

    private static Asset<Texture2D> backTexture;
    private static Asset<Texture2D> frontTexture;

    public override void Load()
    {
        frontTexture = ModContent.Request<Texture2D>("FyrfysMod/Content/Items/DirtyMarble");
        backTexture = ModContent.Request<Texture2D>("FyrfysMod/Content/Items/Ammo/GravitraxMarble");
    }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 99;
    }

    public override void SetDefaults()
    {
        Item.width = 12;
        Item.height = 12;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = 0;
        Item.rare = ItemRarityID.Green;
    }

    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(backTexture.Value, position, frame, TintColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(frontTexture.Value, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);
        return false;
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
        Vector2 origin = itemFrame.Size() / 2f;
        Vector2 position = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
        spriteBatch.Draw(backTexture.Value, position, itemFrame, TintColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(frontTexture.Value, position, itemFrame, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}

public class SilverDirtyMarble : DirtyMarble
{
}

public class RedDirtyMarble : DirtyMarble
{
    public override Color TintColor => GravitraxMarbles.Red;
}

public class GreenDirtyMarble : DirtyMarble
{
    public override Color TintColor => GravitraxMarbles.Green;
}

public class BlueDirtyMarble : DirtyMarble
{
    public override Color TintColor => GravitraxMarbles.Blue;
}

public class GoldDirtyMarble : DirtyMarble
{
    public override Color TintColor => GravitraxMarbles.Gold;
}