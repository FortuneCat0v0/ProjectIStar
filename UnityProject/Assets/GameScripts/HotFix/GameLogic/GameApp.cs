using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using GameConfig;
using GameLogic;
#if ENABLE_OBFUZ
using Obfuz;
#endif
using TEngine;
using UnityEngine;

#pragma warning disable CS0436


/// <summary>
/// 游戏App。
/// </summary>
#if ENABLE_OBFUZ
[ObfuzIgnore(ObfuzScope.TypeName | ObfuzScope.MethodName)]
#endif
public partial class GameApp
{
    private static List<Assembly> _hotfixAssembly;

    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        GameEventHelper.Init();
        _hotfixAssembly = (List<Assembly>)objects[0];
        Log.Warning("======= 看到此条日志代表你成功运行了热更新代码 =======");
        Log.Warning("======= Entrance GameApp =======");
        Utility.Unity.AddDestroyListener(Release);
        Log.Warning("======= StartGameLogic =======");
        StartGameLogic().Forget();
    }

    private static async UniTask StartGameLogic()
    {
        GameModule.UI.Active();

        // GameEvent.Get<ILoginUI>().ShowLoginUI();
        await GameModule.Scene.LoadSceneAsync("House");

        foreach (var characterData in GameDataManager.Instance.CharacterDataList)
        {
            CharacterRow characterRow = ConfigSystem.Instance.Tables.TbCharacter.Get(characterData.CharacterId);

            GameObject character = await GameModule.Resource.LoadGameObjectAsync(characterRow.Actor);
            character.GetComponent<CharacterActor>().Initialize(characterData);

            if (characterData.CharacterId == ConfigSystem.Instance.Tables.TbGlobal.CharacterIdMy)
            {
                character.transform.position = new Vector3(0, 0, 0);
            }
            if (characterData.CharacterId == ConfigSystem.Instance.Tables.TbGlobal.CharacterIdErGou)
            {
                character.transform.position = new Vector3(-1, 0, 0);
            }
            if (characterData.CharacterId == ConfigSystem.Instance.Tables.TbGlobal.CharacterIdCuiHua)
            {
                character.transform.position = new Vector3(1, 0, 0);
            }
        }

        GameModule.UI.ShowUIAsync<MainUI>();

        GameplayManager.Instance.TriggerDay(GameDataManager.Instance.Day);
    }

    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }
}