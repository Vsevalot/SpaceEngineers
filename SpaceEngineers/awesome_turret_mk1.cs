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


namespace AwesomeTurret
{
    public sealed class Program : MyGridProgram
    {
        //------------BEGIN--------------
        IMyTextPanel debug_panel;
        IMyLargeTurretBase targeting_turret;
        IMyMotorStator azimuth_rotor, left_rotor, right_rotor;
        Vector3D p, block_p;
        IMySensorBlock sensor;
        List<MyDetectedEntityInfo> detected_entities;
        MyDetectedEntityType player_type = MyDetectedEntityType.CharacterHuman;
        float azimuth_atan2;
        int azimuth_mult = 5;
        float delta;

        public Program()
        {
            debug_panel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("debug_LCD");
            debug_panel.ContentType = ContentType.TEXT_AND_IMAGE;

            targeting_turret = (IMyLargeTurretBase)GridTerminalSystem.GetBlockWithName("targeting_turret");
            
            azimuth_rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("azimuth_rotor");
            left_rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("left_rotor");
            right_rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("right_rotor");

            sensor = (IMySensorBlock)GridTerminalSystem.GetBlockWithName("test_sensor");
            detected_entities = new List<MyDetectedEntityInfo>();

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            debug_panel.WriteText("");
        }

        Vector3D convert_to_block_vector(IMyTerminalBlock block, Vector3D p){
            double x = p.X * block.WorldMatrix.M11 + p.Y * block.WorldMatrix.M12 + p.Z * block.WorldMatrix.M13;
            double y = p.X * block.WorldMatrix.M21 + p.Y * block.WorldMatrix.M22 + p.Z * block.WorldMatrix.M23;
            double z = p.X * block.WorldMatrix.M31 + p.Y * block.WorldMatrix.M32 + p.Z * block.WorldMatrix.M33;
            return new Vector3D(x, y, z);
        }

        float get_azimuth(float current_angle, float desired_angle){
            delta = desired_angle - current_angle;
            if (delta > Math.PI) delta -= 2 * (float)Math.PI;
            else if (delta < -Math.PI) delta += 2 * (float)Math.PI;
            return delta;
        }

        public void Main()
        {
            detected_entities.Clear();
            sensor.DetectedEntities(detected_entities);
            foreach(MyDetectedEntityInfo entity in detected_entities)
            {
                if (entity.Type == player_type)
                {
                    p = entity.Position - azimuth_rotor.GetPosition();
                    break;
                }
            }
            block_p = convert_to_block_vector(azimuth_rotor, p);
            azimuth_atan2 = (float)Math.Atan2(-block_p.X, block_p.Z);
            azimuth_rotor.TargetVelocityRad = get_azimuth(azimuth_rotor.Angle, azimuth_atan2) * azimuth_mult;
        }
        
        void find_xyz(IMyTerminalBlock block, IMyTextPanel debug_panel){
            debug_panel.WriteText($"GPS:Origin:{block.WorldMatrix.M41}:{block.WorldMatrix.M42}:{block.WorldMatrix.M43}:\n");

            double xx = block.WorldMatrix.M41 + block.WorldMatrix.M11;
            double xy = block.WorldMatrix.M42 + block.WorldMatrix.M12;
            double xz = block.WorldMatrix.M43 + block.WorldMatrix.M13;
            debug_panel.WriteText($"GPS:X:{xx}:{xy}:{xz}:\n", true);
            
            double yx = block.WorldMatrix.M41 + block.WorldMatrix.M21;
            double yy = block.WorldMatrix.M42 + block.WorldMatrix.M22;
            double yz = block.WorldMatrix.M43 + block.WorldMatrix.M23;
            debug_panel.WriteText($"GPS:Y:{yx}:{yy}:{yz}:\n", true);
            
            double zx = block.WorldMatrix.M41 + block.WorldMatrix.M31;
            double zy = block.WorldMatrix.M42 + block.WorldMatrix.M32;
            double zz = block.WorldMatrix.M43 + block.WorldMatrix.M33;
            debug_panel.WriteText($"GPS:Z:{zx}:{zy}:{zz}:\n", true);
        }

        public void Save()
        { }

        //------------END--------------
    }
}