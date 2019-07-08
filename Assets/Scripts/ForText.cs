using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForText : MonoBehaviour
{
    [SerializeField]
    private Text speedtext;
    float speed;
    int Intspeed;
 
    // Start is called before the first frame update
    void Start()
    {
        this.speedtext.text = "0";
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        speed = speed + 5 * Time.deltaTime;
        Intspeed = (int)speed;
        this.speedtext.text = Intspeed.ToString();
    }   
    
}
