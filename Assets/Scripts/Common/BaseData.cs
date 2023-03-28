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
    public class AutoGuideCfg : BaseData<AutoGuideCfg>
    {
        /// <summary>
        /// 触发任务的NPC索引号
        /// </summary>
        public int npcID;
        /// <summary>
        /// 对话数据
        /// </summary>
        public string dilogArr;
        /// <summary>
        /// 角色行动ID代码
        /// </summary>
        public int actID;

        public int coin;
        public int exp;
    }
    public class StrongCfg : BaseData<StrongCfg>
    {
        public int pos;
        public int starlv;
        public int addhp;
        public int addhurt;
        public int adddef;
        public int minlv;
        public int coin;
        public int crystal;
    }
    public class TaskRewardCfg : BaseData<TaskRewardCfg>
    {
        public string taskName;
        public int count;
        public int exp;
        public int coin;

    }
    public class TaskRewardData:BaseData<TaskRewardData>
    {
        public int prgs;
        public bool taked;
    }
}