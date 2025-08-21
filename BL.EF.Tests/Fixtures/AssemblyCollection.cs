namespace BL.EF.Tests.Fixtures;

[CollectionDefinition(Name)]
public class DockerDatabaseTests : ICollectionFixture<KisDbContextFactory> {
    public const string Name = "DockerDatabaseTests";
}
