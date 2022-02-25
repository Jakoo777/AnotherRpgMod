﻿using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using AnotherRpgMod.RPGModule;
using Terraria;
using System.Collections.Generic;
using AnotherRpgMod.Utils;

namespace AnotherRpgMod.Items
{
    class ModifierManager
    {

        public static WeaponType GetWeaponType (Item item)
        {
            if (item.DamageType == DamageClass.Summon)
                return WeaponType.Summon;
            if (item.DamageType == DamageClass.Magic)
                return WeaponType.Magic;
            if (item.DamageType == DamageClass.Ranged)
            {
                switch (item.useAmmo)
                {
                    case 40:
                        return WeaponType.Bow;
                    case 97:
                        return WeaponType.Gun;
                    default:
                        return WeaponType.OtherRanged;
                }
            }

            if (item.DamageType == DamageClass.Throwing)
            {
                return WeaponType.Throw;
            }
            if (item.DamageType == DamageClass.Melee)
            {
                if (item.useStyle == ItemUseStyleID.Shoot && item.noMelee)
                    return WeaponType.Spear;
                if (item.noMelee)
                    return WeaponType.OtherMelee;
                switch (item.useStyle)
                {
                    case ItemUseStyleID.Thrust:
                        return WeaponType.Stab;
                    case ItemUseStyleID.Swing:
                        return WeaponType.Swing;
                    default:
                        return WeaponType.OtherMelee;
                }
            }
            return WeaponType.Other;
        }

        public static Dictionary<Modifier, float> ModifierWeight = new Dictionary<Modifier, float>()
        {
            {Modifier.None,0f },
            {Modifier.MoonLight,1.8f },
            {Modifier.SunLight,1.5f },
            {Modifier.Berserker,1},
            {Modifier.MagicConnection,0.9f},
            {Modifier.Sniper,1.2f },
            {Modifier.Brawler,1 },
            {Modifier.Piercing,0.5f},
            {Modifier.Savior,0.25f },
            {Modifier.FireLord,0.1f },
            {Modifier.Thorny,2 },
            {Modifier.Smart,1 },
            {Modifier.SelfLearning,0.75f },
            {Modifier.VampiricAura,0.05f },
            {Modifier.Executor,1 },
            {Modifier.Confusion,1 },
            {Modifier.Poisones,1 },
            {Modifier.Venom,0.5f },
            {Modifier.Chaotic,0.75f },
            {Modifier.Cunning,0.25f },
            {Modifier.BloodSeeker,0.25f },
            {Modifier.Cleave,0.5f },
            {Modifier.Random,0.01f } 

        };


