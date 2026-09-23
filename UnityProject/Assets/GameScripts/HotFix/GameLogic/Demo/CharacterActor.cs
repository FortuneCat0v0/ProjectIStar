using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class CharacterActor : MonoBehaviour
    {
        public CharacterData CharacterData { get; private set; }

        public void Initialize(CharacterData characterData)
        {
            CharacterData = characterData;
        }

        private void OnMouseDown()
        {
            Log.Debug($"点击 {CharacterData.CharacterId}");
        }
    }
}