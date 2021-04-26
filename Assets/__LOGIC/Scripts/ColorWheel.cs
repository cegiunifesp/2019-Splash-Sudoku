using UnityEngine;
using UnityEngine.EventSystems;

public class ColorWheel : MonoBehaviour //, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float _raio;  // Usado para posicionar as cores na roda

    //private Vector2 _initPos; // Posicao inicial do mouse quando foi clickada
    //private float _initAng;
    //private Vector2 _currentPos;

    //private bool adjust = false; // Ajusta bolinha ao centro
    //private int meta;


    // Use this for initialization
    void Start()
    {
        float ang =  0;

        // Pega todos os filhos
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            _raio = Vector2.Distance(child.position, transform.position);

            child.position = transform.position + new Vector3(Mathf.Cos(ang) * _raio, Mathf.Sin(ang) * _raio, -0.01f);
            child.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * ang - 90);

            ang +=  Mathf.PI / 4;
        }
    }
    /*
    void Update()
    {

        if (adjust)
        {
            float angle = transform.rotation.eulerAngles.z;
            float delta;

            delta = meta - angle;

            if (delta > 180)
                delta = delta - 360;
            else if (delta < -180)
                delta = 360 + delta;
  


            if (Mathf.Abs(delta) < 0.5f )
            {
                transform.eulerAngles = new Vector3(0, 0, meta);
                adjust = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, angle + Time.deltaTime * delta * 5);
            }

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _initPos = Input.mousePosition - transform.position;
        _initAng = transform.eulerAngles.z;
        adjust = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        _initPos = Input.mousePosition - transform.position;
        _initAng = transform.eulerAngles.z;
        adjust = true;
        float angle = transform.rotation.eulerAngles.z;
        meta = (int)((int)(angle - 45) / 72) * 72 + 45;
        if (meta > 180)
            meta = -1 * (360 - meta);

    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentPos = Input.mousePosition - transform.position;

        if (Vector3.Distance(_currentPos, _initPos) > 0.01f)
        {
            float current_ang = _initAng + Vector3.SignedAngle(_currentPos, _initPos, Vector3.back);
            transform.eulerAngles = new Vector3(0, 0, current_ang);
        }
    }

        
    public void EndAnimation()
    {
        GetComponent<Animator>().enabled = false;
    }
    */
}
