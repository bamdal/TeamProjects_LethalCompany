using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Overlapbox : TestBase
{
    Transform newModul;
    void Start()
    {

        newModul = GetComponent<Transform>();


    }

    // Update is called once per frame
    void Update()
    {
        BoxCollider newmoduleCollider = newModul.GetComponent<BoxCollider>();
        Collider[] moduleCollider = Physics.OverlapBox(newmoduleCollider.transform.position, newmoduleCollider.size*0.5f, newmoduleCollider.transform.rotation);
       
        foreach (Collider collider in moduleCollider)
        {
            if (collider.gameObject != newModul.gameObject) // 자기 자신은 제외
            {
                Debug.Log(collider.name);

            }
        }
    }
}
