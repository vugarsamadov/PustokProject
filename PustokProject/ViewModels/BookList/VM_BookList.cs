using PustokProject.CoreModels;
using PustokProject.ViewModels.AuthorVMS;

namespace PustokProject.ViewModels.BookList
{
	public record AuthorsBookListVmItem(string Name,int bookCount,int Id);
	public record CategoriesBookListVmItem(string Name,int bookCount,int Id,IEnumerable<CategoriesBookListVmItem> subCategories);

	public class VM_BookList
	{
        public IEnumerable<AuthorsBookListVmItem> Authors { get; set; }
        public IEnumerable<CategoriesBookListVmItem> Categories { get; set; }
    }
}
