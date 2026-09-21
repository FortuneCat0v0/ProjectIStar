namespace GameLogic
{
    [Window(UILayer.UI)]
    public partial class MapUI
    {
        private partial void OnClickCloseBtn()
        {
            Close();
        }

        private partial void OnClickClassroomBtn()
        {
            GameModule.Scene.LoadScene("Classroom");
        }

        private partial void OnClickHouseBtn()
        {
            GameModule.Scene.LoadScene("House");
        }
    }
}