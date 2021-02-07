using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class FollowingRepo : IFollowingRepo
    {
        private readonly ApplicationDbContext _db;

        public FollowingRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Following following)
        {
            _db.Followings.Add(following);
        }

        public Following GetFollowing(string followerId, string followeeId)
        {
            return _db.Followings.SingleOrDefault(a => a.FollowerId == followerId && a.FolloweeId == followeeId);
        }

        public IEnumerable<Following> GetFollowingsByUser(string userId)
        {
            return _db.Followings
                            .Include(f => f.Followee)
                            .Where(f => f.FollowerId == userId).ToList();
        }

        public bool IsFollowing(string followerId, string followeeId)
        {
            return _db.Followings.Any(a => a.FollowerId == followerId && a.FolloweeId == followeeId);
        }

        public void Remove(Following following)
        {
            _db.Followings.Remove(following);
        }
    }
}