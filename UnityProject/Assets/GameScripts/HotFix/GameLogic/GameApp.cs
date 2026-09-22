using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
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
        int index = 0;
        foreach (var item in GameDataManager.Instance.CharacterDataList)
        {
            GameObject character = await GameModule.Resource.LoadGameObjectAsync("Character");
            character.name = item.Name;

            character.transform.position = new Vector2(index, 0);
            index++;
        }
        
        GameModule.UI.ShowUIAsync<MainUI>();
    }
    
    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }
}