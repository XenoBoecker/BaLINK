using UnityEngine;

public class SceneRoot : MonoBehaviour
{
    [SerializeField] private TrainCarGenerator _trainCarGenerator;
    public TrainCarGenerator TrainCarGenerator => _trainCarGenerator;
}