        public static Dictionary<int, RarityWeightManager> rarityConversion = new Dictionary<int, RarityWeightManager>()
        {
            { -1 ,
                new RarityWeightManager(new RarityWeight[6]{
                    new RarityWeight(Rarity.Broken,7.5f),
                    new RarityWeight(Rarity.Imperfect,15),
                    new RarityWeight(Rarity.Inferior,5),
                    new RarityWeight(Rarity.Superior,2),
                    new RarityWeight(Rarity.Rare,0.5f),
                    new RarityWeight(Rarity.MasterPiece,0.05f)
                })
            },
            {0 ,
                new RarityWeightManager(new RarityWeight[8]{
                    new RarityWeight(Rarity.Broken,2),
                    new RarityWeight(Rarity.Imperfect,15),
                    new RarityWeight(Rarity.Inferior,10),
                    new RarityWeight(Rarity.Superior,3),
                    new RarityWeight(Rarity.Rare,0.5f),
                    new RarityWeight(Rarity.MasterPiece,0.075f),
                    new RarityWeight(Rarity.Epic,0.001f),
                    new RarityWeight(Rarity.Legendary,0.0003f),
                })
            },
            
            { 1 ,
                new RarityWeightManager(new RarityWeight[9]{
                    new RarityWeight(Rarity.Broken,0.5f),
                    new RarityWeight(Rarity.Imperfect,10),
                    new RarityWeight(Rarity.Inferior,10),
                    new RarityWeight(Rarity.Superior,8),
                    new RarityWeight(Rarity.Rare,2),
                    new RarityWeight(Rarity.MasterPiece,0.35f),
                    new RarityWeight(Rarity.Epic,0.01f),
                    new RarityWeight(Rarity.Legendary,0.0005f),
                    new RarityWeight(Rarity.Mythical,0.000025f)
                })
            },
            { 2 ,
                new RarityWeightManager(new RarityWeight[9]{
                    new RarityWeight(Rarity.Broken,0.1f),
                    new RarityWeight(Rarity.Imperfect,5),
                    new RarityWeight(Rarity.Inferior,10),
                    new RarityWeight(Rarity.Superior,8),
                    new RarityWeight(Rarity.Rare,4),
                    new RarityWeight(Rarity.MasterPiece,0.9f),
                    new RarityWeight(Rarity.Epic,0.12f),
                    new RarityWeight(Rarity.Legendary,0.006f),
                    new RarityWeight(Rarity.Mythical,0.00005f)
                })
            },
            { 3 ,
                new RarityWeightManager(new RarityWeight[9]{
                    new RarityWeight(Rarity.Broken,0.05f),
                    new RarityWeight(Rarity.Imperfect,3),
                    new RarityWeight(Rarity.Inferior,5),
                    new RarityWeight(Rarity.Superior,8),
                    new RarityWeight(Rarity.Rare,6),
                    new RarityWeight(Rarity.MasterPiece,1.5f),
                    new RarityWeight(Rarity.Epic,0.1f),
                    new RarityWeight(Rarity.Legendary,0.01f),
                    new RarityWeight(Rarity.Mythical,0.0001f)
                })
            },
            { 4 ,
                new RarityWeightManager(new RarityWeight[8]{
                    new RarityWeight(Rarity.Imperfect,1),
                    new RarityWeight(Rarity.Inferior,2),
                    new RarityWeight(Rarity.Superior,6),
                    new RarityWeight(Rarity.Rare,10),
                    new RarityWeight(Rarity.MasterPiece,3.5f),
                    new RarityWeight(Rarity.Epic,0.2f),
                    new RarityWeight(Rarity.Legendary,0.03f),
                    new RarityWeight(Rarity.Mythical,0.00025f)
                })
            },
            { 5 ,
                new RarityWeightManager(new RarityWeight[8]{
                    new RarityWeight(Rarity.Imperfect,0.2f),
                    new RarityWeight(Rarity.Inferior,3),
                    new RarityWeight(Rarity.Superior,10),
                    new RarityWeight(Rarity.Rare,12),
                    new RarityWeight(Rarity.MasterPiece,7.5f),
                    new RarityWeight(Rarity.Epic,0.3f),
                    new RarityWeight(Rarity.Legendary,0.1f),
                    new RarityWeight(Rarity.Mythical,0.0005f)
                })
            },
            { 6 ,
                new RarityWeightManager(new RarityWeight[8]{
                    new RarityWeight(Rarity.Imperfect,0.1f),
                    new RarityWeight(Rarity.Inferior,1),
                    new RarityWeight(Rarity.Superior,5),
                    new RarityWeight(Rarity.Rare,10),
                    new RarityWeight(Rarity.MasterPiece,5),
                    new RarityWeight(Rarity.Epic,0.5f),
                    new RarityWeight(Rarity.Legendary,0.25f),
                    new RarityWeight(Rarity.Mythical,0.001f)
                })
            },
            { 7 ,
                new RarityWeightManager(new RarityWeight[8]{
                    new RarityWeight(Rarity.Imperfect,0.1f),
                    new RarityWeight(Rarity.Inferior,0.3f),
                    new RarityWeight(Rarity.Superior,3),
                    new RarityWeight(Rarity.Rare,7),
                    new RarityWeight(Rarity.MasterPiece,10),
                    new RarityWeight(Rarity.Epic,1),
                    new RarityWeight(Rarity.Legendary,0.5f),
                    new RarityWeight(Rarity.Mythical,0.0025f)
                })
            },
            { 8 ,
                new RarityWeightManager(new RarityWeight[7]{
                    new RarityWeight(Rarity.Inferior,0.1f),
                    new RarityWeight(Rarity.Superior,0.8f),
                    new RarityWeight(Rarity.Rare,4),
                    new RarityWeight(Rarity.MasterPiece,8),
                    new RarityWeight(Rarity.Epic,2),
                    new RarityWeight(Rarity.Legendary,0.75f),
                    new RarityWeight(Rarity.Mythical,0.005f)
                })
            },
            { 9 ,
                new RarityWeightManager(new RarityWeight[7]{
                    new RarityWeight(Rarity.Inferior,0.05f),
                    new RarityWeight(Rarity.Superior,1.0f),
                    new RarityWeight(Rarity.Rare,5),
                    new RarityWeight(Rarity.MasterPiece,10),
                    new RarityWeight(Rarity.Epic,9),
                    new RarityWeight(Rarity.Legendary,1f),
                    new RarityWeight(Rarity.Mythical,0.001f)
                })
            },
            { 10 ,
                new RarityWeightManager(new RarityWeight[6]{
                    new RarityWeight(Rarity.Superior,0.25f),
                    new RarityWeight(Rarity.Rare,1),
                    new RarityWeight(Rarity.MasterPiece,3),
                    new RarityWeight(Rarity.Epic,5),
                    new RarityWeight(Rarity.Legendary,1f),
                    new RarityWeight(Rarity.Mythical,0.005f)
                })
            },
            { 11 ,
                new RarityWeightManager(new RarityWeight[5]{
                    new RarityWeight(Rarity.Rare,0.5f),
                    new RarityWeight(Rarity.MasterPiece,2),
                    new RarityWeight(Rarity.Epic,4),
                    new RarityWeight(Rarity.Legendary,2f),
                    new RarityWeight(Rarity.Mythical,0.01f)
                })
            },
            { 12 ,
                new RarityWeightManager(new RarityWeight[4]{
                    new RarityWeight(Rarity.MasterPiece,1),
                    new RarityWeight(Rarity.Epic,5),
                    new RarityWeight(Rarity.Legendary,2f),
                    new RarityWeight(Rarity.Mythical,0.02f)
                })
            },
            { 13 ,
                new RarityWeightManager(new RarityWeight[3]{
                    new RarityWeight(Rarity.Epic,4),
                    new RarityWeight(Rarity.Legendary,3f),
                    new RarityWeight(Rarity.Mythical,0.05f)
                })
            },
            { 14 ,
                new RarityWeightManager(new RarityWeight[3]{
                    new RarityWeight(Rarity.Epic,5f),
                    new RarityWeight(Rarity.Legendary,5f),
                    new RarityWeight(Rarity.Mythical,0.1f)
                })
            },
            { 15 ,
                new RarityWeightManager(new RarityWeight[3]{
                    new RarityWeight(Rarity.Epic,4f),
                    new RarityWeight(Rarity.Legendary,8f),
                    new RarityWeight(Rarity.Mythical,0.25f)
                })
            },
            { 16 ,
                new RarityWeightManager(new RarityWeight[3]{
                    new RarityWeight(Rarity.Epic,2),
                    new RarityWeight(Rarity.Legendary,8f),
                    new RarityWeight(Rarity.Mythical,0.5f)
                })
            },
            { 17 ,
                new RarityWeightManager(new RarityWeight[2]{
                    new RarityWeight(Rarity.Legendary,10f),
                    new RarityWeight(Rarity.Mythical,1f)
                })
            },
            { -13 ,
                new RarityWeightManager(new RarityWeight[4]{
                    new RarityWeight(Rarity.MasterPiece,0.5f),
                    new RarityWeight(Rarity.Epic,4),
                    new RarityWeight(Rarity.Legendary,2f),
                    new RarityWeight(Rarity.Mythical,0.01f)
                })
            },
            { -12 ,
                new RarityWeightManager(new RarityWeight[4]{
                    new RarityWeight(Rarity.MasterPiece,1f),
                    new RarityWeight(Rarity.Epic,4),
                    new RarityWeight(Rarity.Legendary,2f),
                    new RarityWeight(Rarity.Mythical,0.01f)
                })
            },
            { -11 ,
                new RarityWeightManager(new RarityWeight[6]{
                    new RarityWeight(Rarity.Superior,0.5f),
                    new RarityWeight(Rarity.Rare,2),
                    new RarityWeight(Rarity.MasterPiece,6),
                    new RarityWeight(Rarity.Epic,10),
                    new RarityWeight(Rarity.Legendary,1f),
                    new RarityWeight(Rarity.Mythical,0.001f)
                })
            }


        };

        

