using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FurnitureDestruction : MonoBehaviour {

	[SerializeField]private FurnitureObject furniture;

	private SpriteRenderer spriteRenderer;
	private FurnitureObjectSO furnitureObjectSO;
	private GameController gameController;
	

	private void Awake() {
		spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
		furnitureObjectSO = furniture.GetFurnitureObjectSO();
		gameController = FindFirstObjectByType<GameController>();
	}

	public void ChangeImage(int i) {
		spriteRenderer.sprite = furnitureObjectSO.images[i];
		if (i + 1 == furnitureObjectSO.images.Length) {
			spriteRenderer.material = gameController.destroyedMaterial;
		}
	}

}
