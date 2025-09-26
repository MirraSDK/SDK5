namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IPlatformInfo {

        PlatformType Current { get; }
        DeploymentType Deployment { get; }
        string AppId { get; }

    }

}