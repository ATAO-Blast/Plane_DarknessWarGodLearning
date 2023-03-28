using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance { get; private set; }
        public LoadingWnd loadingWnd;
        public DynamicWnd dynamicWnd;
        void Start()
        {
            Instance = this;
            ClearUIRoot();

            Init();
            DontDestroyOnLoad(this);
        }
        private void ClearUIRoot()
        {
            Transform canvas = transform.Find("Canvas");
            for (int i = 0; i < canvas.childCount; i++)
            {
                canvas.GetChild(i).gameObject.SetActive(false);
            }
            dynamicWnd.SetWndState();
        }

        private void Init()
        {
            //����ģ���ʼ��
            NetSvc netSvc = GetComponent<NetSvc>();
            netSvc.InitSvc();
            ResSvc resSvc = GetComponent<ResSvc>();
            resSvc.InitSvc();

            AudioSvc audioSvc = GetComponent<AudioSvc>();
            audioSvc.InitSvc();

            //ҵ��ϵͳ��ʼ��
            LoginSys loginSys = GetComponent<LoginSys>();
            loginSys.InitSys();
            MainCitySys mainCitySys = GetComponent<MainCitySys>();
            mainCitySys.InitSys();

            //�����¼������������ӦUI
            loginSys.EnterLogin();
        }
        public static void AddTips(string text)
        {
            Instance.dynamicWnd.AddTipsQue(text);
        }

        private PlayerData playerData = null;
        public PlayerData PlayerData => playerData;
        
        public void SetPlayerData(RspLogin data)
        {
            playerData = data.playerData;
        }
        public void SetPalyerName(string name)
        {
            playerData.name = name;
        }
    }
}