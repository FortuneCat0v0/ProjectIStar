namespace GameLogic
{
    public partial class MainCharacterDataWidget
    {
        public void SetData(CharacterData characterData)
        {
            m_tmpName.text = characterData?.Name ?? string.Empty;
            m_tmpAffection.text = $"好感度:{characterData?.Affection ?? 0}";
            m_tmpTrust.text = $"信任度:{characterData?.Trust ?? 0}";
        }
    }
}
