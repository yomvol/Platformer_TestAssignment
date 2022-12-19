using System.Collections;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Vector3 Offset;
    [SerializeField]
    private Vector3 Min;
    [SerializeField]
    private Vector3 Max;
    [SerializeField]
    private float TimeToMove = 1f;

    private Vector3 _startPos;

    private void Start()
    {
        StartCoroutine(Stalk());    
    }

    private IEnumerator Stalk()
    {
        while (true)
        {
            float t = 0f;
            _startPos = transform.position;
            Vector3 goalPoint = Target.position + Offset;
            goalPoint.x = Mathf.Clamp(goalPoint.x, Min.x, Max.x);
            goalPoint.y = Mathf.Clamp(goalPoint.y, Min.y, Max.y);
            goalPoint.z = Mathf.Clamp(goalPoint.z, Min.z, Max.z);
            while (t < 1f)
            {
                transform.position = Vector3.Lerp(_startPos, goalPoint, t);
                t += Time.deltaTime / TimeToMove;
                yield return new WaitForEndOfFrame();
            }
            transform.position = goalPoint;
        }
    }
}
