public interface IRequirement
{
    bool IsSatisfied(out string failureReason);

    void OnSatisfied();
}
