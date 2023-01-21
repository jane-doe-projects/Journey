using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEffect : MonoBehaviour
{
    ParticleSystem effect;
    [SerializeField] Vector3 effectPositionOffset;
    [SerializeField] GameObject effectObject;
    GameObject effectHolder;

    private void Start()
    {
        // TODO DOES NOT WORK
        effectHolder = Instantiate(effectObject, transform);
        effectHolder.transform.localPosition = effectPositionOffset;

        effect = effectObject.GetComponent<ParticleSystem>();
    }

    public void PlayEffect()
    {
        Debug.Log("playing effect hehe");
        effect.Play();
    }
}
