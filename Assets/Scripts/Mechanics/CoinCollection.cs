using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    CoinsManager coinsManager;
    public ParticleSystem EndPartical;
    public MMFeedbacks FeelFeedBack;

    void Start()
    {
        //find the CoinsManager
        coinsManager = FindObjectOfType<CoinsManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMoveY(transform.position.y + 6, .8f).SetEase(Ease.InOutBack));
            //seq.AppendInterval(.1f);
            seq.AppendCallback((() => { EndPartical.Play(); }));
            seq.Append(transform.DOScale(Vector3.zero, .1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Truck"))
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMoveY(transform.position.y + 9, .8f).SetEase(Ease.InOutBack));
            //seq.AppendInterval(.1f);
            seq.AppendCallback((() => { EndPartical.Play(); }));
            seq.Append(transform.DOScale(Vector3.zero, .1f));
            FeelFeedBack?.PlayFeedbacks();
        }
    }
}