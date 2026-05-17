using Microsoft.EntityFrameworkCore;
using Movie_Streaming.Data;
using Movie_Streaming.Models;
namespace Movie_Streaming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //  إنشاء Object من DbContext للتعامل مع قاعدة البيانات
            // هذا هو الرابط بين الكود والـ Database
            ApplicationDbContext db = new ApplicationDbContext();

            // حلقة لا نهائية لعرض Menu للمستخدم
            while (true)
            {
                Console.WriteLine("\n===== Movie Streaming System =====");
                Console.WriteLine("1. Add Category");
                Console.WriteLine("2. Add User");
                Console.WriteLine("3. Add Movie");
                Console.WriteLine("4. Show All Movies");
                Console.WriteLine("5. Update Movie");
                Console.WriteLine("6. Delete Movie");
                Console.WriteLine("7. Add Movie to Watchlist");
                Console.WriteLine("8. Show User Watchlist");
                Console.WriteLine("9. Add Review");
                Console.WriteLine("10. Show Reviews for Movie");
                Console.WriteLine("11. Top Rated Movies");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                //  قراءة اختيار المستخدم
                string choice = Console.ReadLine();

                // ===========================
                // إضافة Category
                // ===========================
                if (choice == "1")
                {
                    // أخذ اسم التصنيف من المستخدم
                    Console.Write("Enter Category Name: ");
                    string name = Console.ReadLine();

                    // إنشاء Object جديد
                    Category category = new Category
                    {
                        Name = name
                    };

                    // إضافة إلى قاعدة البيانات
                    db.Categories.Add(category);

                    // حفظ التغييرات في Database
                    db.SaveChanges();

                    Console.WriteLine("Category added successfully");
                }

                // ===========================
                //  إضافة User
                // ===========================
                else if (choice == "2")
                {
                    Console.Write("Enter User Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine();

                    // إنشاء User جديد
                    User user = new User
                    {
                        Name = name,
                        Email = email
                    };

                    // إضافة المستخدم
                    db.Users.Add(user);

                    // حفظ
                    db.SaveChanges();

                    Console.WriteLine("User added successfully");
                }

                // ===========================
                //  إضافة Movie
                // ===========================
                else if (choice == "3")
                {
                    Console.Write("Enter Movie Title: ");
                    string title = Console.ReadLine();

                    Console.Write("Enter Description: ");
                    string description = Console.ReadLine();

                    Console.Write("Enter Release Year: ");
                    int year = int.Parse(Console.ReadLine());

                    // عرض التصنيفات الموجودة
                    Console.WriteLine("Available Categories:");
                    foreach (var cat in db.Categories)
                    {
                        Console.WriteLine($"{cat.Id} - {cat.Name}");
                    }

                    // اختيار Category
                    Console.Write("Enter Category Id: ");
                    int categoryId = int.Parse(Console.ReadLine());

                    // إنشاء Movie
                    Movie movie = new Movie
                    {
                        Title = title,
                        Description = description,
                        ReleaseYear = year,
                        CategoryId = categoryId
                    };

                    // حفظ الفيلم
                    db.Movies.Add(movie);
                    db.SaveChanges();

                    Console.WriteLine("Movie added successfully");
                }

                // ===========================
                //  عرض Movies
                // ===========================
                else if (choice == "4")
                {
                    // Include => يجلب الـ Category مع الفيلم
                    var movies = db.Movies
                        .Include(m => m.Category)
                        .ToList();
                    //عرض جميع الافلام مع تصنيفها:
                    Console.WriteLine("\nMovies List:");

                    foreach (var m in movies)
                    {
                        Console.WriteLine($"{m.Title} - {m.ReleaseYear} - {m.Category.Name}");
                    }
                }

                else if (choice == "5")
                {
                    // تعديل بيانات فيلم
                    Console.Write("Enter Movie Id to Update: ");
                    int id = int.Parse(Console.ReadLine());

                    Movie movie = db.Movies.Find(id);

                    if (movie == null)
                    {
                        Console.WriteLine("Movie not found.");
                    }
                    else
                    {
                        Console.Write("Enter New Title: ");
                        movie.Title = Console.ReadLine();

                        Console.Write("Enter New Description: ");
                        movie.Description = Console.ReadLine();

                        Console.Write("Enter New Release Year: ");
                        movie.ReleaseYear = int.Parse(Console.ReadLine());

                        db.SaveChanges();

                        Console.WriteLine("Movie updated successfully.");
                    }
                }

                else if (choice == "6")
                {
                    // حذف فيلم
                    Console.Write("Enter Movie Id to Delete: ");
                    int id = int.Parse(Console.ReadLine());

                    Movie movie = db.Movies.Find(id);

                    if (movie == null)
                    {
                        Console.WriteLine("Movie not found.");
                    }
                    else
                    {
                        db.Movies.Remove(movie);
                        db.SaveChanges();

                        Console.WriteLine("Movie deleted successfully.");
                    }
                }

                else if (choice == "7")
                {
                    // إضافة فيلم إلى Watchlist
                    Console.Write("Enter User Id: ");
                    int userId = int.Parse(Console.ReadLine());

                    Console.Write("Enter Movie Id: ");
                    int movieId = int.Parse(Console.ReadLine());

                    bool exists = db.Watchlists.Any(w => w.UserId == userId && w.MovieId == movieId);

                    if (exists)
                    {
                        Console.WriteLine("This movie is already in the watchlist.");
                    }
                    else
                    {
                        Watchlist watchlist = new Watchlist
                        {
                            UserId = userId,
                            MovieId = movieId,
                            AddedDate = DateTime.Now
                        };

                        db.Watchlists.Add(watchlist);
                        db.SaveChanges();

                        Console.WriteLine("Movie added to watchlist successfully.");
                    }
                }

                else if (choice == "8")
                {
                    // عرض Watchlist لمستخدم معين
                    Console.Write("Enter User Id: ");
                    int userId = int.Parse(Console.ReadLine());

                    var watchlist = db.Watchlists
                        .Include(w => w.Movie)
                        .Where(w => w.UserId == userId)
                        .ToList();

                    Console.WriteLine("\nUser Watchlist:");

                    foreach (var item in watchlist)
                    {
                        Console.WriteLine($"{item.Movie.Id}. {item.Movie.Title} - Added Date: {item.AddedDate}");
                    }
                }

                else if (choice == "9")
                {
                    // إضافة Review لفيلم
                    Console.Write("Enter User Id: ");
                    int userId = int.Parse(Console.ReadLine());

                    Console.Write("Enter Movie Id: ");
                    int movieId = int.Parse(Console.ReadLine());

                    Console.Write("Enter Comment: ");
                    string comment = Console.ReadLine();

                    Console.Write("Enter Rating from 1 to 5: ");
                    int rating = int.Parse(Console.ReadLine());

                    Review review = new Review
                    {
                        UserId = userId,
                        MovieId = movieId,
                        Comment = comment,
                        Rating = rating
                    };

                    db.Reviews.Add(review);
                    db.SaveChanges();

                    Console.WriteLine("Review added successfully.");
                }

                else if (choice == "10")
                {
                    // عرض Reviews لفيلم معين
                    Console.Write("Enter Movie Id: ");
                    int movieId = int.Parse(Console.ReadLine());

                    var reviews = db.Reviews
                        .Include(r => r.User)
                        .Where(r => r.MovieId == movieId)
                        .ToList();

                    Console.WriteLine("\nMovie Reviews:");

                    foreach (var review in reviews)
                    {
                        Console.WriteLine($"User: {review.User.Name}");
                        Console.WriteLine($"Rating: {review.Rating}");
                        Console.WriteLine($"Comment: {review.Comment}");
                        Console.WriteLine("-----------------------");
                    }
                }

                else if (choice == "11")
                {
                    // عرض أعلى الأفلام تقييمًا
                    var topMovies = db.Reviews
                        .Include(r => r.Movie)
                        .GroupBy(r => r.Movie)
                        .Select(g => new
                        {
                            MovieTitle = g.Key.Title,
                            AverageRating = g.Average(r => r.Rating)
                        })
                        .OrderByDescending(x => x.AverageRating)
                        .ToList();

                    Console.WriteLine("\nTop Rated Movies:");

                    foreach (var item in topMovies)
                    {
                        Console.WriteLine($"{item.MovieTitle} - Average Rating: {item.AverageRating:F1}");
                    }
                }

                else if (choice == "0")
                {
                    // خروج من البرنامج
                    Console.WriteLine("Exiting program...");
                    break;
                }

                else
                {
                    // إذا المستخدم كتب رقم غير موجود
                    Console.WriteLine("Invalid choice. Try again.");
                }
            }
        }
    
    }
}
