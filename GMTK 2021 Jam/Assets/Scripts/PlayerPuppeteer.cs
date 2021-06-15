using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPuppeteer : MonoBehaviour
{
    public GameObject currentBlob;

    

    public float rayLength;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
      currentBlob = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
      //extend raycast from player to object
      //if object is a blob, take control of it and disable this until another blob gets clicked?

          RaycastHit hit;
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          if(Physics.Raycast(ray, out hit, rayLength, layerMask))
          {
            Debug.Log(hit.collider.name);

            GameObject obj = hit.collider.gameObject;

            if (obj.GetComponent<PlayerController>())
            {
              if(currentBlob == null)
                currentBlob = obj;
              else
              {
                currentBlob.tag = "Blob";
                currentBlob.GetComponent<PlayerController>().enabled = false;
                obj.GetComponent<PlayerController>().enabled = true;
                
                currentBlob = obj;
                currentBlob.tag = "Player";
                
              }

              
            }
              

          }
        }
    }

    
}
