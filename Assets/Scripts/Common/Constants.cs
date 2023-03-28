using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class Constants 
    {
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
        #endregion

        #region Screen
        public const int ScreenStandardWidth = 1920;
        public const int ScreenStandardHeight = 1080;
        public const int ScreenOPDis = 90;//ҡ�˵��׼����
        #endregion

        //��ɫ�ƶ��ٶ�
        public const int PlayerMoveSpeed = 8;
        public const int MonsterMoveSpeed = 4;

        //�˶�ƽ�����ٶ�
        public const float AccelerSpeed = 5f;

        //��ϲ���
        public const int BlendIdle = 0;
        public const int BlendWalk = 1;
    }
}