using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using System.Linq;
//using Sandbox.ModAPI;


namespace Turret
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------
        IMyTextPanel test_panel;
        IMyLargeTurretBase turret;
        IMySensorBlock sensor;
        List<MyDetectedEntityInfo> detected_entities;
        MyDetectedEntityType player_type = MyDetectedEntityType.CharacterHuman;

        public Program()
        {
            test_panel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("LCD");
            test_panel.ContentType = ContentType.TEXT_AND_IMAGE;

            turret = (IMyLargeTurretBase)GridTerminalSystem.GetBlockWithName("test_turret");
            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            sensor = (IMySensorBlock)GridTerminalSystem.GetBlockWithName("turret_sensor");
            detected_entities = new List<MyDetectedEntityInfo>();

            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Main()
        {
            test_panel.WriteText("");
            detected_entities.Clear();
            sensor.DetectedEntities(detected_entities);
            foreach(MyDetectedEntityInfo entity in detected_entities)
            {
                if (entity.Type == player_type)
                {
                    if (entity.Velocity == new Vector3D(0.0))
                    {
                        test_panel.WriteText($"I see: {entity.Type} at {entity.Name}\n", true);
                        turret.SetTarget(entity.Position);
                    }
                    break;
                }
                
            }
        }

        public void Save()
        { }

        //------------END--------------
    }
}