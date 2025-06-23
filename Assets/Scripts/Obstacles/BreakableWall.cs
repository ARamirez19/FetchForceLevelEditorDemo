using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
#endif
public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float minBreakSpeed = 5.0f;
    [SerializeField] private List<SpriteRenderer> pieces = new List<SpriteRenderer>();

    private Rigidbody2D[] parts;
    private ParticleSystem[] vfx;
    private float fadeTime = 2f;
    private void Start()
    {
        parts = GetComponentsInChildren<Rigidbody2D>();
        vfx = GetComponentsInChildren<ParticleSystem>();
        foreach (Rigidbody2D part in parts)
        {
            part.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.magnitude >= minBreakSpeed)
        {
            foreach (ParticleSystem s in vfx)
            {
                s.Play();
            }
            foreach (Rigidbody2D part in parts)
            {
                part.constraints = RigidbodyConstraints2D.None;
            }
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "ShadowCubark")
            {
                StartCoroutine(FadeOut(() => gameObject.SetActive(false)));
                this.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(DisableColliders());
            }
        }
		if(other.gameObject.name.Contains("Charger"))
		{
			foreach (ParticleSystem s in vfx)
			{
				s.Play();
			}
			foreach (Rigidbody2D part in parts)
			{
				part.constraints = RigidbodyConstraints2D.None;
			}
			StartCoroutine(FadeOut(() => gameObject.SetActive(false)));
			this.GetComponent<Collider2D>().enabled = false;
			StartCoroutine(DisableColliders());
		}
    }

    private IEnumerator FadeOut(System.Action callback)
    {
        float lerpStep = 1f;
        while (lerpStep != 0f)
        {
            lerpStep = Mathf.Lerp(lerpStep, 0, 2f * Time.deltaTime);
            foreach (var piece in pieces)
            {
                var temp = piece.color;
                temp.a = lerpStep;
                piece.color = temp;
            }
            yield return null;
        }
        callback();
    }

    private IEnumerator DisableColliders()
    {
        yield return new WaitForSeconds(1.0f);
        foreach (var part in GetComponentsInChildren<Collider2D>())
        {
            Debug.Log("test");
            part.enabled = false;
        }
        yield return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Get Pieces")]
    private void GetPieces()
    {
        pieces = GetComponentsInChildren<SpriteRenderer>().ToList();
        EditorSceneManager.MarkSceneDirty(PrefabStageUtility.GetCurrentPrefabStage().scene);
    }
#endif
}
