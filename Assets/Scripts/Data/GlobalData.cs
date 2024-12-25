using static GlobalEnums;

public static class GlobalData
{
    public static UniRx<GameStates> gameState = new(GameStates.Menu);
    public static UniRx<FishingStates> FishingState = new(FishingStates.WaitThrowing);

    public static float animationBaseTime = 3f;
    public static float animationFishOnHookTime = 0.5f;
    public static float animationRodTime = 0.4f;
}