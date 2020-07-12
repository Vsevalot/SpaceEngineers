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
//using Sandbox.ModAPI;

namespace Manipulator_mk2
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------
        IMyCockpit cockpit;
        IMyTextSurface cockpit_lcd;
        IMyMotorStator roll_rotor, pitch_rotor;


        public Program()
        {
            cockpit = (IMyCockpit)GridTerminalSystem.GetBlockWithName("manipulator_cockpit");
            cockpit_lcd = (IMyTextSurface)cockpit.GetSurface(4);
            cockpit_lcd.ContentType = ContentType.TEXT_AND_IMAGE;

            roll_rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("roll_rotor");
            pitch_rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("pitch_rotor");
            
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Main()
        {
            roll_rotor.TargetVelocityRPM = cockpit.RollIndicator;
            pitch_rotor.TargetVelocityRPM = cockpit.RotationIndicator.X;
            cockpit_lcd.WriteText($"MoveIndicator.X: {cockpit.MoveIndicator.X}\n" +
                                  $"MoveIndicator.Y: {cockpit.MoveIndicator.Y}\n" +
                                  $"MoveIndicator.Z: {cockpit.MoveIndicator.Z}\n" +
                                  $"Pitch: {cockpit.RotationIndicator.X}\n" +
                                  $"Horizontal rotation: {cockpit.RotationIndicator.Y}\n" +
                                  $"Roll: {cockpit.RollIndicator}");
        }

        public void Save()
        { }

        //------------END--------------
    }
}