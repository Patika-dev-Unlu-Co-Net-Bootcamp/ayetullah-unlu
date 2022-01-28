using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UnluCo.Egitim.API.Ikinci.Hafta.Db;
using UnluCo.Egitim.API.Ikinci.Hafta.Models;
using UnluCo.Egitim.API.Ikinci.Hafta.Models.Dto;
using UnluCo.Egitim.API.Ikinci.Hafta.Validators;

namespace UnluCo.Egitim.API.Ikinci.Hafta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private static List<Student> _students = Data.Students;
        private static List<School> _schools = Data.Schools;
        private readonly IMapper _mapper;

        public StudentsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _students.OrderBy(x => x.No).ToList<Student>();
            var studentList = _mapper.Map<List<StudentDto>>(students);
            studentList.ForEach(x => x.SchoolName = _schools.FirstOrDefault(a => a.Id.ToString().Equals(x.SchoolName))?.Name ?? string.Empty );
            return Ok(studentList);
        }

        [HttpGet("{no}")]
        public IActionResult GetStudent(int no)
        {
            var student = _students.FirstOrDefault(x => x.No == no);
            if (student == null) // Kullanıcı olup olmadığı kontrol edildi.
            {
                return NotFound("Öğrenci Bulunamadı");
            }

            var studentDto = _mapper.Map<StudentDto>(student);
            studentDto.SchoolName = _schools.FirstOrDefault(x => x.Id == student.SchoolId)?.Name ?? "";
            return Ok(studentDto);
        }

        [HttpGet("{name}/{surname}")] // Öğrenci isim veya soyisime göre arama
        public IActionResult GetStudent(string name, string surname)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname))
            {
                return BadRequest("İsim boş olamaz");
            }

            var studentList = _students;
            if (!string.IsNullOrEmpty(name))
            {
                studentList = studentList.Where(x => x.Name.ToLower().Equals(name.Trim().ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(surname))
            {
                studentList = studentList.Where(x => x.Surname.ToLower().Equals(surname.Trim().ToLower())).ToList();
            }

            var students = _mapper.Map<List<StudentDto>>(studentList);
            return Ok(students);
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] StudentDto student)
        {
            StudentValidations validatior = new StudentValidations(ProcessType.Add);
            validatior.ValidateAndThrow(student);
            var message = CheckStudent(student);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            var school = _students.FirstOrDefault(x => x.Name == student.SchoolName);
            if (school == null)
            {
                return NotFound("Okul ismi bulunamadı. Lütffen kontrol ediniz");
            }

            var lastId = _students.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            _students.Add(new Student()
            {
                Id = lastId + 1,
                Name = student.Name,
                Surname = student.Surname,
                No = student.No,
                SchoolId = school.Id
            });
            return Ok("Öğrenci eklendi!!");
        }

        [HttpPut("{no}")]
        public IActionResult UpdateStudent([FromBody] StudentDto student, int no)
        {
            StudentValidations validatior = new StudentValidations(ProcessType.Update);
            validatior.ValidateAndThrow(student);
            var message = CheckStudent(student, true);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            var school = _students.FirstOrDefault(x => x.Name == student.SchoolName);
            if (school == null)
            {
                return NotFound("Okul ismi bulunamadı. Lütffen kontrol ediniz");
            }

            _students.Where(w => w.No == no).ToList().ForEach(u =>
            {
                u.Id = u.Id;
                u.Name = student.Name;
                u.Surname = student.Surname;
                u.No = student.No;
                u.SchoolId = school.Id;
            });
            return Ok(_students);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int no)
        {
            var student = _students.FirstOrDefault(x => x.No == no);
            if (student == null)
            {
                return NotFound("Öğrenci bulunamadı");
            }

            _students.Remove(student);
            return Ok(_students);
        }

        [HttpPatch("{no}")]
        public IActionResult UpdateStudentWithPatch([FromBody] StudentDto student, int no)
        {
            StudentValidations validatior = new StudentValidations(ProcessType.Update);
            validatior.ValidateAndThrow(student);
            var message = CheckStudent(student, true);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            var school = _students.FirstOrDefault(x => x.Name == student.SchoolName);
            if (school == null)
            {
                return NotFound("Okul ismi bulunamadı. Lütffen kontrol ediniz");
            }

            _students.Where(w => w.No == no).ToList().ForEach(u =>
            {
                u.Id = u.Id;
                u.Name = student.Name;
                u.Surname = student.Surname;
                u.No = student.No;
                u.SchoolId = school.Id;
            });
            return Ok(_students);
        }


        private string CheckStudent(StudentDto student, bool isUpdate = false)
        {
            if (_students.FirstOrDefault(x => x.No == student.No) != null)
            {
                if (!isUpdate)
                {
                    return "Farklı bir öğrenci numarası giriniz";
                }
                else if (isUpdate && _students.FirstOrDefault(x => x.No == student.No &&
                    x.Name != student.Name && x.Surname != student.Surname) != null)
                {
                    return "Farklı bir öğrenci giriniz";
                }

            }

            return string.Empty;
        }
    }
}
