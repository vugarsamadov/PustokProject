using PustokProject.CoreModels;
using PustokProject.ViewModels.AuthorVMS;

namespace PustokProject.ViewModels.BookList
{
	public record AuthorsBookListVmItem(string Name,int bookCount,int Id);
	public record CategoriesBookListVmItem(string Name,int bookCount,int Id,IEnumerable<CategoriesBookListVmItem>? subCategories);

	public class VM_BookList
	{
        public IEnumerable<AuthorsBookListVmItem> Authors { get; set; }
        public IEnumerable<CategoriesBookListVmItem> Categories { get; set; }
        public BookListForm FormModel { get; set; }
    }

    public class BookListForm
    {
        public int CategoryId { get; set; }
        public IEnumerable<int> AuthorIds { get; set; }

    }

	public class VM_BookListItem
	{
        public string Name { get; set; }
        public string Authors { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? DiscountedPrice { get; set; }

    }

}
