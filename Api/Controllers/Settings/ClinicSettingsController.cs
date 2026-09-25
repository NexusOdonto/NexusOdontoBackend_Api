using System.Text.Json;
using Application.DTOs.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Settings;

[ApiController]
[Route("api/v1/clinic-settings")]
public sealed class ClinicSettingsController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ClinicSettingsController> _logger;
    private static readonly object _fileLock = new();
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public ClinicSettingsController(IWebHostEnvironment environment, ILogger<ClinicSettingsController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    private string GetFilePath()
    {
        var dataDir = Path.Combine(_environment.ContentRootPath, "App_Data");
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }

        return Path.Combine(dataDir, "clinic_settings.json");
    }

    /// <summary>
    /// Obtiene la configuración actual de la clínica. Si aún no existe, devuelve los valores por defecto.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ClinicSettingsDto), StatusCodes.Status200OK)]
    public IActionResult GetSettings()
    {
        var path = GetFilePath();
        lock (_fileLock)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    var json = System.IO.File.ReadAllText(path);
                    var settings = JsonSerializer.Deserialize<ClinicSettingsDto>(json, _jsonOptions);
                    if (settings != null)
                    {
                        return Ok(settings);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al leer clinic_settings.json");
                }
            }

            return Ok(new ClinicSettingsDto());
        }
    }

    /// <summary>
    /// Actualiza y persiste la configuración corporativa y operativa de la clínica.
    /// </summary>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(ClinicSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateSettings([FromBody] ClinicSettingsDto? settings)
    {
        if (settings == null)
        {
            return BadRequest(new { message = "La configuración no puede ser nula." });
        }

        settings.UpdatedAt = DateTime.UtcNow;
        var path = GetFilePath();

        lock (_fileLock)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, _jsonOptions);
                System.IO.File.WriteAllText(path, json);
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar clinic_settings.json");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al guardar en el servidor." });
            }
        }
    }
}
