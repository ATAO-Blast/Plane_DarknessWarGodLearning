using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DarknessWarGodLearning
{
    public class MainCitySys : SystemRoot
    {
        public static MainCitySys Instance { get;private set; }
        public MainCityWnd mainCityWnd;
        public InfoWnd infoWnd;
        public GuideWnd guideWnd;
        public StrongWnd strongWnd;
        public ChatWnd chatWnd;
        public BuyWnd buyWnd;
        public TaskWnd taskWnd;

        private PlayerController playerController;
        private Transform charCamTrans;

        private AutoGuideCfg curTaskData;
        private Transform[] npcPosTrans;

        private NavMeshAgent nav;
        public override void InitSys()
        {
            base.InitSys();
            Instance = this;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void EnterMainCity()
        {
            MapCfg mapData = resSvc.GetMapCfg(Constants.MainCityMapID);
            resSvc.AsyncLoadScene(mapData.sceneName, () =>
            {
                //������Ϸ����
                LoadPlayer(mapData);
                //�����ǳ���UI
                mainCityWnd.SetWndState();
                //�������Ǳ�������
                audioSvc.PlayBGMusic(Constants.BGMainCity);

                GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
                MainCityMap mainCityMap = map.GetComponent<MainCityMap>();
                npcPosTrans = mainCityMap.NpcPosTrans;

                //�ر�����չʾ������ڴ򿪽�ɫ�������ʱ�ٴ�
                if(charCamTrans != null)
                {
                    charCamTrans.gameObject.SetActive(false);
                }
                
            });
        }
        
        private void LoadPlayer(MapCfg mapData)
        {
            GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab,true);
            player.transform.position = mapData.playerBornPos;
            player.transform.localEulerAngles = mapData.playerBornRota;
            player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            //�����ʼ��
            Camera.main.transform.position = mapData.mainCamPos;
            Camera.main.transform.localEulerAngles= mapData.mainCamRota;

            playerController = player.GetComponent<PlayerController>();
            playerController.Init();

            nav = player.GetComponent<NavMeshAgent>();
        }
        public void SetMoveDir(Vector2 dir)
        {
            StopNavTask();
            if (dir == Vector2.zero)
            {
                playerController.SetAnimTreeBlend(Constants.BlendIdle);
            }
            else
            {
                playerController.SetAnimTreeBlend(Constants.BlendWalk);
            }
            playerController.Dir = dir;
        }

        #region InfoWnd Functions
        public void OpenInfoWnd()
        {
            StopNavTask();
            if (charCamTrans == null)
            {
                charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
            }
            //��������չʾ��������λ��
            charCamTrans.localPosition = playerController.transform.position + playerController.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
            charCamTrans.localEulerAngles = new Vector3(0, 180 + playerController.transform.localEulerAngles.y, 0);
            charCamTrans.localScale = Vector3.one;
            charCamTrans.gameObject.SetActive(true);
            infoWnd.SetWndState();
        }
        public void CloseInfoWnd()
        {
            if (charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }
        }

        private float startRotate = 0;
        public void SetStartRotate()
        {
            startRotate = playerController.transform.localEulerAngles.y;
        }
        public void SetPalyerRotate(float degrees)
        {
            playerController.transform.localEulerAngles = new Vector3(0, startRotate + degrees, 0);
        }
        #endregion

        #region GuideWnd Functions
        private bool isNaviguiding = false;
        public void RunTask(AutoGuideCfg autoGuideCfg)
        {
            if (autoGuideCfg != null)
            {
                curTaskData = autoGuideCfg;
            }
            //������������
            nav.enabled = true;//��ֹPlayer�ں�NPC����ʱ���������Ҳ���nav;
            if (curTaskData.npcID != -1)
            {
                float dis = Vector3.Distance(playerController.transform.position, npcPosTrans[autoGuideCfg.npcID].position);
                if (dis < 0.5f)
                {
                    isNaviguiding = false;
                    nav.isStopped = true;
                    playerController.SetAnimTreeBlend(Constants.BlendIdle);
                    nav.enabled = false;

                    OpenGuideWnd();
                }
                else
                {
                    isNaviguiding = true;
                    nav.enabled = true;
                    nav.speed = Constants.PlayerMoveSpeed;
                    nav.SetDestination(npcPosTrans[autoGuideCfg.npcID].position);
                    //nav.stoppingDistance = 0.5f;
                    playerController.SetAnimTreeBlend(Constants.BlendWalk);
                }
            }
            else
            {
                OpenGuideWnd();
            }
        }
        private void StopNavTask()
        {
            if (isNaviguiding)
            {
                isNaviguiding = false;
                nav.isStopped = true;
                playerController.SetAnimTreeBlend(Constants.BlendIdle);
                nav.enabled = false;
            }
        }
        private void IsArrivedNavPos()
        {
            float dis = Vector3.Distance(playerController.transform.position, npcPosTrans[curTaskData.npcID].position);
            if (dis < 0.5f)
            {
                isNaviguiding = false;
                nav.isStopped = true;
                playerController.SetAnimTreeBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd();
            }
        }
        private void OpenGuideWnd()
        {
            guideWnd.SetWndState();
        }
        public AutoGuideCfg GetCurtTaskData()
        {
            return curTaskData;
        }
        public void RspGuide(GameMsg msg)
        {
            RspGuide rspGuide = msg.rspGuide;

            GameRoot.AddTips(Constants.Color("Quest Award Coin +" + curTaskData.coin + " Exp +" + curTaskData.exp, TxtColor.Blue));

            switch (curTaskData.actID)
            {
                case 0:
                    //�����߶Ի�
                    break;
                case 1:
                    //���븱��
                    EnterFuben();
                    break;
                case 2:
                    //����ǿ������
                    OpenStrongWnd();
                    break;
                case 3:
                    //������������
                    OpenBuyWnd(0);
                    break;
                case 4:
                    //����������
                    OpenBuyWnd(1);
                    break;
                case 5:
                    //������������
                    OpenChatWnd();
                    break;
            }
            GameRoot.Instance.SetPalyerDataByGuide(rspGuide);
            mainCityWnd.RefreshUI();
        }
        #endregion

        #region StrongWnd Functions
        public void OpenStrongWnd()
        {
            StopNavTask();
            strongWnd.SetWndState();
        }
        public void RspStrong(GameMsg msg)
        {
            int fightNumPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
            GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
            int fightNumNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);

            GameRoot.AddTips(Constants.Color("FightNum Upped: " + (fightNumNow - fightNumPre),TxtColor.Blue));
            strongWnd.UpdateUI();
            mainCityWnd.RefreshUI();
        }
        #endregion

        #region ChatWnd Functions
        public void OpenChatWnd()
        {
            StopNavTask();
            chatWnd.SetWndState();
        }

        public void PushChat(GameMsg msg)
        {
            chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.chat);
        }
        #endregion

        #region BuyWnd Functions
        public void OpenBuyWnd(int buyType)
        {
            StopNavTask();
            buyWnd.SetBuyType(buyType);
            buyWnd.SetWndState();
        }
        public void RspBuy(GameMsg msg)
        {
            RspBuy rspBuy = msg.rspBuy;
            GameRoot.Instance.SetPlayerDataByBuy(rspBuy);
            GameRoot.AddTips("Buy Succeed");
            mainCityWnd.RefreshUI();
            buyWnd.SetWndState(false);

            if (msg.pshTaskPrgs != null)
            {
                GameRoot.Instance.SetPlayerTaskPrgs(msg.pshTaskPrgs);
                if (taskWnd.GetWndState()) taskWnd.RefreshUI();
            }
        }
        public void PshPower(GameMsg msg)
        {
            PshPower data = msg.pshPower;
            GameRoot.Instance.SetPlayerDataByPower(data);
            if (mainCityWnd.GetWndState())
            {
                mainCityWnd.RefreshUI();
            }
        }
        #endregion

        #region TaskWnd Functions
        public void OpenTaskWnd()
        {
            StopNavTask();
            taskWnd.SetWndState();
        }

        public void RspTaskReward(GameMsg msg)
        {
            RspTaskReward taskReward = msg.rspTaskReward;
            GameRoot.Instance.SetPlayerDataByTask(taskReward);
            if(taskWnd.GetWndState()) taskWnd.RefreshUI();
            if(mainCityWnd.GetWndState()) mainCityWnd.RefreshUI();
        }
        public void PshTaskPrgs(GameMsg msg)
        {
            PshTaskPrgs taskPrgs = msg.pshTaskPrgs;
            GameRoot.Instance.SetPlayerTaskPrgs(taskPrgs);
            if(taskWnd.GetWndState()) taskWnd.RefreshUI();
        }
        #endregion

        #region Enter FubenSys
        public void EnterFuben()
        {
            StopNavTask();
            FubenSys.Instance.EnterFuben();
        }
        #endregion
        private void Update()
        {
            if (isNaviguiding)
            {
                playerController.SetCam();
                IsArrivedNavPos();
            }
        }
    }
}