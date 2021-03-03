namespace RulesEngine.RuleModel
{
    internal interface IEntityIdProvider
    {
        object Id { get; set; }
        string EntityType { get; }
    }
}