using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        
        public async Task<ActionResult> GetAsync([FromServices] BlogDataContext context)
            
        {
            //User.Identity.IsAuthenticated(); // retorna bolleano de se esta verificado ou não
            //User.IsInRole("admin"); // retorna o perfil do usuario
            //User.Identity.Name; // retorna o nome do usuario

            var categories = await context.Categories.ToListAsync();
            return Ok(new ResultViewModel<List<Category>>(categories));
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (categories == null)
                    return NotFound(new ResultViewModel<List<Category>>("05XE7 - Não encontrado"));


                return Ok(new ResultViewModel<Category>(categories));
            }
            catch (Exception ex)
            {
                return NotFound(new ResultViewModel<List<Category>>("05XE8 - Não encontrado"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<ActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Posts = [],
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };
                await context
                   .Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "05XE9 -Não foi possivel criar");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "05XE10 - Falha no servidor");

            }

        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context
                 .Categories
                 .FirstOrDefaultAsync(x => x.Id == id);

                if (categories == null)
                    return NotFound();

                categories.Name = model.Name;
                categories.Slug = model.Slug;

                context.Categories.Update(categories);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(categories));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<Category>("05XE11 - Nao foi possivel atualizar a Categoria"));
            }
        }

        [HttpDelete("v1/categories{id:int}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (categories == null)
                    return NotFound();

                context.Categories.Remove(categories);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(categories));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<Category>("05XE12 - Não foi possivel remover a Categoria"));
            }
        }

    }

}

