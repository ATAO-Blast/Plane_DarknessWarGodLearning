namespace DarknessWarGodLearning
{
    public class PETools
    {
        /// <summary>
        /// �������������������Сֵ�����ֵ
        /// </summary>
        public static int RandomInt(int min, int max,System.Random rd = null)
        {
            if (rd == null) { rd = new System.Random(); }
            
            int val = rd.Next(min, max + 1);
            return val;
        }
    }
}