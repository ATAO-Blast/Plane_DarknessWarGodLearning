using UnityEngine;

namespace DarknessWarGodLearning
{
    public class FubenSys : SystemRoot
    {
        [SerializeField] FubenWnd fubenWnd;
        public static FubenSys Instance { get; private set; }
        public override void InitSys()
        {
            base.InitSys();
            Instance = this;
        }
        public void EnterFuben()
        {
            OpenFubenWnd();
        }

        #region FubenWnd Functions
        public void OpenFubenWnd()
        {
            fubenWnd.SetWndState();
        }
        #endregion

    }
}