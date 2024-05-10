using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_backend.DataContext;
using PMS_backend.Dto;
using PMS_backend.Model;
using PMS_backend.Services;

namespace PMS_backend.Controllers
{
    public class PatientConroller : Controller
    {
        private readonly UserDbContext _context;
        private readonly IEmailService _emailService;
   
        public PatientConroller(UserDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [Authorize]
        [HttpPost("patient/add")]
        public async Task<IActionResult> RegisterUser([FromBody] PatientBody body)
        {
            if (await _context.Patients.AnyAsync(x => x.email == body.email))
            {
                var error_response = new StandardResponse(false, "Patient Already Exist!");
                return Ok(error_response);
            }
            var patient = new PatientModel
            {
                email = body.email,
                name = body.name,
                mobile_number = body.mobile_number,
                gender = body.gender,
                dob = body.dob
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync(patient.email, "Welcome to Our App", "Welcome!");

            var response = new StandardResponse(true, "Patient Created Successfuly.");

            return Ok(response);
        }

        [HttpGet("patient/get/all")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            var response = new StandardResponse(true, "Patient Retrieve Successfully", patients);
            return Ok(response);
        }

        [HttpGet("patient/get/detail/{patientId}")]
        public async Task<IActionResult> GetPatientDetail(int patientId)
        {
            var patientData = await _context.Patients
                            .Where(pr => pr.Id == patientId).FirstAsync();

            if (patientData == null)
            {
                var response = new StandardResponse(false, "Patient Not Found.");

                return Ok(response);
            }

            var patientResponse = new StandardResponse(true, "Patient Retrieved", patientData);
            return Ok(patientResponse);
        }
    }
}
