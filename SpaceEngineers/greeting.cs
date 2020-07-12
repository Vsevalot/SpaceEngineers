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
// using Sandbox.ModAPI;


namespace Greetings
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------
        IMyTextPanel greet_panel, debug_panel;
        IMySensorBlock sensor;
        List<MyDetectedEntityInfo> detected_entities;
        MyDetectedEntityInfo player_entity;
        IMyTerminalBlock jukebox;
        MyDetectedEntityType player_type = MyDetectedEntityType.CharacterHuman;
        DateTime last_greetings = DateTime.Now;
        TimeSpan greetings_interval = new TimeSpan(0, 15, 0);
        string authorization_spaces = "                ";
        string greeted_player_name = "Viandante";
        int ticks_passed;
        string central_n = "\n\n\n\n\n\n\n";
        bool player_is_inside = false;
        bool can_greet = true;
        bool greeting_started = false;

        public Program()
        {
            greet_panel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("greetings_LCD");
            greet_panel.ContentType = ContentType.TEXT_AND_IMAGE;
            debug_panel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("debug_panel");
            debug_panel.ContentType = ContentType.TEXT_AND_IMAGE;

            sensor = (IMySensorBlock)GridTerminalSystem.GetBlockWithName("greetings_sensor");
            detected_entities = new List<MyDetectedEntityInfo>();

            jukebox = GridTerminalSystem.GetBlockWithName("greetings_jukebox");
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Main(string arg)
        {
            if ((DateTime.Now - last_greetings) < greetings_interval){
                debug_panel.WriteText($"Last greetings: {DateTime.Now - last_greetings} {((DateTime.Now - last_greetings) < greetings_interval)}");
                return;
            } else 
                can_greet = true;

            if (can_greet){
                if (!player_is_inside){
                detected_entities.Clear();
                sensor.DetectedEntities(detected_entities);
                    foreach(MyDetectedEntityInfo entity in detected_entities)
                    {
                        if (entity.Type == player_type)
                        {
                            player_entity = entity;
                            player_is_inside = true;
                            sensor.Enabled = false;
                            ticks_passed = 0;
                            break;
                        }
                    }
                } else {
                    if (ticks_passed < 100) {
                        greet_panel.WriteText($"{central_n}{authorization_spaces}\\    Authorization    \\");
                    } else if (ticks_passed < 200) {
                        greet_panel.WriteText($"{central_n}{authorization_spaces}|    Authorization.   |");
                    } else if (ticks_passed < 300) {
                        greet_panel.WriteText($"{central_n}{authorization_spaces}/   Authorization..  /");
                    } else if (ticks_passed < 400) {
                        greet_panel.WriteText($"{central_n}{authorization_spaces}\\   Authorization... \\");
                    } else if (ticks_passed < 500) {
                        greet_panel.WriteText($"{central_n}{authorization_spaces}|   Authorization... |");
                    } else if (ticks_passed < 600) {
                        greet_panel.WriteText($"{central_n}{authorization_spaces}/   Authorization... /");
                    } else if (ticks_passed < 1200) {
                        if (player_entity.Name == greeted_player_name){
                            if (!greeting_started){
                                greet_panel.WriteText($"{central_n}            Welcome back, commander");
                                jukebox.ApplyAction("OnOff_On");
                                greeting_started = true;
                            }   
                        }
                    } else {
                        jukebox.ApplyAction("OnOff_Off");
                        sensor.Enabled = true;
                        player_is_inside = false;
                        can_greet = false;
                        greeting_started = false;
                        last_greetings = DateTime.Now;
                        return;
                    }
                    ticks_passed = ticks_passed + 100;
                }
            }
        }

        public void Save()
        { }

        //------------END--------------
    }
}