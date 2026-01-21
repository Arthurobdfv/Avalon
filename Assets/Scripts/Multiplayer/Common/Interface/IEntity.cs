public interface IEntity
{
    public string Id { get; set; }
    public EntityTypeEnum EntityType { get; set; }

    public void SetId(string id) => Id = id;
    public void SetEntityType(EntityTypeEnum entityType) => EntityType = entityType;
}