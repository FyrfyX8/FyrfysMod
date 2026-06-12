using FyrfysMod.Common.Systems;
using FyrfysMod.Content.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace FyrfysMod.Content.NPCs
{
    [AutoloadHead]
    internal class Gravitraxer : ModNPC
    {
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        public static LocalizedText UpgradedText {  get; private set; }

        public override LocalizedText DeathMessage => this.GetLocalization("DeathMessage");

        // add shimer version

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26; // total amount of frames this NPC has.

            NPCID.Sets.ExtraFramesCount[Type] = 10; // amount of extra frames this NPC has.
            NPCID.Sets.AttackFrameCount[Type] = 5; // amount of frames used for the NPC's attack animation.
            NPCID.Sets.DangerDetectRange[Type] = 800; // amount of pixels the NPC can detect danger from its center.
            NPCID.Sets.AttackType[Type] = 0; // the attack type the NPC uses, 0 = trowing, 1 = shooting, 2 = magic, 3 = melee.
            NPCID.Sets.AttackTime[Type] = 90; // the amount of ticks it takes for the NPC to use its attack, once it has detected a target.
            NPCID.Sets.AttackAverageChance[Type] = 40; // change of this NPC to attack.
            NPCID.Sets.HatOffsetY[Type] = 4; // the amount of pixels the NPC's party hat is offset.
            NPCID.Sets.ShimmerTownTransform[Type] = false; // toggle to true when shimer version is done.

            // Connect this NPC with a custom emote.
            NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType <GravitraxerEmote>();

            // Draw modifiers for the bestiary.
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Walking speed.
                Direction = -1 // Facing left.
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Gravitraxer's biome and neighbor preferences.
            NPC.Happiness
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love) // Gravitraxer likes the underground.
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love) // Gravitraxer likes the forest.
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike) // Gravitraxer dislikes the ocean.
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Love) // Gravitraxer loves living near the Merchant.
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Love) // Gravitraxer loves living near the Goblin Tinkerer.
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Gravitraxer likes living near the Guide.
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Gravitraxer hates living near the Demolitionist.
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Hate); // Gravitraxer hates living near the Angler.
            ;

            ContentSamples.NpcBestiaryRarityStars[Type] = 4; // rarity of the NPC in the bestiary, from 0 to 5 stars.
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Mods.FyrfysMod.Bestiary.Gravitraxer"),
            ]);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                // Retrieve the gore types. This NPC has shimmer and party variants for head, arm, and leg gore. (12 total gores)
                string variant = "";
                if (NPC.IsShimmerVariant)
                    variant += "_Shimmer";
                int hatGore = NPC.GetPartyHatGore();
                int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
                int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
                int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

                // Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
                if (hatGore > 0)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_SpawnNPC)
            {
                // A TownNPC is "unlocked" once it successfully spawns into the world.
                TownNPCRespawnSystem.unlockedGravitraxerSpawn = true;
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        { // Requirements for the town NPC to spawn.
            if (TownNPCRespawnSystem.unlockedGravitraxerSpawn)
            {
                // If Gravitraxer has spawned in this world before, a DustyGravitraxMarble is not required.
                return true;
            }

            foreach (var player in Main.ActivePlayers)
            {
                // Player has to have any colour of DustyGravitraxMarble in their inventory for Gravitraxer to spawn for the first time.
                if (player.HasItem(DirtyMarbleSet.DirtyMarble))
                {
                    return true;
                }
            }
            return false;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Eric", // Eric M Gravitrax
                "Elyas", // Gravibahn
                "Fynn", // Fytrax
                "Levi", // Levi K Gravitrax
                "Jonas", // MegaTrax
                "Fabian", // MegaTrax
                "Bjarne",
                "Luis", // Gravitrax Player
                "Max", // BlueBlizzard
                "Steffan", // Cooglebahn
                "Emil", // Graviscraper
                "Benni",
                "Clemens",
                "Lennox", // Lennox - Marble Runs
                "Jona", // Gravitrax Masters
                "Justus", // Gravitrax Masters
            };
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int merchant = NPC.FindFirstNPC(NPCID.Merchant);
            if (merchant >= 1 && Main.rand.NextBool(6))
            {
                chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.Merchant", Main.npc[merchant].GivenName));
            }

        // Random things the Gravitraxer can tell the player
            chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.StandardDialogue4"));
            chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.CommonDialogue1", Main.LocalPlayer.name), 2.0);
            chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.RareDialogue1"), 0.1);

            NumberOfTimesTalkedTo++;
            if (NumberOfTimesTalkedTo >= 100)
            {
                chat.Add(Language.GetTextValue("Mods.FyrfysMod.Dialogue.Gravitraxer.TalkALot"));
            }

            string chosenChat = chat;

            return chosenChat;
        }


        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("Mods.FyrfysMod.ChatButton.Gravitraxer.Button2");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (!firstButton)
            {
                return;
            }
            shopName = ShopName;
        }
    }
}