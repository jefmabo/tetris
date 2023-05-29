using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameObject.transform.Rotate(0, 0, 90);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.transform.position += new Vector3(0, -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.transform.position += new Vector3(1, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.transform.position += new Vector3(-1, 0, 0);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.transform.position += new Vector3(0, -1, 0);

        StartCoroutine(Move());
    }
}
