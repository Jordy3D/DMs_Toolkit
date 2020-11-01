﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DMCharacter : MonoBehaviour, IPointerClickHandler
{
  public string characterName;
  public Sprite characterImage;
  public SpriteRenderer characterPortrait;

  public bool showName = true;
  public TextMeshPro characterNameText;

  public Transform statusHolder;
  public Transform menuHolder;

  [Header("Side")]
  public bool showSide = false;
  public SpriteRenderer sideDisplay;
  public Color sideColour = Color.red;

  [Header("Range")]
  public bool showRange;
  [Range(0, 5)]
  public float range = 1;
  public SpriteRenderer rangeDisplay;
  public Color rangeColour = Color.red;
  [Range(0, 1)]
  public float rangeAlpha = .3f;

  [Header("Size"), Range(1,4)]
  public int size;

  [Header("Menu")]
  public DMCharacterMenu menu;
  bool menuActive = false;
  public float menuOriginalScale = 0.005025044f;


  Camera cam;
  private Plane dragPlane;
  private Vector3 offset;

  void Start()
  {
    cam = Camera.main;

    UpdateName();

    UpdatePortrait();

    UpdateSide();

    UpdateRange();

    UpdateSize();
  }

  void Update()
  {
    if (showName)
      UpdateName();

    if (showRange)
      UpdateRange();

    if (showSide)
      UpdateSide();
  }

  void ShowMenu(bool _state)
  {
    menu.gameObject.SetActive(_state);
    menuActive = _state;
    menu.transform.localScale = (Vector3.one * 0.005025044f);
  }

  void UpdateRange()
  {
    rangeDisplay.gameObject.SetActive(showRange);

    rangeColour.a = rangeAlpha;
    rangeDisplay.color = rangeColour;
    rangeDisplay.transform.localScale = Vector3.one * range;
  }
  void UpdateSide()
  {
    sideDisplay.gameObject.SetActive(showRange);
    sideDisplay.color = sideColour;
  }
  void UpdatePortrait()
  {
    characterPortrait.sprite = characterImage;
  }
  public void UpdateName()
  {
    characterNameText.gameObject.SetActive(showName);
    characterNameText.SetText(characterName);
  }

  void UpdateSize()
  {
    transform.localScale = Vector3.one * size;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Right)
    {
      menu.character = this;
      menu.statusHolder = statusHolder;
      menu.menuHolder = menuHolder;
      print("Right Clicked!");
      ShowMenu(!menuActive);
    }
  }

  #region Drag
  void OnMouseDown()
  {
    dragPlane = new Plane(cam.transform.forward, transform.position);
    Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

    float planeDist;
    dragPlane.Raycast(camRay, out planeDist);
    offset = transform.position - camRay.GetPoint(planeDist);
  }

  void OnMouseDrag()
  {
    Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

    float planeDist;
    dragPlane.Raycast(camRay, out planeDist);
    transform.position = camRay.GetPoint(planeDist) + offset;
  }
  #endregion

  private void OnMouseUp()
  {
    transform.position = new Vector3(
      RoundedValue(transform.position.x, ((float)size / 4)), 
      RoundedValue(transform.position.y, ((float)size / 4)),
      0);

    //print("Offset: " + ((float)size / 4) + " // Fraction: " + ((float)size * 2));
    //print("Rounded Value is " + RoundedValue(transform.position.x, ((float)size / 4)));
  }

  float RoundedValue(float input, float offset = .25f, float fraction = 2)
  {
    return ((Mathf.Round((input - offset) * fraction)) / fraction) + offset;
  }
}