        public static RollInfo RollItem(ItemUpdate ItemRoll,Item item, bool bAscendWorldDrop, int AscendWorldDropLevel)
        {
            RollInfo info = new RollInfo();
            info.rarity = GetRarity(item, bAscendWorldDrop, AscendWorldDropLevel);
            info.modifier = GetModifier(item, info.rarity, bAscendWorldDrop, AscendWorldDropLevel);

            if (item.accessory)
                info.stats = GetStatsAccesories(info.rarity, bAscendWorldDrop, AscendWorldDropLevel);
            else if (item.bodySlot > 0 || item.legSlot > 0 || item.headSlot > 0)
                info.stats = GetStatsArmor(info.rarity, bAscendWorldDrop, AscendWorldDropLevel);

            return info;
        }


        
        public static Color GetRarityColor (Rarity rarity)
        {
            Color color = new Color(1f, 1f, 1f);

            switch (rarity)
            {
                case (Rarity.Broken):
                    color = new Color(0.5f, 0.2f, 0.2f);
                    break;
                case (Rarity.Imperfect):
                    color = new Color(0.4f, 0.4f, 0.4f);
                    break;
                case (Rarity.Inferior):
                    color = new Color(0.7f, 0.7f, 0.7f);
                    break;
                case (Rarity.Superior):
                    color = new Color(1f,1f,1f);
                    break;
                case (Rarity.Rare):
                    color = new Color(81, 255, 255);
                    break;
                case (Rarity.MasterPiece):
                    color = new Color(81, 165, 255);
                    break;
                case (Rarity.Epic):
                    color = new Color(133, 81, 255);
                    break;
                case (Rarity.Legendary):
                    color = new Color(255, 176, 20);
                    break;
                case (Rarity.Mythical):
                    color = new Color(255, 47, 20);
                    break;

            }
            return color;
        }

