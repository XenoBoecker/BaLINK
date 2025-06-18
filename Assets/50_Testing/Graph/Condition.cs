using UnityEngine;

public abstract class Condition : ScriptableObject
{
    public GameObject ReferencedObject;

    public virtual void Initialize(GameObject referencedObject)
    {
        ReferencedObject = referencedObject;
    }
    public abstract bool ConditionIsMet();
    public abstract string GetConditionName();
}
