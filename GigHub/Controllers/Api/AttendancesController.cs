using System.Web.Http;
using GigHub.Core.Models;
using GigHub.Core.Dtos;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using GigHub.Core;

namespace GigHub.Controllers.Api
{

    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendancesController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        /// <summary>
        /// Mark current user as attending an AttendanceDto.Gig
        /// </summary>
        /// <param name="dto">attendance dto</param>
        /// <returns>OK if successful, or BadRequest if user is already going</returns>
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (_unitOfWork.Attendances.IsAttending(userId, dto.GigId))
                return BadRequest("The attendance already exists!");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _unitOfWork.Attendances.Add(attendance);
            _unitOfWork.Complete();

            return Ok();
        }

        /// <summary>
        /// Mark current user as not attending an AttendanceDto.Gig
        /// </summary>
        /// <param name="id">Gig id</param>
        /// <returns>Ok if successful, NotFound if missing an attendance</returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();

            var attendance = _unitOfWork.Attendances.GetAttendance(userId, id);
            if (attendance == null)
                return NotFound();

            _unitOfWork.Attendances.Remove(attendance);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
