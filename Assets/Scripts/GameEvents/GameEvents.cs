using System;
public static partial class GameEvents
{
    public static event Action  TapToPlay;
    public static event Action OnDoneDrawingtrack;
    public static event Action GameWin;
    public static event Action GameLose;
}
public static partial class GameEvents
{
    public static void InvokeTapToPlay() => TapToPlay?.Invoke();
    public static void InvokeOnDoneDrawingtrack() => OnDoneDrawingtrack?.Invoke();
    public static void InvokeGameWin() => GameWin?.Invoke();
    public static void InvokeGameLose(int result) => GameLose?.Invoke();
    
}
