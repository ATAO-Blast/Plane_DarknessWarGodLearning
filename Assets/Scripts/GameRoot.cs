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
            Application.targetFrameRate = 60;
            //服务模块初始化
            NetSvc netSvc = GetComponent<NetSvc>();
            netSvc.InitSvc();
            ResSvc resSvc = GetComponent<ResSvc>();
            resSvc.InitSvc();
            TimerSvc timerSvc = GetComponent<TimerSvc>();
            timerSvc.InitSvc();

            AudioSvc audioSvc = GetComponent<AudioSvc>();
            audioSvc.InitSvc();

            //业务系统初始化
            LoginSys loginSys = GetComponent<LoginSys>();
            loginSys.InitSys();
            MainCitySys mainCitySys = GetComponent<MainCitySys>();
            mainCitySys.InitSys();
            FubenSys fubenSys = GetComponent<FubenSys>();
            fubenSys.InitSys();

            //进入登录场景并加载相应UI
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
        public void SetPalyerDataByGuide(RspGuide data)
        {
            playerData.lv = data.lv;
            playerData.exp = data.exp;
            playerData.coin = data.coin;
            playerData.guideid = data.guideid;
        }
        public void SetPlayerDataByStrong(RspStrong strongData)
        {
            playerData.ad = strongData.ad;
            playerData.ap = strongData.ap;
            playerData.addef = strongData.addef;
            playerData.apdef = strongData.apdef;
            playerData.crystal = strongData.crystal;
            playerData.coin = strongData.coin;

            playerData.strongArr = strongData.strongArr;
        }
        public void SetPlayerDataByBuy(RspBuy rspBuy)
        {
            playerData.diamond = rspBuy.diamond;
            switch(rspBuy.buytype)
            {
                case 0:
                    playerData.power = rspBuy.power;
                    break;
                case 1:
                    playerData.coin = rspBuy.coin;
                    break;
            }
        }
        public void SetPlayerDataByPower(PshPower pshPower)
        {
            playerData.power = pshPower.power;
        }
        public void SetPlayerDataByTask(RspTaskReward taskReward)
        {
            playerData.exp = taskReward.exp;
            playerData.lv = taskReward.lv;
            playerData.coin = taskReward.cion;
            playerData.taskArr = taskReward.taskArr;
        }
        public void SetPlayerTaskPrgs(PshTaskPrgs pshTaskPrgs)
        {
            playerData.taskArr = pshTaskPrgs.taskArr;
        }
    }
}