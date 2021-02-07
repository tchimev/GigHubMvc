using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGenreRepo
    {
        IEnumerable<Genre> GetGenres();
    }
}