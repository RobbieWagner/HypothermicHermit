using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    [SerializeField] private bool startOnAwake = true;

    [SerializeField] private float startX;
    [SerializeField] private float endX;

    [SerializeField] private float cloudSpeed = 1f;

    private void Awake() 
    {
        transform.position = new Vector2(startX, transform.position.y);

        if(startOnAwake)
        {
            StartCoroutine(CloudScrollCo());
        }    
    }

    private IEnumerator CloudScrollCo()
    {
        Vector2 initialPosition = new Vector2(startX, transform.position.y);
        Vector2 finalPosition = new Vector2(endX, transform.position.y);

        while(true)
        {
            yield return null;
            if((startX > endX && transform.position.x < endX) || (startX < endX && transform.position.x > endX))
            {
                transform.position = initialPosition;
            }
            else 
            {
                transform.position = Vector2.MoveTowards(transform.position, finalPosition, Time.deltaTime * cloudSpeed);
            }
        }
    }
}
