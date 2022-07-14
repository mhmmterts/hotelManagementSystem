using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class BlogManager
    {
        Repository<Blog> repoblog = new Repository<Blog>();

        public List<Blog> GetAll()
        {
            return repoblog.List();
        }

        public List<Blog> GetBlogByID(int id)
        {
            return repoblog.List(x => x.BlogId == id);
        }
        public List<Blog> GetBlogByAuthor(int id)
        {
            return repoblog.List(x => x.AuthorID == id);
        }
        public List<Blog> GetBlogByCategory(int id)
        {
            return repoblog.List(x => x.CategoryID == id);
        }

        public int BlogAddBL(Blog p)
        {
            return repoblog.Insert(p);
        }

        public int DeleteBL(int p)
        {
            Blog blog = repoblog.Find(x => x.BlogId == p);
            return repoblog.Delete(blog);
        }

        public Blog FindBlog(int id)
        {
            return repoblog.Find(x => x.BlogId == id);
        }

        public int UpdateBlog(Blog b)
        {
            Blog blog = repoblog.Find(x => x.BlogId == b.BlogId);
            blog.BlogTitle = b.BlogTitle;
            blog.BlogDate = b.BlogDate;
            blog.BlogImage = b.BlogImage;
            blog.CategoryID = b.CategoryID;
            blog.BlogContent = b.BlogContent;
            return repoblog.Update(blog);
        }

    }
}
