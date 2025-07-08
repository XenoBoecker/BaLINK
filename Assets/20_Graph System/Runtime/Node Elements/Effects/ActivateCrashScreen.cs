using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(CrashScreen))]
public class ActivateCrashScreen : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out CrashScreen crashScreen))
        {
            crashScreen.ShowCrashScreen();
        }
    }

    public override string GetName()
    {
        return "Crash the Game";
    }
}
