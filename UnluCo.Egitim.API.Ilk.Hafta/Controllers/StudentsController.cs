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
    public class StudentsController : ControllerBase
    {
        public static List<Student> Students = DefaultData.Students;
        public static List<School> Schools = DefaultData.Schools;

        [HttpGet]
        public IActionResult GetStudents()
        {
            var studentList = ConvertStudentsDto(Students).OrderBy(x => x.No);
            return Ok(studentList);
        }

        [HttpGet("{no}")]
        public IActionResult GetStudent(int no)
        {
            var student = Students.FirstOrDefault(x => x.No == no);
            if(student == null) // Kullanıcı olup olmadığı kontrol edildi.
            {
                return NotFound("Öğrenci Bulunamadı");
            }

            return Ok(ConvertStudentDto(student));
        }

        [HttpGet("{name}/{surname}")] // Öğrenci isim veya soyisime göre arama
        public IActionResult GetStudent(string name, string surname)
        {
            if(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname))
            {
                return BadRequest("İsim boş olamaz");
            }

            var studentList = Students;
            if(!string.IsNullOrEmpty(name)) 
            {
                studentList = studentList.Where(x => x.Name.ToLower().Equals(name.Trim().ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(surname))
            {
                studentList = studentList.Where(x => x.Surname.ToLower().Equals(surname.Trim().ToLower())).ToList();
            }

            return Ok(ConvertStudentsDto(studentList));
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] StudentDto student)
        {
            var message = CheckStudent(student);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            var school = Schools.FirstOrDefault(x => x.Name == student.SchoolName);
            if ( school == null)
            {
                return NotFound("Okul ismi bulunamadı. Lütffen kontrol ediniz");
            }

            var lastId = Students.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            Students.Add(new Student() { 
                Id = lastId+1,
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
            var message = CheckStudent(student, true);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            var school = Schools.FirstOrDefault(x => x.Name == student.SchoolName);
            if (school == null)
            {
                return NotFound("Okul ismi bulunamadı. Lütffen kontrol ediniz");
            }
           
            Students.Where(w => w.No == no).ToList().ForEach(u =>
            {
                u.Id = u.Id;
                u.Name = student.Name;
                u.Surname = student.Surname;
                u.No = student.No;
                u.SchoolId = school.Id;
            });
            return Ok(Students);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int no)
        {
            var student = Students.FirstOrDefault(x => x.No == no);
            if(student == null)
            {
                return NotFound("Öğrenci bulunamadı");
            }

            Students.Remove(student);
            return Ok(Students);
        }

        [HttpPatch("{no}")]
        public IActionResult UpdateStudentWithPatch([FromBody] StudentDto student, int no)
        {
            var message = CheckStudent(student, true);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            var school = Schools.FirstOrDefault(x => x.Name == student.SchoolName);
            if (school == null)
            {
                return NotFound("Okul ismi bulunamadı. Lütffen kontrol ediniz");
            }

            Students.Where(w => w.No == no).ToList().ForEach(u =>
            {
                u.Id = u.Id;
                u.Name = student.Name;
                u.Surname = student.Surname;
                u.No = student.No;
                u.SchoolId = school.Id;
            });
            return Ok(Students);
        }


        private string CheckStudent(StudentDto student, bool isUpdate = false)
        {
            if(student == null)
            {
                return "Bir hata oluştu";
            }

            if (student.Name.Equals("string") || string.IsNullOrEmpty(student.Name)) {
                return "Öğrenci ismi eksik";
            }
            else if (student.SchoolName.Equals("string") || string.IsNullOrEmpty(student.SchoolName))
            {
                return "Öğrenci okul ismi eksik";
            }
            else if (student.Surname.Equals("string") || string.IsNullOrEmpty(student.Surname))
            {
                return "Öğrenci soyadı eksik";
            }
            else if (student.No is default(int))
            {
                return "Öğrenci numarası eksik";
            }

            if(Students.FirstOrDefault(x => x.No == student.No) != null)
            {
                if(!isUpdate)
                {
                    return "Farklı bir öğrenci numarası giriniz";
                }
                else if (isUpdate && Students.FirstOrDefault(x => x.No == student.No &&
                    x.Name != student.Name && x.Surname != student.Surname) != null)
                {
                    return "Farklı bir öğrenci giriniz";
                }
                
            }

            return string.Empty;
        }

        private List<StudentDto> ConvertStudentsDto(List<Student> students)
        {
            List<StudentDto> studentList = new List<StudentDto>();
            foreach (var student in students) // mapleme işlemi kullanılmadığı için bu yöntemle yapıldı
            {
                studentList.Add(ConvertStudentDto(student));
            }

            return studentList;
        }

        private StudentDto ConvertStudentDto(Student student)// mapleme işlemi kullanılmadığı için bu yöntemle yapıldı
        {
            return new StudentDto()
            {
                Name = student.Name,
                Surname = student.Surname,
                No = student.No,
                SchoolName = Schools.FirstOrDefault(x => x.Id == student.SchoolId)?.Name ?? ""
            };
        }
    }
}
