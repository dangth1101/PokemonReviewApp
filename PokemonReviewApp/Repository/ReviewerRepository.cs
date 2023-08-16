using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext context;

        public ReviewerRepository(DataContext context)
        {
            this.context = context;
        }

        public bool createReviewer(Reviewer reviewer)
        {
            context.Add(reviewer);
            return save();
        }

        public bool deleteReviewer(Reviewer reviewer)
        {
            context.Remove(reviewer);
            return save();
        }

        public bool exists(int id)
        {
            return context.reviewers.Any(rer =>  rer.id == id);
        }

        public Reviewer getReviewer(int id)
        {
            return context.reviewers.Where(rer => rer.id == id).FirstOrDefault();
        }

        public ICollection<Reviewer> getReviewers()
        {
            return context.reviewers.ToList();
        }

        public ICollection<Review> getReviewsByReviewer(int id)
        {
            return context.reviews.Where(r => r.reviewer.id == id).ToList();
        }

        public bool save()
        {
            var save = context.SaveChanges() > 0;
            return save;
        }

        public bool updateReviewer(Reviewer reviewer)
        {
            context.Update(reviewer); 
            return save();
        }
    }
}
