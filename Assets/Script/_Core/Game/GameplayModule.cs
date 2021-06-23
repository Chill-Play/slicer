using UnityEngine;


namespace GameFramework.Core
{
    [CreateAssetMenu(fileName = "gameplay_module_", menuName = "Game/Settings/Module")]
    public class GameplayModule : MonoBehaviour, IObject
    {
        public bool Enabled { get => enabled; set => enabled = value; }
    }
}