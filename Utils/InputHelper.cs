using BepInEx.IL2CPP.UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5.HisaCat.AllItemsHere.Utils
{
    internal static class InputHelper
    {
        public static bool GetKeyDown(KeyCode key, bool useEvent = false)
        {
            if (UnityEngine.Event.current == null) return false;
            if (UnityEngine.Event.current.type != UnityEngine.EventType.KeyDown) return false;
            if (Input.GetKeyInt(key))
            {
                if (useEvent) UnityEngine.Event.current.Use();
                return true;
            }
            return false;
        }
        public static bool GetKey(KeyCode key, bool useEvent = false)
        {
            if (UnityEngine.Event.current == null) return false;
            if (Input.GetKeyInt(key))
            {
                if (useEvent) UnityEngine.Event.current.Use();
                return true;
            }
            return false;
        }
        public static bool GetKeyUp(KeyCode key, bool useEvent = false)
        {
            if (UnityEngine.Event.current == null) return false;
            if (UnityEngine.Event.current.type != UnityEngine.EventType.KeyUp) return false;
            if (Input.GetKeyInt(key))
            {
                if (useEvent) UnityEngine.Event.current.Use();
                return true;
            }
            return false;
        }

        public static bool GetKeyDown(RF5Input.Key key) { return GetKeyEdge(key); }
        public static bool GetKeyEdge(RF5Input.Key key) { return RF5Input.Pad.Edge(key); }
        public static bool GetKeyDown(RF5Input.AKey key) { return GetKeyEdge(key); }
        public static bool GetKeyEdge(RF5Input.AKey key) { return RF5Input.Pad.Edge(key); }

        public static bool GetKey(RF5Input.Key key) { return GetKeyPush(key); }
        public static bool GetKeyPush(RF5Input.Key key) { return RF5Input.Pad.Push(key); }
        public static bool GetKey(RF5Input.AKey key) { return GetKeyPush(key); }
        public static bool GetKeyPush(RF5Input.AKey key) { return RF5Input.Pad.Push(key); }

        public static bool GetKeyAll(RF5Input.Key key) { return GetKeyPushAll(key); }
        public static bool GetKeyPushAll(RF5Input.Key key)
        {
            if (key == 0) return false;
            return ((RF5Input.Pad.Data.PushData & key) == key);
        }

        public static bool GetKeyUp(RF5Input.Key key) { return GetKeyEnd(key); }
        public static bool GetKeyEnd(RF5Input.Key key) { return RF5Input.Pad.End(key); }
        public static bool GetKeyUp(RF5Input.AKey key) { return GetKeyEnd(key); }
        public static bool GetKeyEnd(RF5Input.AKey key) { return RF5Input.Pad.End(key); }

        public static bool GetKeyRepeat(RF5Input.Key key) { return RF5Input.Pad.Repeat(key); }
        public static bool GetKeyRepeat(RF5Input.AKey key) { return RF5Input.Pad.Repeat(key); }

        public static float GetAnalogLX() { return RF5Input.Pad.AnalogLX(); }
        public static float GetAnalogLY() { return RF5Input.Pad.AnalogLY(); }
        public static UnityEngine.Vector2 GetAnalogL()
        {
            return new UnityEngine.Vector2(RF5Input.Pad.AnalogLX(), RF5Input.Pad.AnalogLY());
        }

        public static float GetAnalogRX() { return RF5Input.Pad.AnalogRX(); }
        public static float GetAnalogRY() { return RF5Input.Pad.AnalogRY(); }
        public static UnityEngine.Vector2 GetAnalogR()
        {
            return new UnityEngine.Vector2(RF5Input.Pad.AnalogRX(), RF5Input.Pad.AnalogRY());
        }

        public static float ScrollWheelY() { return RF5Input.Pad.ScrollWheelY(); }

        public static void PlayVibration(RF5Input.Pad.VibrationType type)
        {
            RF5Input.Pad.PlayVibration(type);
        }
        public static void StopVibration()
        {
            RF5Input.Pad.StopVibration();
        }

        public static bool GetCursorPos(out RF5SteamInput.SteamInputManager.POINT lpPoint)
        {
            return RF5SteamInput.SteamInputManager.GetCursorPos(out lpPoint);
        }
        public static bool GetCursorPos(out UnityEngine.Vector2Int position)
        {
            RF5SteamInput.SteamInputManager.POINT lpPoint;
            if (RF5SteamInput.SteamInputManager.GetCursorPos(out lpPoint))
            {
                position = new UnityEngine.Vector2Int(lpPoint.X, lpPoint.Y);
                return true;
            }
            position = default;
            return false;
        }

        public static UnityEngine.CursorLockMode GetOptionMouseCursorLockMode()
        {
            return RF5SteamInput.SteamInputManager.GetOptionMouseCursorLockMode();
        }
        public static bool SetCursorPos(int X, int Y)
        {
            return RF5SteamInput.SteamInputManager.SetCursorPos(X, Y);
        }
        public static void SetMouseCursorLock(bool doLock)
        {
            RF5SteamInput.SteamInputManager.SetMouseCursorLock(doLock);
        }

        public static class ControllerBindings
        {
            //Key Bindings
            //Examine Button / 조사 버튼
            //  A | CK3
            //Action Button / 액션 버튼
            //  B | CK2
            //RA/Magic Button 1 / RA/마법 버튼 1
            //  Y
            //RA/Magic Button 2 / RA/마법 버튼 2
            //  X
            //Pocket Button / 포켓 버튼
            //  L
            //Dash Button / 회피 버튼
            //  R
            //Spell Seal Button / 서클 버튼
            //  ZL
            //Mini-Map Button / 미니맵 버튼
            //  ZR
            //Map Menu Button / 맵 메뉴 버튼
            //  MS
            //Camp Menu Button / 캠프 메뉴 버튼
            //  PS
            //Link Attack Button / 연계기 버튼
            //  AL
            //Lock-on Button / 록 온 버튼
            //  AR
            //Select Button (Up) / 선택 (위) 버튼
            //  JU
            //Select Button (Down) / 선택 (아래) 버튼
            //  JD
            //Swap Gear Button 1 / 장비 전환 버튼 1
            //  JL
            //Swap Gear Button 2장비 전환 버튼 2
            //  JR
            //Camera Control Button (Up) / 카메라 조작 버튼 (상)
            //  ARU
            //Camera Control Button (Down) / 카메라 조작 버튼 (하)
            //  ARD
            //Camera Control Button (Left) / 카메라 조작 버튼 (좌)
            //  ARL
            //Camera Control Button (Right) / 카메라 조작 버튼 (우)
            //  ARR
            //Movement Button (Forward) / 이동 버튼 (전)
            //  ALU
            //Movement Button (Backward) / 이동 버튼 (후)
            //  ALD
            //Movement Button (Left) / 이동 버튼 (좌)
            //  ALL
            //Movement Button (Right) / 이동 버튼 (우)
            //  ALR
            //RA/Magic Button 3 / RA/마법 버튼 3
            //  CK4
            //RA/Magic Button 4 / RA/마법 버튼 4
            //  CK5
            //Cursor Display Button / 커서 표시 버튼
            //  CK8

            [Flags]
            public enum XBoxControllerBindings : uint
            {
                LT = RF5Input.Key.ZL,
                RT = RF5Input.Key.ZR,
                LB = RF5Input.Key.L,
                RB = RF5Input.Key.R,

                LS = RF5Input.Key.AL,
                LSUp = RF5Input.Key.ALU,
                LSRight = RF5Input.Key.ALR,
                LSDown = RF5Input.Key.ALD,
                LSLeft = RF5Input.Key.ALL,

                RS = RF5Input.Key.AR,
                RSUp = RF5Input.Key.ARU,
                RSRight = RF5Input.Key.ARR,
                RSDown = RF5Input.Key.ARD,
                RSLeft = RF5Input.Key.ARL,

                Y = RF5Input.Key.X,
                B = RF5Input.Key.B | RF5Input.Key.CK2,
                A = RF5Input.Key.A | RF5Input.Key.CK3,
                X = RF5Input.Key.Y,

                DPadUp = RF5Input.Key.JU,
                DPadRight = RF5Input.Key.JR,
                DPadDown = RF5Input.Key.JD,
                DPadLeft = RF5Input.Key.JL,

                Select = RF5Input.Key.MS,
                Start = RF5Input.Key.PS,
            }

            /// <summary>
            /// This is 'Default' windows key bindings.
            /// Windows key bindings can be changed from RF5 Settings window.
            /// </summary>
            [Flags]
            public enum WindowsDefaultBindings : uint
            {
                Space = RF5Input.Key.ZL,
                Z = RF5Input.Key.ZR,
                Q = RF5Input.Key.L,
                MouseRightClick = RF5Input.Key.R,

                LeftShift = RF5Input.Key.AL,
                W = RF5Input.Key.ALU,
                D = RF5Input.Key.ALR,
                S = RF5Input.Key.ALD,
                A = RF5Input.Key.ALL,

                MouseWheelClick = RF5Input.Key.AR,
                I = RF5Input.Key.ARU,
                L = RF5Input.Key.ARR,
                K = RF5Input.Key.ARD,
                J = RF5Input.Key.ARL,

                G = RF5Input.Key.X,
                F = RF5Input.Key.B | RF5Input.Key.CK2,
                E = RF5Input.Key.A | RF5Input.Key.CK3,
                R = RF5Input.Key.Y,

                Escape = RF5Input.Key.CK2,
                Enter = RF5Input.Key.CK3,

                UpArrow = RF5Input.Key.JU,
                RightArrow = RF5Input.Key.JR,
                DownArrow = RF5Input.Key.JD,
                LeftArrow = RF5Input.Key.JL,

                M = RF5Input.Key.MS,
                Tab = RF5Input.Key.PS,

                MouseLeftClick = RF5Input.Key.CK1,

                T = RF5Input.Key.CK8,
                F1 = RF5Input.Key.CK4,
                F2 = RF5Input.Key.CK5,
            }
        }
    }
}
