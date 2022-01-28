using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UnluCo.Egitim.API.Ikinci.Hafta.Db;
using UnluCo.Egitim.API.Ikinci.Hafta.Models;
using UnluCo.Egitim.API.Ikinci.Hafta.Models.Dto;

namespace UnluCo.Egitim.API.Ikinci.Hafta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : Controller
    {
        private static List<School> _schools = Data.Schools;

        [HttpGet]
        public IActionResult GetSchools()
        {
            return Ok(_schools.OrderBy(x => x.Name));// ismine göre listeledik
        }

        [HttpGet("{id}/{schoolName}")]
        public IActionResult GetSchool(int id, string schoolName)
        {
            var school = _schools;
            if (id != 0)
            {
                school = _schools.Where(x => x.Id == id).ToList();
            }

            if (!string.IsNullOrEmpty(schoolName))
            {
                school = school.Where(x => x.Name.ToLower().Contains(schoolName.ToLower())).ToList();
            }

            if (school == null || !school.Any())
            {
                return NotFound("Okul bulunamadı");
            }


            return Ok(school);
        }

        [HttpPost]
        public IActionResult AddSchool([FromBody] SchoolDto schoolDto)
        {
            if (_schools.FirstOrDefault(x => x.Name.Equals(schoolDto.Name)) != null)
            {
                return BadRequest("Farklı bir okul adı giriniz");
            }

            var lastId = _schools.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            _schools.Add(new School() { Id = lastId + 1, Name = schoolDto.Name });
            return Ok(_schools);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var school = _schools.FirstOrDefault(x => x.Id == id);
            if (school == null)
            {
                return NotFound("Okul bulunamadı");
            }

            _schools.Remove(school);
            return Ok(_schools);
        }
    }
}
