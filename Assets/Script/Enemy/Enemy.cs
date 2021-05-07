using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private ParticleSystem enemyPS;

    private void Start()
    {
        enemyPS = gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        CheckRaycast(Vector3.back);
        CheckRaycast(Vector3.forward);

        //check down raycast for track situation
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .5f))
        {
            if (hit.collider.GetComponent<PieceController>().CurrentPieceState == PieceState.TRACK)
            {
                GameplayController.Instance.LoseLevel();
            }
        }
    }

    //check back and forward with raycast
    private void CheckRaycast(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, .1f))
        {
            if (hit.collider.tag == "Piece")
            {
                StartCoroutine(DestroyAnimation());
            }
        }
    }


    //Before enemy destroy, particle effect active.
    IEnumerator DestroyAnimation()
    {
        enemyPS.Play();
        yield return new WaitForSeconds(enemyPS.main.duration);
        Destroy(gameObject);
    }

}
