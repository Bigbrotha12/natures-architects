using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float speed = 10;
    Coroutine activeCoroutine;

    public event Action movementCompletedEvent;

    public void MoveToLocation(Transform moverTransform, Vector3 newPosition)
    {
        FaceDirection(moverTransform, newPosition.x >= moverTransform.position.x);

        if (activeCoroutine == null)
        {
            activeCoroutine = StartCoroutine(MoveCoroutine(moverTransform, newPosition));
        }
    }

    void FaceDirection(Transform moverTransform, bool faceRight)
    {
        float scaleX = Mathf.Abs(moverTransform.localScale.x);
        scaleX = faceRight ? scaleX : -scaleX;
        moverTransform.localScale = new Vector3(scaleX, moverTransform.localScale.y, moverTransform.localScale.z);
    }

    IEnumerator MoveCoroutine(Transform moverTransform, Vector3 newPosition)
    {
        while (!Mathf.Approximately(Vector3.Distance(moverTransform.position, newPosition), 0))
        {
            moverTransform.position = Vector3.MoveTowards(moverTransform.position, newPosition, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        moverTransform.position = newPosition;
        movementCompletedEvent?.Invoke();
        activeCoroutine = null;
    }
}