        public static Rarity GetRarity(Item item, bool bAscendWorldDrop, int AscendWorldDropLevel)
        {
            int rarity = item.rare;
            if (rarity >= -1) { 

                if (WorldManager.ascended)
                {
                    rarity+= 2;
                    rarity += Mathf.CeilInt(Mathf.Logx(1 + AscendWorldDropLevel * 0.1f, 10));
                    if (Main.hardMode)
                        rarity++;
                }
                if (NPC.downedMoonlord)
                {
                    rarity++;
                }
            }
            if (rarityConversion.ContainsKey(rarity))
                return rarityConversion[rarity].DrawRarity();
            else return rarityConversion[17].DrawRarity();
        }

        
       

        private static ItemStat GenRandomStat(Rarity rarity,bool bAscendWorldDrop = false, int AscendWorldDropLevel = 0, bool accesories = false)
        {
            float value = 0;
            int rn = Mathf.RandomInt(0, 8);
            Stat stat = (Enum.GetValues(typeof(Stat)) as Stat[])[rn];
            switch (rarity)
            {
                case Rarity.Broken:
                    value = Mathf.Random(-15, -5);
                    break;
                case Rarity.Imperfect:
                    value = .0f;
                    break;
                case Rarity.Inferior:
                    value = Mathf.Random(1, 2);
                    break;
                case Rarity.Superior:
                    value = Mathf.Random(2, 3);
                    break;
                case Rarity.Rare:
                    value = Mathf.Random(3, 5);
                    break;
                case Rarity.MasterPiece:
                    value = Mathf.Random(5, 8);
                    break;
                case Rarity.Epic:
                    value = Mathf.Random(6, 12);
                    break;
                case Rarity.Legendary:
                    value = Mathf.Random(8, 16);
                    break;
                case Rarity.Mythical:
                    value = Mathf.Random(9, 20);
                    break;
            }
            if (accesories)
                value *= Mathf.Random(0.4f, 0.7f);
            if (bAscendWorldDrop)
            {
                float bonus = Mathf.Logx(1 + AscendWorldDropLevel * 0.1f, 10) * 0.2f;
                    value *= (Mathf.Random(1.0f, 1.35f) + bonus);
            }
                
            value = (float)Math.Round(value, 2);
            return new ItemStat(stat, value);

        }

