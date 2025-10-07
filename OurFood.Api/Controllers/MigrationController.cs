using Microsoft.AspNetCore.Mvc;
using OurFood.Api.Commands;

namespace OurFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MigrationController : ControllerBase
{
    private readonly MigrateWwwrootToS3Command _migrateCommand;

    public MigrationController(MigrateWwwrootToS3Command migrateCommand)
    {
        _migrateCommand = migrateCommand;
    }

    [HttpPost("migrate-wwwroot-to-s3")]
    public async Task<IActionResult> MigrateWwwrootToS3()
    {
        try
        {
            await _migrateCommand.ExecuteAsync();
            return Ok(new { message = "Migração concluída com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
