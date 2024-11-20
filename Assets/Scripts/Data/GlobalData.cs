public static class GlobalData
{
    public enum GameStates
    { 
        Menu,
        Map,
        Gameplay
    }    

    public enum FishingStates
    {
        WaitThrowing,
        WaitingFish,
        FishOnHook,
        Hooking
    }

    public static UniRx<GameStates> gameState = new(GameStates.Menu);
    public static UniRx<FishingStates> FishingState = new(FishingStates.WaitThrowing);

    public static float animationBaseTime = 3f;
    public static float animationFishOnHookTime = 0.5f;
}