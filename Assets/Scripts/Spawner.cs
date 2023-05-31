using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Transform> transforms;
    public List<GameObject> Pieces;
    public int NextPiece;

    // Start is called before the first frame update
    void Start()
    {
        NextPiece = Random.Range(0, Pieces.Count);
        ShowNextPiece();
    }

    public void ShowNextPiece()
    {
        Instantiate(transforms[NextPiece], transform.position, Quaternion.identity);

        NextPiece = Random.Range(0, Pieces.Count);

        for(int i = 0; i < Pieces.Count; i++)
            Pieces[i].SetActive(false);

        Pieces[NextPiece].SetActive(true);
    }
}
