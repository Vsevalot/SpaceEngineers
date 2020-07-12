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

namespace Manipulator_mk1
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------
        IMyCockpit cockpit;
        IMyPistonBase piston_y, piston_z;
        IMyMotorStator rotor;
        IMyTextSurface cockpit_lcd;


        public Program()
        {
            cockpit = (IMyCockpit)GridTerminalSystem.GetBlockWithName("drill_cockpit");
            piston_y = (IMyPistonBase)GridTerminalSystem.GetBlockWithName("piston_y");
            piston_z = (IMyPistonBase)GridTerminalSystem.GetBlockWithName("piston_z");
            rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("circle_rotor");
            cockpit_lcd = (IMyTextSurface)cockpit.GetSurface(4);
            cockpit_lcd.ContentType = ContentType.TEXT_AND_IMAGE;
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Main()
        {
            rotor.TargetVelocityRPM = cockpit.MoveIndicator.X;
            piston_y.Velocity = -cockpit.MoveIndicator.Z;
            piston_z.Velocity = -cockpit.MoveIndicator.Y;
            cockpit_lcd.WriteText($"MoveIndicator.X: {cockpit.MoveIndicator.X}\n" +
                                  $"MoveIndicator.Y: {cockpit.MoveIndicator.Y}\n" +
                                  $"MoveIndicator.Z: {cockpit.MoveIndicator.Z}\n" +
                                  $"Vertical rotation: {cockpit.RotationIndicator.X}\n" +
                                  $"Horizontal rotation: {cockpit.RotationIndicator.Y}\n");
        }

        public void Save()
        { }

        //------------END--------------
    }
}