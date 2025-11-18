using InsureYouAIProject.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardCommentListComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardCommentListComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Comments.Include(x => x.AppUser).OrderByDescending(y => y.CommentId).Take(7).ToListAsync();
            return View(values);
        }
    }
}
