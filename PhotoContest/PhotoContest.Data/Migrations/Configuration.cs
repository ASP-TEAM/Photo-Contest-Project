namespace PhotoContest.Data.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    internal sealed class Configuration : DbMigrationsConfiguration<PhotoContestDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }
    }
}