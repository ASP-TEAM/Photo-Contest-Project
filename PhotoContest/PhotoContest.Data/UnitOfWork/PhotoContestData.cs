namespace PhotoContest.Data.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Repositories;
    using PhotoContest.Models;

    public class PhotoContestData : IPhotoContestData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public PhotoContestData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<RewardStrategy> RewardStrategies
        {
            get
            {
                return this.GetRepository<RewardStrategy>();
            }
        }

        public IRepository<VotingStrategy> VotingStrategies
        {
            get
            {
                return this.GetRepository<VotingStrategy>();
            }
        }

        public IRepository<ParticipationStrategy> ParticipationStrategies
        {
            get
            {
                return this.GetRepository<ParticipationStrategy>();
            }
        }

        public IRepository<DeadlineStrategy> DeadlineStrategies
        {
            get
            {
                return this.GetRepository<DeadlineStrategy>();
            }
        }

        public IRepository<Contest> Contests
        {
            get
            {
                return this.GetRepository<Contest>();
            }
        }

        public IRepository<Picture> Pictures
        {
            get
            {
                return this.GetRepository<Picture>();
            }
        }

        public IRepository<Vote> Votes
        {
            get
            {
                return this.GetRepository<Vote>();
            }
        }

        public IRepository<ContestWinners> ContestWinners
        {
            get
            {
                return this.GetRepository<ContestWinners>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(GenericEfRepository<T>), this.context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}