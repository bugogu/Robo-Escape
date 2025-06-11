using System.Collections;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] private GameObject _brokenObject;
    [SerializeField] private float _destroyDelay = 3;

    private MeshRenderer _meshRenderer;
    private Collider[] _brokenColliders;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _brokenColliders = _brokenObject.GetComponentsInChildren<Collider>(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tags.PLAYER))
            HandleTriggerAction(other);
    }

    private void HandleTriggerAction(Collider triggerObject)
    {
        _brokenObject.SetActive(true);

        _meshRenderer.enabled = false;
        GetComponent<Collider>().enabled = false;

        SoundManager.Instance.PlaySFX(SoundManager.Instance.PropBreakSfx);

        StartCoroutine(HandleDestroyAction());
    }

    private IEnumerator HandleDestroyAction()
    {
        yield return new WaitForSeconds(_destroyDelay);

        foreach (Collider col in _brokenColliders) col.enabled = false;

        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
    }
}
