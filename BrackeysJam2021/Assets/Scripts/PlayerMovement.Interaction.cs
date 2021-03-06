﻿using UnityEngine;

public partial class PlayerMovement
{
    /// <summary>
    /// Called from PlayerMovement.Input
    /// </summary>
    private void UseClosest()
    {
        OnUseInteractible?.Invoke();
    }

    /// <summary>
    /// Physic tick that updates the available objects around the player
    /// </summary>
    private void PopulateInteractiblesInRange()
    {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        if (hitColliders != null)
        {
            foreach (var go in objectsInRange)
            {
                IInteractible interactionScript;
                GameManager.interactibles.TryGetValue(go, out interactionScript);
                if (interactionScript != null)
                {
                    interactionScript.outline.enabled = false;
                }
            }

            objectsInRange.Clear();
        }

        foreach (var collider in hitColliders)
        {
            if (collider.transform.CompareTag("Interactible"))
            {
                IInteractible interactionScript;
                GameManager.interactibles.TryGetValue(collider.gameObject, out interactionScript);
                if (interactionScript != null)
                {
                    if (interactionScript.Usable)
                    {
                        objectsInRange.Add(collider.gameObject);
                    }
                }
            }
        }

        if (interactionTarget != null && !objectsInRange.Contains(interactionTarget))
        {
            interactionTargetScript.outline.enabled = false;
            interactionTargetScript = null;
            interactionTarget = null;
        }

        SelectClosest();
    }

    private void UseInteractibleTarget()
    {
        if (interactionTarget != null)
        {
            var target = interactionTarget.GetComponent<IInteractible>();

            if (target != null)
            {
                target.Use();
                target.Used = true;
                interactionTargetScript.outline.enabled = false;
                interactionTargetScript = null;
                interactionTarget = null;
            }
        }
    }

    #region Interactions Helper
    private void SelectClosest()
    {
        float shortest = Mathf.Infinity;

        foreach (var collider in objectsInRange)
        {
            float currentDistance = (transform.position - collider.transform.position).sqrMagnitude;
            if (shortest > currentDistance)
            {
                shortest = currentDistance;
                interactionTarget = collider.gameObject;
                GameManager.interactibles.TryGetValue(interactionTarget, out interactionTargetScript);
                interactionTargetScript.outline.enabled = true;
            }
        }

        foreach (var go in objectsInRange)
        {
            if (go != interactionTarget)
            {
                IInteractible interactionScript;
                GameManager.interactibles.TryGetValue(interactionTarget, out interactionScript);
                if (interactionScript != null)
                {
                    interactionScript.outline.enabled = false;
                }
            }

        }
    }

    private bool SameSign(float num1, float num2)
    {
        return (((num1 == 0) == (num2 == 0)) &&
        ((num1 < 0) == (num2 < 0)));
    }

    private bool CanFusion()
    {
        return (transform.position - aloneStayGO.transform.position).sqrMagnitude <= interactionRange;
    }

    private bool CanInteract()
    {
        return objectsInRange.Count > 0;
    }
    #endregion
}
