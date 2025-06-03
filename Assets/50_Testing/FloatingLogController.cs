using UnityEngine;

public class FloatingLogController : MonoBehaviour, ITimeAffected
{
    float _timeScale = 1;
    [SerializeField] Vector3 _moveDirection;

    public void SetTimeScale(float timeScale)
    {
        _timeScale = timeScale;
    }

    void Update()
    {
        transform.position += _moveDirection * Time.deltaTime * _timeScale;

        if (transform.position.x > 50)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
