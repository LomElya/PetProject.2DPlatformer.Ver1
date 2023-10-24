using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 _deference;
    private float _rotateZ;
    private Vector3 _scale;
    private void Update()
    {
        _deference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _rotateZ = Mathf.Atan2(_deference.y, _deference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _rotateZ);

        _scale = transform.localScale;
        if (_rotateZ > 90 || _rotateZ < -90)
        {
            _scale.x = -1;
            _scale.y = -1;
        }
        else
        {
            _scale.x = 1;
            _scale.y = 1;
        }

        transform.localScale = _scale;
    }
}
