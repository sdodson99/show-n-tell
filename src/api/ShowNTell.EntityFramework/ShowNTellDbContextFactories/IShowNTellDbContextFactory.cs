namespace ShowNTell.EntityFramework.ShowNTellDbContextFactories
{
    public interface IShowNTellDbContextFactory
    {
        ShowNTellDbContext CreateDbContext();
    }
}