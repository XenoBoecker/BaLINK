using UnityEngine;

[NodeElement(NodeType.Effect)]
public class PlayDialogueLine : Effect
{
    [SerializeField] private DialogueSequence _sequence;
    [SerializeField] private int _lineIndex;

    private DialogueRunner _runner;

    public override void TriggerEffect()
    {
        GameObject foundObject = GameObject.FindGameObjectWithTag("Dialogue Runner");
        if (foundObject == null || !foundObject.TryGetComponent(out _runner))
        {
            throw new System.Exception("There is no object tagged 'Dialogue Runner' in the scene");
        }

        _runner = ReferencedObject.GetComponent<DialogueRunner>();
        _runner.PlayDialogueLine(_sequence, _lineIndex);
    }

    public override string GetName()
    {
        return "Play Dialogue Line";
    }
}