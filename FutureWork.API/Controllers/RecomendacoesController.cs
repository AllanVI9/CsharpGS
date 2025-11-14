using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FutureWork.API.Data;
using FutureWork.API.Models;

namespace FutureWork.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecomendacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecomendacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var recomendacoes = await _context.Recomendacoes
                .Include(r => r.Profissional)
                .Include(r => r.Vaga)
                .ToListAsync();
            return Ok(recomendacoes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Recomendacao recomendacao)
        {
            _context.Recomendacoes.Add(recomendacao);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new
            {
                profissionalId = recomendacao.ProfissionalId,
                vagaId = recomendacao.VagaId
            }, recomendacao);
        }

        [HttpDelete("{profissionalId}/{vagaId}")]
        public async Task<IActionResult> Delete(int profissionalId, int vagaId)
        {
            var recomendacao = await _context.Recomendacoes
                .FindAsync(profissionalId, vagaId);

            if (recomendacao == null) return NotFound();

            _context.Recomendacoes.Remove(recomendacao);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
