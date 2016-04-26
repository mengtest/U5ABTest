using UnityEngine;
using System.Collections;
using ResetCore.Event;
using ResetCore.Util;

public class Bullet : MonoBehaviour {

    public GameObject parent { get; set; }
    public Rigidbody2D rig { get; private set; }

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonoEventDispatcher.GetMonoController(other.gameObject).TriggerEvent<Bullet>(BaseBlock.eventShoted, this);
        ObjectPool.Instance.HideOrDestroyObject(gameObject);
    }

}
