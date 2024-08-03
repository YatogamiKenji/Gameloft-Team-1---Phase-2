using UnityEngine;
using UnityEngine.Pool;

public class ShootProjectiles : MonoBehaviour
{
    [SerializeField] Bullet bullletPref;
    private IObjectPool<Bullet> bulletPool;
    [SerializeField] private bool collectionCheck;


    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(CreateBullet, GetBullet, ReleaseBullet, DestroyBullet, collectionCheck);
    }


    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bullletPref);
        return bullet;
    }

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.enabled = false;
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.enabled = true;
    }
    private void DestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            bulletPool.Release(bullet);
            // collect event
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Bullet bullet = bulletPool.Get();
            bullet.transform.position = this.transform.position;
            Vector2 direction = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2) transform.position;
            bullet.Shoot(direction, false);
        }
    }
}
