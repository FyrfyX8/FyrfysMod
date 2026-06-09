using FyrfysMod.Content.Items;
using FyrfysMod.Content.Items.Accessories;
using FyrfysMod.Content.Items.Accessories.FeyaButterflyNectar;
using FyrfysMod.Content.Items.Armor.Vanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Common.GlobalNPCs
{
    class NPCShops : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            // Condition to check if the player has exactly 314 fallen Stars in their inventory.
            Condition piAmountOfStars = new Condition("Mods.FyrfysMod.Conditions.piAmountOfStars", () => Main.LocalPlayer.CountItem(ItemID.FallenStar) == 314);
            // Condition to check if the player has any of the sprinting boots in their inventory.
            Condition anySpeedBootsCondition = new Condition("Mods.FyrfysMod.Conditions.PlayerHasAnySpeedBoots", () =>
            Condition.PlayerCarriesItem(ItemID.HermesBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.HermesBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.SailfishBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.SailfishBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.SandBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.SandBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.AmphibianBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.AmphibianBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.SpectreBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.SpectreBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.FairyBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.FairyBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.HellfireTreads).IsMet() || Main.LocalPlayer.HasItem(ItemID.HellfireTreads, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.LightningBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.LightningBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.FrostsparkBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.FrostsparkBoots, Main.LocalPlayer.armor) ||
            Condition.PlayerCarriesItem(ItemID.TerrasparkBoots).IsMet() || Main.LocalPlayer.HasItem(ItemID.TerrasparkBoots, Main.LocalPlayer.armor)
            );
            
            // check if shop is the Clothier's shop.
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Clothier, "Shop"))
            {
                // add items of FyrfyX8's vanity set to the Clothier's shop, with the condition that the player must have exactly 314 Stars and any of the sprinting boots in their inventory.
                // adding custom price of 10 gold to each item.
                shop.Add(new Item(ModContent.ItemType<FyrfyX8Head>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, piAmountOfStars, anySpeedBootsCondition);

                shop.Add(new Item(ModContent.ItemType<FyrfyX8Chestplate>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, piAmountOfStars, anySpeedBootsCondition);

                shop.Add(new Item(ModContent.ItemType<FyrfyX8Leggings>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, piAmountOfStars, anySpeedBootsCondition);

                shop.Add(new Item(ModContent.ItemType<FyrfyX8Tail>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, piAmountOfStars, anySpeedBootsCondition);

                shop.Add(new Item(ModContent.ItemType<FyrfyX8Wings_Inactive>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 16),
                }, piAmountOfStars, anySpeedBootsCondition);
            }

            // check if shop is the Dryad's shop.
            if (shop.FullName == NPCShopDatabase.GetShopName(NPCID.Dryad, "Shop"))
            {
                shop.Add(new Item(ModContent.ItemType<FeyaHeadwear>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, Condition.PlayerCarriesItem(ItemID.GoldButterfly), Condition.DownedSkeletron, Condition.AnglerQuestsFinishedOver(15));

                shop.Add(new Item(ModContent.ItemType<FeyaFlowerDress>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, Condition.PlayerCarriesItem(ItemID.GoldButterfly), Condition.DownedSkeletron, Condition.AnglerQuestsFinishedOver(15));

                shop.Add(new Item(ModContent.ItemType<FeyaButterflyNectar>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, Condition.PlayerCarriesItem(ItemID.GoldButterfly), Condition.DownedSkeletron, Condition.AnglerQuestsFinishedOver(15));

                shop.Add(new Item(ModContent.ItemType<FeyaFariyWings_Inactive>())
                {
                    shopCustomPrice = Item.buyPrice(gold: 10),
                }, Condition.PlayerCarriesItem(ItemID.GoldButterfly), Condition.DownedSkeletron, Condition.AnglerQuestsFinishedOver(15));
            }
        }
    }
}
