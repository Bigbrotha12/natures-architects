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
        if (activeCoroutine == null)
        {
            activeCoroutine = StartCoroutine(MoveCoroutine(moverTransform, newPosition));
        }
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
