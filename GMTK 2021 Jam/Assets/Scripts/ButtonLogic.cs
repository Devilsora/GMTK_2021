using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject object_To_remove;
    public int requiredWeight;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


  public void OnTriggerEnter(Collider other)
  {
    Debug.Log("Other obj: " + other.gameObject);

    if (other.gameObject.tag == "Player" || other.gameObject.tag == "Blob")
    {
      if (other.gameObject.transform.parent.GetComponent<PlayerController>().weightVal >= requiredWeight)
      {
        Destroy(object_To_remove);
      }
    }
    else
    {
      Debug.Log("Other tag: " + other.gameObject.tag);
    }
  }

}
