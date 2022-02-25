﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using AnotherRpgMod.RPGModule.Entities;
namespace AnotherRpgMod.RPGModule
{
    class ClassNode : Node
    {
        ClassType classType;
        public ClassType GetClassType
        {
            get
            {
                return classType;
            }
        }

        public string GetClassName
        {
            get
            {
                switch(classType)
                {
                    case ClassType.AscendedDeity:
                        return "Ascended Deity";
                    case ClassType.AscendedFortress:
                        return "Ascended Fortress";
                    case ClassType.AscendedHitman:
                        return "Ascended Hitman";
                    case ClassType.AscendedMystic:
                        return "Ascended Mystic";
                    case ClassType.AscendedShadowDancer:
                        return "Ascended ShadowDancer";
                    case ClassType.AscendedSoulLord:
                        return "Ascended SoulLord";
                    case ClassType.AscendedSwordSaint:
                        return "Ascended SwordSaint";
                    case ClassType.AscendedWindWalker:
                        return "Ascended WindWalker";
                    case ClassType.TranscendentalBeing:
                        return "Transcendental Being";
                    default:
                        return "" + classType;

                }

                
            }
        }

        public ClassNode(ClassType _classType, NodeType _type, bool _unlocked = false, float _value = 1, int _levelrequirement = 0, int _maxLevel = 1, int _pointsPerLevel = 1, bool _ascended = false) : base(_type, _unlocked, _value, _levelrequirement, _maxLevel, _pointsPerLevel, _ascended)
        {
            classType = _classType;
        }

        public void loadingUpgrade()
        {
            base.Upgrade();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            UpdateClass();
        }

        public void Disable(RPGPlayer p)
        {
            if (p.GetskillTree.ActiveClass == this)
                p.GetskillTree.ActiveClass = null;
            enable = false;
        }

        public void Disable()
        {
            if (Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.ActiveClass == this)
                Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.ActiveClass = null;
            enable = false;
        }

        public void UpdateClass()
        {
            NodeParent _node;
            ClassType Active = ClassType.Hobo;
            if (Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.ActiveClass != null)
                Active = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.ActiveClass.GetClassType;
            for (int i = 0; i < Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.nodeList.nodeList.Count; i++)
            {
                _node = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.nodeList.nodeList[i];
                if (_node.GetNodeType == NodeType.Class)
                {
                    ClassNode classNode = (ClassNode)_node.GetNode;
                    if (Active != classNode.GetClassType && classNode.GetActivate)
                    {
                        classNode.Disable();
                    }

                }
            }
        }

        public override void ToggleEnable()
        {
            base.ToggleEnable();
            
            if (enable)
                Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.ActiveClass = this;
            else
            {
                Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>().GetskillTree.ActiveClass = null;
            }
            
            RPGPlayer player = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>();
            

            UpdateClass();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                player.SendClientChanges(player);
        }


    }
}
