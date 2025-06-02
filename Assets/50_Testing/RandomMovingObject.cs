using UnityEngine;

public class RandomMovingObject : MonoBehaviour, TimeAffected
{
    [SerializeField] private Vector3 speedMinValues, speedMaxValues;
    [SerializeField] private float spawnDist = 30;
    [SerializeField] private float spawnHeight = 8, spawnHeightRange = 5;

    [SerializeField] private float lifeTime = 30;


    Vector3 moveVector;

    float timeScale = 1;

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }

    private void Start()
    {
        transform.position = new Vector3(Random.Range(-spawnDist, spawnDist), spawnHeight + Random.Range(-spawnHeightRange, spawnHeightRange), Random.Range(-spawnDist, spawnDist));

        moveVector = new Vector3(Random.Range(speedMinValues.x, speedMaxValues.x), Random.Range(speedMinValues.y, speedMaxValues.y), Random.Range(speedMinValues.z, speedMaxValues.z));
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * timeScale * moveVector);

        lifeTime -= Time.deltaTime * timeScale;

        if(lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
