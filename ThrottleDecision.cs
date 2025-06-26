namespace SecureFileSharingAPI
{
    /// <summary>
    /// Possible results of evaluating a rate limit rule.
    /// </summary>
    public enum ThrottleDecision
    {
        Allow,
        Block,
        Log
    }
}
