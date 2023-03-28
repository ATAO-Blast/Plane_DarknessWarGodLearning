using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
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
        public MapCfg GetMapCfgData(int ID)
        {
            MapCfg mapCfg = null;
            if(mapCfgDataDic.TryGetValue(ID, out mapCfg))
            {
                return mapCfg;
            };
            return null;
        }
        #endregion
        #endregion
    }
}