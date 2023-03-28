namespace DarknessWarGodLearning
{
    public class PETools
    {
        /// <summary>
        /// 生成随机整数，包含最小值和最大值
        /// </summary>
        public static int RandomInt(int min, int max,System.Random rd = null)
        {
            if (rd == null) { rd = new System.Random(); }
            
            int val = rd.Next(min, max + 1);
            return val;
        }
    }
}