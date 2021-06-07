using UnityEngine;


namespace GameFramework.Core
{
    [CreateAssetMenu(fileName = "_data_0", menuName = "Game/DataId")]
    public class DataId : SubjectId, IDataId
    {
        #region Variables

        int cachedId = -1;

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

        #endregion
    }
}
