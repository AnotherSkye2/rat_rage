using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("ChangeStateOfGameObject",1f,1f);

    }

    void ChangeStateOfGameObject()
    {
        targetObject.SetActive(!targetObject.activeInHierarchy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
