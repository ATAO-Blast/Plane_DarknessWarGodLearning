using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class MainCitySys : SystemRoot
    {
        public static MainCitySys Instance { get;private set; }
        public MainCityWnd mainCityWnd;
        public InfoWnd infoWnd;
        private PlayerController playerController;
        private Transform charCamTrans;
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
            MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);
            resSvc.AsyncLoadScene(mapData.sceneName, () =>
            {
                //������Ϸ����
                LoadPlayer(mapData);
                //�����ǳ���UI
                mainCityWnd.SetWndState();
                //�������Ǳ�������
                audioSvc.PlayBGMusic(Constants.BGMainCity);
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

        }
        public void SetMoveDir(Vector2 dir)
        {
            if (dir == Vector2.zero)
            {
                playerController.SetBlend(Constants.BlendIdle);
            }
            else
            {
                playerController.SetBlend(Constants.BlendWalk);
            }
            playerController.Dir = dir;
        }
        public void OpenInfoWnd()
        {
            if(charCamTrans == null)
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
            if(charCamTrans != null)
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
            playerController.transform.localEulerAngles = new Vector3(0,startRotate+degrees, 0);
        }
    }
}