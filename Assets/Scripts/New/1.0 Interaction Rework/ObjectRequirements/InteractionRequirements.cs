using UnityEngine;

public class InteractionRequirements : MonoBehaviour
{
    private RequirementBase[] requirements;

    private void Awake()
    {
        CacheRequirements();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CacheRequirements();
    }
#endif

    private void CacheRequirements()
    {
        requirements = GetComponents<RequirementBase>();
    }

    public bool AreSatisfied(out string failureReason)
    {
        foreach (RequirementBase requirement in requirements)
        {
            if (!requirement.IsSatisfied(out failureReason))
            {
                Debug.Log($"{name}: Failed requirement '{requirement.GetType().Name}'.");

                return false;
            }
        }

        failureReason = string.Empty;
        return true;
    }
}
