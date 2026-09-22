namespace GameLogic
{
    public partial class MainCharacterDataWidget
    {
        public void SetData(CharacterData characterData)
        {
            m_tmpName.text = characterData?.Name ?? string.Empty;
            m_tmpSatiety.text = $"饱腹:{characterData?.Satiety ?? 0}";
        }
    }
}
