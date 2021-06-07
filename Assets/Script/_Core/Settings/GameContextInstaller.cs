using UnityEngine;
using System.Collections;

namespace GameFramework.Core
{
    public class GameContextInstaller
    {
        #region Variables

        //[OdinSerialize] IEntityService entityService;
        //[OdinSerialize] IEventsService eventsService;
        //[OdinSerialize] IGameFlowService gameFlowService;
        //[OdinSerialize] IDataVault dataVault;
        //[OdinSerialize] IGameplayService gameplayService;
        //[OdinSerialize] IGUIService gUIService;
        //[OdinSerialize] IScreenPool screenPool;
        //[OdinSerialize] IStorageService storageService;
        //[OdinSerialize] IRemoteConfigService remoteConfigService;

        #endregion



        #region IContextInstaller

        public static void Install()
        {
            //ServicesSystem.Instance.RegisterService(new EntityService() as IEntityService);
            //ServicesSystem.Instance.RegisterService(new EventsService() as IEventsService);
            //ServicesSystem.Instance.RegisterService(new GameFlowService() as IGameFlowService);
            //ServicesSystem.Instance.RegisterService(new GameplayService() as IGameplayService);
            //ServicesSystem.Instance.RegisterService(new GUIService() as IGUIService);
            //ServicesSystem.Instance.RegisterService(new BinaryStorageService() as IStorageService);
            //ServicesSystem.Instance.RegisterService(new RemoteCo);

            //IoCContainer.Bind(new DataVault() as IDataVault);
            //IoCContainer.Bind(new UnityScreenPool());
        }

        #endregion
    }
}