        public static ItemStats GetStatsArmor(Rarity rarity, bool bAscendWorldDrop, int AscendWorldDropLevel)
        {
            ItemStats itemStat = new ItemStats()
            {
                Stats = new List<ItemStat>()
            };
            int statSlot = 0;
            switch (rarity)
            {
                case Rarity.Imperfect:
                    return itemStat;
                case Rarity.Inferior:
                case Rarity.Broken:
                    statSlot = 1;
                    break;
                case Rarity.Superior:
                case Rarity.Rare:
                    statSlot = 2;
                    break;
                case Rarity.MasterPiece:
                case Rarity.Epic:
                    statSlot = 3;
                    break;
                case Rarity.Legendary:
                    statSlot = 4;
                    break;
                case Rarity.Mythical:
                    statSlot = 6;
                    break;
            }

            if (bAscendWorldDrop)
            {
                float bonusSlot = Mathf.CeilInt( Mathf.Logx(1 + AscendWorldDropLevel * 0.1f, 10)*0.5);
                statSlot++;
            }
                

            for (int i = 0; i < statSlot; i++)
            {
                ItemStat st = GenRandomStat(rarity, bAscendWorldDrop);
                itemStat.CreateStat(st);
            }

            return itemStat;
        }

        public static float GetRarityDamageBoost(Rarity rarity)
        {
            if (!Config.gpConfig.ItemRarity)
                return 0;
            float value = 0;
            switch (rarity)
            {
                case (Rarity.Broken):
                    value = -20;
                    break;
                case (Rarity.Imperfect):
                    value = -5;
                    break;
                case (Rarity.Inferior):
                    value = 4;
                    break;
                case (Rarity.Superior):
                    value = 8;
                    break;
                case (Rarity.Rare):
                    value = 12;
                    break;
                case (Rarity.MasterPiece):
                    value = 16;
                    break;
                case (Rarity.Epic):
                    value = 20;
                    break;
                case (Rarity.Legendary):
                    value = 25;
                    break;
                case (Rarity.Mythical):
                    value = 30;
                    break;
            }
            return value;
        }

        public static ItemStats GetStatsAccesories(Rarity rarity, bool bAscendWorldDrop, int AscendWorldDropLevel)
        {
            ItemStats itemStat = new ItemStats()
            {
                Stats = new List<ItemStat>()
            };
            int statSlot = 0;
            switch (rarity)
            {
                case Rarity.Imperfect:
                case Rarity.Inferior:
                    return itemStat;
                case Rarity.Broken:
                    statSlot = (Mathf.Random(0, 2) < 1) ? 0:1;
                    break;
                case Rarity.Superior:
                    statSlot = 1;
                    break;
                case Rarity.Rare:
                    statSlot = (Mathf.Random(0, 2) < 1) ? 1 : 2;
                    break;
                case Rarity.MasterPiece:
                    statSlot = 2;
                    break;
                case Rarity.Epic:
                case Rarity.Legendary:
                    statSlot = (Mathf.Random(0, 2) < 1) ? 2:3;
                    break;
                case Rarity.Mythical:
                    statSlot = 3;
                    break;
            }

            for (int i = 0; i < statSlot; i++)
            {
                ItemStat st = GenRandomStat(rarity, bAscendWorldDrop,AscendWorldDropLevel,true);
                itemStat.CreateStat(st);
            }

            return itemStat;
        }

        static public int GetModCount(Rarity rarity, Item item, bool bAscendWorldDrop, int AscendWorldDropLevel)
        {
            float count = 1;
            switch (rarity)
            {
                case Rarity.Broken:
                case Rarity.Imperfect:
                    count = 0;
                    break;
                case Rarity.Inferior:
                    count = 0.5f;
                    break;
                case Rarity.Superior:
                    count = 1;
                    break;
                case Rarity.Rare:
                    count = 1.5f;
                    break;
                case Rarity.MasterPiece:
                    count = 2;
                    break;
                case Rarity.Epic:
                    count = 2.5f;
                    break;
                case Rarity.Legendary:
                    count = 3f;
                    break;
                case Rarity.Mythical:
                    count = 3.5f;
                    break;
            }
            if (bAscendWorldDrop)
            {
                count *= (1f + 0.25f * Mathf.Logx( 1 + AscendWorldDropLevel*0.1f,10));
            }
            if (item.accessory)
            {
                count -= 1;
                count *= 0.5f;
            }
            else if (item.damage > 0)
            {
                count *= 0.5f;
            }
            return  Mathf.CeilInt( Mathf.Clamp(count,0,5));
        }

        private static Modifier AddRandomModifier(List<Modifier> pool)
        {
            float totalWeight = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                Modifier mod = pool[i];
                if (ModifierWeight.ContainsKey(mod))
                    totalWeight += ModifierWeight[mod];
            }
                

