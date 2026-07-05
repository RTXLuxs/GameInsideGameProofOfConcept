using UnityEngine;

public abstract class RequirementBase : MonoBehaviour, IRequirement
{
    [Header("Requirement")]
    [SerializeField] protected bool invertResult = false;

    public bool IsSatisfied(out string failureReason)
    {
        bool result = CheckRequirement(out failureReason);

        return invertResult ? !result : result;
    }

    protected abstract bool CheckRequirement(out string failureReason);
}