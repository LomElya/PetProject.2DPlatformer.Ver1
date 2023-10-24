using UnityEngine;

public class MousePosition : MonoBehaviour
{

    private Vector3 _deference;
    private float _rotateZ;
    private Vector3 _scale;
    private void Update()
    {
        //transform.position = Input.mousePosition;
        _deference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
       
        Debug.Log("Позиция: " + _deference);
    }
}
