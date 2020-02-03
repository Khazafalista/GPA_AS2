using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.forward = mainCamera.transform.forward;
    }

    public void ChangeHealthNinja(int health, int maxHealth)
    {
        transform.Find("Health").GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
        //Debug.Log(GetComponentInChildren<Image>().fillAmount);
    }
}
