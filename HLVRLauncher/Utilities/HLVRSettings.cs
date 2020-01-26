﻿using Microsoft.Collections.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLVRLauncher.Utilities
{
    public enum SettingValueType
    {
        STRING,
        BOOLEAN,
        FLOAT,
        INT
    }

    public class Setting
    {
        public SettingValueType Type { get; set; } = SettingValueType.BOOLEAN;
        public string Description { get; set; } = null;
        public string DefaultValue { get; set; } = null;
        public string Value { get; set; } = null;
        public readonly OrderedDictionary<string, string> AllowedValues = new OrderedDictionary<string, string>();

        public bool IsTrue()
        {
            return Type == SettingValueType.BOOLEAN && !Value.Equals("0");
        }

        public static Setting Create(string description, bool value, OrderedDictionary<string, string> allowedValues = null)
        {
            var setting = new Setting()
            {
                Type = SettingValueType.BOOLEAN,
                Value = value ? "1" : "0",
                DefaultValue = value ? "1" : "0",
                Description = description
            };
            if (allowedValues != null)
            {
                foreach (var allowedValue in allowedValues)
                    setting.AllowedValues.Add(allowedValue.Key, allowedValue.Value);
            }
            return setting;
        }

        public static Setting Create(string description, int value, OrderedDictionary<string, string> allowedValues = null)
        {
            var setting = new Setting()
            {
                Type = SettingValueType.INT,
                Value = value.ToString(),
                DefaultValue = value.ToString(),
                Description = description
            };
            if (allowedValues != null)
            {
                foreach (var allowedValue in allowedValues)
                    setting.AllowedValues.Add(allowedValue.Key, allowedValue.Value);
            }
            return setting;
        }

        public static Setting Create(string description, float value, OrderedDictionary<string, string> allowedValues = null)
        {
            var setting = new Setting()
            {
                Type = SettingValueType.FLOAT,
                Value = value.ToString(),
                DefaultValue = value.ToString(),
                Description = description
            };
            if (allowedValues != null)
            {
                foreach (var allowedValue in allowedValues)
                    setting.AllowedValues.Add(allowedValue.Key, allowedValue.Value);
            }
            return setting;
        }

        public static Setting Create(string description, string value, OrderedDictionary<string, string> allowedValues = null)
        {
            var setting = new Setting()
            {
                Type = SettingValueType.STRING,
                Value = value,
                DefaultValue = value,
                Description = description
            };
            if (allowedValues != null)
            {
                foreach (var allowedValue in allowedValues)
                    setting.AllowedValues.Add(allowedValue.Key, allowedValue.Value);
            }
            return setting;
        }

        // default constructor for json
        public Setting()
        {
        }
    }

    public class HLVRSettings
    {
        public OrderedDictionary<string, OrderedDictionary<string, Setting>> InputSettings = new OrderedDictionary<string, OrderedDictionary<string, Setting>>()
        {
            { "Anti-nausea features", new OrderedDictionary<string, Setting>() {
                { "vr_move_instant_accelerate", Setting.Create( "Instant acceleration", true ) },
                { "vr_move_instant_decelerate", Setting.Create( "Instant deceleration", true ) },
                { "vr_rotate_with_trains", Setting.Create( "Rotate with trains/elevators", true ) },
                { "vr_no_gauss_recoil", Setting.Create( "Disable gauss recoil", true ) },
                { "vr_disable_triggerpush", Setting.Create( "Disable areas that push the player (e.g. strong wind)", true ) },
                { "vr_disable_func_friction", Setting.Create( "Disable slippery surfaces", true ) },
                { "vr_xenjumpthingies_teleporteronly", Setting.Create( "Disable being pushed up by xen jump thingies (you can use the teleporter on those things)", true ) },
                { "vr_semicheat_spinthingyspeed", Setting.Create( "Set maximum speed for those fast spinning platforms near the tentacle monster (map c1a4f) (game default: 110, mod default: 50)", 50,
                    new OrderedDictionary<string, string>(){ { "10", "10" }, { "20", "20" }, { "30", "30" }, { "40", "40" }, { "50", "50" }, { "60", "60" }, { "70", "70" }, { "80", "80" }, { "90", "90" }, { "100", "100" }, { "110", "110" } } ) },
                { "vr_smooth_steps", Setting.Create( "Smooth out steps (may affect performance!)", 0, new OrderedDictionary<string, string>(){ {"0", "Disabled" }, { "1", "Smooth out most steps" }, { "2", "Smooth out all steps" } } ) },
            } },

            { "Controls & Accessibility", new OrderedDictionary<string, Setting>() {
                { "vr_lefthand_mode", Setting.Create( "Left hand mode", false ) },

                { "vr_crowbar_vanilla_attack_enabled", Setting.Create( "Enable crowbar melee attack on 'fire' action.", false ) },

                { "vr_flashlight_toggle", Setting.Create( "Flashlight should toggle", false ) },

                { "vr_enable_aim_laser", Setting.Create( "Enable aim laser pointer for range weapons", false ) },

                { "vr_make_levers_nonsolid", Setting.Create( "Make levers non-solid (fixes some collision issues)", true ) },

                { "vr_use_animated_weapons", Setting.Create( "Use weapon models with animated movement (affects aiming)", false ) },

                { "vr_melee_swing_speed", Setting.Create( "Minimum controller speed to trigger melee damage", 100 ) },

                { "vr_headset_offset", Setting.Create( "HMD height offset", 0 ) },

                { "vr_weapon_grenade_mode", Setting.Create( "Grenade throw mode", 0, new OrderedDictionary<string, string>(){ {"0", "Use controller velocity" }, { "1", "Aim with controller, but fly with fixed speed (as in original game)" } } ) },

                { "vr_ledge_pull_mode", Setting.Create( "Ledge pulling", 1, new OrderedDictionary<string, string>(){ {"0", "Disabled" }, { "1", "Move onto ledge when pulling" }, { "2", "Teleport onto ledge instantly when pulling" } } ) },
                { "vr_ledge_pull_speed", Setting.Create( "Minimum controller speed to trigger ledge pulling", 50 ) },

                { "vr_teleport_attachment", Setting.Create( "Teleporter attachment", 0, new OrderedDictionary<string, string>(){ {"0", "Hand" }, { "1", "Weapon" }, { "2", "Head (HMD)" }, { "3", "SteamVR Input pose" } } ) },
                { "vr_flashlight_attachment", Setting.Create( "Flashlight attachment", 0 , new OrderedDictionary<string, string>(){ {"0", "Hand" }, { "1", "Weapon" }, { "2", "Head (HMD)" }, { "3", "SteamVR Input pose" } }) },
                { "vr_movement_attachment", Setting.Create( "Movement attachment", 0 , new OrderedDictionary<string, string>(){ {"0", "Hand" }, { "1", "Weapon" }, { "2", "Head (HMD)" }, { "3", "SteamVR Input pose" } }) },
            } },

            { "Movement", new OrderedDictionary<string, Setting>() {
                { "vr_rl_ducking_enabled", Setting.Create( "Enable real-life crouch detection", true ) },
                { "vr_rl_duck_height", Setting.Create( "Real-life crouch height", 100 ) },
                { "vr_playerturn_enabled", Setting.Create( "Enable player turning", true ) },
                { "vr_autocrouch_enabled", Setting.Create( "Enabled automatic crouching", true ) },
                { "vr_move_analogforward_inverted", Setting.Create( "Invert analog forward input", false ) },
                { "vr_move_analogsideways_inverted", Setting.Create( "Invert analog sideways input", false ) },
                { "vr_move_analogupdown_inverted", Setting.Create( "Invert analog updown input", false ) },
                { "vr_move_analogturn_inverted", Setting.Create( "Invert analog turn input", false ) },
            } },

            { "Ladders", new OrderedDictionary<string, Setting>() {
                { "vr_ladder_immersive_movement_enabled", Setting.Create( "Enabled immersive climbing of ladders", true ) },
                { "vr_ladder_immersive_movement_swinging_enabled", Setting.Create( "Enable momentum from immersive ladder climbing", true ) },
                { "vr_ladder_legacy_movement_enabled", Setting.Create( "Enable legacy climbing of ladders", false ) },
                { "vr_ladder_legacy_movement_only_updown", Setting.Create( "Restrict movement on ladders to up/down only (legacy movement)", false ) },
                { "vr_ladder_legacy_movement_speed", Setting.Create( "Climb speed on ladders (legacy movement)", 100 ) },
            } },

            { "Trains", new OrderedDictionary<string, Setting>() {
                // { "vr_legacy_train_controls_enabled", Setting.Create( "Enable control of usable trains with 'LegacyUse' and forward/backward movement.", true ) },
            } },

            { "Mounted guns", new OrderedDictionary<string, Setting>() {
                { "vr_tankcontrols", Setting.Create( "Immersive controls", 2, new OrderedDictionary<string, string>(){ {"0", "disabled" }, { "1", "enabled with one hand" }, { "2", "enabled with both hands" } } ) },
                { "vr_tankcontrols_max_distance", Setting.Create( "Maximum controller distance to gun for immersive controls", 128 ) },
                { "vr_tankcontrols_instant_turn", Setting.Create( "Enable instant-turn (if off, guns turn with a predefined speed, lagging behind your hands)", false ) },
                { "vr_make_mountedguns_nonsolid", Setting.Create( "Make mounted guns non-solid (fixes some collision issues)", true ) },
                { "vr_legacy_tankcontrols_enabled", Setting.Create( "Enable control of usable guns with 'LegacyUse'.", false ) },
            } },
        };

        public OrderedDictionary<string, OrderedDictionary<string, Setting>> GraphicsSettings = new OrderedDictionary<string, OrderedDictionary<string, Setting>>()
        {
            { "FPS & Performance", new OrderedDictionary<string, Setting>() {
                { "vr_headset_fps", Setting.Create( "VR FPS (actual fps in your headset, sync these with your SteamVR settings)", 90 ) },
                { "vr_displaylist_fps", Setting.Create( "Engine FPS (for animations and moving objects, keep these as low as possible for best performance)", 25 ) },
                { "vr_displaylist_synced", Setting.Create( "Sync VR and Engine FPS (fixes model jitter, but poor performance)", false ) },
            } },

            { "Quality", new OrderedDictionary<string, Setting>() {
                { "vr_use_hd_models", Setting.Create( "Use HD models", false ) },
                { "vr_hd_textures_enabled", Setting.Create( "Use HD textures", false ) }
            } },

            { "World customization & scaling", new OrderedDictionary<string, Setting>() {
                { "vr_world_scale", Setting.Create( "World scale", 1.0f ) },
                { "vr_world_z_strech", Setting.Create( "World height factor", 1.0f ) },

                { "vr_npcscale", Setting.Create( "NPC scale", 1.0f ) },
            } },

            { "HUD", new OrderedDictionary<string, Setting>() {
                { "vr_hud_mode", Setting.Create( "HUD mode", 2, new OrderedDictionary<string, string>(){ { "2", "On Controllers" }, { "1", "In View (true HUD)" }, { "0", "Disabled" } } ) },

                { "vr_hud_health", Setting.Create( "Show health in HUD", true ) },
                { "vr_hud_damage_indicator", Setting.Create( "Show damage indicator in HUD", true ) },
                { "vr_hud_flashlight", Setting.Create( "Show flashlight in HUD", true ) },
                { "vr_hud_ammo", Setting.Create( "Show ammo in HUD", true ) },
            } },
        };

        public OrderedDictionary<string, OrderedDictionary<string, Setting>> AudioSettings = new OrderedDictionary<string, OrderedDictionary<string, Setting>>()
        {
            { "FMOD", new OrderedDictionary<string, Setting>() {
                { "vr_use_fmod", Setting.Create( "Use FMOD", true ) },
                { "vr_fmod_3d_occlusion", Setting.Create( "Enable 3D occlusion (only works with FMOD enabled)", true ) },
                { "vr_fmod_wall_occlusion", Setting.Create( "Wall occlusion (how much sound gets blocked by walls in %)", 40 ) },
                { "vr_fmod_door_occlusion", Setting.Create( "Door occlusion (how much sound gets blocked by doors in %)", 30 ) },
                { "vr_fmod_water_occlusion", Setting.Create( "Water occlusion (how much sound gets blocked by water in %)", 20 ) },
                { "vr_fmod_glass_occlusion", Setting.Create( "Glass occlusion (how much sound gets blocked by glass in %)", 10 ) },
            } },

            { "Speech recognition", new OrderedDictionary<string, Setting>() {
                { "vr_speech_commands_enabled", Setting.Create( "Enable speech recognition", false ) },

                { "vr_speech_commands_follow", Setting.Create( "Follow commands", "follow-me|come|lets-go" ) },
                { "vr_speech_commands_wait", Setting.Create( "Wait commands", "wait|stop|hold") },
                { "vr_speech_commands_hello", Setting.Create( "Greetings", "hello|good-morning|hey|hi|morning|greetings" ) },
            } },
        };

        public OrderedDictionary<string, OrderedDictionary<string, Setting>> OtherSettings = new OrderedDictionary<string, OrderedDictionary<string, Setting>>()
        {
            { "Advanced HUD", new OrderedDictionary<string, Setting>() {
                { "vr_hud_size", Setting.Create( "HUD scale", 1 ) },
                { "vr_hud_textscale", Setting.Create( "HUD text scale", 1 ) },

                { "vr_hud_ammo_offset_x", Setting.Create( "HUD ammo display offset X", 3 ) },
                { "vr_hud_ammo_offset_y", Setting.Create( "HUD ammo display offset Y", -3 ) },

                { "vr_hud_flashlight_offset_x", Setting.Create( "HUD ammo display offset X", -5 ) },
                { "vr_hud_flashlight_offset_y", Setting.Create( "HUD ammo display offset Y", -2 ) },

                { "vr_hud_health_offset_x", Setting.Create( "HUD ammo display offset X", -4 ) },
                { "vr_hud_health_offset_y", Setting.Create( "HUD ammo display offset Y", -3 ) },
            } },

            { "Individual weapon scaling", new OrderedDictionary<string, Setting>() {
                { "vr_weaponscale", Setting.Create( "Weapon scale", 1.0f ) },

                { "vr_gordon_hand_scale", Setting.Create( "Hand scale", 1.0f ) },
                { "vr_crowbar_scale", Setting.Create( "Crowbar scale", 1.0f ) },

                { "vr_9mmhandgun_scale", Setting.Create( "9mm handgun scale", 1.0f ) },
                { "vr_357_scale", Setting.Create( "357 scale", 1.0f ) },

                { "vr_9mmar_scale", Setting.Create( "9mm AR scale", 1.0f ) },
                { "vr_shotgun_scale", Setting.Create( "Shotgun scale", 1.0f ) },
                { "vr_crossbow_scale", Setting.Create( "Crossbow scale", 1.0f ) },

                { "vr_rpg_scale", Setting.Create( "RPG scale", 1.0f ) },
                { "vr_gauss_scale", Setting.Create( "Gauss scale", 1.0f ) },
                { "vr_egon_scale", Setting.Create( "Egon scale", 1.0f ) },
                { "vr_hgun_scale", Setting.Create( "Hornet gun scale", 1.0f ) },

                { "vr_grenade_scale", Setting.Create( "Grenade scale", 1.0f ) },
                { "vr_tripmine_scale", Setting.Create( "Tripmine scale", 1.0f ) },
                { "vr_satchel_scale", Setting.Create( "Satchel", 1.0f ) },
                { "vr_satchel_radio_scale", Setting.Create( "Satchel radio scale", 1.0f ) },
                { "vr_squeak_scale", Setting.Create( "Snark scale", 1.0f ) },
            } },

            { "Other", new OrderedDictionary<string, Setting>() {
                { "vr_force_introtrainride", Setting.Create( "Enable hacky fix for issues with intro train ride", true ) },
                { "vr_view_dist_to_walls", Setting.Create( "Minimum view distance to walls", 5.0f ) },
                { "vr_classic_mode", Setting.Create( "Classic mode (unchanged vanilla 1998 models and textures, overrides HD graphics settings)", false ) },
            } },
        };

        public OrderedDictionary<string, OrderedDictionary<string, Setting>> LauncherSettings = new OrderedDictionary<string, OrderedDictionary<string, Setting>>()
        {
            { HLVRLauncherConfig.CategoryLauncher, new OrderedDictionary<string, Setting>() {
                { HLVRLauncherConfig.MinimizeToTray, Setting.Create( "Minimize to tray", true ) },
                { HLVRLauncherConfig.StartMinimized, Setting.Create( "Start HLVRLauncher minimized", false ) },
                { HLVRLauncherConfig.AutoRunMod, Setting.Create( "Run mod automatically when starting HLVRLauncher", false ) },
                { HLVRLauncherConfig.AutoCloseLauncher, Setting.Create( "Exit HLVRLauncher automatically when Half-Life: VR exits", false ) },
                { HLVRLauncherConfig.AutoCloseGame, Setting.Create( "Terminate game automatically when HLVRLauncher is closed", false ) },
            } },
        };

        public HLVRSettings()
        {
        }

    }
}
