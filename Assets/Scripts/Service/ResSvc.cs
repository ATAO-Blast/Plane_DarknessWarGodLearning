using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarknessWarGodLearning
{
    public class ResSvc : MonoBehaviour
    {
        public static ResSvc Instance = null;
        public void InitSvc()
        {
            Instance = this;
            InitRDNameCfg(PathDefine.RDNameCfgPath);
            InitMapCfg(PathDefine.MapCfgPath);
            InitGuideCfg(PathDefine.GuideCfgPath);
            InitStrongCfg(PathDefine.StrongCfgPath);
            InitTaskCfg(PathDefine.TaskRewardCfgPath);
        }
        private Action prgCB = null;
        public void AsyncLoadScene(string sceneName,Action onSceneLoaded)
        {
            GameRoot.Instance.loadingWnd.SetWndState();

            AsyncOperation handler = SceneManager.LoadSceneAsync(sceneName);

            prgCB = () =>
            {
                float val = handler.progress;
                GameRoot.Instance.loadingWnd.SetProgerss(val);

                if (handler.isDone)
                {
                    prgCB = null;
                    handler = null;
                    GameRoot.Instance.loadingWnd.SetWndState(false);
                    onSceneLoaded?.Invoke();
                }
            };
        }
        private Dictionary<string,AudioClip> audioClipDic = new Dictionary<string,AudioClip>(16);
        public AudioClip LoadAudio(string path,bool cache = false) 
        {
            AudioClip audioClip;
            if (!audioClipDic.TryGetValue(path, out audioClip))
            {
                audioClip = Resources.Load<AudioClip>(path);
                if(cache)
                {
                    audioClipDic.Add(path, audioClip);
                }
            }
            return audioClip;
        }
        private Dictionary<string,GameObject> goDic = new Dictionary<string,GameObject>();
        public GameObject LoadPrefab(string path,bool cache = false)
        {
            GameObject prefab = null;
            if (!goDic.TryGetValue(path, out prefab))
            {
                prefab = Resources.Load<GameObject>(path);
                if (cache)
                {
                    goDic.Add(path, prefab);
                }
            }
            GameObject go = null;
            if(prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
            }
            return go;
        }
        private Dictionary<string,Sprite> spriteDic = new Dictionary<string,Sprite>();
        public Sprite LoadSprite(string path,bool cache = false)
        {
            Sprite sprite;
            if(!spriteDic.TryGetValue(path,out sprite))
            {
                sprite = Resources.Load<Sprite>(path);
                if (cache)
                {
                    spriteDic.Add(path, sprite);
                }
            }
            return sprite;
        }
        private void Update()
        {
            if (prgCB != null)
            {
                prgCB();
            }
        }
        #region 初始化各种配置文件
        #region 随机名字
        private List<string> surnameList = new List<string>(10);
        private List<string> manList = new List<string>(10);
        private List<string> womanList = new List<string>(10);
        private void InitRDNameCfg(string path)
        {
            TextAsset rdNameCfgText = Resources.Load<TextAsset>(path);
            if(rdNameCfgText != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(rdNameCfgText.text);

                XmlNodeList nodeList = xmlDocument.SelectSingleNode("root").ChildNodes;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlElement ele = nodeList[i] as XmlElement;
                    if (ele.GetAttributeNode("ID") == null) continue;

                    int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                    foreach (XmlElement e in nodeList[i].ChildNodes)
                    {
                        switch (e.Name)
                        {
                            case "surname":
                                surnameList.Add(e.InnerText);
                                break;
                            case "man":
                                manList.Add(e.InnerText);
                                break;
                            case "woman":
                                womanList.Add(e.InnerText);
                                break;
                        }
                    }
                }
            }else
            {
                PECommon.Log("xml file:" +  path + "nor exist",LogType.Error);
            }
        }
        public string GetRDNameData(bool man = true)
        {
            System.Random rd = new System.Random();
            string rdName = surnameList[PETools.RandomInt(0, surnameList.Count-1,rd)];
            if (man) rdName += manList[PETools.RandomInt(0, manList.Count - 1,rd)];
            else rdName += womanList[PETools.RandomInt(0, womanList.Count - 1,rd)];
            return rdName;
        }
        #endregion

        #region 地图
        private Dictionary<int,MapCfg> mapCfgDataDic = new Dictionary<int,MapCfg>();
        private void InitMapCfg(string path)
        {
            TextAsset mapCfgText = Resources.Load<TextAsset>(path);
            if(mapCfgText != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(mapCfgText.text);

                XmlNodeList nodeList = xmlDocument.SelectSingleNode("root").ChildNodes;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlElement ele = nodeList[i] as XmlElement;
                    if(ele.GetAttributeNode("ID") == null)
                    {
                        continue;
                    }
                    int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                    MapCfg mapCfg = new MapCfg() { ID = ID};

                    foreach (XmlElement element in nodeList[i].ChildNodes)
                    {
                        switch(element.Name)
                        {
                            case "mapName":
                                mapCfg.mapName = element.InnerText;
                                break;
                            case "sceneName":
                                mapCfg.sceneName = element.InnerText;
                                break;
                            case "mainCamPos":
                                {
                                    string[] valArr = element.InnerText.Split(',');
                                    mapCfg.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                                }
                                break;
                            case "mainCamRota":
                                {
                                    string[] valArr = element.InnerText.Split(',');
                                    mapCfg.mainCamRota = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                                }
                                break;
                            case "playerBornPos":
                                {
                                    string[] valArr = element.InnerText.Split(',');
                                    mapCfg.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                                }
                                break;
                            case "playerBornRota":
                                {
                                    string[] valArr = element.InnerText.Split(',');
                                    mapCfg.playerBornRota = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                                }
                                break;
                        }
                    }
                    mapCfgDataDic.Add(ID, mapCfg);
                }
            }
        }
        public MapCfg GetMapCfg(int ID)
        {
            MapCfg mapCfg = null;
            if(mapCfgDataDic.TryGetValue(ID, out mapCfg))
            {
                return mapCfg;
            };
            return null;
        }
        #endregion

        #region 自动引导配置
        private Dictionary<int,AutoGuideCfg> guideCfgDataDic = new Dictionary<int,AutoGuideCfg>();
        private void InitGuideCfg(string path)
        {
            TextAsset guideCfgAsset = Resources.Load<TextAsset>(path);
            if(guideCfgAsset != null )
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(guideCfgAsset.text);

                XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
                for(int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlElement ele = xmlNodeList[i] as XmlElement;
                    if(ele.GetAttributeNode("ID")  == null)
                    {
                        continue;
                    }
                    int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                    AutoGuideCfg autoGuideCfg = new AutoGuideCfg { ID = ID };

                    foreach(XmlElement element in ele.ChildNodes)
                    {
                        switch(element.Name)
                        {
                            case "npcID":
                                autoGuideCfg.npcID = int.Parse(element.InnerText);
                                break;
                            case "dilogArr":
                                autoGuideCfg.dilogArr = element.InnerText;
                                break;
                            case "actID":
                                autoGuideCfg.actID = int.Parse(element.InnerText);
                                break;
                            case "coin":
                                autoGuideCfg.coin = int.Parse(element.InnerText);
                                break;
                            case "exp":
                                autoGuideCfg.exp = int.Parse(element.InnerText);
                                break;
                        }
                    }
                    guideCfgDataDic.Add(ID, autoGuideCfg);
                }
            }
        }

        public AutoGuideCfg GetAutoGuideCfg(int ID)
        {
            AutoGuideCfg autoGuideCfg = null;
            if (guideCfgDataDic.TryGetValue(ID, out autoGuideCfg))
            {
                return autoGuideCfg;
            };
            return null;
        }
        #endregion

        #region 强化升级配置
        /// <summary>
        /// 第一个int是pos，内部字典的int是starlv
        /// </summary>
        private Dictionary<int,Dictionary<int, StrongCfg>> strongCfgDic = new Dictionary<int,Dictionary<int, StrongCfg>>();
        private void InitStrongCfg(string path)
        {
            TextAsset strongCfgAsset = Resources.Load<TextAsset>(path);
            if (strongCfgAsset != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strongCfgAsset.text);

                XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlElement ele = xmlNodeList[i] as XmlElement;
                    if (ele.GetAttributeNode("ID") == null)
                    {
                        continue;
                    }
                    int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                    StrongCfg strongCfg = new StrongCfg { ID = ID };

                    foreach (XmlElement element in ele.ChildNodes)
                    {
                        int val = int.Parse(element.InnerText);
                        switch (element.Name)
                        {
                            case "pos":
                                strongCfg.pos = val;
                                break;
                            case "starlv":
                                strongCfg.starlv = val;
                                break;
                            case "addhp":
                                strongCfg.addhp = val;
                                break;
                            case "addhurt":
                                strongCfg.addhurt = val;
                                break;
                            case "adddef":
                                strongCfg.adddef = val;
                                break;
                            case "minlv":
                                strongCfg.minlv = val;
                                break;
                            case "coin":
                                strongCfg.coin = val;
                                break;
                            case "crystal":
                                strongCfg.crystal = val;
                                break;

                        }
                    }
                    Dictionary<int, StrongCfg> dic = null;
                    if (strongCfgDic.TryGetValue(strongCfg.pos,out dic))
                    {
                        dic.Add(strongCfg.starlv, strongCfg);
                    }
                    else
                    {
                        dic = new Dictionary<int, StrongCfg>();
                        dic.Add(strongCfg.starlv , strongCfg);

                        strongCfgDic.Add(strongCfg.pos , dic);
                    }
                }
            }
        }

        public StrongCfg GetStrongCfg(int pos,int starlv)
        {
            StrongCfg strongCfg = null;
            Dictionary<int,StrongCfg> dic = null;
            if(strongCfgDic.TryGetValue(pos,out dic))
            {
                if (dic.ContainsKey(starlv))
                {
                    strongCfg = dic[starlv];
                }
            }
            return strongCfg;
        }
        public int GetPropAddValPreLv(int pos,int starlv,int type) 
        {
            Dictionary<int,StrongCfg> posDic = null;
            int val = 0;
            if(strongCfgDic.TryGetValue(pos,out posDic))
            {
                for (int i = 0; i < starlv; i++)
                {
                    StrongCfg sd;
                    if(posDic.TryGetValue(i,out sd))
                    {
                        switch(type)
                        {
                            case 0://hp
                                val += sd.addhp;
                                break;
                            case 1://hurt
                                val += sd.addhurt;
                                break;
                            case 2://def
                                val += sd.adddef;
                                break;
                        }
                    }
                }
            }
            return val;
        }
        #endregion

        #region 任务奖励配置
        private Dictionary<int, TaskRewardCfg> taskRewardCfgDataDic = new Dictionary<int, TaskRewardCfg>();
        private void InitTaskCfg(string path)
        {
            TextAsset guideCfgAsset = Resources.Load<TextAsset>(path);
            if (guideCfgAsset != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(guideCfgAsset.text);

                XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlElement ele = xmlNodeList[i] as XmlElement;
                    if (ele.GetAttributeNode("ID") == null)
                    {
                        continue;
                    }
                    int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                    TaskRewardCfg taskRewardCfg = new TaskRewardCfg { ID = ID };

                    foreach (XmlElement element in ele.ChildNodes)
                    {
                        switch (element.Name)
                        {
                            case "taskName":
                                taskRewardCfg.taskName = element.InnerText;
                                break;
                            case "count":
                                taskRewardCfg.count = int.Parse(element.InnerText);
                                break;
                            case "coin":
                                taskRewardCfg.coin = int.Parse(element.InnerText);
                                break;
                            case "exp":
                                taskRewardCfg.exp = int.Parse(element.InnerText);
                                break;
                        }
                    }
                    taskRewardCfgDataDic.Add(ID, taskRewardCfg);
                }
            }
        }

        public TaskRewardCfg GetTaskRewardCfg(int ID)
        {
            TaskRewardCfg taskRewardCfg = null;
            if (taskRewardCfgDataDic.TryGetValue(ID, out taskRewardCfg))
            {
                return taskRewardCfg;
            };
            return null;
        }
        #endregion
        #endregion
    }
}