            float rn = Mathf.Random(0, totalWeight);
            float checkingWeight = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                Modifier mod = pool[i];
                if (rn < checkingWeight + ModifierWeight[pool[i]])
                    return pool[i];
                checkingWeight += ModifierWeight[pool[i]];

            }
            return pool[pool.Count - 1];

        }

        static public Modifier GetModifier(Item item, Rarity rarity, bool bAscendWorldDrop, int AscendWorldDropLevel)
        {
            int t = 0;
            if (item.accessory)
                t = 1;
            else if ((item.damage > 0 && item.stack == 1) && item.defense<1 )
                t = 2;

            if (t == 2 && (item.DamageType == DamageClass.Melee  || item.DamageType == DamageClass.Summon))
                t = 3;

            Modifier RModifier = 0;
            Modifier modifierlist = ModifierList(t);

            List<Modifier> ModifierListToAdd = new List<Modifier>();

            List<Modifier> grandlist = (Enum.GetValues(typeof(Modifier)) as Modifier[]).ToList();
            for (int i = 0; i < grandlist.Count; i++)
                if ((grandlist[i] & modifierlist) == grandlist[i])
                {
                    ModifierListToAdd.Add(grandlist[i]);
                }


            int slot = GetModCount(rarity,item, bAscendWorldDrop, AscendWorldDropLevel);
            for (int i = 0; (i < slot) ; i++)
            {
                Modifier modid = AddRandomModifier(ModifierListToAdd);
                RModifier = RModifier | modid;
                ModifierListToAdd.Remove(modid); 
            }
            
            

            return RModifier;

        }

        static public Modifier ModifierList(int type)
        {
            if (type == 0) //Armor
            {
                return Modifier.Berserker |
                    Modifier.MoonLight |
                    Modifier.SunLight |
                    Modifier.MagicConnection |
                    Modifier.Sniper |
                    Modifier.Brawler |
                    Modifier.Savior |
                    Modifier.FireLord |
                    Modifier.Thorny |
                    Modifier.Smart |
                    Modifier.VampiricAura |
                    Modifier.SelfLearning |
                    Modifier.BloodSeeker;
            }
            if (type == 1) // accesories
            {
                return Modifier.Berserker |
                    Modifier.MoonLight |
                    Modifier.SunLight |
                    Modifier.MagicConnection |
                    Modifier.Sniper |
                    Modifier.Brawler |
                    Modifier.Savior |
                    Modifier.Thorny |
                    Modifier.Confusion |
                    Modifier.Executor |
                    Modifier.Confusion |
                    Modifier.Smart |
                    Modifier.Chaotic |
                    Modifier.Cunning |
                    Modifier.VampiricAura |
                    Modifier.BloodSeeker;
            }
            if (type == 2) // weapon
            {
                return Modifier.Berserker |
                    Modifier.MoonLight |
                    Modifier.SunLight |
                    Modifier.MagicConnection |
                    Modifier.Sniper |
                    Modifier.Brawler |
                    Modifier.Piercing |
                    Modifier.Smart |
                    Modifier.SelfLearning |
                    Modifier.Executor |
                    Modifier.Confusion |
                    Modifier.Poisones |
                    Modifier.Venom |
                    Modifier.Chaotic |
                    Modifier.Cunning |
                    Modifier.BloodSeeker|
                    Modifier.VampiricAura |
                    Modifier.Cleave |
                    Modifier.Random;
            }
            if (type == 3) //melee and summon weapon
            {
                return ModifierList(2) - (int)Modifier.Random;
            }
            return Modifier.None;
        }

        static public bool HaveModifier(Modifier _modifier,Modifier list)
        {
            if (!Config.gpConfig.ItemModifier)
                return false;
            return ((_modifier & list) == _modifier);
        }

        static public float GetModifierBonusAlt(Modifier mod, ItemUpdate itemU)
        {
            if (!Config.gpConfig.ItemModifier)
                return 0;

            float value = 0;
            switch (mod)
            {
                case (Modifier.BloodSeeker):
                    value = 0.05f + Mathf.Log2((float)itemU.rarity) * 0.025f + (itemU.Level * 0.005f) + itemU.Ascention * 0.01f;
                    break;
                case (Modifier.Cleave):
                    value = Mathf.Clamp(5 + (itemU.Level * 0.5f) + itemU.Ascention * 2.5f + Mathf.Log2((float)itemU.rarity) * 3f, 0f, 80f);
                    break;

            }
            return value;
        }

        static public float GetModifierBonus(Modifier mod, ItemUpdate itemU)
        {
            if (!Config.gpConfig.ItemModifier)
                return 0;
            float value = 0;
            switch (mod)
            {
                case (Modifier.SunLight):
                case (Modifier.MoonLight):
                    value = 5 + (itemU.Level * 0.1f) + itemU.Ascention * 0.5f + Mathf.Log2((float)itemU.rarity)* 1.5f;
                    break;
                case (Modifier.Berserker):
                    value = (itemU.Level * 0.0025f) + itemU.Ascention * 0.01f + Mathf.Log2((float)itemU.rarity) * 0.025f;
                    break;
                case (Modifier.MagicConnection):
                    value = (itemU.Level * 0.0025f) + itemU.Ascention * 0.01f + Mathf.Log2((float)itemU.rarity) * 0.025f;
                    break;
                case (Modifier.Sniper):
                    value = Mathf.Clamp(0.02f + (itemU.Level * 0.005f) + itemU.Ascention * 0.01f + Mathf.Log2((float)itemU.rarity) * 0.025f,0.05f, Mathf.Log2((float)itemU.rarity)*0.05f);
                    break;
                case (Modifier.Brawler):
                    value = 15 + (itemU.Level * 0.5f) + itemU.Ascention * 2.5f + Mathf.Log2((float)itemU.rarity) * 5f;
                    break;
                case (Modifier.Piercing):
                    value = 10 + (itemU.Level * 0.2f) + itemU.Ascention * 0.5f + Mathf.Log2((float)itemU.rarity) * 2f; ;
                    break;
                case (Modifier.Savior): 
                    value = Mathf.Clamp(10 + (itemU.Level * 0.2f) + itemU.Ascention * 1f + Mathf.Log2((float)itemU.rarity) * 2.5f,10,50);
                    break;
                case (Modifier.FireLord):
                    value = 350 + (itemU.Level * 2.5f) + itemU.Ascention * 10 + Mathf.Log2((float)itemU.rarity) * 25f;
                    break;
                case (Modifier.Thorny):
                    value = 5 + (itemU.Level * 0.5f) + itemU.Ascention * 2.5f + Mathf.Log2((float)itemU.rarity) * 2.5f;
                    break;
                case (Modifier.Smart):
                    value = 10 + (itemU.Level * 5f) + itemU.Ascention * 10f + (float)Math.Round(Mathf.Logx((float)itemU.rarity,1.5f),2)  * 5f;
                    break;
                case (Modifier.SelfLearning):
                    value = 15 + (itemU.Level * 5f) + itemU.Ascention * 10f + (float)Math.Round( Mathf.Logx((float)itemU.rarity,1.3f),2) * 5;
                    break;
                case (Modifier.VampiricAura):
                    float DamageValue = 0;
                    foreach (DamageClass DC in (DamageClass[]) Enum.GetValues(typeof(DamageClass)))
                    {
                        DamageValue += Main.LocalPlayer.GetDamage(DamageClass.Melee).Multiplicative;
                    }
                    
                    value = 0.5f + Mathf.Round((0.05 + (itemU.Level * 0.005f) + itemU.Ascention * .01f + Mathf.Log2((float)itemU.rarity) * .025f) * Mathf.Logx((DamageValue),1.3f),3);
                    break;
                case (Modifier.Executor):
                    value = 0.1f + (itemU.Level * 0.005f) + itemU.Ascention * 0.02f + Mathf.Log2((float)itemU.rarity) * 0.05f;
                    break;
                case (Modifier.Poisones):
                    value = Mathf.Clamp(20 + (itemU.Level * 1f) + itemU.Ascention * 5f,0f,90f);
                    break;
                case (Modifier.Venom):
                    value = 20 + (itemU.Level * 1f) + itemU.Ascention * 5f + Mathf.Log2((float)itemU.rarity) * 5f; ;
                    break;
                case (Modifier.Chaotic):
                    value = Mathf.Clamp(5 + (itemU.Level * 0.05f) + itemU.Ascention * 0.5f + Mathf.Log2((float)itemU.rarity) * 1f, 5f, Mathf.Log2((float)itemU.rarity) * 8f);
                    break;
                case (Modifier.Cunning):
                    value = Mathf.Clamp(5 + (itemU.Level * 0.05f) + itemU.Ascention * 0.5f + Mathf.Log2((float)itemU.rarity) * 1f, 5f, Mathf.Log2((float)itemU.rarity) * 8f);
                    break;
                case (Modifier.BloodSeeker):
                    value = Mathf.Clamp(5f + (itemU.Level * 0.1f) + itemU.Ascention * 1f + Mathf.Log2((float)itemU.rarity) * 2f, 0f, 50f);
                    break;
                case (Modifier.Cleave):
                    value = Mathf.Clamp(5 + (itemU.Level * 0.15f) + itemU.Ascention * 1f + Mathf.Log2((float)itemU.rarity) * 1.5f, 0f, 80f);
                    break;
                case (Modifier.Random):
                    break;
            }
            return value;
        }

        static string GetModDesc (Modifier mod, ItemUpdate itemU)
        {
            string desc = "";
            switch (mod)
            {
                case (Modifier.MoonLight):
                    desc = "Increase damage during night by " + Math.Round(GetModifierBonus(mod, itemU),2) + " %";
                    break;
                case (Modifier.SunLight):
                    desc = "Increase damage during Day by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " %";
                    break;
                case (Modifier.Berserker):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % per missing % of health";
                    break;
                case (Modifier.MagicConnection):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % per missing % of mana";
                    break;
                case (Modifier.Sniper):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % per units of distance up to " + Math.Round(Mathf.Log2((float)itemU.rarity)* 5f,2) + "%";
                    break;
                case (Modifier.Brawler):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % when close to ennemy";
                    break;
                case (Modifier.Piercing):
                    desc = "Pierce " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % of ennemy armor";
                    break;
                case (Modifier.Savior):
                    desc = "" + Math.Round(GetModifierBonus(mod, itemU), 2) + " % Chance to survive a deadly wound";
                    break;
                case (Modifier.FireLord):
                    desc = "Inflict burn debuff to ennemies in a range of " + Math.Round(GetModifierBonus(mod, itemU), 2) + " units";
                    break;
                case (Modifier.Thorny):
                    desc = "reflect " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % of damage receive";
                    break;
                case (Modifier.Smart):
                    desc = "Increase XP gain by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " %";
                    break;
                case (Modifier.SelfLearning):
                    desc = "Increase Item XP gain by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " %";
                    break;
                case (Modifier.VampiricAura):
                    desc = "Drain " + Math.Round(GetModifierBonus(mod, itemU), 2) + " Health Per Seconds to ennemies arround you";
                    break;
                case (Modifier.Executor):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % per missing % of ennemies health";
                    break;
                case (Modifier.Poisones):
                    desc = "" + Math.Round(GetModifierBonus(mod, itemU), 2) + " % chance to inflict Poison debuff";
                    break;
                case (Modifier.Venom):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % against poisoned ennemies";
                    break;
                case (Modifier.Chaotic): 
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % per debuff/buff on Player up to " + Math.Round(Mathf.Log2((float)itemU.rarity) * 8f, 2) + "%";
                    break;
                case (Modifier.Cunning):
                    desc = "Increase damage by " + Math.Round(GetModifierBonus(mod, itemU), 2) + " % per debuff/buff on Ennemy up to " + Math.Round(Mathf.Log2((float)itemU.rarity) * 8f, 2) + "%";
                    break;
                case (Modifier.BloodSeeker):
                    desc = "lifesteal "+ Math.Round(GetModifierBonusAlt(mod, itemU), 2) + " % against ennemies with less than " + Math.Round(GetModifierBonus(mod, itemU),2) + " % health";
                    break;
                case (Modifier.Cleave):
                    desc = "" + Math.Round(GetModifierBonus(mod, itemU),2) + " % AOE damage";
                    break;
                case (Modifier.Random):
                    desc = "Shoot a random projectile every time";
                    break;
            }
            return desc;
        }

        static public string[] GetModifierDesc(ItemUpdate itemU)
        {
            List<string> SList = new List<string>();

            Modifier[] List = (Enum.GetValues(typeof(Modifier)) as Modifier[]);
            for (int i = 1;i< List.Length;i++)
                if (HaveModifier(List[i], itemU.modifier))
                    SList.Add(GetModDesc(List[i], itemU));

            return SList.ToArray();
        }
    }
}
