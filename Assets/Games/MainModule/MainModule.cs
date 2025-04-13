
using System; 
using System.Collections.Generic; 
using XFGameFramework;
using TowerDefence;
using XFFSM;
using XFABManager;

public class MainModule : Module
{ 
    // 当前模块所使用的资源模块名称
    public override string ProjectName => "MainModule";
     
    // 是否为基础模块，基础模块不可关闭，如果模块能够关闭，请设置为false
    public override bool BaseModule => false;

    // 默认启动的场景的名称，如果不需要启动场景保持默认即可
    public override string DefaultStartUpScene => "Start";

    // 需要预加载的表(仅支持json) key:json文件名称 value:类型
    public override Dictionary<string, Type> PreloadConfigTables => base.PreloadConfigTables;

    // 当模块启动时触发
    public override void OnStart()
    {
        UnityEngine.Debug.Log("OnStart:"+ModuleName);
        //RuntimeFSMController fsm = AssetBundleManager.LoadAsset<RuntimeFSMController>(ProjectName,"GameState");
        //FSMController.StartupFSM("GameState",fsm,this);
        LoadController<GameController>().SetGameState(EGameState.Start);
    }

}
