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

namespace test_script
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------

        bool startFlag = false;
        IMyTextPanel LCD;
        List<IMyCargoContainer> CargosList;
        List<MyInventoryItem> ContainerItems;
        Dictionary<string, float> dict;
        int counter;
        Program()
        {
            LCD = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("LCD");
            LCD.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
        }

        void Main(string args)
        {
            if (args == "start")
            {
                startFlag = true;
                LCD.WriteText("start flag\n");
                Runtime.UpdateFrequency = UpdateFrequency.Update100;

            }

            if (args == "stop")
            {
                startFlag = false;
                Runtime.UpdateFrequency = UpdateFrequency.None;
                LCD.WriteText("start flag\n");
            }

        }

        void Save()
        {

        }

        //------------END--------------
    }
}