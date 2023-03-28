using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public enum TxtColor
    {
        Red,
        Green, 
        Blue,
        Yellow,
    }
    public class Constants 
    {
        private const string ColorRed = "<color=#FF0000FF>";
        private const string ColorGreen = "<color=#00FF00FF>";
        private const string ColorBlue = "<color=#00B4FFFF>";
        private const string ColorYellow = "<color=#FFFF00FF>";
        private const string ColorEnd = "</color>";

        public static string Color(string str,TxtColor c)
        {
            string result = "";
            switch (c)
            {
                case TxtColor.Red:
                    result = ColorRed + str + ColorEnd;
                    break;
                case TxtColor.Blue:
                    result = ColorBlue + str + ColorEnd;
                    break;
                case TxtColor.Green:
                    result = ColorGreen + str + ColorEnd;
                    break;
                case TxtColor.Yellow:
                    result = ColorYellow + str + ColorEnd;
                    break;
            }
            return result;
        }

        #region scenes
        public const int MainCityMapID = 10000;
        public const string SceneLogin = "SceneLogin";
        //public const string SceneMainCity = "SceneMainCity";
        #endregion

        #region bgaudios
        public const string BGLogin = "bgLogin";
        public const string BGMainCity = "bgMainCity";
        #endregion

        #region uiaudios
        public const string UILoginBtn = "uiLoginBtn";
        public const string UIClickBtn = "uiClickBtn";
        public const string UIExtenBtn = "uiExtenBtn";
        public const string UIOpenPage = "uiOpenPage";
        public const string FBItemEnter = "fbitem";
        #endregion

        #region Screen
        public const int ScreenStandardWidth = 1920;
        public const int ScreenStandardHeight = 1080;
        public const int ScreenOPDis = 90;//摇杆点标准距离
        #endregion

        //角色移动速度
        public const int PlayerMoveSpeed = 8;
        public const int MonsterMoveSpeed = 4;

        //运动平滑加速度
        public const float AccelerSpeed = 5f;

        //混合参数
        public const int BlendIdle = 0;
        public const int BlendWalk = 1;

        //AutoGuideNPC
        public const int NPCWiseMan = 0;
        public const int NPCGeneral = 1;
        public const int NPCArtisan = 2;
        public const int NPCTrader = 3;
    }
}