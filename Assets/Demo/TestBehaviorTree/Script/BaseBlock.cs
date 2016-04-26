using UnityEngine;
using System.Collections;
using ResetCore.Event;
using UnityEngine.UI;
using ResetCore.Asset;
using ResetCore.Util;



public class BaseBlock : MonoBehaviour {

    public static readonly string eventShoted = "Event.Shoted";

    float _hp;
    float Hp
    {
        get { return _hp; }
        set 
        {
            _hp = value;
            HpText.text = _hp.ToString(); 
        }
    }
    [SerializeField]
    private Text HpText;

    void Awake()
    {
        Hp = 100;
        MonoEventDispatcher.GetMonoController(gameObject).AddEventListener<Bullet>(eventShoted, Shooted);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(Vector3 pos)
    {

    }

    public IEnumerator DoMove(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > 1)
        {
            Vector3 interval = (transform.position - pos);
            transform.position += interval.normalized * 2;
            yield return null;
        }
    }


    public IEnumerator FindPlayer()
    {
        yield return null;
    }

    public void Attack(Vector3 pos)
    {
        GameObject bulletGo = ObjectPool.Instance.CreateOrFindGameObject("Bullet", "Bullet");
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        bullet.parent = gameObject;

        Vector3 interval = (pos - transform.position);
        float distance = interval.magnitude;

        bullet.transform.position = transform.position + interval.normalized * 10;
        bullet.transform.localScale = Vector2.one * 0.3f;
        bullet.rig.velocity = interval.normalized * 20;
    }

    void Shooted(Bullet bullet)
    {
        if (bullet.parent != gameObject)
        {
            Hp -= 10;
        }
    }

}
