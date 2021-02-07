using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepo
    {
        void Add(Following following);
        void Remove(Following following);
        bool IsFollowing(string followerId, string followeeId);
        Following GetFollowing(string followerId, string followeeId);
        IEnumerable<Following> GetFollowingsByUser(string userId);
    }
}
