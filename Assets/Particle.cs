using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    //消滅するまでの時間
    private float lifeTime;
    //消滅するまでの残り時間
    private float leftlifeTime;
    //移動量
    private Vector3 velocity;
    //初期Scale
    private Vector3 defaultScale;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime =0.3f;
        leftlifeTime =lifeTime;
        defaultScale = transform.localScale;
        float maxVelocity = 5;

        velocity = new Vector3(
            Random.Range(-maxVelocity, maxVelocity),
            Random.Range(-maxVelocity, maxVelocity),
            0
            );
    }

    // Update is called once per frame
    void Update()
    {
        leftlifeTime -= Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        transform.localScale = Vector3.Lerp(
            new Vector3(0, 0, 0),
            defaultScale,
            leftlifeTime / lifeTime
            );
        if(leftlifeTime<= 0) { Destroy(gameObject); }
    }
}
