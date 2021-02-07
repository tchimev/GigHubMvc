using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Persistence;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class GenreRepo : IGenreRepo
    {
        private readonly ApplicationDbContext _db;

        public GenreRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _db.Genres.ToList();
        }
    }
}