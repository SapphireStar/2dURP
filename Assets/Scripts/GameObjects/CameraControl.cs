using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    
    public Transform camera;
    float m_inputX;
    float m_inputY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_inputX = Input.GetAxis("Vertical");
        m_inputY = Input.GetAxis("Horizontal");
        camera.Translate(new Vector3( m_inputY * Time.deltaTime, m_inputX * Time.deltaTime, 0));
    }
}
