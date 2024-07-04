using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;

namespace Todo;

[Route("/api/todo-items")]
[ApiController]
public class TodoItemsApiController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public TodoItemsApiController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpPatch("update-rank")]
    public async Task<IActionResult> UpdateRank([FromBody] UpdateRankModel model)
    {
        var item = await dbContext.TodoItems.FindAsync(model.Id);
        if (item == null)
        {
            return NotFound();
        }

        item.Rank = model.Rank;
        await dbContext.SaveChangesAsync();

        return Ok(item);
    }
}
