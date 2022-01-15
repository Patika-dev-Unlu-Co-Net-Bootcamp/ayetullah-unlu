using System.Collections.Generic;
using UnluCo.Egitim.API.Ilk.Hafta.Models;

namespace UnluCo.Egitim.API.Ilk.Hafta.Db
{
    public class DefaultData
    {
        //Öğrenci bilgileri default eklendi
        public static List<Student> Students = new List<Student>()
        {
            new Student()
            {
                Id = 1,
                Name = "Ayetullah",
                Surname = "Ünlü",
                No = 148,
                SchoolId = 2
            },
            new Student()
            {
                Id = 2,
                Name = "Merve",
                Surname = "Ünlü",
                No = 101,
                SchoolId = 3
            },
            new Student()
            {
                Id = 3,
                Name = "İbrahim",
                Surname = "Bayrak",
                No = 1,
                SchoolId = 2
            },
            new Student()
            {
                Id = 4,
                Name = "Tarık",
                Surname = "Yüzgül",
                No = 1048,
                SchoolId = 3
            }
        };

        //Okul bilgileri default eklendi
        public static List<School> Schools = new List<School>()
        {
            new School() { Id = 1, Name = "Yahya Kemal Lisesi"},
            new School() { Id = 2, Name = "Necip Fazıl Kısakürek Anadolu Lisesi"},
            new School() { Id = 3, Name = "Gazi Lisesi"},
            new School() { Id = 4, Name = "Yahya Karakaya Anadolu Lisesi"},
            new School() { Id = 5, Name = "Yahya Karakaya Fen Lisesi"}
        };
    }
}
