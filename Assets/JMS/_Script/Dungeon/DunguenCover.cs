using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunguenCover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.Difficulty != Difficulty.S)
        {
            gameObject.SetActive(false);
        }
    }


}
