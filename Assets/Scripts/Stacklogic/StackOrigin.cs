using UnityEngine;


namespace GameManagers
{
    [CreateAssetMenu(fileName = "New Game Phase", menuName = "CrypticCabinet/Game Phase")]
    public class StackOrigin : MonoBehaviour
    {
        //this class has the location of the Stackable object
        /// <summary>
        ///     Represents a game phase for the gameplay.
        /// </summary>
        [TextArea, SerializeField] private string m_gamePhaseDescription;

        public void Initialize()
        {
            InitializeInternal();
        }

        public void Deinitialize()
        {
            DeinitializeInternal();
        }

        protected virtual void InitializeInternal()
        {
            // Handle initialization on child class, if needed.
        }

        protected virtual void DeinitializeInternal()
        {
            // Handle de-initialization on child class, if needed.
        }
    }
}

