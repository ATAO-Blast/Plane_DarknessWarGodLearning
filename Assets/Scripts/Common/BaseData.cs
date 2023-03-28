using UnityEngine;

namespace DarknessWarGodLearning
{
    public class BaseData<T> where T : BaseData<T>
    {
        public int ID;
    }

    public class MapCfg : BaseData<MapCfg>
    {
        public string mapName;
        public string sceneName;
        public Vector3 mainCamPos;
        public Vector3 mainCamRota;
        public Vector3 playerBornPos;
        public Vector3 playerBornRota;

    }
}