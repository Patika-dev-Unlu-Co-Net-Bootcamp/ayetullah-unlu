using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UnluCo.Egitim.API.Ilk.Hafta.Db;
using UnluCo.Egitim.API.Ilk.Hafta.Models;
using UnluCo.Egitim.API.Ilk.Hafta.Models.Dto;

namespace UnluCo.Egitim.API.Ilk.Hafta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        public static List<School> Schools = DefaultData.Schools;

        [HttpGet]
        public IActionResult GetSchools()
        {
            return Ok(Schools.OrderBy(x => x.Name));
        }

        [HttpGet("{id}/{schoolName}")]
        public IActionResult GetSchool(int id, string schoolName)
        {
            var school = Schools;
            if (id != 0)
            {
                school = Schools.Where(x => x.Id == id).ToList();
            }

            if(!string.IsNullOrEmpty(schoolName))
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
            if(Schools.FirstOrDefault(x => x.Name.Equals(schoolDto.Name)) != null)
            {
                return BadRequest("Farklı bir okul adı giriniz");
            }

            var lastId = Schools.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            Schools.Add(new School() {Id = lastId+1, Name = schoolDto.Name });
            return Ok(Schools);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var school = Schools.FirstOrDefault(x => x.Id == id);
            if(school == null)
            {
                return NotFound("Okul bulunamadı");
            }

            Schools.Remove(school);
            return Ok(Schools);
        }
    }
}
