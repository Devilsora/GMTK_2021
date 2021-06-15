using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelZone : MonoBehaviour
{

    public int levelToGoTo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Player")
    {
      SceneManager.LoadScene(levelToGoTo);
    }
  }
}
