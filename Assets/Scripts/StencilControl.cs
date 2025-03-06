using UnityEngine;
using System.Collections;

public class StencilControl : MonoBehaviour
{
    public Transform squeegee; 
    public Transform[] pasteAreas;

    public float moveSpeed = 0.1f;
    public float moveDistance = 1f;
    private bool movingRight = true;
    public static int moveCount = 0;
    public int maxMoves = 2;
    public float liftHeight = 2f;
    public float squeegeeDropHeight = 0.1f;
    public float squeegeeLiftHeight = 0.2f;

    private Vector3 stencilStartPosition;
    private Vector3 squeegeeStartPosition;

    void Start()
    {
        stencilStartPosition = transform.position;
        squeegeeStartPosition = squeegee.position;
        StartCoroutine(StencilProcess());
    }

    IEnumerator StencilProcess()
    {
        //Move Stencil & Squeegee Down Together to PCB Level
        while (transform.position.y > 0.01f)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //Squeegee Moves a Bit Further Down
        Vector3 squeegeeLowerPosition = squeegee.position - new Vector3(0, squeegeeDropHeight, 0);
        while (Vector3.Distance(squeegee.position, squeegeeLowerPosition) > 0.08f)
        {
            squeegee.position = Vector3.MoveTowards(squeegee.position, squeegeeLowerPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //Move Squeegee Left-Right
        while (moveCount < maxMoves)
        {
            Vector3 targetPosition = new Vector3(
                (movingRight ? moveDistance : -moveDistance),
                squeegee.localPosition.y,
                squeegee.localPosition.z
            );

            while (Vector3.Distance(squeegee.localPosition, targetPosition) > 0.01f)
            {
                squeegee.localPosition = Vector3.MoveTowards(squeegee.localPosition, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            movingRight = !movingRight;

            if (moveCount < pasteAreas.Length)
            {
                pasteAreas[moveCount].GetComponent<Renderer>().enabled = true;
            }

            moveCount++;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);

        //Squeegee Moves Up Slightly
        Vector3 squeegeeUpperPosition = squeegee.localPosition + new Vector3(0, squeegeeLiftHeight, 0);
        while (Vector3.Distance(squeegee.localPosition, squeegeeUpperPosition) > 0.01f) 
        {
            Debug.Log("Squeegee moving up");
            squeegee.localPosition = Vector3.MoveTowards(squeegee.localPosition, squeegeeUpperPosition, moveSpeed * 100 * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //Move Stencil & Squeegee Up Together
        Vector3 liftPosition = stencilStartPosition + new Vector3(0, liftHeight, 0);

        while (Vector3.Distance(transform.position, liftPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, liftPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the stencil stops exactly at the final position
        transform.position = liftPosition;

        Debug.Log("Stencil printing completed!");
    }
}
