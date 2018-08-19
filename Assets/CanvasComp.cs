using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CanvasComp : MonoBehaviour {

    protected SpriteRenderer m_SpriteRenderer;

	// Use this for initialization
	void Start () {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //Texture2D t2d = (Texture2D)m_SpriteRenderer.sprite.texture.GetRawTextureData();

	}
}
