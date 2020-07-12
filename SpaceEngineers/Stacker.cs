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

namespace Stacker
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------

        IMyTextPanel test_panel;
        List<MyInventoryItem> inventory_list;
        List<IMyCargoContainer> cargo_list;
        IMyInventory cargo_inventory;
        public Program()
        {
            test_panel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("test_lcd");
            test_panel.ContentType = ContentType.TEXT_AND_IMAGE;

            inventory_list = new List<MyInventoryItem>();
            cargo_list = new List<IMyCargoContainer>();
        }
        public void Reset()
        {
            test_panel.WriteText("");
        }
        public void Resort()
        {
            Reset();
            cargo_list.Clear();
            GridTerminalSystem.GetBlocksOfType(cargo_list);

            foreach (IMyCargoContainer cargo in cargo_list)
            {
                inventory_list.Clear();

                cargo_inventory = cargo.GetInventory();
                cargo_inventory.GetItems(inventory_list);

                foreach (MyInventoryItem item in inventory_list)
                {
                    cargo_inventory.TransferItemFrom(cargo_inventory, item, item.Amount);
                    test_panel.WriteText($"{item.Type.SubtypeId}: {item.Amount}\n", true);
                }

            }
        }

        public void ShowInventory()
        {
            Reset();
            GridTerminalSystem.GetBlocksOfType(cargo_list);

            foreach (IMyCargoContainer cargo in cargo_list)
            {
                inventory_list.Clear();
                cargo.GetInventory().GetItems(inventory_list);
                test_panel.WriteText($"{cargo.CustomName}\n", true);
                foreach (MyInventoryItem item in inventory_list)
                {
                    test_panel.WriteText($"{item.Type.SubtypeId}: {item.Amount}\n", true);
                }
                test_panel.WriteText("\n", true);
            }

        }
        public void Main(string arg)
        {
            switch (arg)
            {
                case "Reset":
                    {
                        Reset();
                        break;
                    }
                case "Resort":
                    {
                        Resort();
                        break;
                    }
                case "Show":
                    {
                        ShowInventory();
                        break;
                    }
            }
        }

        public void Save()
        { }

        //------------END--------------
    }
}