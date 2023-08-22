
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace judo_univ_rennes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> logger;
        private readonly IPdfRepo _pdfRepo;
        public PdfController(ILogger<PdfController> logger, IPdfRepo pdfRepo)
        {
            this.logger = logger;
            this._pdfRepo = pdfRepo;
        }
        /// <summary>
        /// Download pdf
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet]

        [Route("{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetOne( string filename)
        {
            logger.LogInformation($"Pdf Attempt GET {filename}");

            try
            {
                var db = _pdfRepo.GetByNameAsync(filename).Result;
                return File(db.Content, "application/pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(GetOne)}");
                return Problem($"Something Went Wrong in the {nameof(GetOne)}", statusCode: 500);
            }
        }
    }
}
