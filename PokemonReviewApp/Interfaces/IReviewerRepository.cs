using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> getReviewers();
        Reviewer getReviewer(int id);
        ICollection<Review> getReviewsByReviewer(int id);

        bool exists(int id);
        bool createReviewer(Reviewer reviewer);
        bool updateReviewer(Reviewer reviewer);

        bool deleteReviewer(Reviewer reviewer);
        bool save();
    }
}
