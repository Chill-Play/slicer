using UnityEngine;


namespace GameFramework.Core
{
    [CreateAssetMenu(fileName = "_event_0", menuName = "Game/EventType")]
    public class EventType : ScriptableObject 
    {
        #region Variables

        int cachedId = -1;
        [SerializeField] bool cacheData;
        [SerializeField] DataId dataId;

        #endregion



        #region Properties

        public int Id
        {
            get
            {
                if (cachedId == -1)
                {
                    cachedId = name.GetHashCode();
                }
                return cachedId;
            }
        }


        public bool CacheData
        {
            get
            {
                return cacheData;
            }
        }


        public DataId DataId
        {
            get
            {
                return dataId;
            }
        }


        #endregion
    }
}
