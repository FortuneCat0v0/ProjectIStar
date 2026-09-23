namespace GameLogic
{
    public partial class MainCharacterDataWidget
    {
        public void SetData(CharacterData characterData)
        {
            m_tmpName.text = characterData.Name;
            m_tmpHp.text= $"HP:{characterData.Hp}";
            m_tmpSatiety.text = $"饱腹:{characterData.Satiety}";
        }
    }
}
