using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] Animator characterAnimator;
    
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
        Vector3 previousPosition = moverTransform.position;
        while (!Mathf.Approximately(Vector3.Distance(moverTransform.position, newPosition), 0))
        {
            moverTransform.position = Vector3.MoveTowards(moverTransform.position, newPosition, speed * Time.deltaTime);
            SetSpeedForAnimator(previousPosition, moverTransform.position);
            previousPosition = moverTransform.position;
            yield return new WaitForEndOfFrame();
        }

        moverTransform.position = newPosition;
        ZeroSpeeds();
        movementCompletedEvent?.Invoke();
        activeCoroutine = null;
    }

    private void SetSpeedForAnimator(Vector3 previousPosition, Vector3 position)
    {
        var horizontalSpeed = Mathf.Abs(position.x - previousPosition.x) / Time.deltaTime;
        var verticalSpeed = (position.y - previousPosition.y) / Time.deltaTime;

        characterAnimator.SetFloat("Vertical speed", verticalSpeed);
        characterAnimator.SetFloat("Horizontal speed", horizontalSpeed);
    }

    void ZeroSpeeds()
    {
        characterAnimator.SetFloat("Vertical speed", 0);
        characterAnimator.SetFloat("Horizontal speed", 0);
    }
}
