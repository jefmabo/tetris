using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Transform> transforms;

    // Start is called before the first frame update
    void Start()
    {
        NextPiece();
    }

    public void NextPiece()
    {
        Instantiate(transforms[Random.Range(0, transforms.Count)], transform.position, Quaternion.identity);
    }
}
