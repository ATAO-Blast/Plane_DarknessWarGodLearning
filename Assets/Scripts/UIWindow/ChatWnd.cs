using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PEProtocol;

namespace DarknessWarGodLearning
{
    public class ChatWnd : WindowRoot
    {
        public Button btnWorld, btnGuild, btnFriend,btnClose,btnSend;
        public TextMeshProUGUI txtChat;
        public TMP_InputField iptChat;

        private Image imgWorldBtn, imgGuildBtn, imgFriendBtn;
        /// <summary>
        /// World = 0,Guild = 1,Friend = 2
        /// </summary>
        private int chatType;

        private List<string> chatList = new List<string>();
        protected override void InitWnd()
        {
            base.InitWnd();
            imgWorldBtn = btnWorld.GetComponent<Image>();
            imgGuildBtn = btnGuild.GetComponent<Image>();
            imgFriendBtn = btnFriend.GetComponent<Image>();
            chatType = 0;
            btnClose.onClick.AddListener(ClickCloseBtn);
            btnWorld.onClick.AddListener(ClickWorldBtn);
            btnGuild.onClick.AddListener(ClickGuildBtn);
            btnFriend.onClick.AddListener(ClickFriendBtn);
            btnSend.onClick.AddListener(ClickSendBtn);
            UpdateUI();
        }
        public void UpdateUI()
        {
            if(chatType == 0)
            {
                string chatMsg = "";
                chatList.ForEach(msg =>
                {
                    chatMsg += (msg + "\n");
                });
                SetText(txtChat, chatMsg);

                SetSprite(imgWorldBtn, PathDefine.ChatBtnTypeFul);
                SetSprite(imgGuildBtn, PathDefine.ChatBtnTypeEpt);
                SetSprite(imgFriendBtn, PathDefine.ChatBtnTypeEpt);
            }
            else if(chatType == 1)
            {
                SetText(txtChat, "尚未加入公会");
                SetSprite(imgWorldBtn, PathDefine.ChatBtnTypeEpt);
                SetSprite(imgGuildBtn, PathDefine.ChatBtnTypeFul);
                SetSprite(imgFriendBtn, PathDefine.ChatBtnTypeEpt);
            }
            else if(chatType == 2)
            {
                SetText(txtChat, "暂无好友信息");
                SetSprite(imgWorldBtn, PathDefine.ChatBtnTypeEpt);
                SetSprite(imgGuildBtn, PathDefine.ChatBtnTypeEpt);
                SetSprite(imgFriendBtn, PathDefine.ChatBtnTypeFul);
            }
        }
        private void ClickCloseBtn()
        {
            chatType = 0;
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetWndState(false);
        }

        private void ClickWorldBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            chatType = 0;
            UpdateUI();
        }
        private void ClickGuildBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            chatType = 1;
            UpdateUI();
        }
        private void ClickFriendBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            chatType = 2;
            UpdateUI();
        }
        private bool canSend = true;
        private void ClickSendBtn()
        {
            if (!canSend)
            { 
                GameRoot.AddTips("Chat Message Can Only be Sent in 5 Seconds"); 
                return; 
            }
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            if (iptChat.text != null && iptChat.text != "" && iptChat.text != " ")
            {
                if(iptChat.text.Length > 12)
                {
                    GameRoot.AddTips("12 Characters Limit");
                }
                else
                {
                    //发送消息
                    GameMsg msg = new GameMsg
                    {
                        cmd = (int)CMD.SndChat,

                        sndChat = new SndChat
                        {
                            chat = iptChat.text
                        }
                    };
                    iptChat.text = "";
                    netSvc.SendMsg(msg);
                    canSend = false;
                    timerSvc.AddTimeTask(tid =>
                    {
                        canSend = true;
                    },5000);
                }
            }
            else
            {
                return;
            }
        }
        

        public void AddChatMsg(string name, string chat)
        {
            chatList.Add(Constants.Color(name+"：",TxtColor.Blue) + chat);
            if (chatList.Count > 12)
            {
                chatList.RemoveAt(0);
            }
            if (GetWndState())
            {
                UpdateUI();
            }
        }
        private void OnDisable()
        {
            btnClose.onClick.RemoveAllListeners();
            btnWorld.onClick.RemoveAllListeners();
            btnGuild.onClick.RemoveAllListeners();
            btnFriend.onClick.RemoveAllListeners();
            btnSend.onClick.RemoveAllListeners();
        }
    }
}