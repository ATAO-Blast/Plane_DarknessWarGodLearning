using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PEProtocol;

namespace DarknessWarGodLearning
{
    public class TaskWnd : WindowRoot
    {
        [SerializeField] Button btnClose;
        [SerializeField] Transform content;
        PlayerData playerData;
        List<TaskRewardData> trdList = new List<TaskRewardData>();
        protected override void InitWnd()
        {
            btnClose.onClick.AddListener(ClickCloseBtn);
            base.InitWnd();

            playerData = GameRoot.Instance.PlayerData;
            RefreshUI();
        }
        public void RefreshUI()
        {
            trdList.Clear();
            List<TaskRewardData> todoList = new List<TaskRewardData>();
            List<TaskRewardData> doneList = new List<TaskRewardData>();
            string[] taskArr = playerData.taskArr;
            for (int i = 0; i < taskArr.Length; i++)
            {
                if (taskArr[i] == "") continue;
                string[] taskInfo = taskArr[i].Split('|');
                TaskRewardData taskReward = new TaskRewardData();
                taskReward.ID = int.Parse(taskInfo[0]);
                taskReward.prgs = int.Parse(taskInfo[1]);
                taskReward.taked = taskInfo[2].Equals("1");
                if (taskReward.taked)
                {
                    doneList.Add(taskReward);
                }
                else
                {
                    todoList.Add(taskReward);
                }

            }
            trdList.AddRange(todoList);
            trdList.AddRange(doneList);
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            for (int i = 0; i < trdList.Count; i++)
            {
                GameObject itemTaskUI = resSvc.LoadPrefab(PathDefine.TaskItemPrefab, true);
                itemTaskUI.transform.SetParent(content);
                itemTaskUI.transform.localPosition = Vector3.zero;
                itemTaskUI.transform.localScale = Vector3.one;
                itemTaskUI.name = "taskItem_" + i;

                TaskRewardData trd = trdList[i];
                TaskRewardCfg trf = resSvc.GetTaskRewardCfg(trd.ID);

                SetText(GetTrans("txtName", itemTaskUI),trf.taskName);
                SetText(GetTrans("txtPrg", itemTaskUI), trd.prgs + "/" + trf.count);
                SetText(GetTrans("txtExp", itemTaskUI), "奖励：<sprite=\"itemexp\" index=0>经验" + trf.exp);
                SetText(GetTrans("txtCoin", itemTaskUI), "<sprite=\"coin\" index=0>金币" + trf.coin);
                Image imgPrg = GetTrans("prgBar/prgVal", itemTaskUI).GetComponent<Image>();
                float prgVal = trd.prgs * 1.0f / trf.count;
                imgPrg.fillAmount = prgVal;

                Button btnTake = GetTrans("btnTake",itemTaskUI).GetComponent<Button>();
                btnTake.onClick.AddListener(() =>
                {
                    ClickTakeBtn(itemTaskUI.name);
                });

                Transform transComp = GetTrans("imgComp", itemTaskUI);
                if (trd.taked)
                {
                    btnTake.interactable = false;
                    SetActive(transComp);
                }
                else
                {
                    SetActive(transComp, false);
                    if(trd.prgs == trf.count) btnTake.interactable = true;
                    else btnTake.interactable = false;
                }
            }
        }
        private void ClickTakeBtn(string name)
        {
            Debug.Log(name);
            string[] nameArr = name.Split('_');
            int index = int.Parse(nameArr[1]);
            GameMsg msg = new GameMsg()
            {
                cmd = (int)CMD.ReqTakeTaskReward,
                reqTakeTaskReward = new ReqTakeTaskReward()
                {
                    id = trdList[index].ID,
                }
            };
            netSvc.SendMsg(msg);

            //待修改
            TaskRewardCfg trc = resSvc.GetTaskRewardCfg(trdList[index].ID);
            int coin = trc.coin;
            int exp = trc.exp;
            GameRoot.AddTips(Constants.Color("Gain Reward:", TxtColor.Blue) +
                Constants.Color(" Coin+"+coin+" Exp+"+exp,TxtColor.Green)
                );
        }
        private void ClickCloseBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetWndState(false);
        }
        private void OnDisable()
        {
            btnClose.onClick.RemoveAllListeners();

        }

    }
}