using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeckManager))]
public class EditorDeckManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DeckManager deckManager = (DeckManager)target;

        if(GUILayout.Button("Update card list in \"AvailableDeckUI\""))
        {
            deckManager.AvaliableDeckUI.SetAllCardsCount(deckManager.Library.Cards);
            Debug.Log("Update card list in \"AvailableDeckUI\"");
        }
    }
}
