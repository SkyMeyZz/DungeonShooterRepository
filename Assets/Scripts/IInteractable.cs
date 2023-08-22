using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);

    Transform GetTransform();

    //void OnCanInterract(GameObject interactor);
}
