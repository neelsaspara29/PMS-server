using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_backend.DataContext;
using PMS_backend.Dto;
using PMS_backend.Model;
using PMS_backend.Services;

namespace PMS_backend.Controllers
{
    public class PatientReportController : Controller
    {
        private readonly UserDbContext _context;

        public PatientReportController(UserDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("patient/report/add")]
        public async Task<IActionResult> ReportAdd([FromBody] PatienReportCreateBody body)
        {

            var report = new PatientReportsModel
            {
                PatientId = body.PatientId,
                report_date = DateTime.UtcNow,
                report_type = body.report_type,
                description = body.description,
                active_status = body.active_status
            };
            
            _context.PatientReports.Add(report);
            await _context.SaveChangesAsync();

            var response = new StandardResponse(true, "Patient Report Created Successfuly.");

            return Ok(response);
        }

        [Authorize]
        [HttpPost("patient/report/edit")]
        public async Task<IActionResult> ReportEdit([FromBody] ReportEditBody body)
        {

            var report = await _context.PatientReports.Where(rp => rp.Id == body.id).FirstOrDefaultAsync();

            report.report_type = body.report_type;
            report.description = body.description;
            report.active_status = body.active_status;
            await _context.SaveChangesAsync();

            var response = new StandardResponse(true, "Patient Report Edited Successfuly.");

            return Ok(response);
        }

        [Authorize]
        [HttpGet("report/{patientId}")]
        public async Task<IActionResult> GetPatientReports(int patientId)
        {
            var reports = await _context.PatientReports
                                        .Where(pr => pr.PatientId == patientId)
                                        .ToListAsync();

            if (reports == null)
            {
                var response = new StandardResponse(false, "Patient Reports Not Found.");

                return Ok(response);
            }

            var reportsResponse = new StandardResponse(true, "Patient Report Retrieved", reports);
            return Ok(reportsResponse);
        } 

    [HttpPost("/user/public/detail")]
    public async Task<IActionResult> GetPattientDetailByEmail([FromBody] GetReportByIdBody body)
    {

        var user = await _context.Patients.Where(rp => rp.email == body.email).FirstOrDefaultAsync();

        if (user == null)
        {
            var response = new StandardResponse(false, "Patient Detail Not Found.");

            return Ok(response);
        }

        var reportsResponse = new StandardResponse(true, "Patient Report Retrieved", user);
        return Ok(reportsResponse);
    }


    [HttpGet("report/publish/{id}")]
        public async Task<IActionResult> GetPatientReportsById( int id)
        {
            
            var reports = await _context.PatientReports.Where(rp => rp.PatientId == id && rp.active_status == "publish").ToListAsync();

            var reportsResponse = new StandardResponse(true, "Patient Report Retrieved",   reports);
            return Ok(reportsResponse);
        }


    }
}
